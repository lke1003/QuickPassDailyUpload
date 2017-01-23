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

        public static void ExecuteCommand(string command)
        {
            // string str = Console.ReadLine();

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(command + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            Console.WriteLine(output);
            //Console.ReadLine();
        
        }
        
    }
}
