using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.API.Endpoints
{
    public class GetBanChecksumEndPoint : EndPoint
    {
        public GetBanChecksumEndPoint()
        {
            this.Url = "get-ban-checksum.php";
            this.Prepare();
        }
    }
}
