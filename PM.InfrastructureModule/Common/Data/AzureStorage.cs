using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Entity.Shared;

namespace PM.InfrastructureModule.Common.Data
{
    /// <summary>
    /// Manage File Azure Storage
    /// </summary>
    [UsedImplicitly]
    public class AzureStorage
    {
        private static readonly IConfigurationRoot Config = AppSettingBuilder.GetAppSettings();

        /// <summary>
        /// Загрузка файлов Azure Storage
        /// </summary>
        public static async Task<AttachmentEntity> UploadFileAzureStorage(AttachmentEntity uploadedFile)
        {
            var fileNameBlobPath = $@"{uploadedFile.object_guid}/{uploadedFile.name}";
            var blockBlob = AzureStorage.GetCloudBlock(uploadedFile.container, fileNameBlobPath);
            await AzureStorage.UploadBlobFile(fileNameBlobPath, uploadedFile);

            uploadedFile.uri = blockBlob.Uri.ToString();
            return uploadedFile;
        }

        public static async Task UploadBlobFile(string blobPathName, AttachmentEntity uploadFile)
        {
            //client Azwure Storage
            var blobClient = CloudAzureStorageClient();

            // Retrieve a reference to a container.
            var container = blobClient.GetContainerReference(uploadFile.container);


            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
                {PublicAccess = BlobContainerPublicAccessType.Blob});

            // Retrieve reference to a blob named "myblob".
            var blockBlob = container.GetBlockBlobReference($@"{blobPathName}");

            await blockBlob.UploadFromStreamAsync(uploadFile.content);
        }

        /// <summary>
        /// Initial Azure Storage Client
        /// </summary>
        /// <returns>CloudBlobClient</returns>
        private static CloudBlobClient CloudAzureStorageClient()
        {
            var config = Config["AzureStorage"];
            var storageAccount = CloudStorageAccount.Parse(config);
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient;
        }

        /// <summary>
        /// Retrieve reference to a blob named "myblob".
        /// </summary>
        public static CloudBlockBlob GetCloudBlock(string blobContainerName, string fileNameBlobPath)
        {
            var blobClient = CloudAzureStorageClient();

            // Retrieve a reference to a container.
            var container = blobClient.GetContainerReference(blobContainerName);
            // Retrieve reference to a blob named "myblob".
            var blockBlob = container.GetBlockBlobReference(fileNameBlobPath);

            return blockBlob;
        }

        /// <summary>
        /// Del blob file from Azure Storage
        /// </summary>
        public static void DelBlobFile(string blobContainerName, string fileNameBlobPath)
        {
            var blobClient = CloudAzureStorageClient();
            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference(blobContainerName);

            // Retrieve reference to a blob named "myblob.txt".
            var blockBlob = container.GetBlockBlobReference(fileNameBlobPath);

            // Delete the blob.
            blockBlob.DeleteIfExistsAsync();
        }
    }
}