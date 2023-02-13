using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.API.Endpoints
{
    public class GetAdminsEndPoint : EndPoint
    {
        public GetAdminsEndPoint()
        {
            this.Url = "https://api.bannerlord.au/getadmins.php";
            this.Prepare();
        }
    }
}
