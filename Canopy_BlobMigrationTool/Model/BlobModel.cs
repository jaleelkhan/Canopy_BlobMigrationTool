using System;
using System.Collections.Generic;
using System.Text;

namespace Canopy_BlobMigrationTool.Model
{
    public class BlobModel
    {
        public int Id { get; set; }
        public byte[] BlobByte { get; set; }
        public string BlobContent { get; set; }
        public string BlobFileName { get; set; }
        public string BlobPath { get; set; }
    }
}
