using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Common.Logging;
using EffectiveDating.Core.Extensions;
using EffectiveDating.Core.Interceptors;
using EffectiveDating.Core.Maps;
using Highway.Data;

namespace EffectiveDating.Core.Services
{
    public class EffectiveDatingService
    {
        private readonly IHistoricalContext _historicalContext;

        public EffectiveDatingService(IHistoricalContext historicalContext)
        {
            _historicalContext = historicalContext;
        }

        public void EffectiveDateObject(DbEntityEntry entityThatIsEffectiveDated)
        {
            var attribute = entityThatIsEffectiveDated.Entity.GetType().GetCustomAttributes(true).SingleOrDefault(x => x is TemporalRepresentationAttribute) as TemporalRepresentationAttribute;
            var temporalType = Activator.CreateInstance(attribute.TemporalType, entityThatIsEffectiveDated.Entity);
            _historicalContext.InvokeGenericMethod("Add",attribute.TemporalType,temporalType);
            _historicalContext.Commit();
        }
    }

    public class HistoricalContext : DomainContext<HistoricalDomain>, IHistoricalContext
    {
        public HistoricalContext(HistoricalDomain domain) : base(domain)
        {
        }

        public HistoricalContext(HistoricalDomain domain, ILog logger) : base(domain, logger)
        {
        }
    }

    public interface IHistoricalContext : IDomainContext<HistoricalDomain>
    {

    }
}