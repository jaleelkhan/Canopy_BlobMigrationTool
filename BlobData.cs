using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PushBlobToAWS_S3Bucket
{
    public class BlobData
    {
        private static string _connectionString;
        public BlobData(IConfiguration iconfiguration)
        {
            // _connectionString = iconfiguration.GetConnectionString("Default");
            _connectionString = iconfiguration.GetConnectionString("Local");
        }
        public static BlobModel GetBlobBytes(int id, string fileName)
        {
            string res = GetMapping();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var listBlobModel = new BlobModel();
                    SqlCommand cmd = new SqlCommand("GetBlobDataToS3BucketById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BlbId", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@BlbFileName", SqlDbType.VarChar, 500).Value = fileName;
                    cmd.Parameters.Add("@template", SqlDbType.VarChar, 500).Value = res;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listBlobModel = new BlobModel
                        {
                            Id = Convert.ToInt32(rdr[0]),
                            BlobByte = (byte[])rdr[1],
                            BlobContent = rdr[2].ToString(),
                            BlobFileName = rdr[3].ToString(),
                            BlobPath = rdr[4].ToString()
                        };

                    }
                    return listBlobModel;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetMapping()
        {
            StringBuilder mappingList = new StringBuilder();
            mappingList.Append("CompanyDocs;");
            mappingList.Append("Photos;");
            mappingList.Append("Notes;");
            mappingList.Append("HCompany;");

            return mappingList.ToString();
        }
    }
}
