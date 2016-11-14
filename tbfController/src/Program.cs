using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tbfController.Classes.Core;
using Support;
using System.IO;

namespace tbfController
{
    class Program
    {

        static string sChoice="";
        static bool bProgramIsRunning = true;
        static ControllerCore controllerManagement;


        static void Main(string[] args)
        {
            Console.WriteLine("WhiteCode - Controller");
            Console.WriteLine("Developed by Anderson, supported by XXX");
            Console.WriteLine("-----------------------------------------------");
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "config.ini"))
            {
                Console.WriteLine("ERROR: Cant find config.ini! Please provide the file in the same folder as your application!");
                Console.ReadLine();
                return;
            }
            IniReader iniFile = new IniReader(AppDomain.CurrentDomain.BaseDirectory + "config.ini");

            
            displayCommands();
           

            while(bProgramIsRunning)
            {
                Console.Write(Environment.UserName + "@WhiteCode-Controller:");
                sChoice = Console.ReadLine();
                switch (sChoice)
                {
                    case "/help":
                        displayCommands();
                        break;
                    case "/start":
                        //Start Server

                        Console.WriteLine(iniFile.ReadString("Network", "Port"));
                        //controllerManagement = new ControllerCore(
                            //Convert.ToInt16(), 
                            //iniFile.ReadString("Network", "AesKey"));
                        break;
                    default:
                        break;
                }
            }
        }

        static void displayCommands()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("/start   -> Starts the server!");
            Console.WriteLine("/help    -> Shows all possible commands!");
            Console.WriteLine("-----------------------------------------------");
        }
    }
}
