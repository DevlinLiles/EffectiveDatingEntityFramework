using System.Collections.Generic;
using Highway.Data;
using Highway.Data.EventManagement.Interfaces;

namespace EffectiveDating.Core.Maps
{
    public class HistoricalDomain : IDomain
    {
        public HistoricalDomain(string connectionString)
        {
            ConnectionString = connectionString;
            Mappings = new TestHistoricalMappings();
            Context = new DefaultContextConfiguration();
            Events = new List<IInterceptor>();
        }

        public string ConnectionString { get; private set; }
        public IMappingConfiguration Mappings { get; private set; }
        public IContextConfiguration Context { get; private set; }
        public List<IInterceptor> Events { get; private set; }
    }
}