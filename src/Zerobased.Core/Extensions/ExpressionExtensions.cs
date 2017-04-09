using System;
using System.Linq.Expressions;

namespace Zerobased.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Returns member name from <paramref name="propertyExpression"/>.
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static string RetrieveMemberName<TArg, TRes>(this Expression<Func<TArg, TRes>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;
                if (unaryExpression != null)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }

            if (memberExpression != null)
            {
                var parameterExpression = memberExpression.Expression as ParameterExpression;
                if (parameterExpression != null && parameterExpression.Name == propertyExpression.Parameters[0].Name)
                    return memberExpression.Member.Name;
            }

            throw new ArgumentException("Invalid expression.", nameof(propertyExpression));
        }
    }
}
