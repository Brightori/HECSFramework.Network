using Components;
using HECSFramework.Core;
using HECSFramework.Network;
using LiteNetLib;
using MessagePack;
using System;

namespace Systems
{
    public partial class DataSenderSystem : BaseSystem,  IDataSenderSystem
    {
        private ConnectionsHolderComponent connectionsHolder;

        public override void InitSystem()
        {
            connectionsHolder = Owner.GetHECSComponent<ConnectionsHolderComponent>();
        }

        public void SendCommandToAll<T>(T networkCommand, Guid address, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, address);

            foreach (var kvp in connectionsHolder.ClientConnectionsGUID)
                kvp.Value.Send(data, deliveryMethod);
        }
        
        private void SendAll(byte[] data, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable)
        {
            foreach (var kvp in connectionsHolder.ClientConnectionsGUID)
                kvp.Value.Send(data, deliveryMethod);
        }

        private byte[] PackResolverContainer<T>(T command, Guid guid) where T: INetworkCommand, IData
        {
            var resolverDataContainer = EntityManager.ResolversMap.GetCommandContainer(command, guid);
            return MessagePackSerializer.Serialize(resolverDataContainer);
        }

        private byte[] PackResolverContainer(ResolverDataContainer container)
        {
            return MessagePackSerializer.Serialize(container);
        }

        public void SendCommand<T>(Guid client, T networkCommand) where T : INetworkCommand, IData
        {
            var peer = connectionsHolder.ClientConnectionsGUID[client];
            SendCommand(peer, client,  networkCommand);
        }

        public void SendCommand<T>(NetPeer peer, Guid address, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, address);
            peer.Send(data, deliveryMethod);
        }

        public void SyncSendComponentToAll(INetworkComponent component, Guid entityOfComponent, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable)
        {
            var resolverContainer = EntityManager.ResolversMap.GetComponentContainer(component);
            var data = PackResolverContainer(resolverContainer);

            SendAll(data, deliveryMethod);
        }
    }

    public partial interface IDataSenderSystem : ISystem
    {
        void SendCommandToAll<T>(T networkCommand, Guid address, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData;
        void SendCommand<T>(NetPeer peer, Guid address, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData;
        void SendCommand<T>(Guid client, T networkCommand) where T : INetworkCommand, IData;
        void SyncSendComponentToAll(INetworkComponent component, Guid entityOfComponent, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable);
    }

    [MessagePack.MessagePackObject]
    public struct ByteData
    {
        [Key(0)]
        public byte LastIndex; 
        [Key(1)]
        public byte PartNumber; 
        [Key(2)]
        public byte OverallParts;
        [Key(3)]
        public byte _0;
        [Key(4)]
        public byte _1;
        [Key(5)]
        public byte _2;
        [Key(6)]
        public byte _3;
        [Key(8)]
        public byte _4;
        [Key(9)]
        public byte _5;
        [Key(10)]
        public byte _6;
        [Key(11)]
        public byte _7;
        [Key(12)]
        public byte _8;
        [Key(13)]
        public byte _9;
        [Key(14)]
        public byte _10;
        [Key(15)]
        public byte _11;
        [Key(16)]
        public byte _12;
        [Key(17)]
        public byte _13;
        [Key(18)]
        public byte _14;
        [Key(19)]
        public byte _15;
        [Key(20)]
        public int Index;
    }
}