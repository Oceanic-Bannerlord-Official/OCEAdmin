using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class HashedFile
    {
        public string file;
        public string checksum;

        public HashedFile(string file, string checksum)
        {
            this.file = file;
            this.checksum = checksum;
        }
    }
}
