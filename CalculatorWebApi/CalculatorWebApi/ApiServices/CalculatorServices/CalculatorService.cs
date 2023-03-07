using CalculatorWebApi.Exceptions;


namespace CalculatorWebApi.ApiServices.CalculatorServices
{
    public class CalculatorService : ICalculatorService
    {
        public int GetOperationRequiredArgumentsNum(string? operation)
        {
            switch (operation?.ToUpper())
            {
                case "PLUS":
                case "MINUS":
                case "TIMES":
                case "DIVIDE":
                case "POW":
                    return 2;

                case "ABS":
                case "FACT":
                    return 1;

                default:
                    return 0;
            }
        }

        public int Calculate(int[]? arguments, string? operation)
        {
            int operationResult;

            try
            {
                if (arguments != null)
                {
                    switch (operation?.ToUpper())
                    {
                        case "PLUS":
                            operationResult = Plus(arguments);
                            break;

                        case "MINUS":
                            operationResult = Minus(arguments);
                            break;

                        case "TIMES":
                            operationResult = Times(arguments);
                            break;

                        case "DIVIDE":
                            operationResult = Divide(arguments);
                            break;

                        case "POW":
                            operationResult = Pow(arguments);
                            break;

                        case "ABS":
                            operationResult = Abs(arguments);
                            break;

                        case "FACT":
                            operationResult = Fact(arguments);
                            break;

                        default:
                            throw new OperationException();
                    }

                    return operationResult;
                }
                else
                {
                    throw new NotEnoughArgumentsException();
                }
            }
            catch (NotEnoughArgumentsException)
            {
                throw new NotEnoughArgumentsException(
                    arguments != null ? arguments.Length : 0,
                    GetOperationRequiredArgumentsNum(operation), operation);
            }
            catch (OperationException)
            {
                throw new ArgumentException($"Error: unknown operation {operation}");
            }
            catch (TooManyArgumentsException)
            {
                throw new ArgumentException($"Error: Too many arguments to perform the operation {operation}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int Plus(int[] arguments)
        {
            if (arguments.Length == 2)
            {
                return arguments[0] + arguments[1];
            }
            else if (arguments.Length < 2)
            {
                throw new NotEnoughArgumentsException();
            }
            else
            {
                throw new TooManyArgumentsException();
            }

        }

        public int Minus(int[] arguments)
        {
            if (arguments.Length == 2)
            {
                return arguments[0] - arguments[1];
            }
            else if (arguments.Length < 2)
            {
                throw new NotEnoughArgumentsException();
            }
            else
            {
                throw new TooManyArgumentsException();
            }
        }

        public int Times(int[] arguments)
        {
            if (arguments.Length == 2)
            {
                return arguments[0] * arguments[1];
            }
            else if (arguments.Length < 2)
            {
                throw new NotEnoughArgumentsException();
            }
            else
            {
                throw new TooManyArgumentsException();
            }
        }

        public int Divide(int[] arguments)
        {
            if (arguments.Length == 2)
            {
                try
                {
                    return arguments[0] / arguments[1];
                }
                catch (DivideByZeroException)
                {
                    throw new DivideByZeroException("Error while performing operation Divide: division by 0");
                }
            }
            else if (arguments.Length < 2)
            {
                throw new NotEnoughArgumentsException();
            }
            else
            {
                throw new TooManyArgumentsException();
            }
        }

        public int Pow(int[] arguments)
        {
            if (arguments.Length == 2)
            {
                return (int)Math.Pow(arguments[0], arguments[1]);
            }
            else if (arguments.Length < 2)
            {
                throw new NotEnoughArgumentsException();
            }
            else
            {
                throw new TooManyArgumentsException();
            }
        }

        public int Abs(int[] arguments)
        {
            if (arguments.Length == 1)
            {
                return Math.Abs(arguments[0]);
            }
            else if (arguments.Length < 1)
            {
                throw new NotEnoughArgumentsException();
            }
            else
            {
                throw new TooManyArgumentsException();
            }
        }

        public int Fact(int[] arguments)
        {
            if (arguments.Length == 1)
            {
                if (arguments[0] >= 0)
                {
                    int factRes = 1;

                    for (int num = 1; num <= arguments[0]; num++)
                    {
                        factRes *= num;
                    }

                    return factRes;
                }
                else
                {
                    throw new OverflowException("Error while performing operation Factorial: not supported for the negative number");
                }
            }
            else if (arguments.Length < 1)
            {
                throw new NotEnoughArgumentsException();
            }
            else
            {
                throw new TooManyArgumentsException();
            }
        }

    }
}
