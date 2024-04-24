using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace RegisterURI
{
	internal static class Program
	{
		[SupportedOSPlatform("windows")]
		static void Main(string[] args)
		{
			if (args == null)
			{
				return;
			}
			string scheme = string.Join("", args);
			if (scheme == null || scheme.Length == 0)
			{
				return;
			}

			if (scheme.Contains("--uninstall"))
			{
				UnregisterCustomScheme(scheme.Replace("--uninstall", "").Replace(" ", ""));
				return;
			}

			bool is_registered = IsSchemeRegistered(scheme);
			if (is_registered)
			{
				return;
			}
			RegisterCustomScheme(scheme);
		}

		[SupportedOSPlatform("windows")]
		static bool IsSchemeRegistered(string scheme)
		{
			using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default))
			{
				// Can be null but we'll simply check
				using (RegistryKey schemeKey = baseKey.OpenSubKey(scheme))
				{
					return schemeKey != null;
				}
			}
		}

		[SupportedOSPlatform("windows")]
		static void RegisterCustomScheme(string scheme)
		{
			string apploc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XboxBackgroundApp.exe");
			if (!File.Exists(apploc))
			{
				// Deleted?
				return;
			}
			using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(scheme))
			{
				key.SetValue("", "URL:" + scheme);
				key.SetValue("URL Protocol", "");

				var key2 = key;
				key2 = key.CreateSubKey("shell");
				key2 = key2.CreateSubKey("open");
				key2 = key2.CreateSubKey("command");
				key2.SetValue("", "\"" + apploc + "\" \"" + "%1\"");
			}
		}

		[SupportedOSPlatform("windows")]
		public static void UnregisterCustomScheme(string scheme)
		{
			if (!IsSchemeRegistered(scheme))
			{
				Print("Can't uninstall, scheme already uninstalled!");
				return;
			}
			try
			{
				using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default))
				{
					// Delete the registry key for the custom scheme
					using (RegistryKey key = baseKey.OpenSubKey(scheme, true))
					{
						if (key != null)
						{
							key.DeleteSubKeyTree("shell", false);
							key.DeleteSubKeyTree("DefaultIcon", false);
							key.DeleteValue("URL Protocol", false);
							key.DeleteValue("", false);
							Print("Successfully uninstalled");
						}
						else
						{
							Print("Key was null: " + scheme);
						}
					}
					baseKey.DeleteSubKeyTree(scheme, false);
				}
			}
			catch (Exception ex)
			{
				Print($"Error while unregistering custom scheme: {ex.Message}");
			}
		}

		public static void Print(string message)
		{
#if DEBUG
			Debug.WriteLine(message);
#else
			Console.WriteLine(message);
#endif
		}
	}
}
