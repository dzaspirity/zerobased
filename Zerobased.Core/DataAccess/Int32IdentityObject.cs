using System;

namespace Zerobased.DataAccess
{
    /// <summary>
    /// Class to represent identity objects with Int32 Id property.
    /// </summary>
    public abstract class Int32IdentityObject : IInt32Identity, IEquatable<IIdentity<int>>
    {
        private static object _newIdIteratorResetLock = -1;
        private static int _newIdIterator = -1;
        private int _id;
        private int? _hashCode;
        private string _typeName;

        protected Int32IdentityObject(int id)
        {
            _id = id;
        }

        protected Int32IdentityObject() : this(_newIdIterator--)
        {
            Locker.DoubleCheck(_newIdIteratorResetLock, () => _newIdIterator < int.MinValue + 256, () => _newIdIterator = -1);
        }

        /// <summary>
        /// Objec identificator
        /// </summary>
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; _hashCode = null; }
        }

        /// <summary>
        /// Define that object doen't have record in storage
        /// </summary>
        bool IIdentity<int>.IsNew { get { return Id < 0; } }

        public bool Equals(IIdentity<int> other)
        {
            if (other == null || other.GetType() != GetType()) return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IIdentity<int>;
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
