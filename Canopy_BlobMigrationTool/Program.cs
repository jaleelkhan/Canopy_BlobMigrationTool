using Canopy_BlobMigrationTool.BAL;
using Canopy_BlobMigrationTool.DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace Canopy_BlobMigrationTool
{
    class Program
    {
        private static IConfiguration _iconfiguration;
        public static string res;
        static void Main(string[] args)
        {
            Console.WriteLine("Blob Migration tool started running");
            res = GetMapping();
            GetAppSettingsFile();
            GetBlobList(res);
            Console.WriteLine("Blob Migration tool pushed the blobs to AWS Bucket");
        }
        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }
       
        static string GetMapping()
        {
            StringBuilder mappingList = new StringBuilder();
            mappingList.Append("CompanyDocs;");
            mappingList.Append("Photos;");
            mappingList.Append("Notes;");
            mappingList.Append("HCompany;");

            return mappingList.ToString();
        }
        public static void GetBlobList(string res)
        {
            var canopyDAL = new CanopyDAL(_iconfiguration);
            var listBlobModel = canopyDAL.GetBlobList(res);
            var canopyBAL = new CanopyBAL(_iconfiguration);
            canopyBAL.UploadToS3(listBlobModel);
            Console.WriteLine("Press any key to stop running....");
            Console.ReadKey();
        }
    }
}
