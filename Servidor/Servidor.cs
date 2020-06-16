using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Servidor;

//Link:
//https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=netframework-4.8

namespace Servidor
{
    class Servidor
    {


        public static Execute exe = new Execute();


        public static String ParseQuery(String message)
        {

            Match match;
            string response = "ERROR Houston, we have a problem!";
            string res;


            const string login = @"<user>([^<]+)</user>";
            const string createAccount = @"<newuser>([^<]+)</newuser>";
            const string query = @"<Query>([^<]+)</Query>";
            
                        
            

            match = Regex.Match(message, createAccount);
            if (match.Success)
            {

                string req = (string)match.Groups[1].Value;
                Console.WriteLine(req);
                //res = exe.RunQuery(req);
                res = "Create Account Successful";

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

            match = Regex.Match(message, login);
            if (match.Success)
            {

                string req = (string)match.Groups[1].Value;
                Console.WriteLine(req);
                //res = exe.RunQuery(req);

                res = "Login Successful";

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

            match = Regex.Match(message, query);
            if (match.Success)
            {

                string req = (string)match.Groups[1].Value;
                Console.WriteLine(req);
                res = exe.RunQuery(req);

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
            
           


            /*match = Regex.Match(message, connection);
            if (match.Success)
            {
                string database = (string)match.Groups[1].Value;
                string user = (string)match.Groups[2].Value;
                string password = (string)match.Groups[3].Value;

                //Console.WriteLine(database +" "+ user+ " " +password );
                string res = exe.Connect(database, user, password);


                if (exe.isConnected())
                {
                    response = "<Success/>";
                }
                else
                {
                    response = string.Format("<Error>{0}</Error>", res);
                }
                return response;
            }
            */

            return response;
        }

        
        
        //process the login finfromation 
        public bool porcessLogin() {

            if (true) {

                return true;
            
            }

            return false;
        }

        // Create a new db example
        /*static bool MakeNewDataBase(string dbInfo, string dbUser, List<Database> l)
        {
            bool uniqueDataBasename = true;
            Console.WriteLine("dbInfo: " + dbInfo);
            Console.WriteLine("dbUser: " + dbUser);

            foreach (Database d in l)
            {
                if (d.dbName == dbInfo)
                {
                    uniqueDataBasename = false;
                }
            }

            // Check if the DB does not exists

            // When db name exists, returns false
            if (!uniqueDataBasename)
            {
                return false;
            }
            // If it's true and does not exists, add db

            return true;
        }*/

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
                        Console.WriteLine("Received: {0}", data);

                                        
                        // Process the data sent by the client.
                        response = ParseQuery(data);


                        byte[] msg = Encoding.ASCII.GetBytes(response);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", response);
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
