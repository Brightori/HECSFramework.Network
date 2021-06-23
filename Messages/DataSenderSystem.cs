using Components;
using HECSFramework.Core;
using HECSFramework.Network;
using LiteNetLib;
using MessagePack;
using System;

namespace Systems
{
    public class DataSenderSystem : BaseSystem,  IDataSenderSystem
    {
        private ConnectionsHolderComponent connectionsHolder;

        public override void InitSystem()
        {
            connectionsHolder = Owner.GetHECSComponent<ConnectionsHolderComponent>();
        }

        public void SendCommandToAll<T>(T networkCommand, Guid address, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand
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

        private byte[] PackResolverContainer<T>(T command, Guid guid) where T: INetworkCommand
        {
            var resolverDataContainer = EntityManager.ResolversMap.GetCommandContainer(command, guid);
            return MessagePackSerializer.Serialize(resolverDataContainer);
        }

        private byte[] PackResolverContainer(ResolverDataContainer container)
        {
            return MessagePackSerializer.Serialize(container);
        }

        public void SendCommand<T>(Guid client, T networkCommand) where T : INetworkCommand
        {
            var peer = connectionsHolder.ClientConnectionsGUID[client];
            SendCommand(peer, client,  networkCommand);
        }

        public void SendCommand<T>(NetPeer peer, Guid address, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand
        {
            peer.Send(PackResolverContainer(networkCommand, address), deliveryMethod);
        }

        public void SyncSendComponentToAll(INetworkComponent component, Guid entityOfComponent, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable)
        {
            var resolverContainer = EntityManager.ResolversMap.GetComponentContainer(component);
            var data = PackResolverContainer(resolverContainer);

            SendAll(data, deliveryMethod);
        }
    }

    public interface IDataSenderSystem : ISystem
    {
        void SendCommandToAll<T>(T networkCommand, Guid address, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand;
        void SendCommand<T>(NetPeer peer, Guid address, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand;
        void SendCommand<T>(Guid client, T networkCommand) where T : INetworkCommand;
        void SyncSendComponentToAll(INetworkComponent component, Guid entityOfComponent, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable);
    }
}