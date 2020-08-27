using Canopy_BlobMigrationTool.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Canopy_BlobMigrationTool.DAL
{
    public class CanopyDAL
    {
        private string _connectionString;
        public CanopyDAL(IConfiguration iconfiguration)
        {
            // _connectionString = iconfiguration.GetConnectionString("Default");
            _connectionString = iconfiguration.GetConnectionString("Local");
        }
       

        public List<BlobModel> GetBlobList(string list)
        {
            var listBlobModel = new List<BlobModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetBlobDataToS3Bucket", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@template", SqlDbType.VarChar, 500).Value = list;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listBlobModel.Add(new BlobModel
                        {
                            Id = Convert.ToInt32(rdr[0]),
                            BlobByte = (byte[])rdr[1],
                            BlobContent = rdr[2].ToString(),
                            BlobFileName = rdr[3].ToString(),
                            BlobPath = rdr[4].ToString()
                        });


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listBlobModel;
        }
    }
}
