using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using GetterSetterExpr.Interfaces;

namespace GetterSetterExpr.Logic
{
    public sealed class GetValue<TSource, TProperty> : ExpressionVisitor, IGetValue<TSource, TProperty>
    {
        private ImmutableStack<MemberExpression> _memberExprAccumulator;
        
        public Expression<Func<TSource, TProperty>> GetExpr { get; }
        
        public Func<TSource, TProperty> GetFunc { get; }

        public GetValue(Expression<Func<TSource, TProperty>> expression)
        {
            _memberExprAccumulator = ImmutableStack<MemberExpression>.Empty;

            Visit(expression);

            if (_memberExprAccumulator.IsEmpty)
            {
                throw new ArgumentException("Failed to find MemberExpression!");
            }

            var sourceArgExpr = Expression.Parameter(typeof(TSource));
            Expression currentExpr = sourceArgExpr;

            foreach (var memberExpression in _memberExprAccumulator)
            {
                currentExpr = Expression.Convert(
                    Expression.MakeMemberAccess(currentExpr, memberExpression.Member),
                    ((PropertyInfo) memberExpression.Member).PropertyType);
            }

            GetExpr = Expression.Lambda<Func<TSource, TProperty>>(
                    currentExpr, sourceArgExpr);
                
            GetFunc = GetExpr
                .Compile();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _memberExprAccumulator = _memberExprAccumulator.Push(node);

            return base.VisitMember(node);
        }

        public TProperty Get(TSource source)
        {
            return GetFunc(source);
        }
    }
}