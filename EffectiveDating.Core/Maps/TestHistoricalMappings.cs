using System.Data.Entity;
using Highway.Data;

namespace EffectiveDating.Core.Maps
{
    public class TestHistoricalMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PhysicalAssetHistoricalMap());
            modelBuilder.Configurations.Add(new PhysicalAssetEffectiveMap());
        }
    }
}