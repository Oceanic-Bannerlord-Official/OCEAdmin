using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.API.Endpoints
{
    public class AddBanEndPoint : EndPoint
    {
        public AddBanEndPoint()
        {
            this.Url = "https://admin.bannerlord.au/api/add-ban.php";
            this.Prepare();
        }
    }
}
