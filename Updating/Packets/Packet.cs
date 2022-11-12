using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    [ProtoContract]
    class Packet
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public byte[] Data { get; set; }
    }
}
