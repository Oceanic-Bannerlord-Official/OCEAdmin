using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMExtended.API.Endpoints
{
    public class SetNicknameEndPoint : EndPoint
    {
        public SetNicknameEndPoint()
        {
            this.Url = "http://localhost/oceadmin-website/api/set-nickname.php";
            this.Prepare();
        }
    }
}
