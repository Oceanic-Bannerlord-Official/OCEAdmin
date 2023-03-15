using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.API
{
    public class EndPoint
    {
        public string Url;
        private string _website = ConfigManager.Instance.GetConfig().APIUrl;
        public WebRequest request;
        public delegate void OnResponse(APIResponse response);
        public OnResponse OnResponseHandler;
        public APIResponse response;

        public virtual bool requireAuth => false;

        public EndPoint()
        {
            this.Prepare();
        }

        // Preparation of the web request so that it knows where to find the
        // endpoint.
        protected void Prepare()
        {
            this.request = new WebRequest();
            this.request.SetRequestURL(_website + Url);
        }

        public virtual void RequestRaw()
        {
            response = JsonConvert.DeserializeObject<APIResponse>(request.GetResponse());
        }

        // Gathering the information for the client to be sending to the server
        // including any auth tokens to verify steam account.
        public virtual void Request()
        {
            string response = request.GetResponse();

            // We only want to handle it if the request was successful.
            if (response != null)
            {
                APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(response);

                // InformationManager.DisplayMessage(new InformationMessage(apiResponse.status_message));

                this.OnResponseHandler(apiResponse);

                // Sometimes we don't want to use a response handler.
                this.response = apiResponse;
            }
        }

        // Appends added arguments to the fetch request.
        public void SetArgs(List<Tuple<string, string>> args)
        {
            this.request.ApplyFetchArgs(args);
        }
    }
}
