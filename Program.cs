using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobStorageAssessment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Azure Storage account connection string
            string connectionString = "your_key"; // Place own connection string from azure blob (replace your_key)
            // Name of the blob container
            string containerName = "your_container"; // Place container name you created in azure (replace your_container)

            // Initialize BlobServiceClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Main loop for the console application
            bool exit = false;
            while (!exit)
            {
                // Display menu options
                Console.WriteLine("Azure Blob Storage Assessment");
                Console.WriteLine("==============================");
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Upload a Blob");
                Console.WriteLine("2. List Blobs");
                Console.WriteLine("3. Download a Blob");
                Console.WriteLine("4. Delete a Blob");
                Console.WriteLine("5. Exit");
                Console.WriteLine("==============================");

                // Read user input for menu selection
                string option = Console.ReadLine();

                // Switch case for handling user-selected option
                switch (option)
                {
                    case "1":
                        // Upload a blob option
                        Console.WriteLine("Enter the full path of the file to upload:");
                        string filePath = Console.ReadLine();
                        await UploadBlobAsync(blobServiceClient, containerName, filePath);
                        break;
                    case "2":
                        // List blobs option
                        await ListBlobsAsync(blobServiceClient, containerName);
                        break;
                    case "3":
                        // Download a blob option
                        Console.WriteLine("Enter the name of the blob to download:");
                        string blobName = Console.ReadLine();
                        Console.WriteLine("Enter the destination path to save the downloaded file:");
                        string destinationPath = Console.ReadLine();
                        await DownloadBlobAsync(blobServiceClient, containerName, blobName, destinationPath);
                        break;
                    case "4":
                        // Delete a blob option
                        Console.WriteLine("Enter the name of the blob to delete:");
                        string blobToDelete = Console.ReadLine();
                        await DeleteBlobAsync(blobServiceClient, containerName, blobToDelete);
                        break;
                    case "5":
                        // Exit the application
                        exit = true;
                        break;
                    default:
                        // Invalid option
                        Console.WriteLine("Invalid option. Please select again.");
                        break;
                }
            }
        }

        // Method to upload a blob to Azure Blob Storage
        static async Task UploadBlobAsync(BlobServiceClient blobServiceClient, string containerName, string filePath)
        {
            // Remove leading and trailing whitespace from file path
            filePath = filePath.Trim();

            // Remove quotation marks if present in file path
            if (filePath.StartsWith("\"") && filePath.EndsWith("\""))
            {
                filePath = filePath.Substring(1, filePath.Length - 2);
            }

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Get the container client and blob client
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

                // Upload the blob
                Console.WriteLine("==============================");
                Console.WriteLine($"Uploading blob from {filePath} to container {containerName}...");
                Console.WriteLine("==============================");

                try
                {
                    // Open the file stream and upload the blob asynchronously
                    using (FileStream fileStream = File.OpenRead(filePath))
                    {
                        await blobClient.UploadAsync(fileStream, overwrite: true);
                    }

                    Console.WriteLine("Upload completed successfully.");
                }
                catch (RequestFailedException ex)
                {
                    Console.WriteLine($"Upload failed: {ex.Message}");
                }
            }
            else
            {
                // File not found
                Console.WriteLine("File not found. Please enter a valid file path.");
            }
        }

        // Method to list blobs in the specified container
        static async Task ListBlobsAsync(BlobServiceClient blobServiceClient, string containerName)
        {
            // Get the container client
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // List blobs in the container
            Console.WriteLine("==============================");
            Console.WriteLine($"Listing blobs in container {containerName}...");
            Console.WriteLine("==============================");

            try
            {
                // Iterate through the blobs and print their names
                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    Console.WriteLine($"Blob: {blobItem.Name}");
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Listing blobs failed: {ex.Message}");
            }
        }

        // Method to download a blob from Azure Blob Storage
        static async Task DownloadBlobAsync(BlobServiceClient blobServiceClient, string containerName, string blobName, string destinationPath)
        {
            // Get the container client and blob client
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Remove leading and trailing whitespace from destination path
            destinationPath = destinationPath.Trim();

            // Remove quotation marks if present in destination path
            if (destinationPath.StartsWith("\"") && destinationPath.EndsWith("\""))
            {
                destinationPath = destinationPath.Substring(1, destinationPath.Length - 2);
            }

            // Download the blob
            Console.WriteLine("==============================");
            Console.WriteLine($"Downloading blob {blobName} from container {containerName} to {destinationPath}...");
            Console.WriteLine("==============================");

            try
            {
                // Download the blob asynchronously
                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

                // Open the file stream and copy the blob content asynchronously
                using (FileStream fileStream = File.OpenWrite(Path.Combine(destinationPath, blobName)))
                {
                    await blobDownloadInfo.Content.CopyToAsync(fileStream);
                }

                Console.WriteLine("Download completed successfully.");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Download failed: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Download failed: {ex.Message}");
            }
        }

        // Method to delete a blob from Azure Blob Storage
        static async Task DeleteBlobAsync(BlobServiceClient blobServiceClient, string containerName, string blobName)
        {
            // Get the container client and blob client
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Delete the blob
            Console.WriteLine("==============================");
            Console.WriteLine($"Deleting blob {blobName} from container {containerName}...");
            Console.WriteLine("==============================");

            try
            {
                // Delete the blob asynchronously
                await blobClient.DeleteAsync();
                Console.WriteLine("Blob deleted successfully.");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
            }
        }
    }
}
