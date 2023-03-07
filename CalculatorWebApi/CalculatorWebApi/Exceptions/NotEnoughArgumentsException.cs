namespace CalculatorWebApi.Exceptions
{
    public class NotEnoughArgumentsException : ArgumentException
    {
        public int ArgumentsCount { get; set; }
        public string? OperationName { get; set; }
        public int RequiredArguments { get; set; }

        public NotEnoughArgumentsException() { }

        public NotEnoughArgumentsException(int argumentsCount, int requiredArguments, string? operation)
        {
            OperationName = operation;
            ArgumentsCount = argumentsCount;
            RequiredArguments = requiredArguments;
        }
    }
}
