using BokButikLab03.Data;
using System.Text.RegularExpressions;

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

            ISBN = TrycatchCorrectISBN13(ISBN);

            return ISBN;
        }


        public static int AmountOfBooks(int bookAmount)
        {
            Console.WriteLine("Enter amount of books: ");
            string? answer = Console.ReadLine();
            answer = RegexCheckNumberInput(answer);
            
            if (answer == null)
            {
                throw new Exception($"{bookAmount} doesn't work");
            }

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

            answerID = TrycatchCorrectAuthorID(answerID);

            return answerID;
        }


        public static int WhichStore(int storeID)
        {
           
            EnterStoreID();
            string? answer = Console.ReadLine();
            answer = RegexCheckNumberInput(answer);

            if (answer == null)
            {
                throw new ArgumentException($"{storeID} doesn't exist");
            }

            storeID = int.Parse(answer);

            storeID = TrycatchCorrectStoreID(storeID);
            
            return storeID;

     }

        private static int TrycatchCorrectAuthorID(int answerID)
        {
            try
            {
                using var context = new Laboration2RBContext();
                {
                    var foundID = context.Författares

                    .First(f => f.Id == answerID);


                }
            }



            catch (InvalidOperationException)
            {
                Console.WriteLine("ID doesn't exist");
                answerID = EnterAuthorID(answerID);

            }

            return answerID;
        }

        private static int TrycatchCorrectStoreID(int storeID)
        {
            try
            {
                using var context = new Laboration2RBContext();
                {
                    var foundID = context.LagerSaldos

                    .First(f => f.ButikId == storeID);


                }
            }



            catch (InvalidOperationException)
            {
                Console.WriteLine("Store doesn't exist");
                storeID = WhichStore(storeID);

            }

            return storeID;
        }

        private static long TrycatchCorrectISBN13(long iSBN)
        {
            try
            {
                using var context = new Laboration2RBContext();
                {
                    var foundID = context.LagerSaldos

                    .First(f => f.Isbn == iSBN);


                }
            }



            catch (InvalidOperationException)
            {
                Console.WriteLine("ISBN13 code doesn't exist!");
                iSBN = WhichISBN(iSBN);

            }

            return iSBN;
        }

        private static void EnterStoreID()
        {
            Console.Write("Enter store ID: ");
        }

        private static void EnterID()
        {
            Console.Write("Enter ID: ");
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
            Console.Write("Enter ISBN: ");
        }
    }
}
