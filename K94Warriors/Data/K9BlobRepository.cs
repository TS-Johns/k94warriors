﻿using System;
using System.IO;
using System.Threading.Tasks;
using K94Warriors.Core.AsyncExtensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace K94Warriors.Data
{
    public class K9BlobRepository : IBlobRepository
    {
        private readonly string _imageContainer;

        private readonly CloudStorageAccount _account;
        private readonly CloudBlobClient _client;

        public K9BlobRepository(string connectionString, string imageContainer)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudBlobClient();

            if (string.IsNullOrEmpty(_imageContainer))
                throw new ArgumentNullException("imageContainer");

            try
            {
                var container = _client.GetContainerReference(imageContainer);
                container.FetchAttributes();
            }
            catch (StorageException ex) { throw new ArgumentException("The provided image container does not exist."); }

            _imageContainer = imageContainer;
        }

        public async Task<T> GetImageAsync<T>(string id)
            where T : Stream, new()
        {
            var container = _client.GetContainerReference(_imageContainer);
            var blockBlob = container.GetBlockBlobReference(id);
            var memoryStream = new T();

            return await blockBlob.DownloadToStreamAsync(memoryStream);
        }

        public async Task InsertOrUpdateImageAsync(string id, Stream stream)
        {
            var container = _client.GetContainerReference(_imageContainer);
            var blockBlob = container.GetBlockBlobReference(id);
            await blockBlob.UploadFromStreamAsync(stream);
        }
    }
}