using CalculatorWebApi.Exceptions;


namespace CalculatorWebApi.ApiServices.CalculatorServices
{
    public class CalculatorStackService : ICalculatorStackService
    {
        private readonly ICalculatorService m_CalculatorUnit;
        private Stack<int> m_NumsStack;

        public CalculatorStackService(ICalculatorService calcService)
        {
            m_NumsStack = new Stack<int>();
            m_CalculatorUnit = calcService;
        }

        public int GetCalcStackSize()
        {
            try
            {
                return m_NumsStack.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void DeleteArgumentsFromStack(int count)
        {
            if (count <= m_NumsStack.Count)
            {
                for (int i = 0; i < count; i++)
                {
                    m_NumsStack.Pop();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void AddArgumentsToStack(int[]? arguments)
        {
            if (arguments != null)
            {
                foreach (int arg in arguments)
                {
                    m_NumsStack.Push(arg);
                }
            }
        }

        public (int[], int) Calculate(string operation)
        {
            int result;
            int[] poppedNums;
            int requiredArgsNum = m_CalculatorUnit.GetOperationRequiredArgumentsNum(operation);

            if (GetCalcStackSize() >= requiredArgsNum)
            {
                poppedNums = new int[requiredArgsNum];
                for (int i = 0; i < requiredArgsNum; i++)
                {
                    poppedNums[i] = m_NumsStack.Pop();
                }

                result = m_CalculatorUnit.Calculate(poppedNums, operation);
                /*if (requiredArgsNum == 2)
                {
                    poppedNums = new int[] { m_NumsStack.Pop(), m_NumsStack.Pop() };
                    result = m_CalculatorUnit.Calculate(poppedNums, operation);
                }
                else if (requiredArgsNum == 1)
                {
                    poppedNums = new int[] { m_NumsStack.Pop() };
                    result = m_CalculatorUnit.Calculate(poppedNums, operation);
                }*/

            }

            else
            {
                throw new NotEnoughArgumentsException(0, m_CalculatorUnit.GetOperationRequiredArgumentsNum(operation), operation);
            }

            return (poppedNums, result);
        }

        public int[] GetStackContent()
        {
            return m_NumsStack.ToArray();
        }
    }
}
