using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EffectiveDating.Core.Entities.Domain;

namespace EffectiveDating.Core.Maps
{
    public class PhysicalAssetMap : EntityTypeConfiguration<PhysicalAsset>
    {
        public PhysicalAssetMap()
        {
            Map(x =>
            {
                x.Properties(c => new {c.Id, c.Name});
                x.Property(c => c.Id).HasColumnName("Id");
                x.ToTable("PhysicalAsset");
            })
            .Map(x =>
            {
                x.Properties(c => new {c.PhysicalAssetEffectiveId, c.ResourceId, c.EffectiveFrom, c.EffectiveTo, c.TestEffectiveThing});
                x.Property(c => c.Id).HasColumnName("Id");
                x.ToTable("PhysicalAssetEffective");
            });

            HasKey(x => x.Id);
            Property(x => x.PhysicalAssetEffectiveId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }    
    }
}