using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Zerobased.Extensions;

namespace Zerobased
{
    public static class ZExpression
    {
        public static MemberExpression PropertyPath(Expression expr, string propertyPath)
        {
            CheckExpressionReturnsValue(expr);

            string[] propertyNames = propertyPath.Split('.');
            Type type = expr.Type;

            foreach (string propertyName in propertyNames)
            {
                PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (pi == null)
                {
                    throw new MissingMemberException(type.FullName, propertyName);
                }

                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            return (MemberExpression)expr;
        }

        public static UnaryExpression Cast<TTarget>(Expression expr)
        {
            return Expression.Convert(expr, typeof(TTarget));
        }

        public static UnaryExpression Cast<TTarget>(object obj)
        {
            return Cast<TTarget>(Expression.Constant(obj));
        }

        public static UnaryExpression Convert<TTarget>(Expression expr)
        {
            Type targetType = typeof(TTarget);
            return Convert(expr, targetType);
        }

        public static UnaryExpression Convert(Expression expr, Type targetType)
        {
            CheckExpressionReturnsValue(expr);

            var converterExpr = Expression.Constant(expr.Type.GetConverter());
            var objExpr = Expression.Convert(expr, typeof(object));
            var convertExpr = Expression.Call(converterExpr, TypeConverterConvertToMethod, objExpr, Expression.Constant(targetType));
            var result = Expression.Convert(convertExpr, targetType);
            return result;
        }

        /// <summary>
        ///     Create expression for changing type thru System.Convert.
        ///     (TTarget)System.Convert.ChangeType((object)expr, typeof(TTarget))
        /// </summary>
        public static UnaryExpression ChangeType<TTarget>(Expression expr)
        {
            Type targetType = typeof(TTarget);
            return ChangeType(expr, targetType);
        }

        /// <summary>
        ///     Create expression for changing type thru System.Convert.
        ///     (TTarget)System.Convert.ChangeType((object)expr, typeof(TTarget))
        /// </summary>
        public static UnaryExpression ChangeType(Expression expr, Type targetType)
        {
            CheckExpressionReturnsValue(expr);
            // we need use explicit cast to object, 'cause MethodCallExpression can't use implicit cast for arguments.
            var objExpr = Expression.Convert(expr, typeof(object));
            var changeTypeExpr = Expression.Call(ConvertChangeTypeMethod, objExpr, Expression.Constant(targetType));
            var result = Expression.Convert(changeTypeExpr, targetType);
            return result;
        }

        private static void CheckExpressionReturnsValue(Expression expr)
        {
            if (expr.Type == typeof(void))
            {
                throw new ArgumentException("Expression must return value to create ChangeType expression.", nameof(expr));
            }
        }


        private static readonly MethodInfo ConvertChangeTypeMethod =
            typeof(Convert).GetMethod("ChangeType", new[] { typeof(object), typeof(Type) });

        private static readonly MethodInfo TypeConverterConvertToMethod =
            typeof(TypeConverter).GetMethod("ConvertTo", new[] { typeof(object), typeof(Type) });

    }
}
