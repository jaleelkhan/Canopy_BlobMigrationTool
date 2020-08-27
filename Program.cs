using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace PushBlobToAWS_S3Bucket
{
    class Program
    {
        private static IConfiguration _iconfiguration;

        static void Main(string[] args)
        {
            Console.WriteLine("*********************************************PushBlobToS3Bucket*********************************************");
            Console.WriteLine("Latest 1");
            Console.WriteLine("*********************************************PushBlobToS3Bucket*********************************************");
            GetAppSettingsFile();
            string bucketName = _iconfiguration["BucketName"];
            IAmazonS3 client;
            client = new AmazonS3Client(_iconfiguration["awsAccessKeyId"], _iconfiguration["awsSecretAccessKey"], RegionEndpoint.USEast1);
            BlobData blob = new BlobData(_iconfiguration);
            //int Id = int.Parse(args[0].Trim());
            //string fileName = args[1];
            //string conType = args[2];
            //string path = args[3];
            //Console.WriteLine(args[0]);
            //Console.WriteLine(args[0].GetType());
            //Console.WriteLine(args[1]);
            //Console.WriteLine(args[2]);
            //Console.WriteLine(args[3]);
            Console.WriteLine("Latest 2");
            Console.WriteLine(args[0]);
            Console.WriteLine(Convert.ToInt32(args[0]).GetType());
            Console.WriteLine("**************");
            Console.WriteLine(args[0].GetType());
            Console.WriteLine(args[1]);
            Console.WriteLine(args[2]);
            Console.WriteLine(args[3]);
            BlobModel data = BlobData.GetBlobBytes(Convert.ToInt32(args[0]), args[1]);
            Console.WriteLine("**************");
            Console.WriteLine(data.Id);
            Console.WriteLine(data.BlobFileName);
            Console.WriteLine(data.BlobContent);
            Console.WriteLine(data.BlobPath);
            Console.WriteLine("**************");
            //BlobModel data = BlobData.GetBlobBytes(4, "Employee Handbook");
            MemoryStream memoryStream = new MemoryStream(data.BlobByte);

            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = memoryStream,
                ContentType = args[3],
                BucketName = bucketName,
                CannedACL = S3CannedACL.PublicRead,
                Key = args[4] // <-- in S3 key represents a path  


                //InputStream = memoryStream,
                //ContentType = data.BlobContent,
                //BucketName = bucketName,
                //CannedACL = S3CannedACL.PublicRead,
                //Key = data.BlobPath
            };

            client.PutObjectAsync(request);
        }
        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }
    }
}
