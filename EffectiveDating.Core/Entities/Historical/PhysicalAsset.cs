using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EffectiveDating.Core.Entities.Historical
{
    public class PhysicalAsset
    {
        public PhysicalAsset()
        {
            PhysicalAssetEffectives = new List<PhysicalAssetTemporal>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<PhysicalAssetTemporal> PhysicalAssetEffectives { get; set; }
        
    }
}