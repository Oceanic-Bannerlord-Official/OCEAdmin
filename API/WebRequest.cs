using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OCEAdmin.API
{
    public class WebRequest
    {
        private EndPoint _endpoint;
        private string _args;
        private Dictionary<EndPoint, string> _urls = new Dictionary<EndPoint, string>()
        {
            { EndPoint.AddBan, "add-ban.php" },
            { EndPoint.DeleteBan, "delete-ban.php" },
            { EndPoint.GetAdmins, "getadmins.php" },
            { EndPoint.GetBanChecksum, "get-ban-checksum.php" },
            { EndPoint.GetBans, "getbans.php" },
        };

        public Action OnError;

        public delegate Task OnResponse(APIResponse response);
        public OnResponse OnResponseHandler;

        public WebRequest() { }

        public WebRequest(EndPoint endpoint)
        {
            SetEndpoint(endpoint);
        }

        public void SetArgs(List<Tuple<string, string>> FetchData)
        {
            for (int i = 0; i < FetchData.Count; i++)
            {
                Tuple<string, string> FetchArg = FetchData[i];
                if (i == 0)
                {
                    _args = $"{_args}?{FetchArg.Item1}={FetchArg.Item2}";
                }
                else
                {
                    _args = $"{_args}&{FetchArg.Item1}={FetchArg.Item2}";
                }
            }
        }

        public void SetEndpoint(EndPoint endPoint)
        {
            _endpoint = endPoint;   
        }

        public string GetRequestURL()
        {
            if (_urls.ContainsKey(_endpoint))
            {
                string endpointUrl = _urls[_endpoint];

                return Config.Get().APIUrl + endpointUrl + _args;
            }

            throw new InvalidOperationException("Invalid endpoint.");
        }

        private async Task<string> GetResponseRaw()
        {
            try
            {
                WebClient client = new WebClient();
                Uri address = new Uri(GetRequestURL());

                return await client.DownloadStringTaskAsync(address);
            }
            catch (WebException ex)
            {
                if (OnError == null)
                {
                    this.DefaultError(ex);
                }
                else
                {
                    OnError();
                }

                return null;
            }
        }

        public async Task Request()
        {
            string response = await GetResponseRaw();

            if(response != null && this.OnResponseHandler != null)
            {
                try
                {
                    APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(response);
                    await this.OnResponseHandler(apiResponse);
                } 
                catch(Exception error)
                {
                    MPUtil.WriteToConsole("Error with deserializing API packet. " + error.ToString(), true);
                }
            }
        }

        public void DefaultError(WebException ex)
        {
            var error = $"An error occurred while retrieving the OCEAdmin API. Error: {ex.Message}";

            MPUtil.WriteToConsole(error, true);
        }
    }

    public enum EndPoint
    {
        AddBan,
        DeleteBan,
        GetAdmins,
        GetBanChecksum,
        GetBans
    }
}
