using BokButikLab03.Data;
using BokButikLab03.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BokButikLab03
{
    public static class MenuCommands
    {

        public static void ListAll()
        {
            using (var dbContext = new Laboration2RBContext())
            {
                foreach (var lager in dbContext.LagerSaldos.AsNoTracking()
                      .Include(lager => lager.Butik)
                      .Include(lager => lager.IsbnNavigation)

                      .OrderBy(lager => lager.ButikId))
                {
                    var lagerAntal = lager.Antal;
                    var titel = lager.IsbnNavigation.Titel;
                    Console.WriteLine($" ButiksID: {lager.ButikId} - Butiken {lager.Butik.Butiksnamn} har boken {titel} med antal i lager:  {lagerAntal}");
                }

            }

        }

        //Update current book stock in store 
        public static void AddBooks(long bookTitle, int storeID, int bookAmount)
        {
            using (var context = new Laboration2RBContext())
            {
                var UpdateBookAmount = context.LagerSaldos
                    .Where(StoresID => StoresID.ButikId == storeID)
                    .SingleOrDefault(lagret => lagret.Isbn == bookTitle);

                //lägg till boken
                if (UpdateBookAmount == null)
                {
                    AddNewBook(storeID, bookTitle, bookAmount);
                    
                }
                else
                {
                    UpdateBookAmount.Antal += bookAmount;


                    context.SaveChanges();
                    Console.WriteLine($"{bookAmount} Books added to Store with ID: {storeID} with ISBN {bookTitle}"); //TODO Lägg till i en egen metod
               
                }
            }

        }

        //Add new book to store
        static void AddNewBook(int ButikID, long ISBN, int Antal)
        {
            using (Laboration2RBContext db = new())
            {
                LagerSaldo n = new()
                {
                    ButikId = ButikID,
                    Isbn = ISBN,
                    Antal = Antal


                };
                db.LagerSaldos.Add(n);
                Console.WriteLine($"Book added with ISBN13: {ISBN}. At storeID {ButikID} with {Antal} books"); //TODO ny metod som skriver meddelandet
                db.SaveChanges();
                //DbUpdateException

            }
        }
        /// Add author
 public static void AddNewAuthor ()
        {
           string Förnamn = "Ange förnamn";
            Förnamn = UserInputs(Förnamn);
          string  EfterNamn = "Ange efternamn";
            EfterNamn = UserInputs(EfterNamn);
            DateTime Födelsedatum = DateTime.Now;
            Födelsedatum = DateInput(Födelsedatum);

            using (var context = new Laboration2RBContext())
            {

                var newAuthor = new Författare
                {
                  
                  Förnamn = Förnamn,
                  Efternamn = EfterNamn,
                  Födelsedatum = Födelsedatum
                  


                };
       

                context.Add(newAuthor);
                Console.WriteLine($" {newAuthor.Förnamn} {newAuthor.Efternamn} added!");
                context.SaveChanges();
            }



        }
        //Remove book stock
        public static void RemoveBooks(long bookTitle, int storeID, int BookAmount)
        {


            using (var context = new Laboration2RBContext())
            {
            

                var UpdateBookAmount = context.LagerSaldos
                    .Where(StoresID => StoresID.ButikId == storeID)
                    .SingleOrDefault(lagret => lagret.Isbn == bookTitle);
                if (UpdateBookAmount != null)
                {
                    UpdateBookAmount.Antal -= BookAmount;
                    context.SaveChanges();
                    Console.WriteLine($"Removing {BookAmount} amount of books from {bookTitle} removed from StoreID {storeID}");
                }
                else
                {
                    throw new Exception("Book doesn't exist!");
                }



            }
        }
        //Remove a book based on ISBN
        public static void RemoveTheBook()
        {
            using (var context = new Laboration2RBContext())
            {
               long ISBN = 0;
                ShowAllISBNs();
               ISBN = InputISBN(ISBN);

                var book = context.Böckers
                    .Include(b=>b.Författares)
                    .Include(b=>b.LagerSaldos)
                    .Single(b => b.Isbn13 == ISBN);
                
                   
                context.Böckers.Remove(book);
                Console.WriteLine("Book removed");
                context.SaveChanges();

            };
        }
        public static void RemoveTheAuthor()
        {
            using (var context = new Laboration2RBContext())
            {
                int ID = 0;
                ShowAllAuthors();
                ID = IntInput(ID);

                var författare = context.Författares
                    .Include(b => b.BöckerIsbns)
                    
                    .Single(b => b.Id == ID);


                context.Författares.Remove(författare);
                Console.WriteLine("Author removed");
                context.SaveChanges();

            };
        }
        public static void EditTheAuthor()
        {
            ShowAllAuthors();
            Console.WriteLine("Skriv ID på vem som du vill ändra");
            int answer = 0;
            string strAnswer = "";
            answer = IntInput(answer);

            using (var context = new Laboration2RBContext())
            {

                var foundName = context.Författares
                                .Single(f => f.Id == answer);


                if (foundName != null)
                {
                    do
                    {
                        Console.WriteLine("Vad vill du ändra?");
                        Console.WriteLine("> ");
                        strAnswer = UserInputs(strAnswer);

                        if (strAnswer.ToLower() == "förnamn")
                        {
                            Console.WriteLine("Ange nytt förnamn");
                            strAnswer = UserInputs(strAnswer);
                            foundName.Förnamn = strAnswer;
                            Console.WriteLine($"{foundName.Förnamn} sparad");
                        }

                        else if (strAnswer.ToLower() == "efternamn")
                        {
                            Console.WriteLine("Ange nytt efternamn");
                            strAnswer = UserInputs(strAnswer);
                            foundName.Efternamn = strAnswer;
                            Console.WriteLine($"{foundName.Efternamn} sparad");

                        }
                        else if (strAnswer.ToLower() == "födelsedatum")
                        {
                            Console.WriteLine("Ange nytt datum");
                            DateTime AddDate = DateTime.Now;
                            AddDate = DateInput(AddDate);
                            foundName.Födelsedatum = AddDate;
                            Console.WriteLine($"{foundName.Födelsedatum} sparad");

                        }
                        context.SaveChanges();
                    } while (strAnswer != "inget");

                }

            }

        }
        public static void EditTheBook()
        {
            ShowAllISBNs();
            Console.WriteLine("Skriv ISBN på vilken bok som du vill ändra");
            long answer = 0;
            string strAnswer = "";
            decimal newPrice = 0;
            answer = InputISBN(answer);

            using (var context = new Laboration2RBContext())
            {

                var foundName = context.Böckers
                                .First(f => f.Isbn13 == answer);


                if (foundName != null)
                {
                    do
                    {
                        Console.WriteLine("Vad vill du ändra?");
                        Console.WriteLine("> ");
                        strAnswer = UserInputs(strAnswer);

                        if (strAnswer.ToLower() == "titel")
                        {
                            Console.WriteLine("Ange ny titel");
                            strAnswer = UserInputs(strAnswer);
                            foundName.Titel = strAnswer;
                            Console.WriteLine($"{foundName.Titel} sparad");
                        }
                        else if (strAnswer.ToLower() == "isbn")
                        {
                            do
                            {
                                Console.WriteLine("Ange nytt isbn13 (minst 13 tecken): ");
                                answer = InputISBN(answer);
                            } while(answer == 13);
                            foundName.Isbn13 = answer;
                            Console.WriteLine($"{foundName.Isbn13} sparad");
                        }

                        else if (strAnswer.ToLower() == "språk")
                        {
                            Console.WriteLine("Ange nytt språk");
                            strAnswer = UserInputs(strAnswer);
                            foundName.Språk = strAnswer;
                            Console.WriteLine($"{foundName.Språk} sparad");

                        }
                        else if (strAnswer.ToLower() == "pris")
                        {
                            Console.WriteLine("Ange nytt pris");
                            newPrice = PriceInput(newPrice);
                            foundName.Pris = answer;
                            Console.WriteLine($"{foundName.Pris} sparad");

                        }
                        else if (strAnswer.ToLower() == "utgivningsdatum")
                        {
                            Console.WriteLine("Ange nytt datum");
                            DateTime AddDate = DateTime.Now;
                            AddDate = DateInput(AddDate);
                            foundName.Utgivningsdatum = AddDate;
                            Console.WriteLine($"{foundName.Utgivningsdatum} sparad");

                        }
                        context.SaveChanges();
                    } while (strAnswer != "inget");

                }

            }

        }

   
        private static void ShowAllAuthors()
        {
            using (var dbContext = new Laboration2RBContext())
            {
                foreach (var författare in dbContext.Författares.AsNoTracking()
                      .OrderBy(f => f.Id))
                {
                    Console.WriteLine($"{författare.Id} | {författare.Förnamn} {författare.Efternamn}");
                }
            }
        }

        private static void ShowAllISBNs()
        {
            using (var dbContext = new Laboration2RBContext())
            {
                foreach (var isbn in dbContext.Böckers.AsNoTracking()
                      .OrderBy(bokens => bokens.Titel))
                {
                    Console.WriteLine($"{isbn.Titel} | {isbn.Isbn13}");
                }
            }
        }

        /// <summary>
        /// Lägga till nya titlar i sortimentet, kunna välja bland befintliga författare
        /// </summary>
        public static void AddNewBookTitle(int answerID)

        { 

            Console.WriteLine("Vilken bok vill du lägga till?");

        {   long ISBN = 0;
            ISBN = InputISBN(ISBN);

            String BookTitel = "Enter title name: ";
            string Språk = "Enter language: ";

            decimal Pris = 0;
            Pris = PriceInput(Pris);

            DateTime AddDate = DateTime.Now;
            AddDate = DateInput(AddDate);

            using (var context = new Laboration2RBContext())
                {   

                    var foundAuthor = context.Författares
                        .SingleOrDefault(author => author.Id == answerID);

                    if (foundAuthor == null)
                    {
                        throw new Exception("Author not found");
                    }


                    var newBookTitle = new Böcker
                    {
                        Isbn13 = ISBN,
                        Titel = BookTitel = UserInputs(BookTitel),
                        Språk = Språk = UserInputs(Språk),
                        Pris = Pris,
                        Utgivningsdatum = AddDate,


                    };
                    //Add to junction table, x de
                    newBookTitle.Författares.Add(foundAuthor);
                  
                
                    context.Add(newBookTitle);
                    Console.WriteLine($"Book {newBookTitle.Titel} added");
                    context.SaveChanges();

            }

            }




        }
        public static void ShowAuthors()
        {
            using (var dbContext = new Laboration2RBContext())
            {
                foreach (var authors in dbContext.Författares.AsNoTracking())
                {
                    Console.WriteLine($" FörfattareID: {authors.Id} - Namn: {authors.Förnamn} {authors.Efternamn}");
                }
            }
        }
    
        private static long InputISBN(long iSBN)
        {
            string usrInput = "";
            do
            {

                Console.WriteLine("Enter ISBN13: ");
                usrInput = Console.ReadLine();
                iSBN = long.Parse(usrInput);
            } while (usrInput == null && iSBN != 13);
        



            return iSBN;
        }

        private static DateTime DateInput(DateTime addDate)
        {
            string DateLine;
            do
            {
                Console.WriteLine("Enter date: ");
                DateLine = Console.ReadLine();
            } while (!DateTime.TryParseExact(DateLine, "yyyy/mm/dd", null, System.Globalization.DateTimeStyles.None, out addDate));
        
            return addDate;
        }

        private static decimal PriceInput(decimal pris)
        {
            string usrInput = "";
            do
            {

                Console.WriteLine("Enter cost of book: ");
                usrInput = Console.ReadLine();
            } while (usrInput == null);
         pris =  Decimal.Parse(usrInput);



            return pris;
        }

        private static string UserInputs(string usrInput)
        {
            Console.WriteLine(usrInput);
            do
            {

                Console.WriteLine("Enter value:> ");
                usrInput = Console.ReadLine();
            }while(usrInput == null);
          



            return usrInput;
        }
      private static  int IntInput(int answer)
        {
            Console.WriteLine("Enter ID: ");
            var Theanswer = Console.ReadLine();
            answer = int.Parse(Theanswer);
            if (Theanswer == null)
            {
                throw new Exception("Enter a value");
            }

            return answer;
        }
    }
}
