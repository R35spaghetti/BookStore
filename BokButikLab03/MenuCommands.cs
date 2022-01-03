

using BokButikLab03.Data;
using BokButikLab03.Models;
using Microsoft.EntityFrameworkCore;
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
            Förnamn = UserInputs.StringInput(Förnamn);
          string  EfterNamn = "Ange efternamn";
            EfterNamn = UserInputs.StringInput(EfterNamn);
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
              ISBN = UserInputs.WhichISBN13ForBookTable(ISBN); 

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
                ID = UserInputs.EnterAuthorID(ID); 

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
      

        public static void EditTheAuthor()
        {
            ShowAllAuthors();
            InputPromptMessage();
            int answer = 0;
            string strAnswer = "";
            answer = UserInputs.EnterAuthorID(answer);

            using var context = new Laboration2RBContext();

            var foundName = context.Författares
                            .Single(f => f.Id == answer);


            if (foundName != null)
            {
                do
                {
                    ChangeWhatMessage();
                    PromptIcon();
                    strAnswer = UserInputs.StringInput(strAnswer);

                    if (strAnswer.ToLower() == "förnamn")
                    {
                        EnterFirstNameMessage();
                        strAnswer = UserInputs.StringInput(strAnswer);
                        foundName.Förnamn = strAnswer;
                        var FoundFirstName = foundName.Förnamn;
                        FirstNameSavedMessage(FoundFirstName);
                    }

                    else if (strAnswer.ToLower() == "efternamn")
                    {
                        EnterLastNameMessage();
                        strAnswer = UserInputs.StringInput(strAnswer);
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
            answer = UserInputs.WhichISBN13ForBookTable(answer); 

            using var context = new Laboration2RBContext();
            
            var foundName = context.Böckers
                .First(f => f.Isbn13 == answer);

            if (foundName != null)
            {
                do
                {
                    ChangeWhatMessage();
                    InputPromptMessage();
                    strAnswer = UserInputs.StringInput(strAnswer);

                    if (strAnswer.ToLower() == "titel")
                    {
                        ChangeTitleMessage();
                        strAnswer = UserInputs.StringInput(strAnswer);
                         var firstName = foundName.Titel = strAnswer;
                        SavedFirstnameMessage(firstName);
                    }
                    else if (strAnswer.ToLower() == "isbn")
                    {

                        GiveNewISBNMessage();
                        answer = UserInputs.WhichISBN13ForBookTable(answer);
                      
                        var ISBN13NEW = foundName.Isbn13 = answer;
                        SavedISBNMessage(ISBN13NEW);
                    }

                    else if (strAnswer.ToLower() == "språk")
                    {
                        GiveNewLanguageMessage();
                        strAnswer = UserInputs.StringInput(strAnswer);
                        var newLanguage = foundName.Språk = strAnswer;
                        SavedLanguageMessage(newLanguage);

                    }
                    else if (strAnswer.ToLower() == "pris")
                    {
                        GiveNewPriceMessage();
                        newPrice = UserInputs.PriceInput(newPrice);
                       var UpdatedPrice = foundName.Pris = answer;
                        SavedNewPrice(UpdatedPrice);

                    }
                    else if (strAnswer.ToLower() == "utgivningsdatum")
                    {
                        GiveNewDateMessage();
                        DateTime AddDate = DateTime.Now;
                        AddDate = UserInputs.DateInput(AddDate);
                     var NewDate =  foundName.Utgivningsdatum = AddDate;
                        DateUpdated(NewDate);

                    }
                    context.SaveChanges();
                } while (strAnswer != "inget");

            }

        }

        private static void DateUpdated(DateTime newDate)
        {
            Console.WriteLine($"{newDate} sparad");
        }

        private static void GiveNewDateMessage()
        {
            Console.WriteLine("Ange nytt datum");
        }

        private static void SavedNewPrice(decimal updatedPrice)
        {
            Console.WriteLine($"{updatedPrice} sparad");
        }

        private static void GiveNewPriceMessage()
        {
            Console.WriteLine("Ange nytt pris");
        }

        private static void SavedLanguageMessage(string newLanguage)
        {
            Console.WriteLine($"{newLanguage} sparad");
        }

        private static void GiveNewLanguageMessage()
        {
            Console.WriteLine("Ange nytt språk");
        }

        private static void SavedISBNMessage(long iSBN13NEW)
        {
            Console.WriteLine($"{iSBN13NEW} sparad");
        }

        private static void SavedFirstnameMessage(string firstName)
        {
            Console.WriteLine($"{firstName} sparad");
        }

        private static void GiveNewISBNMessage()
        {
            Console.WriteLine("Ange nytt isbn13 (minst 13 tecken: ");
        }

        private static void ChangeTitleMessage()
        {
            Console.WriteLine("Ange ny titel");
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
            EnterBookTitleMessage();

        {   long ISBN = 0;
            ISBN = UserInputs.AddAnotherISBN(ISBN);

            string BookTitel = "Enter title name: ";
            string Språk = "Enter language: ";

            decimal Pris = 0;
            Pris = UserInputs.PriceInput(Pris);

            DateTime AddDate = DateTime.Now;
            AddDate = UserInputs.DateInput(AddDate);

                using var context = new Laboration2RBContext();
                //TODO vad händer om det krockar?
                var foundAuthor = context.Författares
                    .SingleOrDefault(author => author.Id == answerID);

                if (foundAuthor == null)
                {
                    throw new Exception("Author not found");
                }


                var newBookTitle = new Böcker
                {
                    Isbn13 = ISBN,
                    Titel = BookTitel = UserInputs.StringInput(BookTitel),
                    Språk = Språk = UserInputs.StringInput(Språk),
                    Pris = Pris,
                    Utgivningsdatum = AddDate,


                };
                //Adds to junction table
                newBookTitle.Författares.Add(foundAuthor);
                // TODO krasch om flera författare läggs till, om samma bok försöker läggas till igen 

                context.Add(newBookTitle);
                SavedNewBookMessage(newBookTitle);
                context.SaveChanges();

            }




        }

        private static void SavedNewBookMessage(Böcker newBookTitle)
        {
            Console.WriteLine($"Book {newBookTitle.Titel} added");
        }

        private static void EnterBookTitleMessage()
        {
            Console.WriteLine("Vilken bok vill du lägga till?");
        }

        public static void ShowAuthors()
        {
            using var dbContext = new Laboration2RBContext();
            foreach (var authors in dbContext.Författares.AsNoTracking())
            {
                Console.WriteLine($" FörfattareID: {authors.Id} - Namn: {authors.Förnamn} {authors.Efternamn}");
            }
        }


   
    }
}

