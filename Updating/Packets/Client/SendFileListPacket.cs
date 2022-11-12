using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    [ProtoContract, PacketId(1)]
    class SendFileListPacket : IDataPacket
    {
        [ProtoMember(1)]
        public RequestType requestType;

        [ProtoMember(2)]
        public string[] files;

        [ProtoMember(3)]
        public string[] checksums;

        public SendFileListPacket(List<HashedFile> hashedFiles)
        {
            files = new string[hashedFiles.Count];
            checksums = new string[hashedFiles.Count];

            int i = 0;
            foreach(HashedFile file in hashedFiles)
            {
                MPUtil.WriteToConsole(string.Format("Attempting to load - File name: {0} / Hash: {1}", file.file, file.checksum));

                files[i] = file.file;
                checksums[i] = file.checksum;
                i++;
            }
        }
    }

    public enum RequestType
    {
        Checksum,
        Download
    }
}
