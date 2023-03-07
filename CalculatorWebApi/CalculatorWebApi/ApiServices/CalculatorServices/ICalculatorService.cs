using CalculatorWebApi.Exceptions;


namespace CalculatorWebApi.ApiServices.CalculatorServices
{
    public interface ICalculatorService
    {
        int GetOperationRequiredArgumentsNum(string? operation);

        int Calculate(int[]? arguments, string? operation);

        int Plus(int[] arguments);

        int Minus(int[] arguments);

        int Times(int[] arguments);

        int Divide(int[] arguments);

        int Pow(int[] arguments);

        int Abs(int[] arguments);

        int Fact(int[] arguments);
    }
}
