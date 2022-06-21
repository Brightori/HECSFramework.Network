using HECSFramework.Core;
using System.Collections.Generic;

namespace HECSFramework.Network
{
    public static class ResolversHelper
    {
        public static List<EntityResolver> GetServerSideResolvers(this List<IEntity> entities)
        {
            var list = new List<EntityResolver>(entities.Count);

            foreach (var e in entities)
                list.Add(new EntityResolver().GetServerEntityResolver(e));

            return list;
        }
    }
}