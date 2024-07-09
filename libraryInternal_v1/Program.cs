using System;
using System.Data.Entity;
using System.Data.SQLite;
using System.Reflection.Emit;
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

            //calls insert Book Data method
            insertBookData();
            //calls show books method
            showBooks();

            //calls insert user data method
            //insertUserData();
            //calls show the users in the database method
            //showUsers();
        }     

        //opens connection
        static public void openConnection()
        {
            if (sqlite_conn.State != System.Data.ConnectionState.Open)
            {
                sqlite_conn.Open();
            }
        }

        //closes connection
        static public void closeConnection()
        {
            if (sqlite_conn.State != System.Data.ConnectionState.Closed)
            {
                sqlite_conn.Close();
            }
        }

        static public void showUsers()
        {
            string query = "SELECT * FROM User";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
            openConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("First Name: " + result["first_name"] + ", Last Name: " + 
                        result["last_name"] + ", Role: " + result["role"] + ", DOB: " + 
                        result["dob"] + ", Year Level: " + result["year_level"] + ", Password: " +
                        result["password"]);
                }
            }
            closeConnection();
        }

        static public void insertUserData()
        {
            //reads the data for a new user
            Console.WriteLine("Enter first name, last name, role, dob(yyyy-mm-dd), year level, " +
                "password");
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
        }
        static public void insertBookData()
        {
            //reads the data for a new user
            Console.WriteLine("Enter title and author.");
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
        }

        static public void showBooks()
        {
            //selects the data from Book
            string query = "SELECT * FROM Book";
            SQLiteCommand myCommand = new SQLiteCommand(query, sqlite_conn);
            openConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("Title: " + result["title"] + ", Author: " + result["author"]);
                }
            }
            closeConnection();
        }
    }
}
