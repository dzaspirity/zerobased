using System;

namespace Zerobased
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class TypeAttribute : Attribute
    {
        private readonly Type _type;

        public TypeAttribute(Type type)
        {
            _type = type;
        }

        public Type Type { get { return _type; } }
    }

    public class StringAttribute : TypeAttribute
    {
        public StringAttribute() : base(typeof(String)) { }
    }

    public class Int64Attribute : TypeAttribute
    {
        public Int64Attribute() : base(typeof(Int64)) { }
    }

    public class DoubleAttribute : TypeAttribute
    {
        public DoubleAttribute() : base(typeof(Double)) { }
    }

    public class BooleanAttribute : TypeAttribute
    {
        public BooleanAttribute() : base(typeof(Boolean)) { }
    }

    public class DateTimeAttribute : TypeAttribute
    {
        public DateTimeAttribute() : base(typeof(DateTime)) { }
    }
}
