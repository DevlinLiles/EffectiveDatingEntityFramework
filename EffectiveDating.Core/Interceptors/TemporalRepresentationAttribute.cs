using System;

namespace EffectiveDating.Core.Interceptors
{
    public class TemporalRepresentationAttribute : Attribute
    {
        public TemporalRepresentationAttribute(Type temporalType)
        {
            TemporalType = temporalType;
        }
        public Type TemporalType { get; set; }
    }
}