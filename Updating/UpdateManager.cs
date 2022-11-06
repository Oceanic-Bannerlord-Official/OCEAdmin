using OCEAdmin.Updating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class UpdateManager
    {
        public HashStorage hashStorage;

        private static UpdateManager instance;
        public static UpdateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UpdateManager();
                }
                return instance;
            }
        }

        public static Telepathy.Client client;
        private bool _isTicking = true;

        public UpdateManager()
        {
            InitialiseRegistries();

            Telepathy.Client _client = new Telepathy.Client(1024 * 1024);
            _client = new Telepathy.Client(1024 * 1024);

            _client.OnConnected = SendChecksum;
            _client.OnData = OnData;
            _client.OnDisconnected = Finish;

            Telepathy.Log.Info = MPUtil.WriteToConsole;
            Telepathy.Log.Error = MPUtil.WriteToConsole;
            Telepathy.Log.Warning = MPUtil.WriteToConsole;

            MPUtil.WriteToConsole("Connecting...");
            _client.Connect("localhost", 1337);
            client = _client;

            System.Threading.Thread.Sleep(100);
            Thread thr = new Thread(() => this.Tick());
            thr.Start();
        }

        public void Tick()
        {
            while (_isTicking)
            {
                client.Tick(60);
                Thread.Sleep(1000 / 60);
            }
        }

        public static void OnData(ArraySegment<byte> message)
        {
            byte[] serializedPacket = message.Skip(message.Offset).Take(message.Count).ToArray();
            Packet packet = ProtobufHelper.ProtoDeserialize<Packet>(serializedPacket);
            Type packetType = NetworkPacketRegistry.storage[packet.Id];
            MethodInfo methodInfo = typeof(ProtobufHelper).GetMethod("ProtoDeserialize", BindingFlags.Public | BindingFlags.Static);
            IServerDataPacket dataPacket = (IServerDataPacket)methodInfo.MakeGenericMethod(packetType).Invoke(null, new object[] { packet.Data });
            HandlerRegistry.storage[packetType].Handle(dataPacket);
        }

        public void InitialiseRegistries()
        {
            foreach (Type networkPacketType in Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IServerDataPacket))))
            {
                PacketIdAttribute attribute = networkPacketType.GetCustomAttribute<PacketIdAttribute>();
                NetworkPacketRegistry.storage[attribute.id] = networkPacketType;
            }
            foreach (Type packetHandlerType in Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IServerPacketHandler))))
            {
                HandlesAttribute attribute = packetHandlerType.GetCustomAttribute<HandlesAttribute>();
                HandlerRegistry.storage[attribute.handles] = (IServerPacketHandler)Activator.CreateInstance(packetHandlerType);
            }
        }

        public void SendChecksum()
        {
            hashStorage = new HashStorage();
            hashStorage.GenerateFromDir();

            SendFileListPacket sendFileListPacket = new SendFileListPacket();
            sendFileListPacket.requestType = RequestType.Checksum;
            sendFileListPacket.hashedFiles = hashStorage.HashMap;

            CommunicatorHelper.ClientSendPacket(sendFileListPacket);
        }

        public void ReceiveFile(SendFilePacket packet)
        {
            string file = Path.Combine(OCEAdminSubModule.baseDir, Path.Combine(packet.path, packet.fileName));

            Directory.CreateDirectory(packet.path);
            File.WriteAllBytes(file, packet.data);
        }

        public List<HashedFile> GetRequiredUpdates(List<HashedFile> localCache, List<HashedFile> serverCache)
        {
            foreach (HashedFile serverFile in serverCache)
            {
                foreach (HashedFile localFile in localCache)
                {
                    // Find the file in the array, if it's not the same checksum we don't need to update.
                    if (localFile.file == serverFile.file && localFile.checksum == serverFile.checksum)
                    {
                        // We don't need to deal with this file.
                        serverCache.Remove(serverFile);
                    }
                }
            }

            return serverCache;
        }

        // We're all up to date or were unable to reach the update server.
        // Let's load the uniform manager with the clan data we have.
        public void Finish()
        {
            UniformManager.Instance.Populate();
            _isTicking = false;
        }
    }
}
