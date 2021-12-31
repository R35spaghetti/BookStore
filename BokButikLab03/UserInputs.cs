using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BokButikLab03
{
    public static class UserInputs
    {
        //Input check for ISBN
        public static string? RegexCheckInput(string? input)
        {
            Regex Check = new(@"^(\d{13})$"); //exakt 13 siffror, börja på och sluta på 
            bool result;
            do
            {
                if (input == null)
                {
                    throw new Exception("input is null");
                }
                result = Check.IsMatch(input);

                if (result == false)
                {
                    Console.WriteLine("Input had invalid characters");
                    Console.Write("Try again: ");
                    input = Console.ReadLine();

                }

            } while (!result);
            return input;

        }
    }
}
