using System;
using Highway.Data;

namespace EffectiveDating.Core.Entities.Domain
{
    public class Resource : IIdentifiable<long>
    {
        public long Id { get; set; }

        public PhysicalAsset Asset { get; set; }

        public string Name { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime TerminationDate { get; set; }
    }
}