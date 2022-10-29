using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin.Updating
{
    [ProtoContract, PacketId(1)]
    class SendFileListPacket : IDataPacket
    {
        [ProtoMember(1)]
        public RequestType requestType;

        [ProtoMember(2)]
        public List<HashedFile> hashedFiles;
    }

    public enum RequestType
    {
        Checksum,
        Download
    }
}
