using BokButikLab03.Data;
using System.Text.RegularExpressions;

namespace BokButikLab03
{
    public static class UserInputs
    {
        /// <summary>
        /// Input for ISBN13 needs to be exactly 13 numbers
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? RegexCheckInput(string? input)
        {
            Regex Check = new(@"^(\d{13})$"); //start and end with exactly 13 digits 
            bool result;
            do
            {
                if (input == null)
                {
                    throw new Exception("input is invalid!");
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
                    throw new Exception("input is invalid!");
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
        /// <summary>
        /// Input for price, to allow decimals
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? RegexCheckDecimalInput(string? input)
        {
            Regex Check = new(@"^([\d,\d]*)$"); //Allows decimals and ",". No need for a "." at the moment.
            bool result;
            do
            {
                if (input == null)
                {
                    throw new Exception("input is invalid!");
                }
                result = Check.IsMatch(input);

                if (result == false)
                {
                    InputInvalid();
                    input = Console.ReadLine();

                }
                
            } while (!result);
            return input;
        }
        /// <summary>
        /// For string inputs
        /// </summary>
        /// <param name="usrInput"></param>
        /// <returns></returns>
        public static string StringInput(string? usrInput)
        {
            do
            {

                Console.Write("> ");
                usrInput = Console.ReadLine();
            } while (usrInput == null || usrInput.Trim() == "");

            return usrInput;
        }

/// <summary>
/// For long inputs, used for ISBN
/// </summary>
/// <param name="ISBN"></param>
/// <returns></returns>
/// <exception cref="Exception"></exception>
        public static long AddAnotherISBN(long ISBN)
        {
            bool flag = true;

            do
            {


                QuestionISBN13();
                string? answer = Console.ReadLine();
                answer = RegexCheckInput(answer);

                if (answer == null)
                {
                    throw new Exception($"{ISBN} value is invalid");
                }

                ISBN = long.Parse(answer);

                flag = FindISBN(ISBN, flag);
            } while (flag);

            return ISBN;
        }

        /// <summary>
        /// Checks if the input for ISBN13 exists
        /// </summary>
        /// <param name="ISBN"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static long WhichISBN13ForBookTable(long ISBN)
        {


            QuestionISBN13();
            string? answer = Console.ReadLine();
            answer = RegexCheckInput(answer);

            if (answer == null)
            {
                throw new Exception($"{ISBN} doesn't exist");
            }

            ISBN = long.Parse(answer);

            ISBN = TrycatchCorrectISBN13BookTable(ISBN);

            return ISBN;
        }



        /// <summary>
        /// Int input for book stock
        /// </summary>
        /// <param name="bookAmount"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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


        /// <summary>
        /// Int input for ID:s
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static  int EnterAuthorID(int answerID)
        {
            EnterID();
            string? answer = Console.ReadLine();
            answer = RegexCheckNumberInput(answer);
            if (answer == null)
            {
                throw new Exception($"{answerID} doesn't exist!");
            }
            answerID = int.Parse(answer);

            answerID = TrycatchCorrectAuthorID(answerID);

            return answerID;
        }

        /// <summary>
        /// Input for store ID:s
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int WhichStore(int storeID)
        {
           
            EnterStoreID();
            string? answer = Console.ReadLine();
            answer = RegexCheckNumberInput(answer);

            if (answer == null)
            {
                throw new ArgumentException($"{storeID} doesn't exist!");
            }

            storeID = int.Parse(answer);

            storeID = TrycatchCorrectStoreID(storeID);
            
            return storeID;

     }
        /// <summary>
        /// Input for dates (such as yyyy-mm-dd nothing else :^) ) 
        /// </summary>
        /// <param name="addDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime DateInput(DateTime addDate)
        {
            ShowCurrentDateMessage();
            

            string? DateLine;
            do
            {
                EnterDateMessage();
                DateLine = Console.ReadLine();

                if (DateLine == null)
                {
                    throw new Exception("Date is invalid");
                }

            } while (!DateTime.TryParseExact(DateLine, "yyyy/mm/dd", null, System.Globalization.DateTimeStyles.None, out addDate));

            return addDate;
        }

      /// <summary>
      /// Input for price
      /// </summary>
      /// <param name="pris"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
        public static decimal PriceInput(decimal pris)
        {
            CurrentPriceMessage(pris);

            string? usrInput;
            do
            {
                CostOfBookMessage();

                usrInput = Console.ReadLine();
                if (usrInput == null)
                {
                    throw new Exception("Input is invalid!");
                }
                usrInput = RegexCheckDecimalInput(usrInput.Trim());
            } while (usrInput == null || usrInput == "" || usrInput == "," || usrInput == "0");
        

            pris = decimal.Parse(usrInput);

            return pris;
        }

        /// <summary>
        /// To check if the ISBN13 exists
        /// </summary>
        /// <param name="iSBN"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private static bool FindISBN(long iSBN, bool flag)
        {
            
                using var context = new Laboration2RBContext();
                {
               
                    var foundID = context.Böckers

                    .Find(iSBN);

                    if (foundID == null)
                    {
                    flag = false;
                    return flag;

                    }
                    else
                    {
                    ISBNAlreadyExistsMessage();
                    return flag;
                    }



                }
 
        }

        private static void ISBNAlreadyExistsMessage()
        {
            Console.WriteLine("This ISBN13 already exist!");
        }

        /// <summary>
        /// Checks if the ISBN13 in the book table exists
        /// </summary>
        /// <param name="iSBN"></param>
        /// <returns></returns>
        private static long TrycatchCorrectISBN13BookTable(long iSBN)
        {
            try
            {
                using var context = new Laboration2RBContext();
                {
                    var foundID = context.Böckers

                    .First(f => f.Isbn13 == iSBN);

                }
            }

            catch (InvalidOperationException)
            {
                ISBNExistMessage();
                iSBN = WhichISBN13ForBookTable(iSBN);

            }

            return iSBN;
        }


        private static void ISBNExistMessage()
        {
            Console.WriteLine("That ISBN13 doesn't exist!");
        }

        /// <summary>
        /// Checks if the Author ID in the author table exists
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
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
                IDDontExistMessage();
                answerID = EnterAuthorID(answerID);

            }

            return answerID;
        }

        private static void IDDontExistMessage()
        {
            Console.WriteLine("ID doesn't exist!");
        }

        /// <summary>
        /// Checks if the store ID exists
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns></returns>
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
                StoreDontExistMessage();
                storeID = WhichStore(storeID);

            }

            return storeID;
        }

        private static void StoreDontExistMessage()
        {
            Console.WriteLine("Store doesn't exist!");

        }

        private static void CurrentPriceMessage(decimal pris)
        {
            Console.WriteLine($"Insert higher than {pris}");
        }

        private static void CostOfBookMessage()
        {
            Console.Write("Enter the book's price: ");
        }

        private static void EnterDateMessage()
        {
            Console.Write("Enter date: ");
        }
        private static void ShowCurrentDateMessage()
        {
            Console.WriteLine($"Enter date as yyyy-mm-dd");
        }


        private static void EnterStoreID()
        {
            Console.Write("Enter store ID: ");
        }

        private static void EnterID()
        {
            Console.Write("To proceed enter the ID: ");
        }

        private static void InputInvalid()
        {
            Console.WriteLine("Input had invalid characters! ");
        }
        private static void TryAgain()
        {
            Console.Write("Try again: ");
        }
        private static void QuestionISBN13()
        {
            Console.Write("To proceed enter the ISBN: ");
        }
    }
}
