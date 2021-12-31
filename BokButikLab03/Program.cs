
using BokButikLab03;

/// <summary>
/// TODO: Check ISBN with current books in database, same with other keys
/// TODO: Priser skall endast kunna skrivas med siffror
/// TODO: try catch? på datuminmatning
/// </summary>
{

    int StoreID = 0, BookAmount = 0;
    long ISBN = 0;

    do
    {
        Console.WriteLine(" \n Welcome! Press the corresponding buttons:\n" +
   " Show all press 1. \n" +
   " Add books press 2. \n" +
   " Remove books press 3. \n" +
   " Add new book title press 4. \n" +
   " Add new author press 5 \n" +
   " Delete a book, press 6 \n" +
   " Delete an author press 7 \n" +
   " Edit the author press 8 \n" +
   " Edit the book press 9");

        Question();
        var question = Console.ReadLine();


        switch (question)
        {
            case "1":
                MenuCommands.ListAll();
                break;

            case "2":
                ISBN = WhichISBN(ISBN);
                StoreID = WhichStore(StoreID);
                BookAmount = AmountOfBooks(BookAmount);
                MenuCommands.AddBooks(ISBN, StoreID, BookAmount);
                break;

            case "3":
                ISBN = WhichISBN(ISBN);
                StoreID = WhichStore(StoreID);
                BookAmount = AmountOfBooks(BookAmount);
                MenuCommands.RemoveBooks(ISBN, StoreID, BookAmount);
                break;

            case "4":
                MenuCommands.ShowAuthors();
                int answerID = 0;
                answerID = UserIntInput(answerID);
                MenuCommands.AddNewBookTitle(answerID);
                break;

            case "5":
                MenuCommands.AddNewAuthor();
                break;

            case "6":
                MenuCommands.RemoveTheBook();
                break;

            case "7":
                MenuCommands.RemoveTheAuthor();
                break;

            case "8":
                MenuCommands.EditTheAuthor();
                break;

            case "9":
                MenuCommands.EditTheBook();
                break;

            default:
                Console.WriteLine("Unknown command. ");
                break;


        }

    } while (true);


    void Question()
    {
        Console.Write("> ");
    }


    int UserIntInput(int answerID)
    {
        Console.WriteLine("Enter ID: ");
        string? answer = Console.ReadLine();
        if (answer == null)
        {
            throw new Exception($"{answerID} doesn't exist");
        }
        answerID = int.Parse(answer);


        return answerID;
    }

    int AmountOfBooks(int bookAmount)
    {
        Console.WriteLine("Enter amount of books: ");
        string? answer = Console.ReadLine();
        if (answer == null)
        {
            throw new Exception($"{bookAmount} doesn't work");
        }
        bookAmount = int.Parse(answer);


        return bookAmount;
    }

    int WhichStore(int storeID)
    {
        Console.WriteLine("Enter store ID: ");
        string? answer = Console.ReadLine();
        if (answer == null)
        {
            throw new ArgumentException($"{storeID} doesn't exist");
        }
        storeID = int.Parse(answer);


        return storeID;
    }

    long WhichISBN(long ISBN)
    {



        Console.WriteLine("Enter ISBN: ");
        string? answer = Console.ReadLine();
        UserInputs.RegexCheckInput(answer);
        if (answer == null)
        {
            throw new Exception($"{ISBN} doesn't exist");
        }

        ISBN = long.Parse(answer);
        return ISBN;
    }
    }
