using System;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Zerobased.Extensions;
using System.Collections.Generic;

namespace Zerobased
{
    /// <summary>
    /// Helpers methods for checking arguments values
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Check if string argument is not null or empty
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
        /// Check if collection argument is not null or empty
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
        /// Check if object argument is not null
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
        /// Check if nullable value type argument is not null
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
        /// Check if value is not less than <paramref name="minValue"/>
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

        private static string GetCallerDataString(string methodName, string filePath, int lineNumber)
        {
            string method = methodName.IsNullOrEmpty() ? string.Empty : $" of method {methodName}";
            string file = filePath.IsNullOrEmpty() ? string.Empty : $" in file {Path.GetFileName(filePath)}";
            string line = lineNumber <= 0 ? string.Empty : $" (line {lineNumber})";
            return $"{method}{file}{line}";
        }
    }
}
