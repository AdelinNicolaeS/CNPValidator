using System;
using System.IO;

namespace CNPValidator
{
    public static class Program
    {
        // credits: https://www.extensionmethod.net/csharp/string/isdate
        public static bool IsDate(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return DateTime.TryParse(input, out DateTime dt);
            }
            else
            {
                return false;
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Introdu CNP (sau ce vrei tu): ");
                var checker = new CNPChecker(Console.ReadLine());
               
                if (checker.CNP.Equals("quit") || checker.CNP.Equals("exit"))
                {
                    break;
                }

                Console.WriteLine(checker.CheckAll() ? "CNP valid" : "CNP invalid");
                Console.WriteLine(checker.GetOutput() + "\n");
            }
        }

    }
}
