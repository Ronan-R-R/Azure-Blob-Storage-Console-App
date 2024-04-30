# Azure Blob Storage

## Capabilities:
- Upload a file to Azure Blob Storage.
- List all blobs in a specified container.
- Download a blob from a specified container.
- Delete a specified blob from the container.

## Limitations:
- Case-sensitive inputs for container names, blob names, and file paths.
- Blobs are deleted automatically after 30 days.

## How to Test:
1. Ensure Visual Studio 2022 Community and .NET 8 are installed.
2. Open the solution in Visual Studio.
3. Set the connection string and container name in the `Program.cs` file.
4. Run the application and select desired options from the menu.

## How to Upload a File:
- Select option 1: "Upload a Blob" from the menu.
- Enter the full path of the file to upload.
- Ensure the file path is correct and case-sensitive.

## How to List Blobs:
- Select option 2: "List Blobs" from the menu.
- The application will list all blobs in the specified container.

## How to Download a Blob:
- Select option 3: "Download a Blob" from the menu.
- Enter the name of the blob to download.
- Enter the destination path to save the downloaded file.

## How to Delete a Blob:
- Select option 4: "Delete a Blob" from the menu.
- Enter the name of the blob to delete.

## How to Change Container or Connection String:
- Open the `Program.cs` file in Visual Studio.
- Update the `containerName` and `connectionString` variables.

## Additional Requirements:
- IDE: Visual Studio 2022 Community
- .NET Version: .NET 8