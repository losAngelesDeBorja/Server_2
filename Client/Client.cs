using System;
using System.Text;
using System.Net.Sockets;

namespace adm
{
    class Client
    {
        public const string Error = "ERROR: ";
        public const string SecurityIncorrectLogin = Error + "Incorrect login";
        public const string SecurityNotSufficientPrivileges = Error + "Not sufficient privileges";
        public const string SecurityProfileAlreadyExists = Error + "Security profile already exists";
        public const string SecurityUserAlreadyExists = Error + "Security user already exists";
        public const string SecurityProfileDoesNotExist = Error + "Security profile does not exist";
        public const string SecurityUserDoesNotExist = Error + "Security user does not exist";
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

        // Login 
        static bool Login(bool newUser)
        {
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
            Console.WriteLine("\nLogin inputs successful... Sending credentials to server...");
            return SendLoginInfo(username, password, newUser);
        }

        // Send login credential to server
        static bool SendLoginInfo(string username, string password, bool newUser)
        {
            TcpClient client;

            // Try connection with server
            try
            {
                client = new TcpClient("127.0.0.1", 1111); // CHange IP and PORT here if necessary
                Console.WriteLine("Credentials sent to server...");
            }
            catch
            {
                return false;
            }

            // Opening network stream with server
            NetworkStream netStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(username + "#.*;#" + password + "#.*;#" + newUser);

            // Sending credentials and waiting for response.
            netStream.Write(bytesToSend, 0, bytesToSend.Length);

            // Reading response from server
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = netStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            client.Close();

            // If Server sends "true", client is logged in
            if (Encoding.ASCII.GetString(bytesToRead, 0, bytesRead).Substring(0, 4) == "True")
            {
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            bool notValid = true;
            char answer;

            // Loggin not successful
            while (notValid)
            {
                try
                {
                    Console.Write("1) Login\n2) Create Account\n> ");
                    answer = Convert.ToChar(Console.ReadLine());
                    Console.WriteLine();

                    // Run Login function to loggin
                    if (answer == '1')
                    {
                        if (Login(false))
                        {
                            notValid = false;
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
                }
                catch
                {
                    Console.WriteLine("\nInput error...");
                }
            }

            Console.WriteLine("You are in!");

            // Put main program from heren inside on this while loop
            while (true)
            {
                // Get message to use databases
                Console.Write("Databases: ");
                string msg = Console.ReadLine();

                // Get options
                string option;
                // Validation
                while (true)
                {
                    try//
                    {
                        Console.Write("Option: ");
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


    }
}

