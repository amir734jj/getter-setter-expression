using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GetterSetterExpr.Interfaces;

namespace GetterSetterExpr.Logic
{
    public sealed class SetValue<TSource, TProperty> : ExpressionVisitor, ISetValue<TSource, TProperty>
    {
        private ImmutableStack<MemberExpression> _memberExprAccumulator;

        public Expression<Action<TSource, TProperty>> SetExpr { get; }
        
        public Action<TSource, TProperty> SetAction { get; }

        public SetValue(Expression<Func<TSource, TProperty>> expression)
        {
            _memberExprAccumulator = ImmutableStack<MemberExpression>.Empty;

            Visit(expression);

            var sourceArgExpr = Expression.Parameter(typeof(TSource));
            var propertyArgExpr = Expression.Parameter(typeof(TProperty));

            Expression currentExpr = sourceArgExpr;

            var memberExpressionsArray = _memberExprAccumulator.ToArray();

            for (var index = 0; index < memberExpressionsArray.Length; index++)
            {
                var memberExpression = memberExpressionsArray[index];
                var member = (PropertyInfo) memberExpression.Member;

                if (index + 1 == memberExpressionsArray.Length)
                {
                    currentExpr = Expression.Call(currentExpr, member.GetSetMethod(), propertyArgExpr);
                }
                else
                {
                    currentExpr = Expression.Convert(
                        Expression.MakeMemberAccess(currentExpr, member),
                        member.PropertyType);
                }
            }

            SetExpr = Expression.Lambda<Action<TSource, TProperty>>(
                currentExpr,
                sourceArgExpr,
                propertyArgExpr
            );

            SetAction = SetExpr
                .Compile();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _memberExprAccumulator = _memberExprAccumulator.Push(node);

            return base.VisitMember(node);
        }

        public void Set(TSource source, TProperty property)
        {
            SetAction(source, property);
        }
    }
}