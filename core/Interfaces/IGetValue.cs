using System;
using System.Linq.Expressions;

namespace GetterSetterExpr.Interfaces
{
    public interface IGetValue<TSource, TProperty>
    {
        Expression<Func<TSource, TProperty>> GetExpr { get; }
        
        Func<TSource, TProperty> GetFunc { get; }
        
        TProperty Get(TSource source);
    }
}