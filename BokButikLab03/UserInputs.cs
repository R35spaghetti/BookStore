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
        //Input for ISBN13 will only be 13 numbers
        public static string? RegexCheckInput(string? input)
        {
            Regex Check = new(@"^(\d{13})$"); //start and end with exactly 13 digits 
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
                    InputInvalid();
                    TryAgain();
                    input = Console.ReadLine();

                }

            } while (!result);
            return input;

        }

        public static string? RegexCheckNumberInput(string? input)
        {
            Regex Check = new(@"^([1-9][0-9]*)$"); //Can't start input with a 0 digit. Can contain unlimited amount of digits from 0-9.
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
                    InputInvalid();
                    TryAgain();
                    input = Console.ReadLine();

                }

            } while (!result);
            return input;
        }

        public static long WhichISBN(long ISBN)
        {


            QuestionISBN13();
            string? answer = Console.ReadLine();
            answer = RegexCheckInput(answer);
            if (answer == null)
            {
                throw new Exception($"{ISBN} doesn't exist");
            }

            ISBN = long.Parse(answer);
            return ISBN;
        }
      public static int AmountOfBooks(int bookAmount)
        {
            Console.WriteLine("Enter amount of books: ");
            string? answer = Console.ReadLine();
            if (answer == null)
            {
                throw new Exception($"{bookAmount} doesn't work");
            }
            answer = RegexCheckNumberInput(answer);
            bookAmount = int.Parse(answer);


            return bookAmount;
        }



        public static  int EnterAuthorID(int answerID)
        {
            EnterID();
            string? answer = Console.ReadLine();
            if (answer == null)
            {
                throw new Exception($"{answerID} doesn't exist");
            }
            answerID = int.Parse(answer);


            return answerID;
        }
      public static int WhichStore(int storeID)
        {
            EnterStoreID();
            string? answer = Console.ReadLine();
            if (answer == null)
            {
                throw new ArgumentException($"{storeID} doesn't exist");
            }
            storeID = int.Parse(answer);


            return storeID;
        }

        private static void EnterStoreID()
        {
            Console.WriteLine("Enter store ID: ");
        }

        private static void EnterID()
        {
            Console.WriteLine("Enter ID: ");
        }

        private static void InputInvalid()
        {
            Console.WriteLine("Input had invalid characters");
        }
        private static void TryAgain()
        {
            Console.Write("Try again: ");
        }
        private static void QuestionISBN13()
        {
            Console.WriteLine("Enter ISBN: ");
        }
    }
}
