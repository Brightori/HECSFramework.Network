using HECSFramework.Core;
using LiteNetLib;
using System;
using System.Collections.Concurrent;

namespace Components
{
    [Documentation(Doc.Network, "This component holds information about any network connections and hold litenetlib netmanager")]
    public partial class ConnectionsHolderComponent : BaseComponent, IWorldSingleComponent
    {
        public ConcurrentDictionary<Guid, DateTime> ClientConnectionsTimes { get; } = new ConcurrentDictionary<Guid, DateTime>();
        public ConcurrentDictionary<Guid, NetPeer> ClientConnectionsGUID { get; } = new ConcurrentDictionary<Guid, NetPeer>();
        public ConcurrentDictionary<int, NetPeer> ClientConnectionsID { get; } = new ConcurrentDictionary<int, NetPeer>();
        public ConcurrentDictionary<Guid, World> EntityToWorldConnections { get; } = new ConcurrentDictionary<Guid, World>();
        public ConcurrentDictionary<NetPeer, World> PeerToWorldConnections { get; } = new ConcurrentDictionary<NetPeer, World>();
        
        public NetManager NetManager { get; set; }
        public EventBasedNetListener Listener { get; } = new EventBasedNetListener();

        public int SyncIndex;

        public void RegisterClient(IEntity entity, NetPeer netPeer, World world)
        {
            var time = Owner.World.GetSingleComponent<TimeComponent>().CurrentTime;
            ClientConnectionsTimes.TryAdd(entity.GUID, time);
            ClientConnectionsGUID.TryAdd(entity.GUID, netPeer);
            ClientConnectionsID.TryAdd(netPeer.EndPoint.GetHashCode(), netPeer);
            EntityToWorldConnections.TryAdd(entity.GUID, world);
            PeerToWorldConnections.TryAdd(netPeer, world);
        }

        public bool TryGetClientByConnectionID(int connectionID, out Guid clientGuid)
        {
            foreach (var connectionInfo in ClientConnectionsGUID)
            {
                if (connectionInfo.Value.EndPoint.GetHashCode() == connectionID)
                {
                    clientGuid = connectionInfo.Key;
                    return true;
                }
            }

            clientGuid = default;
            return false;
        }
    }
}