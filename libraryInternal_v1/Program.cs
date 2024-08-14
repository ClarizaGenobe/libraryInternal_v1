﻿using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Reflection.Emit;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace libraryInternal_v1
{
    internal class Program
    {
        static SQLiteConnection sqlite_conn;

        static void Main(string[] args)
        {
            //sets up database connection
            sqlite_conn = new SQLiteConnection("Data Source=Library.sqlite3");
          
            if (!File.Exists("./Library.sqlite3"))
            {
                SQLiteConnection.CreateFile("Library.sqlite3");
                Console.WriteLine("Database created");
            }
            else
            {
                Console.WriteLine("Database exists already");
            }
            Console.Clear();

            //calls ActionChoice method
            ActionChoice();
        }


        //METHOD for user to pick what they would like to do
        static public void ActionChoice()
        {
            do
            {
                //METHOD prints instructions
                PrintInstructions();
                //asks for user input
                string userInput = Console.ReadLine().ToUpper();

                //ISSUE BOOK 
                if (userInput == "I")
                {
                    Console.Clear();
                    IssueBook();
                }

                //ADD USER
                if (userInput == "U")
                {
                    Console.Clear();
                    InsertUserData();
                }
                //ADD BOOK
                else if (userInput == "B")
                {
                    Console.Clear();
                    InsertBookData();
                }
                //VIEW
                else if (userInput == "V")
                {
                    Console.Clear();
                    ViewDatabase();
                }
                //SEARCH
                else if (userInput == "S")
                {
                    Console.Clear();
                    SearchDatabase();
                }
                //DELETE
                /*else if (userInput == "D")
                {
                    Console.Clear();
                    DeleteEntry();
                }*/
                //EXIT
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                //INVALID INPUT
                else
                {
                    Console.Clear();
                    PrintLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    PrintLines();
                }
            } while (true);
        }

        //METHOD prints lines
        static public void PrintLines()
        {
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
        }

        //METHOD to ask user what action they would like to take
        static public void PrintInstructions()
        {
            PrintLines();
            Console.WriteLine("Please choose your action:");
            Console.WriteLine("Type I and then ENTER if you would like to issue a book.");
            Console.WriteLine("");
            Console.WriteLine("Type U and then ENTER if you would like to add a user to the " +
                "system.");
            Console.WriteLine("Type B and then ENTER if you would like to add a book to the " +
                "system.");
            Console.WriteLine("");
            Console.WriteLine("Type V and then ENTER if you would like to view the contents of " +
                "the database");
            Console.WriteLine("");
            Console.WriteLine("Type S and then ENTER if you would like to search the system.");
            Console.WriteLine("");
            //Console.WriteLine("Type D and then ENTER if you would like to delete an entry.");
            Console.WriteLine("Type E and then ENTER if you would like to exit the system.");
            PrintLines();
        }

        //METHOD to open connection
        static public void OpenConnection()
        {
            if (sqlite_conn.State != System.Data.ConnectionState.Open)
            {
                sqlite_conn.Open();
            }
        }

        //METHOD to close connection
        static public void CloseConnection()
        {
            if (sqlite_conn.State != System.Data.ConnectionState.Closed)
            {
                sqlite_conn.Close();
            }
        }

        //METHOD to issue book
        static public void IssueBook()
        {
            PrintLines();
            Console.WriteLine("You would like to issue a book.");
            PrintLines();

            PrintUsers();
            Console.WriteLine("");
            PrintLines();
            Console.WriteLine("Type in the ID of the USER you would like to issue a book to.");
            PrintLines();

            //reads chosen user id
            //int chosenUser = Convert.ToInt32(Console.ReadLine());

            PrintBooks();
            Console.WriteLine("");
            PrintLines();
            Console.WriteLine("Type in the ID of the BOOK you would like to issue out.");
            PrintLines();

            //reads chosen book id
            //int chosenBook = Convert.ToInt32(Console.ReadLine());

            //finding the user in the database
            string query = "SELECT Book.title, Book.author * FROM USER * " +
                "JOIN User_Book ON (User.user_id = User_Book.user_id) * " +
                "JOIN Book ON (User_Book.book_id = Book.book_id * " +
                "WHERE User.user_id = @user_id";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
            OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();

            //prints out user books
            if (result.HasRows)
            {
                Console.WriteLine("User Books:");
                while (result.Read())
                {
                    Console.WriteLine("ID: " + result["user_book_id"] + " USER ID: " +
                        result["user_id"] + " BOOK ID: " + result["book_id"]);
                }
            }
            CloseConnection();
        }

        //METHOD to insert a new user
        static public void InsertUserData()
        {
            PrintLines();
            Console.WriteLine("You would like to enter a new user into the system.");
            PrintLines();

            //reads the data for a new user
            Console.WriteLine("First name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine("Last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine("Role (either S or T):");
            string role = Console.ReadLine().ToUpper();
            Console.WriteLine("");

            //setting year levels depending on if student or teacher or invalid input
            int yearLevel = 0;
            while (true)
            {
                if (role == "S")
                {
                    Console.WriteLine("Year Level:");
                    yearLevel = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("");
                    break;
                }
                else if (role == "T")
                {
                    yearLevel = 0;
                    break;
                }
                else
                {
                    PrintLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    PrintLines();
                    role = Console.ReadLine().ToUpper();
                }
            }

            Console.WriteLine("Date of birth (yyyy-mm-dd):");
            DateTime dob = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("");

            Console.WriteLine("Password:");
            string password = Console.ReadLine();

            //putting the values into the database
            string query = "INSERT INTO User (first_name, last_name, role, dob, year_level, " +
                "password) VALUES (@first_name, @last_name, @role, @dob, @year_level, @password)";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);

            OpenConnection();
            myCommand.Parameters.AddWithValue("@first_name", firstName);
            myCommand.Parameters.AddWithValue("@last_name", lastName);
            myCommand.Parameters.AddWithValue("@role", role);
            myCommand.Parameters.AddWithValue("@dob", dob);
            myCommand.Parameters.AddWithValue("@year_level", yearLevel);
            myCommand.Parameters.AddWithValue("@password", password);
            myCommand.ExecuteNonQuery();
            CloseConnection();

            //after adding a user
            do
            {
                Console.WriteLine("");
                PrintLines();
                Console.WriteLine("Type U and then ENTER to add another user.");
                Console.WriteLine("Type R and then ENTER to return to the menu.");
                Console.WriteLine("Type E and then ENTER to exit the program.");
                PrintLines();

                //lets user choose whether to insert another user, go back to the menu or exit
                //also has an invalid input
                string userInput = Console.ReadLine().ToUpper();
                if (userInput == "U")
                {
                    Console.Clear();
                    InsertUserData();
                }
                else if (userInput == "R")
                {
                    Console.Clear();
                    ActionChoice();
                }
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                else
                {
                    Console.Clear();
                    PrintLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    PrintLines();
                }
            } while (true);
        }

        //METHOD to insert a new book
        static public void InsertBookData()
        {
            PrintLines();
            Console.WriteLine("You would like to enter a new book into the system.");
            PrintLines();

            //reads the data for a new user
            Console.WriteLine("Title:");
            string title = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("Author:");
            string author = Console.ReadLine();

            //inserts the new data into the database
            string query = "INSERT INTO Book (title, author) VALUES (@title, @author)";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);

            OpenConnection();
            myCommand.Parameters.AddWithValue("@title", title);
            myCommand.Parameters.AddWithValue("@author", author);
            myCommand.ExecuteNonQuery();
            CloseConnection();

            //after adding a book
            do
            {
                Console.WriteLine("");
                PrintLines();
                Console.WriteLine("Type B and then ENTER to add another book.");
                Console.WriteLine("Type R and then ENTER to return to the menu.");
                Console.WriteLine("Type E and then ENTER to exit the program.");
                PrintLines();

                //lets user choose whether to insert another book, return to menu or exit
                //invalid input 
                string userInput = Console.ReadLine().ToUpper();
                
                if (userInput == "B")
                {
                    Console.Clear();
                    InsertBookData();
                }
                else if (userInput == "R")
                {
                    Console.Clear();
                    ActionChoice();
                }
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                else
                {
                    Console.Clear();
                    PrintLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    PrintLines();
                }
            } while (true);
        }

        //METHOD to print Users
        static public void PrintUsers()
        {
            //connects to database and selects users
            Console.Clear();
            string query = "SELECT * FROM User";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
            OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();

            //prints out users
            if (result.HasRows)
            {
                Console.WriteLine("Users:");
                while (result.Read())
                {
                    Console.WriteLine("ID: " + result["user_id"] + " FIRST NAME: " +
                        result["first_name"] + " LAST NAME: " + result["last_name"]
                        + " ROLE: " + result["role"] + " DOB: " + result["dob"] +
                        " YEAR LEVEL: " + result["year_level"] + " PASSWORD: "
                        + result["password"]);
                }
            }
            CloseConnection();
        }

        //METHOD to print Books
        static public void PrintBooks()
        {
            //connects to database and selects books
            Console.Clear();
            string query = "SELECT * FROM Book";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
            OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();

            //prints out books
            if (result.HasRows)
            {
                Console.WriteLine("Books:");
                while (result.Read())
                {
                    Console.WriteLine("ID: " + result["book_id"] + " TITLE: "
                        + result["title"] + " AUTHOR: " + result["author"]);
                }
            }
            CloseConnection();
        }


        //METHOD to view database 
        static public void ViewDatabase()
        {
            PrintLines();
            Console.WriteLine("You would like to view the database.");
            Console.WriteLine("Type U and then ENTER if you would like to view Users.");
            Console.WriteLine("Type B and then ENTER if you would like to view Books.");
            PrintLines();

            //gets user input
            string userInput = Console.ReadLine().ToUpper();
            do
            {
                //USER
                if (userInput == "U")
                {
                    PrintUsers();
                }
                //BOOK
                else if (userInput == "B")
                {
                    PrintBooks();
                }
                //INVALID INPUT
                else
                {
                    Console.Clear();
                    PrintLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    PrintLines();

                    //calls view database method
                    ViewDatabase();
                }

                //after viewing database
                do
                {
                    Console.WriteLine("");
                    PrintLines();
                    Console.WriteLine("Type V and then ENTER to continue viewing the database.");
                    Console.WriteLine("Type R and then ENTER to return to the menu.");
                    Console.WriteLine("Type E and then ENTER to exit the program.");
                    PrintLines();

                    //get user input
                    userInput = Console.ReadLine().ToUpper();

                    //lets user choose to view the database again, return to main menu or exit
                    //invalid input too
                    if (userInput == "V")
                    {
                        Console.Clear();
                        ViewDatabase();
                    }
                    else if (userInput == "R")
                    {
                        Console.Clear();
                        ActionChoice();
                    }
                    else if (userInput == "E")
                    {
                        System.Environment.Exit(-1);
                    }
                    else
                    {
                        Console.Clear();
                        PrintLines();
                        Console.WriteLine("Invalid input. Please type in your answer again.");
                        PrintLines();
                    }
                } while (true);               
            } while (true);
        }

        //METHOD to search database
        static public void SearchDatabase()
        {
            PrintLines();
            Console.WriteLine("You would like to search the database.");
            Console.WriteLine("Type U and then ENTER if you would like to search for a USER");
            Console.WriteLine("Type B and then ENTER if you would like to search for a BOOK");
            PrintLines();

            //gets user input
            string userInput = Console.ReadLine().ToUpper();

            while (true)
            {
                //USER
                if (userInput == "U")
                {
                    Console.Clear();
                    do
                    {
                        PrintLines();
                        Console.WriteLine("You would like to search for a USER.");
                        Console.WriteLine("Type R and then ENTER if you would like to search " +
                            "for a ROLE");
                        Console.WriteLine("Type Y and then ENTER if you would like to search " +
                            "for a YEAR LEVEL");
                        Console.WriteLine("Type I and then ENTER if you would like to search " +
                            "for an ID");
                        PrintLines();

                        //gets user input 
                        userInput = Console.ReadLine().ToUpper();

                        //ROLE
                        if (userInput == "R")
                        {
                            Console.Clear();
                            PrintLines();
                            Console.WriteLine("You would like to search for a ROLE. Please" +
                                " input your search:");
                            PrintLines();

                            //gets the users search
                            string roleChosen = Console.ReadLine();
                            //finds it in the database
                            string query = "SELECT * FROM User WHERE role = @userSearch";
                            //opens connection and finds all the results that match
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            OpenConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", roleChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            //prints out results
                            if (result.HasRows)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Here are the search results for '" + roleChosen + "'");
                                while (result.Read())
                                {
                                    Console.WriteLine("ID: " + result["user_id"] + " " +
                                        " FIRST NAME: " + result["first_name"] +
                                        " LAST NAME: " + result["last_name"] + " ROLE: " +
                                        result["role"] + " DOB: " + result["dob"] +
                                        " YEAR LEVEL: " + result["year_level"] + " PASSWORD: "
                                        + result["password"]);
                                }
                                break;
                            }
                            //if no results
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                                break;
                            }
                        }
                        //YEAR LEVEL
                        else if (userInput == "Y")
                        {
                            Console.Clear();
                            PrintLines();
                            Console.WriteLine("You would like to search for a YEAR LEVEL. Please" +
                                " input your search:");
                            PrintLines();

                            //gets users search
                            string yearLevelChosen = Console.ReadLine();
                            //finds it in the database
                            string query = "SELECT * FROM User WHERE year_level = @userSearch";
                            //opens connection and finds results
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            OpenConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", yearLevelChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            //prints out results
                            if (result.HasRows)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Here are the search results for '" + yearLevelChosen + "'");
                                while (result.Read())
                                {
                                    Console.WriteLine("ID: " + result["user_id"] + " FIRST NAME: " + result["first_name"] +
                                        " LAST NAME: " + result["last_name"] + " ROLE: " +
                                        result["role"] + " DOB: " + result["dob"] +
                                        " YEAR LEVEL: " + result["year_level"] + " PASSWORD: "
                                        + result["password"]);
                                }
                                break;
                            }
                            //if no results
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                                break;
                            }
                        }
                        //ID
                        else if (userInput == "I")
                        {
                            Console.Clear();
                            PrintLines();
                            Console.WriteLine("You would like to search for an ID. Please" +
                                " input your search:");
                            PrintLines();

                            //gets users search
                            string idChosen = Console.ReadLine();
                            //finds it in the database
                            string query = "SELECT * FROM User WHERE user_id = @userSearch";
                            //opens connection and finds results
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            OpenConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", idChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            //prints out results
                            if (result.HasRows)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Here are the search results for '" + idChosen + "'");
                                while (result.Read())
                                {
                                    Console.WriteLine("ID: " + result["user_id"] + " FIRST NAME: " + result["first_name"] +
                                        " LAST NAME: " + result["last_name"] + " ROLE: " +
                                        result["role"] + " DOB: " + result["dob"] +
                                        " YEAR LEVEL: " + result["year_level"] + " PASSWORD: "
                                        + result["password"]);
                                }
                                break;
                            }
                            //if no results
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                                break;
                            }
                        }
                        //INVALID INPUT
                        else
                        {
                            Console.Clear();
                            PrintLines();
                            Console.WriteLine("Invalid input. Please type in your answer again.");
                            PrintLines();
                        }
                    } while (true);
                }
                //BOOK
                else if (userInput == "B")
                {
                    Console.Clear();
                    do
                    {
                        PrintLines();
                        Console.WriteLine("You would like to search for a BOOK.");
                        Console.WriteLine("Type A and then ENTER if you would like to search " +
                            "for an AUTHOR");
                        Console.WriteLine("Type T and then ENTER if you would like to search " +
                            "for a TITLE");
                        PrintLines();

                        //gets user input
                        userInput = Console.ReadLine().ToUpper();

                        //AUTHOR
                        if (userInput == "A")
                        {
                            Console.Clear();
                            PrintLines();
                            Console.WriteLine("You would like to search for an AUTHOR. Please" +
                                " input your search:");
                            PrintLines();

                            //gets user's search
                            string authorChosen = Console.ReadLine();
                            //finds match in database
                            string query = "SELECT * FROM Book WHERE author = @userSearch";
                            //opens connection and finds results
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            OpenConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", authorChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            //print results
                            if (result.HasRows)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Here are the search results for '" + authorChosen + "'");
                                while (result.Read())
                                {
                                    Console.WriteLine("ID: " + result["book_id"] + " TITLE: " + result["title"] + ", AUTHOR: " + result["author"]);
                                }
                                break;
                            }
                            //no results
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                                break;
                            }
                        }
                        //TITLE
                        else if (userInput == "T")
                        {
                            Console.Clear();
                            PrintLines();
                            Console.WriteLine("You would like to search for a TITLE. Please" +
                                " input your search:");
                            PrintLines();

                            //get users search
                            string titleChosen = Console.ReadLine();
                            //finds it in the database
                            string query = "SELECT * FROM Book WHERE title = @userSearch";
                            //opens connection and finds results
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            OpenConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", titleChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            //prints results
                            if (result.HasRows)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Here are the search results for '" + titleChosen + "'");
                                while (result.Read())
                                {
                                    Console.WriteLine("ID: " + result["book_id"] + " TITLE: " + result["title"] + ", AUTHOR: " + result["author"]);
                                }
                                break;
                            }
                            //no search results
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                                break;
                            }
                        }
                        //INVALID INPUT
                        else
                        {
                            Console.Clear();
                            PrintLines();
                            Console.WriteLine("Invalid input. Please type in your answer again.");
                            PrintLines();
                        }
                    } while (true);
                }
                //INVALID INPUT
                else
                {
                    Console.Clear();
                    PrintLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    PrintLines();
                    SearchDatabase();
                }
                CloseConnection();

                do
                {
                    Console.WriteLine("");
                    PrintLines();
                    Console.WriteLine("Type S and then ENTER to continue searching.");
                    Console.WriteLine("Type R and then ENTER to return to the menu.");
                    Console.WriteLine("Type E and then ENTER to exit the program.");
                    PrintLines();

                    //gets user input
                    userInput = Console.ReadLine().ToUpper();

                    //lets user choose whether to make another search, return to menu or exit
                    //also invalid input
                    if (userInput == "S")
                    {
                        Console.Clear();
                        SearchDatabase();
                    }
                    else if (userInput == "R")
                    {
                        Console.Clear();
                        ActionChoice();
                    }
                    else if (userInput == "E")
                    {
                        System.Environment.Exit(-1);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("");
                        PrintLines();
                        Console.WriteLine("Invalid input. Please type in your answer again.");
                        PrintLines();
                    }
                } while (true);
            }
        }

        /*
        static public void DeleteEntry()
        {
            PrintLines();
            Console.WriteLine("You would like to delete an entry off the database.");
            Console.WriteLine("Type U and then ENTER if you would like to delete a USER");
            Console.WriteLine("Type B and then ENTER if you would like to delete a BOOK");
            PrintLines();

            //string userInput = Console.ReadLine();

            //if(userInput == "U")
            {
                int id = Convert.ToInt32(Console.ReadLine());

                string idChosen = Console.ReadLine();
                string query = "DELETE * FROM Book WHERE id = @userSearch";
                SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                openConnection();
                myCommand.Parameters.AddWithValue("@userSearch", idChosen);
                SQLiteDataReader result = myCommand.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        //Console.WriteLine("");
                        //Console.WriteLine("Here are the search results for '" + idChosen + "'");
                        //Console.WriteLine("TITLE: " + result["title"] + ", AUTHOR: " + result["author"]);
                    }
                    //break;
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("No search results");
                }

            }
            //else if(userInput == "B")
            {

            }
            //else
            {

            }
        }*/
    }
}