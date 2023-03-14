﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.API.Endpoints
{
    public class GetBansEndPoint : EndPoint
    {
        public GetBansEndPoint()
        {
            this.Url = "https://admin.bannerlord.au/api/getbans.php";
            this.Prepare();
        }
    }
}