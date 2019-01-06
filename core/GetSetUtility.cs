using System;
using System.Linq.Expressions;
using GetterSetterExpr.Logic;

namespace GetterSetterExpr
{
    public static class GetSetUtility
    {
        public static Func<TSource, TProperty> Get<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> expression)
        {
            return new GetValue<TSource, TProperty>(expression).GetFunc;
        }
        
        public static Action<TSource, TProperty> Set<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> expression)
        {
            return new SetValue<TSource, TProperty>(expression).SetAction;
        }
    }
}