using Commands;
using HECSFramework.Core;

namespace HECSFramework.Network
{
    public static class EntityExtention
    {
        public static void AddHecsNetworkComponent<T>(this IEntity entity, T component) where T : INetworkComponent
        {
            entity.AddHecsComponent(component);
            EntityManager.Command(new AddNetWorkComponentCommand { Component = component });
        }
        
        public static T GetOrAddHecsNetworkComponent<T>(this IEntity entity) where T : INetworkComponent, new()
        {
            if (entity.TryGetHecsComponent(out T component)) return component;

            component = new T();
            entity.AddHecsComponent(component);
            EntityManager.Command(new AddNetWorkComponentCommand { Component = component });
            return component;
        }

        public static void RemoveHecsNetworkComponent<T>(this IEntity entity, T component) where T : INetworkComponent
        {
            entity.RemoveHecsComponent(component);
            EntityManager.Command(new RemoveNetWorkComponentCommand { Component = component });
        }
        
        public static void RemoveHecsNetworkComponent<T>(this IEntity entity) where T : INetworkComponent
        {
            var component = entity.GetHECSComponent<T>();
            entity.RemoveHecsComponent(component);
            EntityManager.Command(new RemoveNetWorkComponentCommand { Component = component });
        }
    }
}