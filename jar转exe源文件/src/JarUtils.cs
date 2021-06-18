using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace JarToExeByCS
{
	class JarUtils
	{
		/// <summary>
		/// 临时目录
		/// </summary>
		public static readonly string TMP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\" + "jar_" + GetDateTime();

		/// <summary>
		/// jar文件位置
		/// </summary>
		public static readonly string JAR_PATH = TMP_PATH + "\\mainJar.jar";

		/// <summary>
		/// 配置文件位置
		/// </summary>
		public static readonly string CFG_PATH = TMP_PATH + "\\cfg.properties";

		/// <summary>
		/// 是否为控制台应用程序。true表示为控制台程序，false表示为窗口或者服务应用程序
		/// </summary>
		public static bool isConsole = false;

		/// <summary>
		/// 是否把程序的标准错误输出重定向到本地文件。false代表否，true代表是。建议控制台应用程序不要开启此项
		/// </summary>
		public static bool writeErrorToLog = false;

		/// <summary>
		/// 标准错误输出文件位置，如果是控制台应用程序请使用%TEMP%代表临时目录。若上面变量writeErrorToLog为false，则此变量无效
		/// </summary>
		public static string logFileLocation = GetDateTime() + "_error.log";

		/// <summary>
		/// java的路径。默认安装了java的电脑直接填"java"，便携式jre需要在此填入java所在相对路径
		/// </summary>
		public static string javaPath = "java";

		/// <summary>
		/// 找不到运行环境提示
		/// </summary>
		public static string errorMsg = "没有找到可用的java运行环境！请先安装java 8以上版本运行环境！";

		/// <summary>
		/// 预置参数，即双击exe的时候会自动加上的参数，先于命令行传给exe的参数
		/// </summary>
		public static string preArgs = "";

		/// <summary>
		/// 控制台程序是否允许完后暂停确认退出（可以防止窗口一闪而过）
		/// </summary>
		public static bool isPause = true;

		/// <summary>
		/// java未找到时的错误值
		/// </summary>
		private static readonly string JRE_NOT_FOUND = "JreInvalid";

		/// <summary>
		/// 释放内嵌资源至指定位置
		/// </summary>
		/// <param name="resource">嵌入的资源，若在vs2019中嵌入资源，则此项为：默认命名空间.文件夹.文件.扩展名；若是通过csc命令的/res参数嵌入资源，则此项为：文件名.扩展名或者自定义的别名（使用参数/res:"资源位置",别名）</param>
		/// <param name="path">指定位置</param>
		public static void ExtractFile(string resource, string path)
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
		/// 获取时间字符串
		/// </summary>
		/// <returns>时间字符串</returns>
		public static string GetDateTime()
		{
			return DateTime.Now.ToString("yyyyMMddHHmmssfff");
		}

		/// <summary>
		/// 使用系统cmd运行java命令（无重定向，控制台应用程序调用此方法）
		/// </summary>
		/// <param name="commandArgs">java命令的参数</param>
		public static void RunCmdUseSystemCmd(string commandArgs)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd";
			process.StartInfo.Arguments = "/c " + javaPath + " " + commandArgs;
			process.StartInfo.UseShellExecute = true;
			process.Start();
			process.WaitForExit();
		}

		/// <summary>
		/// 执行java命令并获取内容（窗口应用程序调用此方法）
		/// </summary>
		/// <param name="commandArgs">java命令的参数</param>
		/// <returns>字符串数组，第一个元素为标准输出，第二个为标准错误。若命令不存在则标准输出为空，则标准错误为"JreInvalid"</returns>
		public static string[] RunCmd(string commandArgs)
		{
			string output = "";
			string err = "";
			string[] result = new string[2];
			Process process = new Process();
			process.StartInfo.FileName = javaPath;
			process.StartInfo.Arguments = commandArgs;
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
				result[0] = null;
				result[1] = JRE_NOT_FOUND;
			}
			finally
			{
				process.Close();
			}
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

		/// <summary>
		/// 检测java运行环境是否存在
		/// </summary>
		/// <returns>是否存在</returns>
		public static bool checkJre()
		{
			bool result = true;
			string[] run = RunCmd("-version");
			if (run[1].Equals(JRE_NOT_FOUND))
			{
				result = false;
				MessageBox.Show(errorMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return result;
		}
	}
}