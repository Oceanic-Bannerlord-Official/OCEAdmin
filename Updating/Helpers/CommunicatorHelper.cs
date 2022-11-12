using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OCEAdmin.Updating
{
    class CommunicatorHelper
    {
        public static void ClientSendPacket<T>(T dataPacket)
        {
            MethodInfo mi = typeof(ProtobufHelper).GetMethod("ProtoSerialize", BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(dataPacket.GetType());
            byte[] content = (byte[])mi.Invoke(null, new object[] { dataPacket });
            Packet packet = new Packet
            {
                Id = 1,
                Data = content
            };

            Console.WriteLine("Sended packet Content " + BitConverter.ToString(ProtobufHelper.ProtoSerialize<Packet>(packet)));
            byte[] serialized = ProtobufHelper.ProtoSerialize<Packet>(packet);
            UpdateManager.Instance.client.Send(new ArraySegment<byte>(serialized));
        }
    }
}
