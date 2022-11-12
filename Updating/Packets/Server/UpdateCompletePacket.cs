using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    [ProtoContract, PacketId(3)]
    public class UpdateCompletePacket : IServerDataPacket { }
}
