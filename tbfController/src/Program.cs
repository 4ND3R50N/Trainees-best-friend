using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tbfController.Classes.Core;
using Support;
using System.IO;
using System.Configuration;

namespace tbfController
{
    class Program
    {
        static string sChoice="";
        static bool bProgramIsRunning = true;
        static ControllerCore controllerManagement;
        iniManager iniEngine;

        //appSettings
        static string automaticStart;
        static string logPath;
        //connectionStrings
        static short serverPort;
        static char protocolDelimiter;
        static char dataDelimiter;
        static string aesKey;
        static string databaseDriver;
        static string databaseAdress;
        static short databasePort;
        static string databaseUser;
        static string databasePass;
        static string databaseDefaultDB;

        static void Main(string[] args)
        {
            Console.WriteLine("WhiteCode - Controller");
            Console.WriteLine("Developed by Anderson, supported and further developments by Niko");
            Console.WriteLine("-----------------------------------------------");

            int configFailure = readConfigfile();

            displayCommands();

            DateTime DateTime = DateTime.Now;
            if (configFailure == 0 && automaticStart.Equals("true"))
            {
                sChoice = "/start";
                automaticStart = "";

                Console.WriteLine("[" + DateTime + "]: " + "Automatic start configured and in action!");
            }
            else if (configFailure != 0)
            {
                Console.WriteLine("[" + DateTime + "] ERROR: " + "Automatic start canceled because of failures in the configuration file!!!");
            }

            while (bProgramIsRunning)
            {
                switch (sChoice)
                {
                    case "/help":
                        displayCommands();
                        break;
                    case "/start":
                        //Start Server
                        controllerManagement = new ControllerCore(serverPort,protocolDelimiter, dataDelimiter, aesKey,
                            databaseDriver, databaseAdress, databasePort, databaseUser, databasePass, databaseDefaultDB, AppDomain.CurrentDomain.BaseDirectory + logPath);
                        //Bei nicht erfolgreicher DB gibt es einen Obj. orientierten fehler, wegen dem return bei nicht erfolgreicher DB connection
                        controllerManagement.Start();
                        break;
                    case "/stop":
                        return;
                    default:
                        break;
                }

                Console.Write(Environment.UserName + "@tbf-controller:");
                sChoice = Console.ReadLine();
            }
        }

        static void displayCommands()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("/start   -> Starts the server!");
            Console.WriteLine("/help    -> Shows all possible commands!");
            Console.WriteLine("/stop    -> Stops the server!");
            Console.WriteLine("-----------------------------------------------");
        }

        static int readConfigfile()
        {
            DateTime DateTime = DateTime.Now;

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + typeof(Program).Assembly.GetName().Name + ".exe.config"))
            {
                Console.WriteLine("[" + DateTime + "] ERROR: Cant find" + typeof(Program).Assembly.GetName().Name + ".exe.config! Please provide the file in the same folder as your application!");
                return 1;
            }

            //appSettings
            automaticStart = ConfigurationManager.AppSettings["automaticStart"];
            logPath = ConfigurationManager.AppSettings["logPath"];
            //Check AppSettings
            if (string.IsNullOrEmpty(automaticStart) == true || string.IsNullOrEmpty(logPath) == true)
            {
                Console.WriteLine("[" + DateTime + "] ERROR: " + "Configuration file has failures at appSettings!!!");
                return 1;
            }

            //connectionStrings
            try
            {
                serverPort = short.Parse(ConfigurationManager.ConnectionStrings["serverPort"].ConnectionString);
                protocolDelimiter = ConfigurationManager.ConnectionStrings["protocolDelimiter"].ConnectionString[0];
                dataDelimiter = ConfigurationManager.ConnectionStrings["dataDelimiter"].ConnectionString[0];
                aesKey = ConfigurationManager.ConnectionStrings["aesKey"].ConnectionString;
                databaseDriver = ConfigurationManager.ConnectionStrings["databaseDriver"].ConnectionString;
                databaseAdress = ConfigurationManager.ConnectionStrings["databaseAdress"].ConnectionString;
                databasePort = short.Parse(ConfigurationManager.ConnectionStrings["databasePort"].ConnectionString);
                databaseUser = ConfigurationManager.ConnectionStrings["databaseUser"].ConnectionString;
                databasePass = ConfigurationManager.ConnectionStrings["databasePass"].ConnectionString;
                databaseDefaultDB = ConfigurationManager.ConnectionStrings["databaseDefaultDB"].ConnectionString;
            }
            catch(Exception e)
            {
                Console.WriteLine("[" + DateTime + "] ERROR: " + "Configuration file has failures at connectionStrings!!!");
                return 1;
            }

            Console.WriteLine("[" + DateTime + "]: " + "Configuration file has been read successful");
            return 0;
        }
    }
}
