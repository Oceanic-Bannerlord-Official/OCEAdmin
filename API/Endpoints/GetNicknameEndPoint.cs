using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMExtended.API.Endpoints
{
    public class GetNicknameEndPoint : EndPoint
    {
        public GetNicknameEndPoint()
        {
            this.Url = "http://localhost/oceadmin-website/api/get-nickname.php";
            this.Prepare();
        }
    }
}
