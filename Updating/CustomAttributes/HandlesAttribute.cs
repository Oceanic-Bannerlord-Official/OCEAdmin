using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    class HandlesAttribute : Attribute
    {
        public Type handles { get; set; }
        public HandlesAttribute(Type _handles)
        {
            this.handles = _handles;
        }
    }
}
