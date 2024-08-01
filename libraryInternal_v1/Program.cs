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

            do
            {
                printInstructions();
                string userInput = Console.ReadLine().ToUpper();

                //ADD USER
                if (userInput == "U")
                {
                    Console.Clear();
                    insertUserData(); 
                }
                //ADD BOOK
                else if (userInput == "B")
                {
                    Console.Clear();
                    insertBookData();
                }
                //VIEW
                else if (userInput == "V")
                {
                    Console.Clear();
                    viewDatabase();
                }
                //SEARCH
                else if (userInput == "S")
                {
                    Console.Clear();
                    searchDatabase();
                }
                //DELETE
                /*else if (userInput == "D")
                {
                    Console.Clear();
                    deleteEntry();
                }*/
                //EXIT
                else if (userInput == "E")
                {
                    System.Environment.Exit(-1);
                }
                else
                {
                    Console.Clear();
                    printLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    printLines();
                    //userInput = Console.ReadLine().ToUpper();
                }
            } while (true);
        }

        static public void printLines()
        {
            Console.WriteLine("------------------------------------------------------------" +
            "------------------------------------------------------------");
        }

        static public void printInstructions()
        {
            printLines();
            Console.WriteLine("Please choose your action:");
            Console.WriteLine("Type U and then ENTER if you would like to add a user to the " +
                "system.");
            Console.WriteLine("Type B and then ENTER if you would like to add a book to the " +
                "system.");
            Console.WriteLine("Type V and then ENTER if you would like to view the contents of " +
                "the database");
            Console.WriteLine("Type S and then ENTER if you would like to search the system.");
            Console.WriteLine("Type D and then ENTER if you would like to delete an entry.");
            Console.WriteLine("Type E and then ENTER if you would like to exit the system.");
            printLines();
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
            printLines();
            Console.WriteLine("You would like to enter a new user into the system. Enter first name, " +
                "last name, role, dob(yyyy-mm-dd), year level and password");
            printLines();

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

            do
            {
                Console.WriteLine("");
                printLines();
                Console.WriteLine("Type U and then ENTER to add another user.");
                Console.WriteLine("Type R and then ENTER to return to the menu.");
                Console.WriteLine("Type E and then ENTER to exit the program.");
                printLines();

                string userInput = Console.ReadLine().ToUpper();
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
                    Console.Clear();
                    printLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    printLines();
                    userInput = Console.ReadLine().ToUpper();
                }
            } while (true);
        }

        //METHOD to insert a new book
        static public void insertBookData()
        {
            //reads the data for a new user
            printLines();
            Console.WriteLine("You would like to enter a new book into the system. Enter title and " +
                "author.");
            printLines();
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

            do
            {
                Console.WriteLine("");
                printLines();
                Console.WriteLine("Type B and then ENTER to add another book.");
                Console.WriteLine("Type R and then ENTER to return to the menu.");
                Console.WriteLine("Type E and then ENTER to exit the program.");
                printLines();

                string userInput = Console.ReadLine().ToUpper();
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
                    Console.Clear();
                    printLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    printLines();
                    userInput = Console.ReadLine().ToUpper();
                }
            } while (true);
        }

        //METHOD to view database 
        static public void viewDatabase()
        {
            do
            {
                printLines();
                Console.WriteLine("You would like to view the database.");
                Console.WriteLine("Type U and then ENTER if you would like to view Users.");
                Console.WriteLine("Type B and then ENTER if you would like to view Books.");
                printLines();

                string userInput = Console.ReadLine().ToUpper();

                //USER
                if (userInput == "U")
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
                    break;
                }
                //BOOK
                else if (userInput == "B")
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
                    closeConnection();
                    break;
                }
                //INVALID INPUT
                else
                {
                    Console.Clear();
                    printLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    printLines();
                    //viewDatabase();
                    //userInput = Console.ReadLine().ToUpper();
                } 
            } while (true);


            do
            {
                Console.WriteLine("");
                printLines();
                Console.WriteLine("Type V and then ENTER to continue viewing the database.");
                Console.WriteLine("Type R and then ENTER to return to the menu.");
                Console.WriteLine("Type E and then ENTER to exit the program.");
                printLines();

                string userInput = Console.ReadLine().ToUpper();

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
                    Console.Clear();
                    printLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    printLines();
                    //userInput = Console.ReadLine().ToUpper();
                }
            } while (true);
        }

        //METHOD to search database
        static public void searchDatabase()
        {
            printLines();
            Console.WriteLine("You would like to search the database.");
            Console.WriteLine("Type U and then ENTER if you would like to search for a USER");
            Console.WriteLine("Type B and then ENTER if you would like to search for a BOOK");
            printLines();

            string userInput = Console.ReadLine().ToUpper();
            while (true)
            {
                //USER
                if (userInput == "U")
                {
                    Console.Clear();
                    printLines();
                    Console.WriteLine("You would like to search for a USER.");
                    Console.WriteLine("Type R and then ENTER if you would like to search " +
                        "for a ROLE");
                    Console.WriteLine("Type Y and then ENTER if you would like to search " +
                        "for a YEAR LEVEL");
                    printLines();

                    userInput = Console.ReadLine().ToUpper();
                    do
                    {
                        //ROLE
                        if (userInput == "R")
                        {
                            Console.Clear();
                            printLines();
                            Console.WriteLine("You would like to search for a ROLE. Please" +
                                " input your search:");
                            printLines();

                            string roleChosen = Console.ReadLine();
                            string query = "SELECT * FROM User WHERE role = @userSearch";
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            openConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", roleChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            if (result.HasRows)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Here are the search results for '" + roleChosen + "'");
                                while (result.Read())
                                {
                                    Console.WriteLine("FIRST NAME: " + result["first_name"] +
                                        " LAST NAME: " + result["last_name"] + " ROLE: " +
                                        result["role"] + " DOB: " + result["dob"] +
                                        " YEAR LEVEL: " + result["year_level"] + " PASSWORD: "
                                        + result["password"]);
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                            }
                        }
                        //YEAR LEVEL
                        else if (userInput == "Y")
                        {
                            Console.Clear();
                            printLines();
                            Console.WriteLine("You would like to search for a YEAR LEVEL. Please" +
                                " input your search:");
                            printLines();

                            string yearLevelChosen = Console.ReadLine();
                            string query = "SELECT * FROM User WHERE year_level = @userSearch";
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            openConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", yearLevelChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            if (result.HasRows)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Here are the search results for '" + yearLevelChosen + "'");
                                while (result.Read())
                                {
                                    Console.WriteLine("FIRST NAME: " + result["first_name"] +
                                        " LAST NAME: " + result["last_name"] + " ROLE: " +
                                        result["role"] + " DOB: " + result["dob"] +
                                        " YEAR LEVEL: " + result["year_level"] + " PASSWORD: "
                                        + result["password"]);
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                            }
                        }
                        else
                        {
                            Console.WriteLine("");
                            printLines();
                            Console.WriteLine("Invalid input. Please type in your answer again.");
                            printLines();
                            userInput = Console.ReadLine().ToUpper();
                        }
                    } while (userInput is not ("R" or "Y"));
                }
                //BOOK
                else if (userInput == "B")
                {
                    Console.Clear();
                    printLines();
                    Console.WriteLine("You would like to search for a BOOK.");
                    Console.WriteLine("Type A and then ENTER if you would like to search " +
                        "for an AUTHOR");
                    Console.WriteLine("Type T and then ENTER if you would like to search " +
                        "for a TITLE");
                    printLines();

                    userInput = Console.ReadLine().ToUpper();
                    do
                    {
                        //AUTHOR
                        if (userInput == "A")
                        {
                            Console.Clear();
                            printLines();
                            Console.WriteLine("You would like to search for an AUTHOR. Please" +
                                " input your search:");
                            printLines();

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
                                    Console.WriteLine("");
                                    Console.WriteLine("Here are the search results for '" + authorChosen + "'");
                                    Console.WriteLine("TITLE: " + result["title"] + ", AUTHOR: " + result["author"]);
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                            }
                        }
                        //TITLE
                        else if (userInput == "T")
                        {
                            Console.Clear();
                            printLines();
                            Console.WriteLine("You would like to search for a TITLE. Please" +
                                " input your search:");
                            printLines();

                            string titleChosen = Console.ReadLine();
                            string query = "SELECT * FROM Book WHERE title = @userSearch";
                            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
                            openConnection();
                            myCommand.Parameters.AddWithValue("@userSearch", titleChosen);
                            SQLiteDataReader result = myCommand.ExecuteReader();
                            if (result.HasRows)
                            {
                                while (result.Read())
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("Here are the search results for '" + titleChosen + "'");
                                    Console.WriteLine("TITLE: " + result["title"] + ", AUTHOR: " + result["author"]);
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("No search results");
                            }
                        }
                        else
                        {
                            Console.WriteLine("");
                            printLines();
                            Console.WriteLine("Invalid input. Please type in your answer again.");
                            printLines();
                            userInput = Console.ReadLine().ToUpper();
                        }
                    } while (userInput is not ("A" or "T"));
                }
                else
                {
                    Console.Clear();
                    printLines();
                    Console.WriteLine("Invalid input. Please type in your answer again.");
                    printLines();
                    searchDatabase();
                    //userInput = Console.ReadLine().ToUpper();
                }
                closeConnection();

                do
                {
                    printLines();
                    Console.WriteLine("Type S and then ENTER to continue searching.");
                    Console.WriteLine("Type R and then ENTER to return to the menu.");
                    Console.WriteLine("Type E and then ENTER to exit the program.");
                    printLines();

                    userInput = Console.ReadLine().ToUpper();
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
                        Console.Clear();
                        Console.WriteLine("");
                        printLines();
                        Console.WriteLine("Invalid input. Please type in your answer again.");
                        printLines();
                        //userInput = Console.ReadLine().ToUpper();
                    }
                } while (userInput is not ("S" or "R" or "E"));
            }
        }

        /*
        static public void deleteEntry()
        {
            printLines();
            Console.WriteLine("You would like to delete an entry off the database.");
            Console.WriteLine("Type U and then ENTER if you would like to delete a USER");
            Console.WriteLine("Type B and then ENTER if you would like to delete a BOOK");
            printLines();

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