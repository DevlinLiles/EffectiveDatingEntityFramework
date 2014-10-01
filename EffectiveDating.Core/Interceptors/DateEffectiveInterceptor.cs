using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using EffectiveDating.Core.Queries;
using EffectiveDating.Core.Services;
using Highway.Data;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;

namespace EffectiveDating.Core.Interceptors
{
    public class DateEffectiveInterceptor : IEventInterceptor<BeforeSave>
    {
        private readonly EffectiveDatingService _effectiveDatingService;

        private DateTime _maxDate;

        public DateEffectiveInterceptor(EffectiveDatingService effectiveDatingService)
        {
            _effectiveDatingService = effectiveDatingService;
            _maxDate = (DateTime) SqlDateTime.MaxValue;
        }

        public int Priority { get; private set; }
        public InterceptorResult Apply(IDataContext context, BeforeSave eventArgs)
        {
            var efContext = context as DbContext;
            efContext.ChangeTracker.DetectChanges();
            if (efContext == null)
            {
                throw new InvalidOperationException("Entity Framework Interceptors must be used with Entity Framework Contexts");
            }

            var dbEntityEntries = efContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified);
            var modifiedEntities = dbEntityEntries
                .Where(e => e.Entity is ITemporalRoot).ToList();

            foreach (var entityEntry in modifiedEntities)
            {
                var typedEntity = entityEntry.Entity as ITemporalRoot;
                var effectiveStartOfNewRecord = typedEntity.EffectiveFrom;
                typedEntity.EffectiveTo = typedEntity.EffectiveTo == DateTime.MaxValue ? _maxDate : typedEntity.EffectiveTo;

                var entityType = entityEntry.Entity.GetType();
                var propertiesToCopy = new Dictionary<string, object>();
                foreach (var propertyName in entityEntry.CurrentValues.PropertyNames)
                {
                    var dbPropertyEntry = entityEntry.Property(propertyName);
                    if (dbPropertyEntry.IsModified && IsEffectiveDatedProperty(entityType, propertyName) )
                    {
                        propertiesToCopy.Add(propertyName, dbPropertyEntry.CurrentValue);
                    }
                }
                if (propertiesToCopy.Any())
                {
                    _effectiveDatingService.EffectiveDateObject(entityEntry);
                    foreach (var propertyName in propertiesToCopy)
                    {
                        entityEntry.Property(propertyName.Key).IsModified = false;
                    }
                    typedEntity.EffectiveTo = effectiveStartOfNewRecord;
                }
            }
            efContext.ChangeTracker.DetectChanges();
            return InterceptorResult.Succeeded();
        }

        private static bool IsEffectiveDatedProperty(Type entityType, string propertyName)
        {
            var propertyInfo = entityType.GetProperty(propertyName);
            var customAttributes = propertyInfo.GetCustomAttributes(true);
            return customAttributes.Any(x => x is TemporalProperty);
        }

        public bool AppliesTo(BeforeSave eventArgs)
        {
            return true;
        }
    }
}