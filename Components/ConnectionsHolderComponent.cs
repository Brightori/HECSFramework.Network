using HECSFramework.Core;
using LiteNetLib;
using System;
using System.Collections.Concurrent;

namespace Components
{
    [Documentation("Network", "Компонент который хранит все возможные связки подключений")]
    public partial class ConnectionsHolderComponent : BaseComponent
    {
        public ConcurrentDictionary<Guid, DateTime> ClientConnectionsTimes { get; } = new ConcurrentDictionary<Guid, DateTime>();
        public ConcurrentDictionary<Guid, NetPeer> ClientConnectionsGUID { get; } = new ConcurrentDictionary<Guid, NetPeer>();
        public ConcurrentDictionary<int, NetPeer> ClientConnectionsID { get; } = new ConcurrentDictionary<int, NetPeer>();
        public ConcurrentDictionary<int, ConcurrentDictionary<int, NetPeer>> WorldToPeerClients { get; } = new ConcurrentDictionary<int, ConcurrentDictionary<int, NetPeer>>();
        public ConcurrentDictionary<Guid, int> EntityToWorldConnections { get; } = new ConcurrentDictionary<Guid, int>();
        public NetManager NetManager { get; set; }
        public EventBasedNetListener Listener { get; } = new EventBasedNetListener();

        public int SyncIndex;

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