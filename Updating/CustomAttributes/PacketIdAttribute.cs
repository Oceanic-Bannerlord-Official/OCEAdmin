using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin.Updating
{
    class PacketIdAttribute : Attribute
    {
        public int id { get; set; }
        public PacketIdAttribute(int _id)
        {
            this.id = _id;
        }
    }
}
