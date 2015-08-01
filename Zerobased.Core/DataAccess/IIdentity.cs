using System;
using System.ComponentModel.DataAnnotations;

namespace Zerobased.DataAccess
{
    /// <summary>
    /// Represents object with property Id.
    /// </summary>
    public interface IIdentity<T> : IEquatable<IIdentity<T>>
    {
        /// <summary>
        /// Objec identificator
        /// </summary>
        [Key]
        T Id { get; set; }

        /// <summary>
        /// Define that object doen't have record in storage
        /// </summary>
        bool IsNew { get; }
    }
}
