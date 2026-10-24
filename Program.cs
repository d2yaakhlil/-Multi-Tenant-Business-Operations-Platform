namespace MininProject_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double n1, n2;
            char op,choice;
            do
            {
                n1= ReadNumber("Enter the first number : ");
                Console.WriteLine("Enter the operation (+, -, *, /) : ");
                op = char.Parse(Console.ReadLine());
               if(op != '!' && op != '√')
                {
                    n2 = ReadNumber("Enter the second number : ");
                }
                else
                {
                    n2 = 0; // Set n2 to 0 for operations that don't require a second number
                }

                Calculate(n1, n2, op);
                Console.WriteLine(" Do you want to continue ?! (y/n)");
                choice = char.Parse(Console.ReadLine());

            } while (choice == 'y' || choice == 'Y');
        }
        static double ReadNumber(string msg)
        {
            double num;
            while(true)
            {
                Console.WriteLine(msg);
                if (double.TryParse(Console.ReadLine(),out num))
                {
                    return num;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
        static double Factorial(int n)
        {
            if (n == 0 || n == 1)
                return 1;
            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        static void Calculate(double n1, double n2, char op)
        {
            switch (op)
            {
                case '+':
                    Console.WriteLine($" the result is : {n1 + n2}");
                    break;
                case '-':
                    Console.WriteLine($" the result is : {n1 - n2}");
                    break;
                case '*':
                    Console.WriteLine($" the result is : {n1 * n2}");
                    break;
                case '/':
                    if (n2 == 0)
                    {
                        Console.WriteLine("Error: Division by zero is not allowed.");
                    }
                    else
                    {
                        Console.WriteLine($" the result is : {n1 / n2}");
                    }
                    break;
                case '%':
                    if (n2 == 0)
                    {
                        Console.WriteLine("Error: Division by zero is not allowed.");
                    }
                    else
                    {
                        Console.WriteLine($" the result is : {n1 % n2}");
                    }
                    break; 
                case '^':
                    Console.WriteLine($" the result is : {Math.Pow(n1, n2)}");
                    break;
                case '√':
                    if (n1 < 0)
                    {
                        Console.WriteLine("Error: Cannot calculate the square root of a negative number.");
                    }
                    else
                    {
                        Console.WriteLine($" the result is : {Math.Sqrt(n1)}");
                    }
                    break;
                case '!':
                    if (n1 < 0 || n1 != Math.Floor(n1))
                    {
                        Console.WriteLine("Error: Factorial is not defined for negative numbers or non-integers.");
                    }
                    else
                    {
                        Console.WriteLine($" the result is : {Factorial((int)n1)}");
                    }
                    break;

                default:
                    Console.WriteLine("Error: Invalid operation. Please use +, -, *, /, %, ^, √, or !.");
                    break;
            }
        }
    }
}
