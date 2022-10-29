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
        List<Tuple<string, string>> HashMap = new List<Tuple<string, string>>();

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
                        HashMap.Add(Tuple.Create(files[i], BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower()));
                    }
                }
            }
        }

        public void ToFile()
        {
            // Reset the previous checksum.
            if (File.Exists(Path.Combine(OCEAdminSubModule.baseDir, "checksum.json")))
            {
                File.Delete(Path.Combine(OCEAdminSubModule.baseDir, "checksum.json"));
            }

            string jsonData = JsonConvert.SerializeObject(HashMap, Formatting.None);
            File.WriteAllText(Path.Combine(OCEAdminSubModule.baseDir, "checksum.json"), jsonData);
        }
    }
}
