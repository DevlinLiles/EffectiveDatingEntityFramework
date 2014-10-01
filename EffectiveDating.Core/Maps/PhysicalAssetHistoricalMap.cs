using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EffectiveDating.Core.Entities.Historical;

namespace EffectiveDating.Core.Maps
{
    public class PhysicalAssetHistoricalMap : EntityTypeConfiguration<PhysicalAsset>
    {
        public PhysicalAssetHistoricalMap()
        {
            ToTable("PhysicalAsset");
            HasKey(x => x.Id);
            HasMany(x => x.PhysicalAssetEffectives)
                .WithRequired(x => x.PhysicalAsset).HasForeignKey(x => x.Id);
        }
    }

    public class PhysicalAssetEffectiveMap : EntityTypeConfiguration<PhysicalAssetTemporal>
    {
        public PhysicalAssetEffectiveMap()
        {
            ToTable("PhysicalAssetEffective");
            HasKey(x => new { x.Id, x.PhysicalAssetEffectiveId } );
            Property(x => x.PhysicalAssetEffectiveId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(x => x.PhysicalAsset).WithMany(x => x.PhysicalAssetEffectives);
        }
    }
}