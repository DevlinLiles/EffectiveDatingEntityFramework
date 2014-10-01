using System;
using EffectiveDating.Core.Interceptors;
using EffectiveDating.Core.Queries;
using Highway.Data;

namespace EffectiveDating.Core.Entities.Domain
{
    [TemporalRepresentation(typeof(Historical.PhysicalAssetTemporal))]
    public class PhysicalAsset : ITemporalRoot
    {
        public long Id { get; set; }

        public long PhysicalAssetEffectiveId { get; set; }
        
        public string Name { get; set; }

        [TemporalProperty]
        public DateTime EffectiveFrom { get; set; }
        
        [TemporalProperty]
        public DateTime EffectiveTo { get; set; }

        [TemporalProperty]
        public long ResourceId { get; set; }

        [TemporalProperty]
        public string TestEffectiveThing { get; set; }
    }
}