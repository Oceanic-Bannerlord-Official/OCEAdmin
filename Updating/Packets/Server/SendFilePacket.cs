using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Updating
{
    [ProtoContract, PacketId(2)]
    public class SendFilePacket : IServerDataPacket
    {
        [ProtoMember(1)]
        public string path;

        [ProtoMember(2)]
        public string fileName;

        [ProtoMember(3)]
        public byte[] data;

        [ProtoMember(4)]
        public bool transferDone;
    }
}
