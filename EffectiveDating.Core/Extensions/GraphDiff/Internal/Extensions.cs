﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Highway.Data;

namespace EffectiveDating.Core.Extensions.GraphDiff.Internal
{
    public static class Extensions
    {
        private static MappingDetailFactory _mappingDetailFactory = new MappingDetailFactory();

        internal static IEnumerable<PropertyInfo> GetPrimaryKeyFieldsFor(this IObjectContextAdapter context, Type entityType)
        {
            var metadata = context.ObjectContext.MetadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .SingleOrDefault(p => p.FullName == entityType.FullName);

            if (metadata == null)
            {
                throw new InvalidOperationException(String.Format("The type {0} is not known to the DbContext.", entityType.FullName));
            }

            return metadata.KeyMembers.Select(k => entityType.GetProperty(k.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)).ToList();
        }

        internal static IEnumerable<PropertyInfo> GetNonKeyFieldsFor(this IObjectContextAdapter context, Type entityType)
        {
            var metadata = context.ObjectContext.MetadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .SingleOrDefault(p => p.FullName == entityType.FullName);

            if (metadata == null)
            {
                throw new InvalidOperationException(String.Format("The type {0} is not known to the DbContext.", entityType.FullName));
            }

            return metadata.Members.Where(p => metadata.KeyMembers.All(k => k != p)).Select(p => entityType.GetProperty(p.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)).ToList();
        }

        internal static IEnumerable<NavigationProperty> GetRequiredNavigationPropertiesForType(this IObjectContextAdapter context, Type entityType)
        {
            return context.GetNavigationPropertiesForType(ObjectContext.GetObjectType(entityType))
                    .Where(navigationProperty => navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One);
        }

        internal static IEnumerable<NavigationProperty> GetNavigationPropertiesForType(this IObjectContextAdapter context, Type entityType)
        {
            return context.ObjectContext.MetadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(p => p.FullName == entityType.FullName)
                    .NavigationProperties;
        }

        internal static string GetEntitySetName(this IObjectContextAdapter context, Type entityType)
        {
            Type type = entityType;
            EntitySetBase set = null;

            while (set == null && type != null)
            {
                set = context.ObjectContext.MetadataWorkspace
                        .GetEntityContainer(context.ObjectContext.DefaultContainerName, DataSpace.CSpace)
                        .EntitySets
                        .FirstOrDefault(item => item.ElementType.Name.Equals(type.Name));

                type = type.BaseType;
            }

            return set != null ? set.Name : null;
        }

        public static MappingDetail GetMappingFor<T>(this IDataContext context)
        {
            return GetMappingFor(context, typeof(T));
        }

        public static MappingDetail GetMappingFor(this IDataContext context, Type entityType)
        {
            return _mappingDetailFactory.CreateDetails(context, entityType);
        }
    }

    public class MappingDetail
    {
        public List<PropertyDetail> Properties { get; set; }
        public string Table { get; set; }
        public string Schema { get; set; }

    }

    public class PropertyDetail
    {
        public PropertyInfo Property { get; set; }

        public string ColumnName { get; set; }
        public bool IsGenerated { get; set; }
    }
}
