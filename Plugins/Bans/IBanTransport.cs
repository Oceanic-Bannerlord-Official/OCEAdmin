using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace OCEAdmin.Plugins.Bans
{
    public interface IBanTransport
    {
        Task OnRemoveBan(string id);
        Task OnAddBan(Ban ban);
        Task Load();
    }
}
