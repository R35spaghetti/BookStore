
using BokButikLab03;

/// <summary>
/// TODO: flera författare på en bok
/// </summary>

{

    int StoreID = 0, BookAmount = 0;
    long ISBN = 0;

    do
    {

        IntroMessage();
        IdleQuestion();

        var question = Console.ReadLine();


        switch (question)
        {
            case "1":
                MenuCommands.ListAll();
                break;

            case "2":
                MenuCommands.ShowAllISBNs();
                ISBN = UserInputs.WhichISBN13ForBookTable(ISBN);
                StoreID = UserInputs.WhichStore(StoreID);
                BookAmount = UserInputs.AmountOfBooks(BookAmount);
                MenuCommands.UpdateBookStock(ISBN, StoreID, BookAmount);
                break;

            case "3":
                MenuCommands.ShowAllISBNs();
                ISBN = UserInputs.WhichISBN13ForBookTable(ISBN);
                StoreID = UserInputs.WhichStore(StoreID);
                BookAmount = UserInputs.AmountOfBooks(BookAmount);
                MenuCommands.RemoveBooks(ISBN, StoreID, BookAmount);
                break;

            case "4":
                MenuCommands.ShowAllAuthors();
                int answerID = 0;
                answerID = UserInputs.EnterAuthorID(answerID);
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
                defaultMessage();
                break;


        }

    } while (true);

    void defaultMessage()
    {
        Console.WriteLine("Unknown command. ");
    }

    void IntroMessage()
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
    }

    void IdleQuestion()
    {
        Console.Write("> ");
    }
}