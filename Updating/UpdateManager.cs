using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OCEAdmin.Updating
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
            set { }
        }

        public Telepathy.Client client;
        private bool _isTicking = true;
        private bool _sentChecksum = false;

        public void Initialise() {
            if (ConfigManager.Instance.UniformSettings.UpdateFiles)
            {
                InitialiseRegistries();

                client = new Telepathy.Client(1024 * 1024);
                client.OnData = OnData;
                client.OnDisconnected = Finish;

                Telepathy.Log.Info = MPUtil.WriteToConsole;
                Telepathy.Log.Error = MPUtil.WriteToConsole;
                Telepathy.Log.Warning = MPUtil.WriteToConsole;

                MPUtil.WriteToConsole("Connecting to the uniform update server...");
                client.Connect("localhost", 25565);

                System.Threading.Thread.Sleep(100);
                Thread thr = new Thread(() => this.Tick());
                thr.Start();
            } else
            {
                Finish();
            }
        }

        private void Tick()
        {
            while (_isTicking)
            {
                if (client.Connected && !_sentChecksum)
                {
                    MPUtil.WriteToConsole("Connected to the uniform update server.");
                    SendChecksum();
                }

                client.Tick(60);
                Thread.Sleep(1000 / 60);
            }
            
            // Close the socket if we no longer want to tick.
            if(client.Connected)
            {
                client.Disconnect();
                MPUtil.WriteToConsole("Uniform update socket closed.");
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

        private void InitialiseRegistries()
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

        private void SendChecksum()
        {
            hashStorage = new HashStorage();
            hashStorage.GenerateFromDir();

            MPUtil.WriteToConsole("Generating checksum list for update server...");

            List<HashedFile> hashMap = new List<HashedFile>();
            hashMap = hashStorage.HashMap;

            SendFileListPacket sendFileListPacket = new SendFileListPacket(hashMap);
            sendFileListPacket.requestType = RequestType.Checksum;

            MPUtil.WriteToConsole("Sending checksum packet...");

            CommunicatorHelper.ClientSendPacket(sendFileListPacket);

            MPUtil.WriteToConsole("Checksum sent!");
            _sentChecksum = true;
        }

        public void ReceiveFile(SendFilePacket packet)
        {
            MPUtil.WriteToConsole(string.Format("Received file: {0} / {1}", packet.path, packet.fileName));

            string filePath = Path.Combine(MPUtil.GetPluginDir(), packet.path);

            Directory.CreateDirectory(filePath);
            File.WriteAllBytes(Path.Combine(filePath, packet.fileName), packet.data);

            if(packet.transferDone)
            {
                Finish();
            }
        }

        // We're all up to date or were unable to reach the update server.
        // Let's load the uniform manager with the clan data we have.
        public void Finish()
        {
            _isTicking = false;
            MPUtil.WriteToConsole("Loading uniforms from storage...");

            UniformManager.Instance.LoadClans();
            UniformManager.Instance.LoadUniforms();
        }
    }
}
