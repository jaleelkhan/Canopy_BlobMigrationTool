using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Canopy_BlobMigrationTool.Model;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Canopy_BlobMigrationTool.BAL
{
    public class CanopyBAL
    {
        public  IConfiguration _iconfiguration;
        private string _connectionString;
        public CanopyBAL(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        public  void UploadToS3(List<BlobModel> blobList)
        {
            string bucketName = _iconfiguration["BucketName"];
            IAmazonS3 client;
            client = new AmazonS3Client(_iconfiguration["awsAccessKeyId"], _iconfiguration["awsSecretAccessKey"], RegionEndpoint.USEast1);
            MemoryStream memoryStream;
            string fileName, path, conType;
            int Id;
            foreach (var item in blobList)
            {
                memoryStream = new MemoryStream(item.BlobByte);
                Id = item.Id;
                fileName = item.BlobFileName;
                path = item.BlobPath;
                conType = GetContentType(item.BlobContent);

                //Start Calling Child application to push blobs to S3 Bucket
                Process p = new Process();
                p.StartInfo.FileName = @"D:\Canopy\Apps\Canopy_BlobMigrationTool\Canopy_BlobMigrationTool\EXE\PushBlobToAWS_S3Bucket.exe";
                //p.StartInfo.Arguments = "/ BlobId:" + Id.ToString().Trim() + " / BlobFileName:" + fileName + " / BlobContent:" + conType + " / BlobPath:" + path;
                p.StartInfo.Arguments = " " + Id.ToString().Trim() + " " + fileName + " " + conType + " " + path;


                p.Start();
                p.WaitForExit();
                var exitCode = p.ExitCode;
                p.Close();
                //End Calling Child application to push blobs to S3 Bucket

                //PutObjectRequest request = new PutObjectRequest()
                //{
                //    InputStream = memoryStream,
                //    ContentType = conType,
                //    BucketName = bucketName,
                //    CannedACL = S3CannedACL.PublicRead,
                //    Key = path // <-- in S3 key represents a path  
                //};
                //client.PutObjectAsync(request);
            }

        }


        private  string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
