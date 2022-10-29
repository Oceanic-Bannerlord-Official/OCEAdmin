using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class HashStorage
    {
        public List<HashedFile> HashMap = new List<HashedFile>();

        public void GenerateFromDir()
        {
            string[] files = Directory.GetFiles(Path.Combine(OCEAdminSubModule.baseDir, "uniforms"), "*", SearchOption.AllDirectories);

            for(int i = 0; i < files.Length; i++)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(files[i]))
                    {
                        var hash = md5.ComputeHash(stream);
                        MPUtil.WriteToConsole(BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower());
                        HashMap.Add(new HashedFile(files[i], BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower()));
                    }
                }
            }
        }
    }
}
