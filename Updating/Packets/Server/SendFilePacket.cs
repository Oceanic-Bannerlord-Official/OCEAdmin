using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Updating
{
    [ProtoContract, PacketId(1)]
    public class SendFilePacket : IServerDataPacket
    {
        [ProtoMember(1)]
        public string fileName;

        [ProtoMember(2)]
        public byte[] data;
    }
}
