using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace OCEAdmin
{
    public interface IBanTransport
    {
        void OnRemoveBan(string id);
        void OnAddBan(Ban ban);
        void LoadList();
    }
}
