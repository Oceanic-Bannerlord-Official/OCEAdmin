using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Updating
{
    public class HashStorage
    {
        public List<HashedFile> HashMap = new List<HashedFile>();

        public void GenerateFromDir()
        {
            string[] files = Directory.GetFiles(Path.Combine(MPUtil.GetPluginDir(), "uniforms"), "*", SearchOption.AllDirectories);

            for(int i = 0; i < files.Length; i++)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(files[i]))
                    {
                        var hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                        var fileName = files[i];

                        fileName = fileName.Replace(MPUtil.GetPluginDir() + @"\", "");
                        fileName = fileName.Replace("/", @"\");

                        MPUtil.WriteToConsole(string.Format("File name: {0} / Hash: {1}", fileName, hash));
                        HashMap.Add(new HashedFile(fileName, hash));
                    }
                }
            }
        }
    }
}
