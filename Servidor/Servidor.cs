using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Servidor;


namespace Servidor
{
    class Servidor
    {

        //Process the information received from Client
        public static String ParseQuery(String message)
        {

            Match match;
            string response = "ERROR Houston, we have a problem!";
            string res;

            string user;
            string password;
            string database;
            string task;
            string query;

            const string login = @"<login><user>([^<]+)</user><password>([^<]+)</password></login>";
            const string createAccount = @"<newuser><user>([^<]+)</user><password>([^<]+)</password></newuser>";
            const string data = @"<Query>([^<]+)</Query>";
            
            //Create a Account
            match = Regex.Match(message, createAccount);
            if (match.Success)
            {

                user = (string)match.Groups[1].Value;
                password = (string)match.Groups[2].Value;

                if (createNewUser(user, password)) 
                {
                    res = "Create Account Successful";
                }
                else 
                {
                    res = "ERROR";
                }
                       
                if (res.StartsWith("ERROR"))
                {
                    response = string.Format("<Answer><Error>{0}</Error></Answer>", res);
                }
                else
                {
                    response = string.Format("<Answer>{0}</Answer>", res);
                }
                return response;
            }
            //Perform the Login
            match = Regex.Match(message, login);
            if (match.Success)
            {

                user = (string)match.Groups[1].Value;
                password = (string)match.Groups[2].Value;

                if (porcessLogin(user, password))
                {
                    res = "Create Account Successful";
                }
                else
                {
                    res = "ERROR";
                }

                if (res.StartsWith("ERROR"))
                {
                    response = string.Format("<Answer><Error>{0}</Error></Answer>", res);
                }
                else
                {
                    response = string.Format("<Answer>{0}</Answer>", res);
                }
                return response;
            }
            
            //Execute the query
            match = Regex.Match(message, data);
            if (match.Success)
            {

                database = (string)match.Groups[1].Value;
                task = (string)match.Groups[2].Value;
                query = (string)match.Groups[1].Value;

                res = Execute.RunQuery(database, task, query);

                if (res.StartsWith("ERROR"))
                {
                    response = string.Format("<Answer><Error>{0}</Error></Answer>", res);
                }
                else
                {
                    response = string.Format("<Answer>{0}</Answer>", res);
                }
                return response;
            }
            
            return response;
        }
                        
        //process the login finfromation 
        static bool porcessLogin(string user, string password) {

            if (true) 
            {
                return true;
            }

        }

        //process the login finfromation 
        static bool createNewUser(string user, string password)
        {

            if (true)
            {
                return true;
            }   
        }
                
        public static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 8001.
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8001);
                
                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                String response = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Information received from Client: {0}", data);

                                        
                        // Process the data sent by the client.
                        response = ParseQuery(data);


                        byte[] msg = Encoding.ASCII.GetBytes(response);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Information sent to Client: {0}", response);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

        }

    }
}
