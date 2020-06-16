using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; 
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace adm
{
    class Client
    {

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Welcome! This is the Client application. You are going to communicate with the Server");
            Console.WriteLine("");
            bool notValid = true;
            bool notValidDB = true;
            bool notEnded = true;

            char answer;
            char answerDB;

            while (notEnded)
            {
                notValidDB = true;
                while (notValid)
                {
                    try
                    {
                        Console.Write("1) Login\n2) Create Account\n0) exit\n> ");
                        answer = Convert.ToChar(Console.ReadLine());
                        Console.WriteLine();

                        // Run Login function to loggin
                        if (answer == '1')
                        {
                            if (Login(false))
                            {
                                notValid = false;
                                Console.WriteLine("You are in!");
                            }
                            else
                            {
                                Console.WriteLine("\n Server down or credentials invalid...");
                            }
                        }
                        else if (answer == '2')
                        { // Registration, Login function with true variable
                            if (Login(true))
                            {
                                notValid = false;
                            }
                            else
                            {
                                Console.WriteLine("\n Server down or credentials invalid...");
                            }
                        }
                        else if (answer == '0')
                        { // Registration, Login function with true variable

                            Console.WriteLine("\n Ending connection...");
                            notValid = false;
                            notValidDB = false;
                            notEnded = false;
                            break;

                        }
                    }
                    catch
                    {
                        Console.WriteLine("\nInput error when login...");
                    }

                }

                while (notValidDB)
                {
                    try
                    {
                        Console.Write("1) Execute a query\n2) Create DataBase\n0) exit\n> ");
                        answer = Convert.ToChar(Console.ReadLine());
                        Console.WriteLine();

                        if (answer == '1')
                        {


                            if (SendDataBaseInfo())
                            {
                                Console.WriteLine("Great!!");
                            }
                            else
                            {
                                Console.WriteLine("\n ERROR...");
                            }

                            
                                

                        }
                        else if (answer == '2')
                        {
                            Console.WriteLine("\n We are working on that functionality my friend, Please, choose another option");
                        }
                        else if (answer == '0')
                        {

                            Console.WriteLine("\n Ending connection...");
                            notValid = false;
                            notValidDB = false;
                            notEnded = false;
                            break;

                        }
                    }
                    catch
                    {
                        Console.WriteLine("\nInput error when login...");
                    }




                }


                /*
                // Put main program from heren inside on this while loop
                while (notValidDB)
                {
                    try
                    {
                        Console.Write("1) show all Databases (Not functional) \n2) create Database Example\n3) processate of input text file with SQLs sentences\n0) <-Back\n> ");
                        answerDB = Convert.ToChar(Console.ReadLine());
                        Console.WriteLine();

                        // Run Database function to createDatabase
                        if (answerDB == '1')
                        {
                            if (database("showDatabases"))
                            {
                                notValidDB = false;
                            }
                            else
                            {
                                Console.WriteLine("\n Server down or error creating DB...");
                            }
                        }
                        else if (answerDB == '2')
                        { // Registration, Database function with true variable
                            if (database("createDataBaseExample"))
                            {
                                Console.WriteLine("\n Success creating DB...");
                                notValidDB = true;
                            }
                            else
                            {
                                Console.WriteLine("\n Server down or error creating DB...");
                            }
                        }
                        else if (answerDB == '3')
                        { // Process sql txt file
                            if (sqlProcessing("processSQL"))
                            {
                                Console.WriteLine("\n Success processing SQL...");
                                notValidDB = true;
                            }
                            else
                            {
                                Console.WriteLine("\n Server down or error...");
                            }
                        }
                        else if (answerDB == '0')
                        { // Registration, Login function with true variable

                            Console.WriteLine("\n Returning to previous screen...Please, press ENTER key");
                            notValid = true;
                            notValidDB = false;
                            notEnded = true;
                            break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("\nInput error when creating DB...");
                    }
                }
                */

                // Get options
                string option;
                // Validation
                while (true)
                {
                    try//
                    {
                        option = Console.ReadLine();
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Input error...");
                    }
                }
            }


        }

        static string GetPassword(string passMsg)
        {
            StringBuilder password = new StringBuilder("");
            bool passwordConfirmed = false;

            ConsoleKeyInfo curKey;

            // Hidding charcaters of password in console. Writting "*"  
            while (!passwordConfirmed)
            {
                Console.Write(passMsg);
                for (int i = 0; i < password.Length; i++) Console.Write("*");

                curKey = Console.ReadKey(true);

                if (curKey.Key.ToString() == "Enter")
                {
                    if (password.Length < 4 || password.Length > 30)
                    {
                        Console.WriteLine("\nPassword must have between 4 and 30 characters long");
                        continue;
                    }
                    passwordConfirmed = true;
                    continue;
                }
                else if (curKey.Key.ToString() == "Backspace" && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                }
                else if (curKey.Key.ToString() != "Backspace")
                {
                    password.Append(curKey.KeyChar);
                }

                Console.Clear();
            }
            return password.ToString();
        }

        
        
        static bool Login(bool newUser)
        {
            
            Regex answer = new Regex(@"<Answer>([^<]+)</Answer>");
            Regex error = new Regex(@"<Answer><Error>([^<]+)</Error></Answer>");


            string username = "";
            string password = "";
            string passwordConfirm = "";

            bool passwordValidated = false;

            // Introduce username
            Console.Write("Enter username: ");
            username = Console.ReadLine();

            // Introduce password two times
            while (!passwordValidated)
            {
                password = GetPassword("Enter password: ");
                Console.WriteLine();
                passwordConfirm = GetPassword("Enter password again: ");

                if (password == passwordConfirm)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nPassword doesn't match...");
                    continue;
                }
            }
            // Once user and password introduced, send them to server to try the login
            Console.WriteLine("\nInputs successful... Sending credentials to server...");

        https://stackoverflow.com/questions/9971722/xml-file-for-login-authentication

            string a;
            if (newUser == true) 
            {
                a = Connect(string.Format(string.Format("<newuser>Username={0} Password={1}</newuser>", username, password)));
            }
            else
            {
                a = Connect(string.Format(string.Format("<user>User={0} Password={1}</user>", username, password)));
            }

            

            if (answer.IsMatch(a)) {
                Console.WriteLine("11111");
                return true;

            }

           
            if (error.IsMatch(a)) {
                Console.WriteLine("2222");
                return false;


            }
            
            Console.WriteLine("33333");
            return false;
        
        }

        static bool SendDataBaseInfo() {

            Console.Write("Write the name of the DataBase: ");
            string databasename = Console.ReadLine();

            Console.Write("Specify the task: ");
            string task = Console.ReadLine();

            Console.Write("Write a query: ");
            string query = Console.ReadLine();
                                  
            string a = Connect(string.Format(string.Format("<Query>Database={0} Task{1} Query{2}</Query>", databasename, task, query)));

            Match match;
                        
            const string good = @"<Answer>([^<]+)</Answer>";
            const string error = @"<Answer><Error>([^<]+)</Error></Answer>";
            
            match = Regex.Match(a, good);
            if (match.Success)
            {
                string req = (string)match.Groups[1].Value;
                Console.WriteLine(req);
                return true;

            }

            match = Regex.Match(a, error);
            if (match.Success)
            {
                string req = (string)match.Groups[1].Value;
                Console.WriteLine(req);
                return false;


            }

            Console.WriteLine("33333");
            return false;
        }

        static String Connect(String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                //Int32 port = 13000;
                TcpClient client = new TcpClient("127.0.0.1", 8001);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sending information through method Connect...");

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();

            return null;

        }

        
        

    }
}


