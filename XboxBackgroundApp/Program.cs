using Microsoft.Win32;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Web;

namespace XboxBackgroundApp
{
	internal static class Program
	{
		static void Print(string message)
		{
#if DEBUG
			Debug.WriteLine(message);
#else
			Console.WriteLine(message);
#endif
		}

		[SupportedOSPlatform("windows")]
		static void Main(string[] args)
		{
			string scheme = "xfxbgapp"; // What needs to be called
			string scheme_protocol = scheme + "://"; // URI

			if (args == null)
			{
				Print("XBA Args == null");
				return;
			}

			bool should_uninstall = false;
			string args_str = string.Join("", args.ToList()).Replace(" ", "").Trim().Replace(scheme_protocol, "").Replace("/", "").Normalize().Replace("%5E", "^");
			foreach (var arg in args)
			{
				Print(arg);
				if (arg.Contains("--uninstall"))
				{
					should_uninstall = true;
				}
			}

			Print("XBA args_str: \"" + args_str + "\"");

			if (should_uninstall)
			{
				CallRURI(scheme + " --uninstall");
				return;
			}

			if (!IsSchemeRegistered(scheme))
			{
				CallRURI(scheme);
			}
			else
			{
				Print("Scheme " + scheme + " is already registered!");
			}

			string[] dividedArgs = args_str.Split('^');

			if (dividedArgs.Length != 2)
			{
				if (dividedArgs.Length > 2)
				{
					Print("Too many arguments!");
					return;
				}
				else
				{
					if (dividedArgs.Length <= 1)
					{
						Print("Not enough arguments!");
						return;
					}
				}
			}

			string AppId = dividedArgs[0].Trim().Normalize();
			string AppExe = dividedArgs[1].Trim().Normalize();
			if (AppExe.ToLower() == "undefined")
			{
				Print("Invalid app exe!");
				return;
			}
			string final_args = "/C start shell:appsFolder\\" + AppId + "!" + AppExe;

			Print("Args: " + final_args);

			ProcessStartInfo cmdInfo = new ProcessStartInfo("cmd.exe", final_args);
			cmdInfo.CreateNoWindow = true;
			cmdInfo.RedirectStandardOutput = true;
			cmdInfo.RedirectStandardError = true;
			cmdInfo.UseShellExecute = false;

			Process cmd = new Process();
			cmd.StartInfo = cmdInfo;
			var output = new StringBuilder();
			var error = new StringBuilder();

			cmd.OutputDataReceived += (o, e) => output.Append(e.Data);
			cmd.ErrorDataReceived += (o, e) => error.Append(e.Data);

			cmd.Start();
			cmd.BeginOutputReadLine();
			cmd.BeginErrorReadLine();
			cmd.WaitForExit();
			cmd.Close();

			var outputData = output.ToString();
			var errorOutput = error.ToString();
			if (outputData.Length > 0)
			{
				Print(outputData);
			}
			if (errorOutput.Length > 0)
			{
				Print(errorOutput);
			}
		}

		[SupportedOSPlatform("windows")]
		static bool IsSchemeRegistered(string scheme)
		{
			using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default))
			{
				// Once again, it's fine if it's null
				using (RegistryKey schemeKey = baseKey.OpenSubKey(scheme))
				{
					return schemeKey != null;
				}
			}
		}

		static void CallRURI(string scheme)
		{
			if (File.Exists("RegisterURI.exe"))
			{
				Print("RegisterURI.exe exists!");
				Process proc = new Process();
				proc.StartInfo.FileName = "RegisterURI.exe";
				proc.StartInfo.Arguments = scheme;
				proc.StartInfo.UseShellExecute = true;
				proc.StartInfo.Verb = "runas"; // Request admin privileges
				proc.Start();
				proc.WaitForExit();
			}
			else
			{
				Print("RegisterURI.exe does not exist!");
			}
		}
	}
}
