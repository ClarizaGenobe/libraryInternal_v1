﻿using System;
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
            //sets up database
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
            printInstructions();

            string userInput = Console.ReadLine();
            do
            {
                if (userInput == "U")
                {
                    Console.Clear();
                    insertUserData();
                }
                else if (userInput == "B")
                {
                    Console.Clear();
                    insertBookData();
                }
                else if (userInput == "V")
                {
                    Console.Clear();
                    viewDatabase();
                }
                else if (userInput == "S")
                {
                    Console.Clear();
                    searchDatabase();
                }
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    userInput = Console.ReadLine();
                }
                printInstructions();
                userInput = Console.ReadLine();
            }
            while (userInput == "U" | userInput == "B" | userInput == "V"
            | userInput == "S" | userInput == "E");
        }

        static public void printInstructions()
        {
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("Please choose your action:");
            Console.WriteLine("Type U and then ENTER if you would like to add a user to the " +
                "system.");
            Console.WriteLine("Type B and then ENTER if you would like to add a book to the " +
                "system.");
            Console.WriteLine("Type V and then ENTER if you would like to view the contents of " +
                "the database");
            Console.WriteLine("Type S and then ENTER if you would like to search the system.");
            Console.WriteLine("Type E and then ENTER if you would like to exit the system.");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
        }

        //METHOD to open connection
        static public void openConnection()
        {
            if (sqlite_conn.State != System.Data.ConnectionState.Open)
            {
                sqlite_conn.Open();
            }
        }

        //METHOD to close connection
        static public void closeConnection()
        {
            if (sqlite_conn.State != System.Data.ConnectionState.Closed)
            {
                sqlite_conn.Close();
            }
        }

        //METHOD to insert a new user
        static public void insertUserData()
        {
            //reads the data for a new user
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("You would like to enter a new user into the system. Enter first name, " +
                "last name, role, dob(yyyy-mm-dd), year level and password");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            string firstName = Console.ReadLine();
            string lastName = Console.ReadLine();
            string role = Console.ReadLine();
            DateTime dob = Convert.ToDateTime(Console.ReadLine());
            int yearLevel = Convert.ToInt32(Console.ReadLine());
            string password = Console.ReadLine();

            string query = "INSERT INTO User (first_name, last_name, role, dob, year_level, " +
                "password) VALUES (@first_name, @last_name, @role, @dob, @year_level, @password)";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);

            //opens connection, adds all the new data and closes connection
            openConnection();
            myCommand.Parameters.AddWithValue("@first_name", firstName);
            myCommand.Parameters.AddWithValue("@last_name", lastName);
            myCommand.Parameters.AddWithValue("@role", role);
            myCommand.Parameters.AddWithValue("@dob", dob);
            myCommand.Parameters.AddWithValue("@year_level", yearLevel);
            myCommand.Parameters.AddWithValue("@password", password);
            myCommand.ExecuteNonQuery();
            closeConnection();

            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("Type U and then ENTER to add another user.");
            Console.WriteLine("Type R and then ENTER to return to the menu.");
            Console.WriteLine("Type E and then ENTER to exit the program.");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");

            string userInput = Console.ReadLine();
            while(userInput != "U" | userInput != "R" | userInput != "E")
            {
                if (userInput == "U")
                {
                    Console.Clear();
                    insertUserData();
                }
                else if (userInput == "R")
                {
                    Console.Clear();
                    return;
                }
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                else
                {
                    Console.WriteLine("------------------------------------------------------------" +
                    "------------------------------------------------------------");
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    Console.WriteLine("------------------------------------------------------------" +
                    "------------------------------------------------------------");
                    userInput = Console.ReadLine();
                }
            }           
        }

        //METHOD to insert a new book
        static public void insertBookData()
        {
            //reads the data for a new user
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("You would like to enter a new book into the system. Enter title and " +
                "author.");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            string title = Console.ReadLine();
            string author = Console.ReadLine();

            //inserts the new data into the database
            string query = "INSERT INTO Book (title, author) VALUES (@title, @author)";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);

            //opens connection, adds all the new data and closes connection
            openConnection();
            myCommand.Parameters.AddWithValue("@title", title);
            myCommand.Parameters.AddWithValue("@author", author);
            myCommand.ExecuteNonQuery();
            closeConnection();

            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("Type B and then ENTER to add another book.");
            Console.WriteLine("Type R and then ENTER to return to the menu.");
            Console.WriteLine("Type E and then ENTER to exit the program.");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");

            string userInput = Console.ReadLine();
            while(userInput != "B" | userInput != "R" | userInput != "E")
            {
                if (userInput == "B")
                {
                    Console.Clear();
                    insertBookData();
                }
                else if (userInput == "R")
                {
                    Console.Clear();
                    return;
                }
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                else
                {
                    Console.WriteLine("------------------------------------------------------------" +
                    "------------------------------------------------------------");
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    Console.WriteLine("------------------------------------------------------------" +
                    "------------------------------------------------------------");
                    userInput = Console.ReadLine();
                }
            }           
        }

        //METHOD to view database 
        static public void viewDatabase()
        {
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("You would like to view the database.");
            Console.WriteLine("Type U and then ENTER if you would like to view Users.");
            Console.WriteLine("Type B and then ENTER if you would like to view Books.");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");

            string viewingChoice = Console.ReadLine();

            while(viewingChoice != "U" | viewingChoice != "B")
            {
                if (viewingChoice == "U")
                {
                    Console.Clear();
                    string query = "SELECT * FROM User";
                    SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                    openConnection();
                    SQLiteDataReader result = myCommand.ExecuteReader();
                    if (result.HasRows)
                    {
                        Console.WriteLine("Users:");
                        while (result.Read())
                        {
                            Console.WriteLine("FIRST NAME: " + result["first_name"] +
                                " LAST NAME: " + result["last_name"] + " ROLE: " +
                                result["role"] + " DOB: " + result["dob"] +
                                " YEAR LEVEL: " + result["year_level"] + " PASSWORD: "
                                + result["password"]);
                        }
                    }
                    closeConnection();
                }
                else if (viewingChoice == "B")
                {
                    Console.Clear();
                    string query = "SELECT * FROM Book";
                    SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                    openConnection();
                    SQLiteDataReader result = myCommand.ExecuteReader();
                    if (result.HasRows)
                    {
                        Console.WriteLine("Books:");
                        while (result.Read())
                        {
                            Console.WriteLine("TITLE: " + result["title"] +
                                " AUTHOR: " + result["author"]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("------------------------------------------------------------" +
                        "------------------------------------------------------------");
                        Console.WriteLine("Invalid input. Please type in your answer again.");
                        Console.WriteLine("------------------------------------------------------------" +
                        "------------------------------------------------------------");
                        viewingChoice = Console.ReadLine();
                    }
                    closeConnection();
                }
                Console.WriteLine("");
                Console.WriteLine("------------------------------------------------------------" +
                "------------------------------------------------------------");
                Console.WriteLine("Type V and then ENTER to continue viewing the database.");
                Console.WriteLine("Type R and then ENTER to return to the menu.");
                Console.WriteLine("Type E and then ENTER to exit the program.");
                Console.WriteLine("------------------------------------------------------------" +
                "------------------------------------------------------------");

                string userInput = Console.ReadLine();
                while (userInput != "V" | userInput != "R" | userInput != "E")
                {
                    if (userInput == "V")
                    {
                        Console.Clear();
                        viewDatabase();
                    }
                    else if (userInput == "R")
                    {
                        Console.Clear();
                        return;
                    }
                    else if (userInput == "E")
                    {
                        System.Environment.Exit(-1);
                    }
                    else
                    {
                        Console.WriteLine("------------------------------------------------------------" +
                        "------------------------------------------------------------");
                        Console.WriteLine("Invalid input. Please type in your answer again.");
                        Console.WriteLine("------------------------------------------------------------" +
                        "------------------------------------------------------------");
                        userInput = Console.ReadLine();
                    }
                }
            }
        }
       
        //METHOD to search database
        static public void searchDatabase()
        {
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("You would like to search the database. Please type in " +
                "your search:");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            string authorChosen = Console.ReadLine();
            string query = "SELECT * FROM Book WHERE author = @userSearch";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
            openConnection();
            myCommand.Parameters.AddWithValue("@userSearch", authorChosen);
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("Here are the search results for '" + authorChosen + "'");
                    Console.WriteLine("TITLE: " + result["title"] + ", AUTHOR: " + result["author"]);
                }
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("No search results");
            }
            closeConnection();

            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
            Console.WriteLine("Type S and then ENTER to continue searching.");
            Console.WriteLine("Type R and then ENTER to return to the menu.");
            Console.WriteLine("Type E and then ENTER to exit the program.");
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");

            string userInput = Console.ReadLine();
            while (userInput != "S" | userInput != "R" | userInput != "E")
            {
                if (userInput == "S")
                {
                    Console.Clear();
                    searchDatabase();
                }
                else if (userInput == "R")
                {
                    Console.Clear();
                    return;
                }
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("------------------------------------------------------------" +
                    "------------------------------------------------------------");
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    Console.WriteLine("------------------------------------------------------------" +
                    "------------------------------------------------------------");
                    userInput = Console.ReadLine();
                }
            }
        }
    }
}

