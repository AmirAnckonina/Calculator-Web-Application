using CalculatorWebApi.Exceptions;

namespace CalculatorWebApi.ApiServices.CalculatorServices
{
    public interface ICalculatorStackService
    {
        int GetCalcStackSize();

        void DeleteArgumentsFromStack(int count);

        void AddArgumentsToStack(int[]? arguments);

        (int[], int) Calculate(string operation);

        int[] GetStackContent();
    }
}
