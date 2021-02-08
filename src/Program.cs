using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace JarInExeByCs
{
    class Utils
    {
        public static string tmpPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\";
        //可修改，是否为控制台应用程序。true表示为控制台程序，false表示为窗口或者服务应用程序。
        public static bool isConsole = true;
        //可修改，是否把程序的标准错误输出重定向到本地文件。false代表否，true代表是。建议控制台应用程序不要开启此项。
        public static bool writeErrorToLog = false;
        //可修改，标准错误输出文件位置，如果是控制台应用程序请使用%TEMP%代表临时目录，若为窗体应用程序请用Utils.tmpPath这个字符串变量代表临时目录。若上面变量writeErrorToLog为false，则此变量无效。
        public static string logFileLocation = Utils.GetDateTime() + "_error.log";

        /// <summary>
        /// 释放内嵌资源至指定位置
        /// </summary>
        /// <param name="resource">嵌入的资源，若在vs2019中嵌入资源，则此项为：默认命名空间.文件夹.文件.扩展名；若是通过csc命令的/res参数嵌入资源，则此项为：文件名.扩展名</param>
        /// <param name="path">指定位置</param>
        public static void ExtractFile(String resource, String path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            BufferedStream input = new BufferedStream(assembly.GetManifestResourceStream(resource));
            FileStream output = new FileStream(path, FileMode.Create);
            byte[] data = new byte[1024];
            int lengthEachRead;
            while ((lengthEachRead = input.Read(data, 0, data.Length)) > 0)
            {
                output.Write(data, 0, lengthEachRead);
            }
            output.Flush();
            output.Close();
        }

        /// <summary>
        /// 获取文件名，以时间命名
        /// </summary>
        /// <returns>文件名</returns>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// 使用系统命令行运行命令（无重定向，控制台应用程序调用此方法）
        /// </summary>
        /// <param name="command">命令</param>
        public static void RunCmdUseSystemCmd(String command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = "/c " + command;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// 执行命令并获取内容（窗口应用程序调用此方法）
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>字符串数组，第一个元素为标准输出，第二个为标准错误。若命令不存在则标准输出为空，则标准错误为“找不到命令”</returns>
        public static string[] RunCmd(String command)
        {
            string output = "";
            string err = "";
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = "/c " + command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            try
            {
                process.Start();
                string outLine = null;
                string errLine = null;
                StreamWriter logFileWriter;
                while ((outLine = process.StandardOutput.ReadLine()) != null || (errLine = process.StandardError.ReadLine()) != null)
                {
                    if (outLine != null)
                    {
                        output = output + outLine + "\r\n";
                    }
                    if (errLine != null)
                    {
                        err = err + errLine + "\r\n";
                        if (writeErrorToLog && !isConsole)
                        {
                            logFileWriter = new StreamWriter(logFileLocation, true);
                            if (!File.Exists(logFileLocation))
                            {
                                File.Create(logFileLocation);
                            }
                            logFileWriter.WriteLine(errLine);
                            logFileWriter.Close();
                        }
                    }
                }
                process.WaitForExit();
            }
            catch (Exception)
            {
                //none
            }
            finally
            {
                process.Close();
            }
            string[] result = new string[2];
            result[0] = output;
            result[1] = err;
            return result;
        }

        /// <summary>
        /// 组合命令行参数
        /// </summary>
        /// <param name="args">传参数组</param>
        /// <returns>组合好的参数</returns>
        public static string consistArgs(string[] args)
        {
            string result = "";
            if (args.Length != 0)
            {
                foreach (string arg in args)
                {
                    string eachArg = " " + arg;
                    result = result + eachArg;
                }
            }
            return result;
        }

        /// <summary>
        /// 用引号包围字符串
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>输出引号包围过的字符串</returns>
        public static string surByQut(string str)
        {
            return "\"" + str + "\"";
        }
    }

    class MainClass
    {
        //可修改，java的路径。默认安装了java的电脑直接填"java"即可，便携式jre需要在此填入java所在相对路径
        private static string javaPath = "java";

        /// <summary>
        /// 检测java运行环境是否存在
        /// </summary>
        /// <param name="errorMsg">若不存在错误提示消息</param>
        /// <returns>是否存在</returns>
        private static bool checkJre(string errorMsg)
        {
            bool result = true;
            string[] run = Utils.RunCmd(javaPath + " -version && echo yes || echo no");
            if (run[0].EndsWith("no\r\n"))
            {
                result = false;
                MessageBox.Show(errorMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        static void Main(string[] args)
        {
            if (checkJre("没有找到可用的java运行环境！请先安装java 8以上版本运行环境！")) //可修改，找不到java运行环境时的错误提示信息
            {
                string originJarFile = "mainJar.jar"; //待打包jar文件位置，必须把jar文件命名为mainJar.jar并放在同cs源文件同目录下
                string preArgs = ""; //可修改，预置参数，即双击exe的时候会自动加上的参数，先于命令行传给exe的参数 
                string outputFile = Utils.tmpPath + Utils.GetDateTime() + ".jar";
                string totalArgs = " " + preArgs + Utils.consistArgs(args);
                string runCommand = javaPath + " -jar " + Utils.surByQut(outputFile) + " " + totalArgs;
                Utils.ExtractFile(originJarFile, outputFile);
                if (Utils.isConsole)
                {
                    bool isPause = true; //可修改，控制台程序是否允许完后暂停确认退出（可以防止窗口一闪而过）
                    if (Utils.writeErrorToLog)
                    {
                        runCommand = runCommand + " 2>>" + Utils.surByQut(Utils.logFileLocation);
                    }
                    if (isPause)
                    {
                        runCommand = runCommand + " & pause";
                    }
                    Utils.RunCmdUseSystemCmd(runCommand);
                }
                else
                {
                    Utils.RunCmd(runCommand);
                }
            }
        }
    }
}