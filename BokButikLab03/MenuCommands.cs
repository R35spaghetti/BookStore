

using BokButikLab03.Data;
using BokButikLab03.Models;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// TODO: Kunna ändra bokISBN?
/// </summary>
namespace BokButikLab03
{
    public static class MenuCommands
    {

        public static void ListAll()
        {
            using var dbContext = new Laboration2RBContext();
            foreach (var lager in dbContext.LagerSaldos.AsNoTracking()
                  .Include(lager => lager.Butik)
                  .Include(lager => lager.IsbnNavigation)

                  .OrderBy(lager => lager.ButikId))
            {
                var lagerAntal = lager.Antal;
                var titel = lager.IsbnNavigation.Titel;
                var butikId = lager.ButikId;
                var butiksnamn = lager.Butik.Butiksnamn;
                ListAllMessage(butikId, butiksnamn, titel, lagerAntal);
            }

        }

        private static void ListAllMessage(int butikId, string butiksnamn, string titel, int lagerAntal)
        {
            Console.WriteLine($" ButiksID: {butikId} - Butiken {butiksnamn} har boken {titel} med antal i lager:  {lagerAntal}");
        }

        //Update current book stock in store 
        public static void UpdateBookStock(long bookTitle, int storeID, int bookAmount)
        {
        

            using var context = new Laboration2RBContext();
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
                UpdateBookMessage(bookAmount, storeID, bookTitle);

            }


        }

        private static void UpdateBookMessage(int bookAmount, int storeID, long bookTitle)
        {
            Console.WriteLine($"{bookAmount} Books added to Store with ID: {storeID} with ISBN {bookTitle}");
        }

        //Add new book to the store
        static void AddNewBook(int ButikID, long ISBN, int Antal)
        {

             using Laboration2RBContext db = new();
            LagerSaldo n = new()
            {
                ButikId = ButikID,
                Isbn = ISBN,
                Antal = Antal


            };


            using var context = new Laboration2RBContext();

            db.LagerSaldos.Add(n);
            AddedNewBookMessage(ISBN, ButikID, Antal);
            db.SaveChanges();
            }

        private static void AddedNewBookMessage(long ISBN, int ButikID, int Antal)
        {
            Console.Write($"Book added with ISBN13: {ISBN}. At storeID {ButikID} with {Antal} books");

        }

        /// Add author
        public static void AddNewAuthor()
        {
           string Förnamn = "Ange förnamn";
            Förnamn = StringInput(Förnamn);
          string  EfterNamn = "Ange efternamn";
            EfterNamn = StringInput(EfterNamn);
            DateTime Födelsedatum = DateTime.Now;
            Födelsedatum = UserInputs.DateInput(Födelsedatum);

            using var context = new Laboration2RBContext();

            var newAuthor = new Författare
            {

                Förnamn = Förnamn,
                Efternamn = EfterNamn,
                Födelsedatum = Födelsedatum



            };


            context.Add(newAuthor);
            var förnamnet = newAuthor.Förnamn;
            var efternamnet = newAuthor.Efternamn;
            AddNewAuthorMessage(förnamnet, efternamnet);
            context.SaveChanges();



        }

        private static void AddNewAuthorMessage(string förnamnet, string efternamnet)
        {
            Console.WriteLine($" {förnamnet} {efternamnet} added!");
        }

        //Remove book stock
        public static void RemoveBooks(long bookTitle, int storeID, int BookAmount)
        {


            using var context = new Laboration2RBContext();


            var UpdateBookAmount = context.LagerSaldos
                .Where(StoresID => StoresID.ButikId == storeID)
                .SingleOrDefault(lagret => lagret.Isbn == bookTitle);
            if (UpdateBookAmount != null)
            {
                UpdateBookAmount.Antal -= BookAmount;
                context.SaveChanges();
                RemoveBookMessage(BookAmount, bookTitle, storeID);
            }

            else
            {
                throw new Exception("Book doesn't exist!");
            }
        }

        private static void RemoveBookMessage(int bookAmount, long bookTitle, int storeID)
        {
            Console.WriteLine($"Removing {bookAmount} amount of books from {bookTitle} removed from StoreID {storeID}");
        }

        //Remove a book based on ISBN
        public static void RemoveTheBook()
        {
            using (var context = new Laboration2RBContext())
            {
               long ISBN = 0;
                ShowAllISBNs();
           //TODO    ISBN = UserInputs.(ISBN); SÖKER I BOKTABELLEN

                var book = context.Böckers
                    .Include(b=>b.Författares)
                    .Include(b=>b.LagerSaldos)
                    .Include(b=>b.Förlags)
                    .Single(b => b.Isbn13 == ISBN);
                
                   
                context.Böckers.Remove(book);
                BookRemovedMessage();
                context.SaveChanges();

            };
        }

