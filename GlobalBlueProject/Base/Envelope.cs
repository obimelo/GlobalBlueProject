namespace GlobalBlueProject.Base
{
    public sealed class Envelope<T>
    {
        public DateTime TimeGeneratedUTC { get; }
        public string? ErrorMessage { get; }
        public T? Result { get; }

        private Envelope(T? result, string? errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGeneratedUTC = DateTime.UtcNow;
        }

        public static Envelope<T> Ok(T? result)
        {
            return new Envelope<T>(result, null);
        }

        public static Envelope<T> Error(string errorMessage)
        {
            return new Envelope<T>(default, errorMessage);
        }
    }
}
