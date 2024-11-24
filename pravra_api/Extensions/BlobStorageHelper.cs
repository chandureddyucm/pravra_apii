using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;
namespace pravra_api.Extensions
{
    public class BlobStorageHelper
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "gifts"; // Replace with your container name

        public BlobStorageHelper(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Ensure the container exists
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);

            // Upload the file to Blob Storage
            await blobClient.UploadAsync(fileStream, overwrite: true);

            // Return the Blob URL
            return blobClient.Uri.ToString();
        }
    }
}