using System;
using System.Linq;
using System.Linq.Dynamic;
using Highway.Data;

namespace EffectiveDating.Core.Queries
{
    public class GetById<TKey, TType> : Scalar<TType> where TType : class, IIdentifiable<TKey> where TKey : IEquatable<TKey>
    {
        private readonly IProvideNow _nowProvider;

        public GetById(TKey id, IProvideNow nowProvider)
        {
            _nowProvider = nowProvider;
            var entityType = typeof (TType);
            if (entityType.IsAssignableFrom(typeof(ITemporalRoot)))
            {
                ContextQuery = c => c.AsQueryable<TType>().Where(x => x.Id.Equals(id)).Where("EffectiveTo > @0 && EffectiveFrom <= @0", nowProvider.Now()).Single();
            }
            else
            {
                ContextQuery = c => c.AsQueryable<TType>().Single(x => x.Id.Equals(id));
            }
            
        }
    }

    public interface IProvideNow
    {
        DateTime Now();
    }

    public class NowProvider : IProvideNow
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }

    public interface ITemporalRoot : IIdentifiable<long>
    {
        DateTime EffectiveTo { get; set; }

        DateTime EffectiveFrom { get; set; }
        
    }
}