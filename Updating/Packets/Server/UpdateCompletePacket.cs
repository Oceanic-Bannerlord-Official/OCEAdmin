using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Updating
{
    [ProtoContract, PacketId(2)]
    public class UpdateCompletePacket : IServerDataPacket {}
}
