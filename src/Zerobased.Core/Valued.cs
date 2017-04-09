using JetBrains.Annotations;

namespace Zerobased
{
    /// <summary>
    /// Wrapper for pain values of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of value to wrap</typeparam>
    /// <remarks>
    /// The class can be useful if you need to return a simple type value
    /// (like <see cref="bool"/>, or <see cref="int"/>, or <see cref="System.DateTime"/>)
    /// serialized as JSON or other object notations.
    /// It may be useful, but think before use it. May be will make more sense to have small class with proper name of property.
    /// </remarks>
    public class Valued<T>
    {
        /// <summary>
        /// Creates an instance with default value of <typeparamref name="T"/>
        /// </summary>
        public Valued() : this(default(T)) { }

        /// <summary>
        /// Creates an instance which wraps provided value
        /// </summary>
        /// <param name="value">Value to wrap</param>
        public Valued(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Wrapped value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Returns <see cref="string"/> which represents the instance of the <see cref="Valued{T}.Value"/> property
        /// or <see cref="string.Empty"/> if <see cref="Valued{T}.Value"/> is NULL
        /// </summary>
        [NotNull]
        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }
    }

    public class Valued : Valued<object>
    {
        /// <summary>
        /// Creates an instance of <see cref="Valued{T}"/> to wrap a value
        /// </summary>
        /// <typeparam name="T">Type of value to wrap</typeparam>
        /// <param name="value">Value to wrap</param>
        [NotNull]
        public static Valued<T> Create<T>(T value)
        {
            return new Valued<T>(value);
        }
    }
}
