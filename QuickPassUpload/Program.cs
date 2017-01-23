using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

using log4net;
using log4net.Config;

namespace QuickPassUpload
{
    class Program
    {
        //Declare an instance for log4net
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        private static string filepath = Properties.Settings.Default.filepath.ToString();
        private static string distpath = Properties.Settings.Default.destinpath.ToString();
        private static string backuppath = Properties.Settings.Default.backuppath.ToString();
        private static int timeout = Properties.Settings.Default.timeout;
        static void Main(string[] args)
        {
            Log.Info("QuickPass Program Start");
           // ImplementLoggingFuntion();
            DateTime today = DateTime.Now.AddDays(-1);
            string curdate = today.ToString("yyyyMMdd");
            string curFile = filepath + "car_bat" + curdate + ".txt";
            string distFile = distpath + "car_bat" + curdate + ".";
            string backupFile = backuppath + "car_bat" + curdate + ".";

            while (!Upload(curFile, distFile, backupFile))
            {
                Log.Info("Timeout and Test The path");
                Thread.Sleep(timeout);
            }
           
          // Console.ReadLine();
        }

        public static bool Upload(string currentFile, string distinFile, string backupFile)
        { 
            try
            {
                if (Directory.Exists(distpath))
                {
                    if (File.Exists(currentFile))
                    {
                        Log.Info("File exists: " + currentFile);
                        Log.Info("Ready copy to " + distinFile + " & " + backupFile);
                        File.Copy(currentFile, distinFile, true);
                        File.Copy(currentFile, backupFile, true);
                    }
                    else
                    {
                        Log.Info("File does not exist: " + currentFile);
                        using (File.Create(distinFile));
                        using (File.Create(backupFile)) ;
                        Log.Info("Create a Empty File: " + distinFile + " & " + backupFile);
                    }
                    Log.Info("Finish the Upload");
                    return true;
                }
                else
                {
                    Log.Info("Path CANNOT Reach");
                    return false;
                }
              
            }catch (Exception ex)
            {
                Log.Error("Test File path and copy error");
                Log.Error(ex.ToString());
                return false;
            }   
        }

    }
}
