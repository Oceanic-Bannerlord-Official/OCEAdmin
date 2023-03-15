using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.API.Endpoints
{
    public class DeleteBanEndPoint : EndPoint
    {
        public DeleteBanEndPoint()
        {
            this.Url = "delete-ban.php";
            this.Prepare();
        }
    }
}
