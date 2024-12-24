﻿using System;
using System.IO;
using System.Net;

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
            else if (Command == "/connect") ;
            else if (Command == "/status") ;
            else if (Command == "/help") Help();
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
