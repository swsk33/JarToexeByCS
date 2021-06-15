using System;
using System.Diagnostics;

namespace JarToExeBuilder
{
	class Program
	{
		private static void printHelp()
		{
			Console.WriteLine("命令行使用方法：\r\nbuildexe -j jar文件路径 -o 输出exe路径 [-p 架构] [-i ico图标文件路径] [-a]\r\n");
			Console.WriteLine("上述命令中中括号括起来部分是可选参数，实际加上这些可选参数执行时不需要写中括号\r\n");
			Console.WriteLine("架构(-p)参数可选值如下：");
			Console.WriteLine("anycpu --- 可在任何架构的cpu上运行（默认）\r\nx86 --- 32位程序\r\nx64 --- 64位程序\r\narm --- arm架构\r\n");
			Console.WriteLine("-a表示该程序是否需要管理员权限，不带-a即为不需要管理员权限");
		}

		/// <summary>
		/// 执行命令
		/// </summary>
		/// <param name="command">命令</param>
		public static void RunCmd(string command, string arg)
		{
			Process process = new Process();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arg;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			try
			{
				process.Start();
				Console.WriteLine(process.StandardOutput.ReadToEnd());
				Console.WriteLine(process.StandardError.ReadToEnd());
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
		}

		static void Main(string[] args)
		{
			int jarFileOption = Array.IndexOf(args, "-j");
			int outputFileOption = Array.IndexOf(args, "-o");
			if (jarFileOption == -1 || outputFileOption == -1)
			{
				Console.WriteLine("参数错误！");
				printHelp();
				return;
			}
			string jarFilePath = args[jarFileOption + 1];
			string outputPath = args[outputFileOption + 1];
			string commandArgs = "/t:winexe /res:\"" + jarFilePath + "\",jar /res:\"cfg.properties\",cfg /out:\"" + outputPath + "\"";
			int platformOption = Array.IndexOf(args, "-p");
			int iconOption = Array.IndexOf(args, "-i");
			int authOption = Array.IndexOf(args, "-a");
			if (platformOption != -1)
			{
				commandArgs = commandArgs + " /platform:" + args[platformOption + 1];
			}
			if (iconOption != -1)
			{
				commandArgs = commandArgs + " /win32icon:\"" + args[iconOption + 1] + "\"";
			}
			if (authOption != -1)
			{
				commandArgs = commandArgs + " /win32manifest:\"app.manifest\"";
			}
			commandArgs = commandArgs + " \"src\\*.cs\"";
			RunCmd("csc", commandArgs);
		}
	}
}