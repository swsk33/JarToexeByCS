﻿using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace JarInExeByCs
{
    class Utils
    {
        public static string NOT_FOUND = "找不到命令";

        public static string TMP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\";

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
        public static string GetDateTimeAsFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + ".jar";
        }

        /// <summary>
        /// 使用系统命令行运行命令（无重定向）
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="args">参数</param>
        public static void RunCmdUseSystemCmd(String command, string args)
        {
            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// 执行命令并获取内容
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="args">参数</param>
        /// <returns>字符串数组，第一个元素为标准输出，第二个为标准错误。若命令不存在则标准输出为空，则标准错误为“找不到命令”</returns>
        public static string[] RunCmd(String command, string args)
        {
            string output = "";
            string err = "";
            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            try
            {
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                err = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception)
            {
                output = "";
                err = NOT_FOUND;
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
        //可修改，是否为控制台应用程序。true表示为控制台程序，false表示为窗口或者服务应用程序。
        private static bool isConsole = true;

        /// <summary>
        /// 检测java运行环境是否存在
        /// </summary>
        /// <param name="errorMsg">若不存在错误提示消息</param>
        /// <returns>是否存在</returns>
        private static bool checkJre(string errorMsg)
        {
            bool result = true;
            string[] run = Utils.RunCmd(javaPath, "-version");
            if (run[1].Equals(Utils.NOT_FOUND))
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
                string outputFile = Utils.TMP_PATH + Utils.GetDateTimeAsFileName();
                string totalArgs = " " + preArgs + Utils.consistArgs(args);
                string runCommand ="-jar " + Utils.surByQut(outputFile) + " " + totalArgs;
                Utils.ExtractFile(originJarFile, outputFile);
                if (isConsole)
                {
                    Utils.RunCmdUseSystemCmd(javaPath, runCommand);
                } else
                {
                    Utils.RunCmd(javaPath, runCommand);
                }
            }
        }
    }
}