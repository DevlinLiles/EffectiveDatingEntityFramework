using System;

namespace EffectiveDating.Core.Entities.Historical
{
    public class PhysicalAssetTemporal
    {
        public PhysicalAssetTemporal()
        {
            
        }

        public PhysicalAssetTemporal(Domain.PhysicalAsset temporalAsset)
        {
            Id = temporalAsset.Id;
            ResourceId = temporalAsset.ResourceId;
            EffectiveTo = temporalAsset.EffectiveTo;
            EffectiveFrom = temporalAsset.EffectiveFrom;
            TestEffectiveThing = temporalAsset.TestEffectiveThing;
        }

        public long Id { get; set; }
        public long PhysicalAssetEffectiveId { get; set; }
        public long ResourceId { get; set; }
        public DateTime EffectiveTo { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public PhysicalAsset PhysicalAsset { get; set; }

        public string TestEffectiveThing { get; set; }
        
    }
}