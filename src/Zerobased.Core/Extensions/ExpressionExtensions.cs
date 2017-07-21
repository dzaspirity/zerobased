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
                if (propertyExpression.Body is UnaryExpression unaryExpression)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }

            if (memberExpression != null)
            {
                if (memberExpression.Expression is ParameterExpression parameterExpression && parameterExpression.Name == propertyExpression.Parameters[0].Name)
                {
                    return memberExpression.Member.Name;
                }
            }

            throw new ArgumentException("Invalid expression.", nameof(propertyExpression));
        }
    }
}
