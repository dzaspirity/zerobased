using System;

namespace Zerobased.DataAccess
{
    /// <summary>
    /// Class to represent identity objects with Int32 Id property.
    /// </summary>
    public abstract class Int32IdentityObject : IInt32Identity
    {
        private static readonly object _newIdIteratorResetLock = new object();
        private static int _newIdIterator = -1;
        private int _id;
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
            set { _id = value; }
        }

        /// <summary>
        /// Define that object doen't have record in storage
        /// </summary>
        bool IIdentity<int>.IsNew => Id < 0;

        public bool Equals(IIdentity<int> other)
        {
            if (other == null || other.GetType() != GetType()) return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IIdentity<int>;
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return (GetTypeName() + "_" + Id).GetHashCode();
        }

        private string GetTypeName()
        {
            return _typeName ?? (_typeName = GetType().FullName);
        }
    }
}
