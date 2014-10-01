using System.Data.Entity;
using Highway.Data;

namespace EffectiveDating.Core.Maps
{
    public class TestMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PhysicalAssetMap());
        }
    }
}
