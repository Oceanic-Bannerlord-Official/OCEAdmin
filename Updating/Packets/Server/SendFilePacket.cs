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
        [ProtoMember(3)]
        public string path;

        [ProtoMember(2)]
        public string fileName;

        [ProtoMember(3)]
        public byte[] data;
    }
}
