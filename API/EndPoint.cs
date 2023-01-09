using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMExtended.API
{
    public class EndPoint
    {
        public string Url;

        protected WebRequest request;

        public delegate void OnResponse(APIResponse response);
        public OnResponse OnResponseHandler;

        public string result;

        public EndPoint()
        {
            result = null;
            this.Prepare();
        }

        // Preparation of the web request so that it knows where to find the
        // endpoint.
        protected void Prepare()
        {
            this.request = new WebRequest();
            this.request.SetRequestURL(Url);
        }

        // Gathering the information for the client to be sending to the server
        // including any auth tokens to verify steam account.
        public virtual void Request() 
        {
            APIResponse response = JsonConvert.DeserializeObject<APIResponse>(request.GetResponse());

            this.OnResponseHandler(response);

            result = response.data;
        }

        // Appends added arguments to the fetch request.
        public void SetArgs(List<Tuple<string, string>> args)
        {
            this.request.ApplyFetchArgs(args);
        }
    }
}
