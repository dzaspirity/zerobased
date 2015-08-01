using System;

namespace Zerobased.DataAccess
{
    /// <summary>
    /// Class to represent identity objects with Giud Id property.
    /// </summary>
    public abstract class GuidIdentityObject : IGuidIdentity, IEquatable<IIdentity<Guid>>
    {
        private Guid _autoId;
        private Guid _id;
        private int? _hashCode;
        private string _typeName;

        protected GuidIdentityObject(Guid id)
        {
            _id = id;
        }

        protected GuidIdentityObject() : this(Guid.NewGuid())
        {
            _autoId = _id;
        }

        /// <summary>
        /// Objec identificator
        /// </summary>
        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; _hashCode = null; }
        }

        /// <summary>
        /// Define that object doen't have record in storage
        /// </summary>
        bool IIdentity<Guid>.IsNew { get { return Id == _autoId; } }

        public bool Equals(IIdentity<Guid> other)
        {
            if (other == null || other.GetType() != GetType())
                return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IIdentity<Guid>;
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (GetTypeName() + "_" + Id.ToString()).GetHashCode();
            }

            return _hashCode.Value;
        }

        private string GetTypeName()
        {
            return _typeName ?? (_typeName = GetType().FullName);
        }
    }
}
