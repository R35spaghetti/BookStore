
using BokButikLab03.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using BokButikLab03;


/// <summary>
/// TODO: Redigera böcker
/// TODO: Redigera författare
/// TODO: Ta bort titlar (tabellen förlag behövs då den använder ISBN från böcker)
/// </summary>

{
    Console.WriteLine("Welcome! Press the corresponding buttons:" +
    " Show all press 1. | Add books press 2.| \n" +
    " Remove books press 3. | Add new book title press 4. |  Add new author press 5  \n" +
    "Delete a book, press 6 | Delete an author press 7");

    int StoreID =0, BookAmount =0;
    long ISBN =0;


    do
    {
        Question();
   var question = Console.ReadLine();
  

        switch (question)
        {
            case "1":
                MenuCommands.ListAll();
                break;

            case "2":
            ISBN =  WhichISBN(ISBN);
            StoreID = WhichStore(StoreID);
             BookAmount =  AmountOfBooks(BookAmount);
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

            default:
                Console.WriteLine("Unknown command. ");
                break;


        }

    } while (true);


    void Question()
    {
        Console.Write("> ");
    }
    

}

int UserIntInput(int answerID)
{
    Console.WriteLine("Enter ID: ");
    var answer = Console.ReadLine();
    answerID = int.Parse(answer);
    if (answer == null)
    {
        throw new Exception("Enter a value");
    }

    return answerID;
}

int AmountOfBooks(int bookAmount)
{
    Console.WriteLine("Enter amount of books: ");
    var answer = Console.ReadLine();
    bookAmount = int.Parse(answer);
    if (answer == null)
    {
        throw new Exception("Enter a value");
    }

    return bookAmount;
}

int WhichStore(int storeID)
{
    Console.WriteLine("Enter store ID: ");
    var answer = Console.ReadLine();
    storeID = int.Parse(answer);
    if (answer == null)
    {
        throw new Exception("Enter a value");
    }

    return storeID;
}

long WhichISBN(long ISBN)
{
    Console.WriteLine("Enter ISBN: ");
    var answer = Console.ReadLine();
    ISBN = long.Parse(answer);
    if (answer == null)
    {
        throw new Exception("Enter a value");
    }
    return ISBN;
}
/*

 Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Laboration2RB;Integrated Security=True;" Microsoft.EntityFrameWorkCore.SqlServer -Table LagerSaldo, Böcker, Författare, FörfattareBöcker, Butiker -ContextDir Data -OutputDir Models -DataAnnotations

*/


//Checks JSON-file
/*
var builder = new ConfigurationBuilder()
.AddJsonFile($"appsettings.json", true, true);
string connectionString =
builder.Build().GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String is: {connectionString}");
Console.ReadKey();
*/