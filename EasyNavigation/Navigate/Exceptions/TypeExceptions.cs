namespace Navigation.Exceptions
{
    public class TypeExceptions : InvalidOperationException
    {
        public Type? ExceptType { get; set; }
        public Type? ActualType { get; set; }

        public TypeExceptions(Type exceptType, Type? actualType)
            : base($"Expected type {exceptType.Name}, but no {actualType?.Name ?? "null"}")
        {
            ExceptType = exceptType;
            ActualType = actualType;
        }

        public TypeExceptions()
            : base(string.Empty) { }

        public TypeExceptions(string? message)
            : base(message) { }

        public TypeExceptions(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