        private static void BookRemovedMessage()
        {
            Console.WriteLine("Book removed");
        }

        public static void RemoveTheAuthor()
        {
            using (var context = new Laboration2RBContext())
            {
                int ID = 0;
                ShowAllAuthors();
                ID = IntInput(ID); //TODO kolla med authormetoderna i userinputs

                var författare = context.Författares
                    .Include(b => b.BöckerIsbns)
                    
                    .Single(b => b.Id == ID);


                context.Författares.Remove(författare);
                AuthorRemovedMessage();
                context.SaveChanges();

            };
        }

        private static void AuthorRemovedMessage()
        {
            Console.WriteLine("Author removed");
        }
      

        //TODO: Kanske en metod för att hitta författare/isbn?
        public static void EditTheAuthor()
        {
            ShowAllAuthors();
            InputPromptMessage();
            int answer = 0;
            string strAnswer = "";
            answer = IntInput(answer);

            using var context = new Laboration2RBContext();

            var foundName = context.Författares
                            .Single(f => f.Id == answer);


            if (foundName != null)
            {
                do
                {
                    ChangeWhatMessage();
                    PromptIcon();
                    strAnswer = StringInput(strAnswer);

                    if (strAnswer.ToLower() == "förnamn")
                    {
                        EnterFirstNameMessage();
                        strAnswer = StringInput(strAnswer);
                        foundName.Förnamn = strAnswer;
                        var FoundFirstName = foundName.Förnamn;
                        FirstNameSavedMessage(FoundFirstName);
                    }

                    else if (strAnswer.ToLower() == "efternamn")
                    {
                        EnterLastNameMessage();
                        strAnswer = StringInput(strAnswer);
                        foundName.Efternamn = strAnswer;
                        var FoundLastname = foundName.Efternamn;
                        LastnameSavedMessage(FoundLastname);


                    }
                    else if (strAnswer.ToLower() == "födelsedatum")
                    {
                        EnterNewDateMessage();
                        DateTime AddDate = DateTime.Now;
                        AddDate = UserInputs.DateInput(AddDate);
                        foundName.Födelsedatum = AddDate;
                        var FoundDate = foundName.Födelsedatum;
                        DateSavedMessage(FoundDate);

                    }
                    context.SaveChanges();
                } while (strAnswer != "inget");

            }

        }

        private static void DateSavedMessage(DateTime foundDate)
        {
            Console.WriteLine($"{foundDate} sparad");
        }

        private static void EnterNewDateMessage()
        {
            Console.WriteLine("Ange nytt datum");
        }

        private static void LastnameSavedMessage(string foundLastname)
        {
            Console.WriteLine($"{foundLastname} sparad");
        }

        private static void EnterLastNameMessage()
        {
            Console.WriteLine("Ange nytt efternamn");
        }

        private static void FirstNameSavedMessage(string foundFirstName)
        {
            Console.WriteLine($"{foundFirstName} sparad");
        }

        private static void EnterFirstNameMessage()
        {
            Console.WriteLine("Ange nytt förnamn");
        }

        private static void PromptIcon()
        {
            Console.WriteLine("> ");
        }

        private static void ChangeWhatMessage()
        {
            Console.WriteLine("Vad vill du ändra? Skriv namn på det som du vill ändra, skriv inget för att avsluta");
        }

        private static void InputPromptMessage()
        {
            Console.WriteLine("Skriv ID på vem som du vill ändra");
        }

