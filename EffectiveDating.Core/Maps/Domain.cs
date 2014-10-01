using System.Collections.Generic;
using EffectiveDating.Core.Interceptors;
using EffectiveDating.Core.Services;
using Highway.Data;
using Highway.Data.EventManagement.Interfaces;

namespace EffectiveDating.Core.Maps
{
    public class Domain : IDomain
    {
        public Domain(string connectionString)
        {
            ConnectionString = connectionString;
            Mappings = new TestMappings();
            Events = new List<IInterceptor>()
            {
                new DateEffectiveInterceptor(new EffectiveDatingService(new HistoricalContext(new HistoricalDomain(connectionString))))
            };
            Context = new DefaultContextConfiguration();
        }
        public string ConnectionString { get; private set; }
        public IMappingConfiguration Mappings { get; private set; }
        public IContextConfiguration Context { get; private set; }
        public List<IInterceptor> Events { get; private set; }
    }
}