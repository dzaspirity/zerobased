namespace Zerobased
{
    public class Valued<T> : IValued<T>
    {
        public Valued() { }

        public Valued(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public override string ToString()
        {
            return Value == null ? string.Empty : Value.ToString();
        }
    }

    public class Valued : Valued<object>
    {
        public static Valued<T> Create<T>(T value)
        {
            return new Valued<T>(value);
        }
    }
}
