namespace DotnetK8S.Worker
{
    public static class FibonacciCalculator
    {
        public static int Calculate(int number)
        {
            switch (number)
            {
                case 0:
                case 1:
                    return number;
                default:
                    return Calculate(number - 1) + Calculate(number - 2);
            }
        }
    }
}