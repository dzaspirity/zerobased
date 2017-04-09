using System;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Zerobased.Extensions;
using System.Collections.Generic;

namespace Zerobased
{
    /// <summary>
    ///     Helpers methods for checking arguments values
    /// </summary>
    public static class Check
    {
        /// <summary>
        ///     Check if string <paramref name="argValue"/> is not null or empty
        /// </summary>
        /// <param name="argValue">Argument value for checking</param>
        /// <param name="argName">Argument name for correct exception</param>
        /// <param name="methodName">Caller name for better exception message</param>
        /// <param name="filePath">Full path of the source file that contains the caller, for better exception message</param>
        /// <param name="lineNumber">Line number in the source file that contains the caller, for better exception message</param>
        /// <returns>Returns argument value without any changes</returns>
        /// <exception cref="ArgumentException">If argument value is null or empty string</exception>
        [NotNull]
        public static string NotNullOrEmpty(string argValue, [InvokerParameterName] string argName,
            [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (argValue.IsNullOrEmpty())
            {
                throw new ArgumentException($"String argument {argName}{GetCallerDataString(methodName, filePath, lineNumber)} cannot be null or empty.", argName);
            }
            return argValue;
        }

        /// <summary>
        ///     Check if collection <paramref name="argValue"/> is not null or empty
        /// </summary>
        /// <param name="argValue">Argument value for checking</param>
        /// <param name="argName">Argument name for correct exception</param>
        /// <param name="methodName">Caller name for better exception message</param>
        /// <param name="filePath">Full path of the source file that contains the caller, for better exception message</param>
        /// <param name="lineNumber">Line number in the source file that contains the caller, for better exception message</param>
        /// <returns>Returns argument value without any changes</returns>
        /// <exception cref="ArgumentException">If argument value is null or empty string</exception>
        [NotNull]
        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> argValue, [InvokerParameterName] string argName,
            [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (argValue.IsNullOrEmpty())
            {
                throw new ArgumentException($"Collection argument {argName}{GetCallerDataString(methodName, filePath, lineNumber)} cannot be null or empty.", argName);
            }
            return argValue;
        }

        /// <summary>
        ///     Check if enumerable <paramref name="argValue"/> is not null or empty and returns <see cref="IEnumerator{T}"/>,
        ///     which points on the first item of the sequence
        /// </summary>
        /// <param name="argValue">Argument value for checking</param>
        /// <param name="argName">Argument name for correct exception</param>
        /// <param name="methodName">Caller name for better exception message</param>
        /// <param name="filePath">Full path of the source file that contains the caller, for better exception message</param>
        /// <param name="lineNumber">Line number in the source file that contains the caller, for better exception message</param>
        /// <returns><see cref="IEnumerator{T}"/> of <paramref name="argValue"/></returns>
        /// <exception cref="ArgumentException">If argument value is null or empty sequence</exception>
        [NotNull]
        public static IEnumerator<T> NotNullOrEmpty<T>(IEnumerable<T> argValue, [InvokerParameterName] string argName,
            [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            IEnumerator<T> enumerator = argValue?.GetEnumerator();
            if (argValue == null || !enumerator.MoveNext())
            {
                throw new ArgumentException($"Enumerable argument {argName}{GetCallerDataString(methodName, filePath, lineNumber)} cannot be null or empty.", argName);
            }
            return enumerator;
        }

        /// <summary>
        ///     Check if object <paramref name="argValue"/> is not null
        /// </summary>
        /// <param name="argValue">Argument value for checking</param>
        /// <param name="argName">Argument name for correct exception</param>
        /// <param name="methodName">Caller name for better exception message</param>
        /// <param name="filePath">Full path of the source file that contains the caller, for better exception message</param>
        /// <param name="lineNumber">Line number in the source file that contains the caller, for better exception message</param>
        /// <returns>Returns argument value without any changes</returns>
        /// <exception cref="ArgumentNullException">If argument value is null</exception>
        [NotNull]
        public static T NotNull<T>([NoEnumeration]T argValue, [InvokerParameterName] string argName,
            [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) where T : class
        {
            if (argValue == null)
            {
                throw new ArgumentNullException(argName, $"Argument {argName}{GetCallerDataString(methodName, filePath, lineNumber)} cannot be null.");
            }
            return argValue;
        }

        /// <summary>
        ///     Check if null-able value type argument is not null
        /// </summary>
        /// <param name="argValue">Argument value for checking</param>
        /// <param name="argName">Argument name for correct exception</param>
        /// <param name="methodName">Caller name for better exception message</param>
        /// <param name="filePath">Full path of the source file that contains the caller, for better exception message</param>
        /// <param name="lineNumber">Line number in the source file that contains the caller, for better exception message</param>
        /// <returns>Returns argument value without any changes</returns>
        /// <exception cref="ArgumentNullException">If argument value is null</exception>
        [NotNull]
        public static T? NotNull<T>(T? argValue, [InvokerParameterName] string argName,
            [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) where T : struct
        {
            if (argValue == null)
            {
                throw new ArgumentNullException(argName, $"Argument {argName}{GetCallerDataString(methodName, filePath, lineNumber)} cannot be null.");
            }
            return argValue;
        }

        /// <summary>
        ///     Check if <paramref name="argValue"/> is not less than <paramref name="minValue"/>
        /// </summary>
        /// <typeparam name="T">Type of the checking argument</typeparam>
        /// <param name="argValue">Argument value for checking</param>
        /// <param name="minValue">Lower bound of acceptable values</param>
        /// <param name="argName">Argument name for correct exception</param>
        /// <param name="methodName">Caller name for better exception message</param>
        /// <param name="filePath">Full path of the source file that contains the caller, for better exception message</param>
        /// <param name="lineNumber">Line number in the source file that contains the caller, for better exception message</param>
        /// <returns>Returns argument value without any changes</returns>
        /// <exception cref="ArgumentOutOfRangeException">If argument value is less than <paramref name="minValue"/></exception>
        public static T Min<T>(T argValue, T minValue, [InvokerParameterName] string argName,
            [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) where T : IComparable<T>
        {
            int compareResult = argValue.CompareTo(minValue);
            if (compareResult < 0)
            {
                throw new ArgumentOutOfRangeException(argName, $"Value of argument {argName}{GetCallerDataString(methodName, filePath, lineNumber)} cannot be less than {minValue}.");
            }
            return argValue;
        }

        /// <summary>
        ///     Checks if <paramref name="argValue"/> satisfies the <paramref name="isValid"/>
        /// </summary>
        /// <typeparam name="T">Type of the checking argument</typeparam>
        /// <param name="argValue"></param>
        /// <param name="isValid"><see cref="Predicate{T}"/> checks if argument value is valid</param>
        /// <param name="argName">Argument name for correct exception</param>
        /// <param name="message">Exception message</param>
        /// <param name="methodName">Caller name for better exception message</param>
        /// <param name="filePath">Full path of the source file that contains the caller, for better exception message</param>
        /// <param name="lineNumber">Line number in the source file that contains the caller, for better exception message</param>
        /// <returns>Returns argument value without any changes</returns>
        /// <exception cref="ArgumentException">If <paramref name="isValid"/> predicate returns false</exception>
        public static T ByPredicate<T>([NoEnumeration]T argValue, Predicate<T> isValid, [InvokerParameterName] string argName, string message,
            [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (!isValid(argValue))
            {
                throw new ArgumentException($"Argument {argName}{GetCallerDataString(methodName, filePath, lineNumber)} is not valid: {message}.", argName);
            }
            return argValue;
        }

        private static string GetCallerDataString(string methodName, string filePath, int lineNumber)
        {
            string method = methodName.IsNullOrEmpty() ? string.Empty : $" of method {methodName}";
            string file = filePath.IsNullOrEmpty() ? string.Empty : $" in file {Path.GetFileName(filePath)}";
            string line = lineNumber <= 0 ? string.Empty : $" (line {lineNumber})";
            return $"{method}{file}{line}";
        }
    }
}
