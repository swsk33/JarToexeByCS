using System.IO;
using System.Text;

namespace JarToExeByCS
{
	class MainClass
	{
		/// <summary>
		/// 初始化程序并读入参数
		/// </summary>
		private static void initApp()
		{
			if (!Directory.Exists(JarUtils.TMP_PATH))
			{
				Directory.CreateDirectory(JarUtils.TMP_PATH);
			}
			JarUtils.ExtractFile("cfg", JarUtils.CFG_PATH);
			JarUtils.ExtractFile("jar", JarUtils.JAR_PATH);
			string[] cfgContents = File.ReadAllLines(JarUtils.CFG_PATH, Encoding.UTF8);
			foreach (string value in cfgContents)
			{
				if (value.Trim().StartsWith("#") || !value.Contains("="))
				{
					continue;
				}
				string key = value.Substring(0, value.IndexOf("=")).Trim();
				string val = "";
				if (!value.Trim().EndsWith("="))
				{
					val = value.Substring(value.IndexOf("=") + 1).Trim();
				}
				if (key.Equals("isConsole"))
				{
					if (val.ToLower().Equals("true"))
					{
						JarUtils.isConsole = true;
					}
					else
					{
						JarUtils.isConsole = false;
					}
				}
				if (key.Equals("isPause"))
				{
					if (val.ToLower().Equals("true"))
					{
						JarUtils.isPause = true;
					}
					else
					{
						JarUtils.isPause = false;
					}
				}
				if (key.Equals("writeErrorToLog"))
				{
					if (val.ToLower().Equals("true"))
					{
						JarUtils.writeErrorToLog = true;
					}
					else
					{
						JarUtils.writeErrorToLog = false;
					}
				}
				if (key.Equals("logFileLocation"))
				{
					JarUtils.logFileLocation = val;
				}
				if (key.Equals("javaPath"))
				{
					JarUtils.javaPath = val;
				}
				if (key.Equals("errorMsg"))
				{
					JarUtils.errorMsg = val;
				}
				if (key.Equals("preArgs"))
				{
					JarUtils.preArgs = val;
				}
			}
		}

		static void Main(string[] args)
		{
			initApp();
			if (JarUtils.checkJre())
			{
				string totalArgs = JarUtils.preArgs + JarUtils.consistArgs(args);
				string javaRunCommandArgs = "-jar " + JarUtils.surByQut(JarUtils.JAR_PATH) + " " + totalArgs;
				if (JarUtils.isConsole)
				{
					if (JarUtils.writeErrorToLog)
					{
						javaRunCommandArgs = javaRunCommandArgs + " 2>>" + JarUtils.surByQut(JarUtils.logFileLocation);
					}
					if (JarUtils.isPause)
					{
						javaRunCommandArgs = javaRunCommandArgs + " & pause";
					}
					JarUtils.RunCmdUseSystemCmd(javaRunCommandArgs);
				}
				else
				{
					JarUtils.RunCmd(javaRunCommandArgs);
				}
			}
			Directory.Delete(JarUtils.TMP_PATH, true);
		}
	}
}