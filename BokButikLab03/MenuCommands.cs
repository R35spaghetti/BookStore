

using BokButikLab03.Data;
using BokButikLab03.Models;
using Microsoft.EntityFrameworkCore;
namespace BokButikLab03
{
    public static class MenuCommands
    {
        /// <summary>
        /// Lists the stock of books in each store
        /// </summary>
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
            Console.WriteLine($" Store ID: {butikId} - Store {butiksnamn}. Title: {titel} - Stock:  {lagerAntal}");
        }

        /// <summary>
        /// //Update current stock in store by adding more books
        /// </summary>
        /// <param name="bookTitle"></param>
        /// <param name="storeID"></param>
        /// <param name="bookAmount"></param>
        public static void UpdateBookStock(long bookTitle, int storeID, int bookAmount)
        {

            using var context = new Laboration2RBContext();
            var UpdateBookAmount = context.LagerSaldos
                .Where(StoresID => StoresID.ButikId == storeID)
                .SingleOrDefault(lagret => lagret.Isbn == bookTitle);


            //adds books to stock
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

        /// <summary>
        /// Adds a new author
        /// </summary>
        public static void AddNewAuthor()
        {
            Console.Clear();

           string Förnamn = "";
            EnterFirstNameMessage();
            Förnamn = UserInputs.StringInput(Förnamn);
          string  EfterNamn = "";
            EnterLastNameMessage();
            EfterNamn = UserInputs.StringInput(EfterNamn);
            DateTime Födelsedatum = new(1900,01,01);
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
            Console.WriteLine($" {förnamnet} {efternamnet} was added!");
        }

        /// <summary>
        /// Updates the stock by decreasing the stock
        /// </summary>
        /// <param name="bookTitle"></param>
        /// <param name="storeID"></param>
        /// <param name="BookAmount"></param>
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
                NoBooksExistMessage(storeID);
                return;
            }
        }

        private static void NoBooksExistMessage(int storeID)
        {
            Console.WriteLine($"No such book exists at StoreID {storeID}");
        }

        private static void RemoveBookMessage(int bookAmount, long bookTitle, int storeID)
        {
            Console.WriteLine($"Removed {bookAmount} books from StoreID {storeID} with the book title: {bookTitle}");
        }

        /// <summary>
        /// Removes a book title, using its ISBN as input
        /// </summary>
        public static void RemoveTheBook()
        {

            Console.Clear();

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
        /// <summary>
        /// Removes an author, using its ID as input
        /// </summary>
        public static void RemoveTheAuthor()
        {
            Console.Clear();

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
      
        /// <summary>
        /// Allows the user to edit the values of one selected author
        /// </summary>
        public static void EditTheAuthor()
        {
            Console.Clear();

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
                    ChangeWhatMessageForAuthors();
                
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
                        DateTime AddDate = new(1900, 01, 01);
                        AddDate = UserInputs.DateInput(AddDate);
                        foundName.Födelsedatum = AddDate;
                        var FoundDate = foundName.Födelsedatum;
                        DateSavedMessage(FoundDate);

                    }
                    context.SaveChanges();
                } while (strAnswer != "end");

            }

        }
     
        private static void DateSavedMessage(DateTime foundDate)
        {
            Console.WriteLine($"{foundDate} saved!");
        }

        private static void EnterNewDateMessage()
        {
            Console.WriteLine("Enter a new date: ");
        }

        private static void LastnameSavedMessage(string foundLastname)
        {
            Console.WriteLine($"{foundLastname} saved!");
        }

        private static void EnterLastNameMessage()
        {
            Console.WriteLine("Enter a new surname");
        }

        private static void FirstNameSavedMessage(string foundFirstName)
        {
            Console.WriteLine($"{foundFirstName} saved!");
        }

        private static void EnterFirstNameMessage()
        {
            Console.WriteLine("Enter a new forename");
        }

        private static void ChangeWhatMessageForAuthors()
        {
           
            Console.WriteLine("Type end to quit the edit mode \n" +
                "To edit the forename type: förnamn \n" +
                "To edit the surname type: efternamn \n" +
                "To edit the date of birth type: födelsedatum \n");
        }

        private static void InputPromptMessage()
        {
            Console.WriteLine("To start editing enter the ID");
        }
        /// <summary>
        /// Allows the user to edit the values of one selected book
        /// </summary>
        public static void EditTheBook()
        {
            Console.Clear();

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
                    ChangeWhatMessageForBooks();
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
                        DateTime AddDate = new(1900, 01, 01);
                        AddDate = UserInputs.DateInput(AddDate);
                     var NewDate =  foundName.Utgivningsdatum = AddDate;
                        DateUpdated(NewDate);

                    }
                    context.SaveChanges();
                } while (strAnswer != "end");

            }

        }

        private static void ChangeWhatMessageForBooks()
        {
           
            Console.WriteLine("Type end to quit the edit mode \n" +
                "To edit the title type: titel \n" +
                "To edit the ISBN type: isbn \n" +
                "To edit the language type: språk \n" +
                "To edit the price type: pris \n" +
                "To edit the release date type: utgivningsdatum");
        }

        private static void DateUpdated(DateTime newDate)
        {
            Console.WriteLine($"{newDate} saved!");
        }

        private static void GiveNewDateMessage()
        {
            Console.WriteLine("Enter a new release date:");
        }

        private static void SavedNewPrice(decimal updatedPrice)
        {
            Console.WriteLine($"{updatedPrice} saved!");
        }

        private static void GiveNewPriceMessage()
        {
            Console.WriteLine("Enter a new price: ");
        }

        private static void SavedLanguageMessage(string newLanguage)
        {
            Console.WriteLine($"{newLanguage} saved!");
        }

        private static void GiveNewLanguageMessage()
        {
            Console.WriteLine("Enter a new language ");
        }

        private static void SavedISBNMessage(long iSBN13NEW)
        {
            Console.WriteLine($"{iSBN13NEW} saved!");
        }

        private static void SavedFirstnameMessage(string firstName)
        {
            Console.WriteLine($"{firstName} saved!");
        }

        private static void GiveNewISBNMessage()
        {
            Console.WriteLine("Enter a new ISBN13 (exactly 13 characters");
        }

        private static void ChangeTitleMessage()
        {
            Console.Write("Enter a new title: ");
        }

        private static void EnterISBNMessage()
        {
            Console.WriteLine("To start editing enter the ISBN13");
        }
        /// <summary>
        /// Shows all the authors
        /// </summary>
        public static void ShowAllAuthors()
        {
            Console.Clear();

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
            Console.WriteLine($"ID: {författarensID} | {förnamnet} {efternamnet}");
        }
        /// <summary>
        /// Shows all the Books
        /// </summary>
        public static void ShowAllISBNs()
        {
            Console.Clear();

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
        /// Add new book titles, applies later to existing authors
        /// </summary>
        public static void AddNewBookTitle(int answerID)
        {
          
            EnterBookTitleMessage();

        {   long ISBN = 0;
            ISBN = UserInputs.AddAnotherISBN(ISBN);

                string BookTitel = "";
                EnterNEWBookTitleMessage();
                BookTitel = UserInputs.StringInput(BookTitel);

                string Språk = "";
                Console.Write("Enter language:");
                Språk = UserInputs.StringInput(Språk);

            decimal Pris = 0;
            Pris = UserInputs.PriceInput(Pris);

            DateTime AddDate = new(1900, 01, 01);
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
                    Titel = BookTitel,
                    Språk = Språk,
                    Pris = Pris,
                    Utgivningsdatum = AddDate,


                };
                //Adds to the junction table
                newBookTitle.Författares.Add(foundAuthor);
                // TODO krasch om flera författare läggs till, en bok med flera författare

                context.Add(newBookTitle);
                SavedNewBookMessage(newBookTitle);
                context.SaveChanges();

            }




        }

        private static void EnterNEWBookTitleMessage()
        {
            Console.Write("Enter title:");
        }

        private static void SavedNewBookMessage(Böcker newBookTitle)
        {
            Console.WriteLine($"The book {newBookTitle.Titel} was added!");
        }

        private static void EnterBookTitleMessage()
        {
            Console.WriteLine("Creating a new book title.");
        }

   
    }
}

