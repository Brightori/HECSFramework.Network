using HECSFramework.Core;
using LiteNetLib;
using System;

namespace HECSFramework.Network
{
    public interface IHECSSender
    {
        void SendToAll(ResolverDataContainer container, Guid sender, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable);
        void SendToPeer(NetPeer netPeer, Guid sender, ResolverDataContainer container, DeliveryMethod deliveryMethod = DeliveryMethod.Unreliable);
    }
}