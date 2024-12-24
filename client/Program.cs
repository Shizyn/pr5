﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace client
{
    internal class Program
    {
        static IPAddress ServerIPAddress;
        static int ServerPort;
        static string ClientToken;
        static DateTime ClientDateConnection;
        static void Main(string[] args)
        {
            OnSettings();
            Thread tCheckToken = new Thread(CheckToken);
            tCheckToken.Start();
            while (true)
                SetCommand();
        }

        static void SetCommand()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string Command = Console.ReadLine();

            if (Command == "/config")
            {
                File.Delete(Directory.GetCurrentDirectory() + "/.config");
                OnSettings();
            }
            else if (Command == "/connect") ConnectServer();
            else if (Command == "/status") GetStatus();
            else if (Command == "/help") Help();
        }

        static void GetStatus()
        {
            int Duration = (int)DateTime.Now.Subtract(ClientDateConnection).TotalSeconds;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Client: {ClientToken}, time connection: {ClientDateConnection.ToString("HH:mm:ss dd.MM")}, " +
                              $"duration: {Duration}");
        }

        static void CheckToken()
        {
            while (true)
            {
                if (ClientToken != "")
                {
                    IPEndPoint EndPoint = new IPEndPoint(ServerIPAddress, ServerPort);
                    Socket Socket = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp);

                    try
                    {
                        Socket.Connect(EndPoint);
                    }
                    catch (Exception exp)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: " + exp.Message);
                    }

                    if (Socket.Connected)
                    {

                        Socket.Send(Encoding.UTF8.GetBytes(ClientToken));

                        byte[] Bytes = new byte[10485760];
                        int ByteRec = Socket.Receive(Bytes);

                        string Response = Encoding.UTF8.GetString(Bytes, 0, ByteRec);
                        if (Response == "/disconect")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("There client is disconect from the server");
                            ClientToken = String.Empty;
                        }
                    }
                }
                Thread.Sleep(1000);
            }
            
        }

        static void ConnectServer()
        {
            IPEndPoint EndPoint = new IPEndPoint(ServerIPAddress, ServerPort);
            Socket Socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            try
            {
                Socket.Connect(EndPoint);
            }
            catch (Exception exp)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + exp.Message);
            }

            if (Socket.Connected)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connection to server successful");

                Socket.Send(Encoding.UTF8.GetBytes("/token"));

                byte[] Bytes = new byte[10485760];
                int ByteRec = Socket.Receive(Bytes);

                string Response = Encoding.UTF8.GetString(Bytes, 0, ByteRec);
                if (Response == "/limit")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is not enough space on the license server");
                }
                else
                {
                    ClientToken = Response;
                    ClientDateConnection = DateTime.Now;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Received connection token: " + ClientToken);
                }
            }
        }

        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Commands to the server:");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/config");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - set initial settings");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/connect");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - connection to the server");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("/status");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - show list users");
        }
        static void OnSettings()
        {
            string Path = Directory.GetCurrentDirectory() + "/.config";

            string IpAddress = "";

            if (File.Exists(Path))
            {
                StreamReader streamReader = new StreamReader(Path);
                IpAddress = streamReader.ReadLine();
                ServerIPAddress = IPAddress.Parse(IpAddress);
                ServerPort = int.Parse(streamReader.ReadLine());
                streamReader.Close();

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Server address: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(IpAddress);

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Server port: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(ServerPort.ToString());
            }
        
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Please provide the IP address of the license server:  ");
                Console.ForegroundColor = ConsoleColor.Green;
                IpAddress = Console.ReadLine();
                ServerIPAddress = IPAddress.Parse(IpAddress);

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Please specify the license server port: ");
                Console.ForegroundColor = ConsoleColor.Green;
                ServerPort = int.Parse(Console.ReadLine());

                StreamWriter streamWriter = new StreamWriter(Path);
                streamWriter.WriteLine(IpAddress);
                streamWriter.WriteLine(ServerPort.ToString());
                streamWriter.Close();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("To change, write the command: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("/config");
        }
    }
}
