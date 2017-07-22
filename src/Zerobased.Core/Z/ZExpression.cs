using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Zerobased
{
    /// <summary>
    /// Provides additional static methods to simplify expressions building
    /// </summary>
    public static class ZExpression
    {
        /// <summary>
        /// Creates a <see cref="MemberExpression"/> that represents accessing a property deeply by path.
        /// </summary>
        /// <param name="expr">Base expression which provides a value where property should accessed</param>
        /// <param name="propertyPath">Dot separated property path</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="expr"/> does not return value
        /// </exception>
        /// <exception cref="MissingMemberException">
        /// If <paramref name="propertyPath"/> contains a property name which is not in type
        /// </exception>
        public static MemberExpression PropertyPath(Expression expr, string propertyPath)
        {
            Check.ByPredicate(expr, exp => exp.Type != typeof(void), nameof(expr), "Expression cannot be void, it must return value.");
            string[] propertyNames = propertyPath.Split('.');
            Type type = expr.Type;

            foreach (string propertyName in propertyNames)
            {
                PropertyInfo pi = type.GetTypeInfo().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pi == null)
                {
                    throw new MissingMemberException($"Type {type.FullName} does not contain property {propertyName}");
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            return (MemberExpression)expr;
        }

        /// <summary>
        /// Creates <see cref="Expression"/> of explicit type casting
        /// </summary>
        /// <typeparam name="TTarget">Target cast type</typeparam>
        /// <param name="expr">Source of casting</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="expr"/> does not return value
        /// </exception>
        public static UnaryExpression Cast<TTarget>(Expression expr)
        {
            Check.ByPredicate(expr, exp => exp.Type != typeof(void), nameof(expr), "Expression cannot be void, it must return value.");
            return Expression.Convert(expr, typeof(TTarget));
        }

        /// <summary>
        /// Creates <see cref="Expression"/> of explicit type casting
        /// </summary>
        /// <typeparam name="TTarget">Target cast type</typeparam>
        /// <param name="obj">Source of casting</param>
        public static UnaryExpression Cast<TTarget>(object obj)
        {
            return Cast<TTarget>(Expression.Constant(obj));
        }

        /// <summary>
        /// Creates <see cref="Expression"/> for changing type via <see cref="Convert.ChangeType(object, Type)"/>
        /// </summary>
        /// <typeparam name="TTarget">Target type of changing</typeparam>
        /// <param name="expr">Source of changing</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="expr"/> does not return value
        /// </exception>
        public static UnaryExpression ChangeType<TTarget>(Expression expr)
        {
            Type targetType = typeof(TTarget);
            return ChangeType(expr, targetType);
        }

        /// <summary>
        /// Creates <see cref="Expression"/> for changing type via <see cref="Convert.ChangeType(object, Type)"/>
        /// </summary>
        /// <param name="expr">Source of changing</param>
        /// <param name="targetType">Target type of changing</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="expr"/> does not return value
        /// </exception>
        public static UnaryExpression ChangeType(Expression expr, Type targetType)
        {
            Check.ByPredicate(expr, exp => exp.Type != typeof(void), nameof(expr), "Expression cannot be void, it must return value.");
            // we need use explicit cast to object, 'cause MethodCallExpression can't use implicit cast for arguments.
            var objExpr = Expression.Convert(expr, typeof(object));
            var changeTypeExpr = Expression.Call(ConvertChangeTypeMethod, objExpr, Expression.Constant(targetType));
            var result = Expression.Convert(changeTypeExpr, targetType);
            return result;
        }

        private static readonly MethodInfo ConvertChangeTypeMethod =
            typeof(Convert).GetTypeInfo().GetMethod("ChangeType", new[] { typeof(object), typeof(Type) });
    }
}
