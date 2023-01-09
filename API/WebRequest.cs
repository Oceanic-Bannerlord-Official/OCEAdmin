using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMExtended.API
{
    public class WebRequest
    {
        public string Url { get; set; }

        public void ApplyFetchArgs(List<Tuple<string, string>> FetchData)
        {
            for (int i = 0; i < FetchData.Count; i++)
            {
                Tuple<string, string> FetchArg = FetchData[i];
                if (i == 0)
                {
                    Url = $"{Url}?{FetchArg.Item1}={FetchArg.Item2}";
                }
                else
                {
                    Url = $"{Url}&{FetchArg.Item1}={FetchArg.Item2}";
                }
            }
        }

        public void SetRequestURL(string Url)
        {
            this.Url = Url;
        }

        public string GetResponse()
        {
            WebClient client = new WebClient();
            return client.DownloadString(Url);
        }
    }
}
