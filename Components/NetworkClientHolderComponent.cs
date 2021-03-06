using HECSFramework.Core;
using LiteNetLib;
using System;

namespace Components
{
    public enum NetWorkSystemState { Wait, Connect, BeforeSync, Sync, Disconnect, FailToConnect }
    public class NetworkClientHolderComponent : BaseComponent, IWorldSingleComponent
    {
        public NetPeer Client { get; private set; }
        public NetManager Manager { get; private set; }
        public EventBasedNetListener Listener { get; private set; }

        public NetWorkSystemState State { get; private set; }

        public NetworkClientHolderComponent()
        {
            Listener = new EventBasedNetListener();
            Listener.PeerConnectedEvent += ConnectedEvent;
            Listener.PeerDisconnectedEvent += DisconnectedEvent;
 
        }

        internal void ConnectTo(string host, int port, string serverKey, int maxConnectAttempts)
        {

            if (Manager == null)
            {
                Manager = new NetManager(Listener);
                Manager.MaxConnectAttempts = maxConnectAttempts;
                Manager.Start();
            }

            Client?.Disconnect();
            Client = Manager.Connect(host, port, serverKey);
            State = NetWorkSystemState.Connect;
        }
 
        private void DisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            State = disconnectInfo.Reason == DisconnectReason.ConnectionFailed ? NetWorkSystemState.FailToConnect : NetWorkSystemState.Disconnect;
        }

        private void ConnectedEvent(NetPeer peer)
        {
            State = NetWorkSystemState.BeforeSync;
        }
        internal void Sync(int disconnectTimeoutMs)
        {
            Manager.DisconnectTimeout = disconnectTimeoutMs;
            State = NetWorkSystemState.Sync;
        }

        internal void PollEvents()
        {
            Manager?.PollEvents();
        }

        internal void Disconnect()
        {
            if (State == NetWorkSystemState.Connect) State = NetWorkSystemState.FailToConnect;
            Client?.Disconnect();
        }

        public override string ToString()
        {
           if(Client != null)  return $"Address:{Client.EndPoint.Address}, Port:{Client.EndPoint.Port} ConnectionState:{Client.ConnectionState}";
            return "Connection not established";
        }

        internal void StartServer(int serverTickMilliseconds, int disconnectTimeout, int channels, bool extendedStatisticsEnabled)
        {
            if (Manager == null)
            {
                Manager = new NetManager(Listener);
            }
            else
            {
                HECSDebug.LogError("Attempt to re-create socket");
            }

            Manager.UpdateTime = serverTickMilliseconds;
            Manager.DisconnectTimeout = disconnectTimeout;
            Manager.ChannelsCount = (byte)channels;
            Manager.EnableStatistics = extendedStatisticsEnabled;

            Manager.Start();
        }
    }
}