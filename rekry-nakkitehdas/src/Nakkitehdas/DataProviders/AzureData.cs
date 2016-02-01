using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Nakkitehdas.ViewModels;

namespace Nakkitehdas.DataProviders
{
    public class AzureData : IAzureData
    {
        public List<IGrouping<int, ItemModel>> GetAzureBlobs()
        {

            var container = GetAzureContainer();

            List<ItemModel> model = new List<ItemModel>();

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                bool hasSubDir = false;

                //from root
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    hasSubDir = Blob_Has_SubDirectories(item);

                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    //add directory
                    model.Add(new ItemModel()
                    {
                        IsFile = false,
                        IsFolder = true,
                        Name = directory.Prefix,
                        ParentId = item.Parent != null ? item.Parent.Uri.Segments.Count() - 2 : 0
                    });
                }

                //root files
                else if (item.GetType() == typeof(CloudBlockBlob))
                {
                    var blobFileName = item.Uri.Segments.Last();

                    //add file
                    model.Add(new ItemModel()
                    {
                        IsFile = true,
                        IsFolder = false,
                        Name = blobFileName,
                        ParentId = item.Parent != null ? item.Parent.Uri.Segments.Count() - 2 : 0
                    });
                }

                //has subfolders
                if (hasSubDir)
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;
                    IEnumerable<IListBlobItem> blops = directory.ListBlobs();
                    var listblob = blops;
                    model.AddRange(Add_Subdirectories_And_Files_To_Model(listblob));
                }
            }
            //this may be needed later on
            List<IGrouping<int,ItemModel>> groupedItems = model.GroupBy(m => m.ParentId).ToList();

            return groupedItems;
        }

        public CloudBlobContainer GetAzureContainer()
        {

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=nakkitehdas;AccountKey=xBRoHzBWZuRSBK9lTZtZYqumA+C95H1M1skHtM1vTC1GDjSuXOOzpwJenvXHb8YReDdFzc9yggEPg1ApZ3rN3A==";

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("juuri");

            return container;
        }

        //this is way too long should be refactored
        public List<ItemModel> Add_Subdirectories_And_Files_To_Model(IEnumerable<IListBlobItem> blops)
        {

            List<ItemModel> model = new List<ItemModel>();

            //Check files  
            foreach (var blop in blops)
            {
                var iteratedBlob = blop;

                if (blop.GetType() == typeof(CloudBlobDirectory))
                {
                    int pid = 0;

                    CloudBlobDirectory blobDir = (CloudBlobDirectory)iteratedBlob;

                    if (blobDir.Parent != null)
                    {
                        pid = blobDir.Parent.Uri.Segments.Count() - 2;
                    }

                    //add directory
                    model.Add(new ItemModel()
                    {
                        IsFile = false,
                        IsFolder = true,
                        Name = blobDir.Prefix,
                        ParentId = pid
                    });
                }
                //is a file
                if (blop.GetType() == typeof(CloudBlockBlob))
                {
                    int id = 0;

                    var blobFile = iteratedBlob as CloudBlockBlob;
                    //should have parents because is a subfolder
                    if (blobFile.Parent != null)
                    {
                        id = blobFile.Parent.Uri.Segments.Count() - 2;
                    }

                    string name = blobFile.Name;

                    //add file
                    model.Add(new ItemModel()
                    {
                        IsFile = true,
                        IsFolder = false,
                        Name = name,
                        ParentId = id
                    });
                }

                if (Blob_Has_SubDirectories(blop))
                {

                    List<ItemModel> subs = new List<ItemModel>();

                    CloudBlobDirectory directory = (CloudBlobDirectory)iteratedBlob;
                    IEnumerable<IListBlobItem> subBlops = directory.ListBlobs();

                    subs = Add_Subdirectories_And_Files_To_Model(subBlops);

                    model.AddRange(subs);
                }
            }
            return model;
        }

        public bool Blob_Has_SubDirectories(IListBlobItem item)
        {
            return item.GetType() == typeof(CloudBlobDirectory) ? true : false;
        }


    }
}
