using Newtonsoft.Json;
using OCEAdmin.Core;
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
            this.Url = ConfigManager.Instance.GetConfig().APIUrl +  "set-nickname.php";
            this.Prepare();
        }
    }
}
