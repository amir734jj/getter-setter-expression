using System;
using System.Linq.Expressions;

namespace GetterSetterExpr.Interfaces
{
    public interface ISetValue<TSource, TProperty>
    {
        Expression<Action<TSource, TProperty>> SetExpr { get; }
        
        Action<TSource, TProperty> SetAction { get; }

        void Set(TSource source, TProperty property);
    }
}