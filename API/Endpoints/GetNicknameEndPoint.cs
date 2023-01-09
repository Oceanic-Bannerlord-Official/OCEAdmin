using Newtonsoft.Json;
using OCEAdmin.Core;
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
            this.Url = ConfigManager.Instance.GetConfig().APIUrl + "get-nickname.php";
            this.Prepare();
        }
    }
}
