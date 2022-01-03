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
        public static string? RegexCheckDecimalInput(string? input)
        {
            Regex Check = new(@"^([\d,\d]*)$"); //sluta och börja på decimal, tillåter att ha , i sig. Nej jag vill inte ha . just nu
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
                    input = Console.ReadLine();

                }
                
            } while (!result);
            return input;
        }
        public static string StringInput(string? usrInput)
        {
            do
            {

                Console.Write("> ");
                usrInput = Console.ReadLine();
            } while (usrInput == null || usrInput.Trim() == "");




            return usrInput;
        }

/* TODO TA BORT OM WHICHISBN13 FUNKAR ISTÄLLET
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
        */
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
            answer = RegexCheckNumberInput(answer);
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
                    throw new Exception("DateLine is null");
                }
            } while (!DateTime.TryParseExact(DateLine, "yyyy/mm/dd", null, System.Globalization.DateTimeStyles.None, out addDate));

            return addDate;
        }

      
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
                    throw new Exception("Input is null");
                }
                usrInput = RegexCheckDecimalInput(usrInput.Trim());
            } while (usrInput == null || usrInput == "" || usrInput == "," || usrInput == "0");
        

            pris = decimal.Parse(usrInput);

            return pris;
        }

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
            Console.WriteLine("ISBN already exist!");
        }

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
                ISBN13AlreadyExistMessage();
                iSBN = WhichISBN13ForBookTable(iSBN);

            }

            return iSBN;
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
                IDDontExistMessage();
                answerID = EnterAuthorID(answerID);

            }

            return answerID;
        }

        private static void IDDontExistMessage()
        {
            Console.WriteLine("ID doesn't exist");
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
                StoreDontExistMessage();
                storeID = WhichStore(storeID);

            }

            return storeID;
        }

        private static void StoreDontExistMessage()
        {
            Console.WriteLine("Store doesn't exist");

        }
/* TODO TA BORT OM ISBN13SÖKERFUNKAR
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
                ISBN13AlreadyExistMessage();
                iSBN = WhichISBN(iSBN);

            }

            return iSBN;
        }
*/
        private static void ISBN13AlreadyExistMessage()
        {
            Console.WriteLine("ISBN13 code doesn't exist!");
        }

        private static void CurrentPriceMessage(decimal pris)
        {
            Console.WriteLine($"Insert higher than {pris}");
        }

        private static void CostOfBookMessage()
        {
            Console.Write("Enter cost of book: ");
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
