using Components;
using HECSFramework.Core;
using HECSFramework.Network;
using LiteNetLib;
using MessagePack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Systems
{
    [Documentation("Network", "Эта система отвечает за пересылку данных по сети")]
    public partial class DataSenderSystem : BaseSystem
    {
        private readonly ConcurrentBag<(Guid client, NetPeer peer)> aliveConnections = new ConcurrentBag<(Guid client, NetPeer peer)>();
        private ConnectionsHolderComponent connectionsHolder;

        public override void InitSystem()
        {
            connectionsHolder = Owner.GetHECSComponent<ConnectionsHolderComponent>();
        }

        /// <summary>
        /// это рассылка общей команды (которую получают все глобальные подписанты) по всем клиентам
        /// </summary>
        public void SendCommandToAllClients<T>(T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, Guid.Empty);
            PopulateAliveConnections();
            foreach (var kvp in aliveConnections)
            {
                kvp.peer.Send(data, deliveryMethod);
                CommandStatistics(networkCommand);
            }
        }

        /// <summary>
        /// это рассылка общей команды по всем клиентам, но получит её конкретная ентити айди которой указан как таргет
        /// </summary>
        public void SendCommandToAllClients<T>(T networkCommand, Guid target, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, target);
            PopulateAliveConnections();
            
            foreach (var kvp in aliveConnections)
            {
                kvp.peer.Send(data, deliveryMethod);
                CommandStatistics(networkCommand);
            }
        }

        /// <summary>
        /// это рассылка общей команды (которую получают все глобальные подписанты) по всем клиентам, кроме указанного в исключении
        /// </summary>
        public void SendCommandToFilteredClients<T>(T networkCommand, DeliveryMethod deliveryMethod, Guid except) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, Guid.Empty);

            PopulateAliveConnections();
            foreach (var kvp in aliveConnections)
            {
                if (kvp.client == except) continue;

                kvp.peer.Send(data, deliveryMethod);
                CommandStatistics(networkCommand);
            }
        }

        /// <summary>
        /// это рассылка общей команды по всем клиентам, кроме указанных в исключении, получит её конкретная ентити айди которой указан как таргет
        /// </summary>
        public void SendCommandToFilteredClients<T>(T networkCommand, Guid address, DeliveryMethod deliveryMethod,  Guid except) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, address);

            PopulateAliveConnections();
            foreach (var kvp in aliveConnections)
            {
                if (kvp.client == except) continue;

                kvp.peer.Send(data, deliveryMethod);
                CommandStatistics(networkCommand);
            }
        }

        /// <summary>
        /// отсылка команды определенному клиенту
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="networkCommand"></param>
        /// <param name="deliveryMethod"></param>
        public void SendCommand<T>(Guid client, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var peer = connectionsHolder.ClientConnectionsGUID[client];
            SendCommand(peer, networkCommand, deliveryMethod);
            CommandStatistics(networkCommand);
        }

        /// <summary>
        /// отсылка команды конкретной  ентити (adress), и  определенному клиенту (client), 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="networkCommand"></param>
        /// <param name="deliveryMethod"></param>
        public void SendCommand<T>(Guid client, Guid adress, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var peer = connectionsHolder.ClientConnectionsGUID[client];
            SendCommand(peer, adress, networkCommand, deliveryMethod);
            CommandStatistics(networkCommand);
        }

        /// <summary>
        /// отсылка команды определенному клиенту
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="networkCommand"></param>
        /// <param name="deliveryMethod"></param>
        public void SendCommand<T>(NetPeer peer, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, Guid.Empty);
            peer.Send(data, deliveryMethod);
            CommandStatistics(networkCommand);
        }

        /// <summary>
        ///отсылка команды конкретной ентити(adress), и определенному клиенту(client), 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="networkCommand"></param>
        /// <param name="deliveryMethod"></param>
        public void SendCommand<T>(NetPeer client, Guid address, T networkCommand, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) where T : INetworkCommand, IData
        {
            var data = PackResolverContainer(networkCommand, address);
            client.Send(data, deliveryMethod);
            CommandStatistics(networkCommand);
        }

        public void SendComponentToAll(INetworkComponent component, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable)
        {
            var resolverContainer = EntityManager.ResolversMap.GetComponentContainer(component);
            var data = PackResolverContainer(resolverContainer);
            PopulateAliveConnections();
            foreach (var kvp in aliveConnections)
            {
                kvp.peer.Send(data, deliveryMethod);
                ComponentStatistics(component);
            }
        }

        public void SendComponentToOwner(INetworkComponent component, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered)
        {
            var client = component.Owner.GetClientIDHolderComponent().ClientID;
            var peer = connectionsHolder.ClientConnectionsGUID[client];
            var resolverContainer = EntityManager.ResolversMap.GetComponentContainer(component);
            var data = PackResolverContainer(resolverContainer);
            peer.Send(data, deliveryMethod);
            ComponentStatistics(component);
        }

        public void SendComponent(Guid client, INetworkComponent component, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered)
        {
            var resolverContainer = EntityManager.ResolversMap.GetComponentContainer(component);
            var data = PackResolverContainer(resolverContainer);
            PopulateAliveConnections();
            foreach (var kvp in aliveConnections)
            {
                if (kvp.client != client)
                    continue;

                kvp.peer.Send(data, deliveryMethod);
                ComponentStatistics(component);
                break;
            }
        }

        public void SendComponentToFilteredClients(INetworkComponent component, DeliveryMethod deliveryMethod, params Guid[] filter)
        {
            var resolverContainer = EntityManager.ResolversMap.GetComponentContainer(component);
            var data = PackResolverContainer(resolverContainer);
            PopulateAliveConnections();
            foreach (var kvp in aliveConnections)
            {
                if (filter.Contains(kvp.client))
                    continue;

                kvp.peer.Send(data, deliveryMethod);
                ComponentStatistics(component);
            }
        }
        
        private byte[] PackResolverContainer<T>(T command, Guid guid) where T : INetworkCommand, IData
        {
            var resolverDataContainer = EntityManager.ResolversMap.GetCommandContainer(command, guid);
            return MessagePackSerializer.Serialize(resolverDataContainer);
        }

        private byte[] PackResolverContainer(ResolverDataContainer container)
        {
            return MessagePackSerializer.Serialize(container);
        }
        
        partial void PopulateAliveConnections();
        partial void ComponentStatistics(IComponent component);
        partial void CommandStatistics<T>(T command);
    }

    [MessagePack.MessagePackObject]
    public struct ByteData //это эксперимент, всё еще хочу его попробовать
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