        public static void EditTheBook()
        {
            ShowAllISBNs();
            EnterISBNMessage();
            long answer = 0;
            string strAnswer = "";
            decimal newPrice = 0;
          //TODO  answer = InputISBN(answer); SÖKER I BÖCKERTABELLEN

            using var context = new Laboration2RBContext();

            var foundName = context.Böckers
                            .First(f => f.Isbn13 == answer);
            //TODO kraschar här då du inte har någon sök till just denna tabel än i USerinputs

            if (foundName != null)
            {
                do
                {
                    Console.WriteLine("Vad vill du ändra?");
                    Console.WriteLine("> ");
                    strAnswer = StringInput(strAnswer);

                    if (strAnswer.ToLower() == "titel")
                    {
                        Console.WriteLine("Ange ny titel");
                        strAnswer = StringInput(strAnswer);
                        foundName.Titel = strAnswer;
                        Console.WriteLine($"{foundName.Titel} sparad");
                    }
                    else if (strAnswer.ToLower() == "isbn")
                    {
                        do
                        {
                            Console.WriteLine("Ange nytt isbn13 (minst 13 tecken): ");
                       //TODO     answer = InputISBN(answer); //BOKTABELLEN
                        } while (answer == 13); //TODO ÄNDRA HÄR
                        foundName.Isbn13 = answer;
                        Console.WriteLine($"{foundName.Isbn13} sparad");
                    }

                    else if (strAnswer.ToLower() == "språk")
                    {
                        Console.WriteLine("Ange nytt språk");
                        strAnswer = StringInput(strAnswer);
                        foundName.Språk = strAnswer;
                        Console.WriteLine($"{foundName.Språk} sparad");

                    }
                    else if (strAnswer.ToLower() == "pris")
                    {
                        Console.WriteLine("Ange nytt pris");
                        newPrice = UserInputs.PriceInput(newPrice);
                        foundName.Pris = answer;
                        Console.WriteLine($"{foundName.Pris} sparad");

                    }
                    else if (strAnswer.ToLower() == "utgivningsdatum")
                    {
                        Console.WriteLine("Ange nytt datum");
                        DateTime AddDate = DateTime.Now;
                        AddDate = UserInputs.DateInput(AddDate);
                        foundName.Utgivningsdatum = AddDate;
                        Console.WriteLine($"{foundName.Utgivningsdatum} sparad");

                    }
                    context.SaveChanges();
                } while (strAnswer != "inget");

            }

        }

        private static void EnterISBNMessage()
        {
            Console.WriteLine("Skriv ISBN på vilken bok som du vill ändra");
        }

        private static void ShowAllAuthors()
        {
            using var dbContext = new Laboration2RBContext();
            foreach (var författare in dbContext.Författares.AsNoTracking()
                  .OrderBy(f => f.Id))
            {
                var FörfattarensID = författare.Id;
                var Förnamnet = författare.Förnamn;
                var Efternamnet = författare.Efternamn;
                ShowAllAuthorsMessage(FörfattarensID, Förnamnet, Efternamnet);
            }
        }

        private static void ShowAllAuthorsMessage(int författarensID, string förnamnet, string efternamnet)
        {
            Console.WriteLine($"{författarensID} | {förnamnet} {efternamnet}");
        }

        public static void ShowAllISBNs()
        {
            using var dbContext = new Laboration2RBContext();
            foreach (var isbn in dbContext.Böckers.AsNoTracking()
                  .OrderBy(bokens => bokens.Titel))
            {
                var titeln = isbn.Titel;
                var isbn13 = isbn.Isbn13;
                ShowAllISBNMessage(titeln, isbn13);
            }
        }

        private static void ShowAllISBNMessage(string titeln, long isbn13)
        {
            Console.WriteLine($"{titeln} | {isbn13}");
        }

        /// <summary>
        /// Lägga till nya titlar i sortimentet, kunna välja bland befintliga författare
        /// </summary>
        public static void AddNewBookTitle(int answerID)

        { 

            Console.WriteLine("Vilken bok vill du lägga till?");

        {   long ISBN = 0;
            ISBN = UserInputs.WhichISBN(ISBN);

            String BookTitel = "Enter title name: ";
            string Språk = "Enter language: ";

            decimal Pris = 0;
            Pris = UserInputs.PriceInput(Pris);

            DateTime AddDate = DateTime.Now;
            AddDate = UserInputs.DateInput(AddDate);

                using var context = new Laboration2RBContext();

                var foundAuthor = context.Författares
                    .SingleOrDefault(author => author.Id == answerID);

                if (foundAuthor == null)
                {
                    throw new Exception("Author not found");
                }


                var newBookTitle = new Böcker
                {
                    Isbn13 = ISBN,
                    Titel = BookTitel = StringInput(BookTitel),
                    Språk = Språk = StringInput(Språk),
                    Pris = Pris,
                    Utgivningsdatum = AddDate,


                };
                //Adds to junction table
                newBookTitle.Författares.Add(foundAuthor);


                context.Add(newBookTitle);
                Console.WriteLine($"Book {newBookTitle.Titel} added");
                context.SaveChanges();

            }




        }
        public static void ShowAuthors()
        {
            using var dbContext = new Laboration2RBContext();
            foreach (var authors in dbContext.Författares.AsNoTracking())
            {
                Console.WriteLine($" FörfattareID: {authors.Id} - Namn: {authors.Förnamn} {authors.Efternamn}");
            }
        }


  

     //TODO strings o intinput kvar. intinput kan behöva vara authorID check
        private static string StringInput(string? usrInput)
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
            Console.WriteLine($"Current input: {answer}");


            Console.WriteLine("Enter ID: ");
            string? Theanswer = Console.ReadLine();
            if (Theanswer == null)
            {
                throw new Exception("Enter a value");
            }

            answer = int.Parse(Theanswer);
         
            return answer;
        }
    }
}

