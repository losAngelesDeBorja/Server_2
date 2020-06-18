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
                //first menu
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
                
                //second menu
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

        //get the password
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

        //Perform a login of a user, or create new user if parameter newUser is true 
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
                    
            string a;
            if (newUser) //if newUser is true,  create a new User in Server
            {

                a = wrapXML(string.Format("<user>{0}</user><password>{1}</password>", username, password), "newUser");
                a = Connect(a);
            }
            else
            {//if not, is a simple login

                a = wrapXML(string.Format("<user>{0}</user><password>{1}</password>", username, password), "login");
                a = Connect(a);

            }
            //make sure that non error occurred in Server during the execution
            if (errorTrat(a)) {
               
                return true;

            }
            
             return false;         
        }

        //Perform a query to the database specified 
        static bool SendDataBaseInfo() {

            Console.Write("Write the name of the DataBase: ");
            string databasename = Console.ReadLine();

            Console.Write("The lsit of suported operation are:\nSELECT\nDELETE\nUPDATE\nSpecify the task: ");
            string task = Console.ReadLine();

            Console.Write("Write a query: ");
            string query = Console.ReadLine();


            string a = wrapXML(string.Format("<Database>{0}</Database><Task>{1}</Task><Query>{2}</Query>", databasename, task, query), "query");
            a = Connect(a);

            //make sure that non error occurred in Server during the execution
            if (errorTrat(a)) {

                return true;

            }

            return false;
                                 
        }

        //Returns true if no errors are reported in the received XML
        static bool errorTrat(string message) {

            Match match1;
            Match match2;

            const string good = @"<Answer>([^<]+)</Answer>";
            const string error = @"<Answer><Error>([^<]+)</Error></Answer>";

            match1 = Regex.Match(message, good);
            match2 = Regex.Match(message, error);

            if (match1.Success && !match2.Success)
            {

                return true;

            }
            
            //If match the error, return the text of that error
            string req = (string)match1.Groups[1].Value;
            Console.WriteLine(req);
          

            return false;
        }

        //Encapsulates the information received in XML, according to the task parameter
        static String wrapXML(String message, String task) {

            
            string a;
            if (task.Equals("newUser"))
            {
                
                a = string.Format(string.Format("<newuser>{0}</newuser>", message));
                return a;
            }
            
            if(task.Equals("login"))
            {

                a = string.Format(string.Format("<login>{0}</login>", message));
                return a;
            }
            if (task.Equals("query"))
            {

                a = string.Format(string.Format("<Query>{0}</Query>", message));
                return a;
            }

            return null;

        }
        
        //Main method to send and received information from Server
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

                //this information must be oculted after the process of debugin finished
                Console.WriteLine("Sending information through method Connect" + ": " + message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                

                // Close everything.
                stream.Close();
                client.Close();

                //this information must be oculted after the process of debugin finished
                Console.WriteLine("The information received through method Connect is: " + responseData);
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


