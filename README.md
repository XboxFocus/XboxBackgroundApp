# README

XboxBackgroundApp (TODO: find a better name) allows you to launch select Microsoft Store games from a supported browser, only on recent versions of Windows 10 or 11.

It is comprised of two programs: RegisterURI and XboxBackgroundApp itself, made with C# NET 7.0.

Note: This software is licensed under a specific license, of which you may find more details below.

# How to compile

1. Ensure you have the following prerequisites installed:
	- Windows 10 or 11 with the latest updates
	- [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
	- [Visual Studio 2022 or newer](https://visualstudio.microsoft.com) with C# and .NET Core 7.0+ support installed

2. Clone the repository to your local machine, while ensuring that you're using the "--recursive" option

3. Open Visual Studio

4. Inside Visual Studio, open the project (.sln file)

5. Compile the project as you normally would: to build the project, go to 'Build' > 'Build Solution' (or press `Ctrl+Shift+B`).

6. Once the build is successful, the compiled executables will be located in their respective `bin/Debug/net7.0` or `bin/Release/net7.0` directories, depending on your build configuration(s) and if any other changes were made.

# How to install

Compile any code that needs to be compiled, then copy all the now-compiled executables (from both XboxBackgroundApp and RegisterURI), from the respective compilation-output folders (should be default) into the "Scripts" folder, then open the "INSTALL.bat" file.

You *must* grant administrator previliges to RegisterURI in order to be able to properly use XboxBackgroundApp.

That's it, as long as you *never* delete OR move those executables, as long as you still care about their functionality.

Please don't move nor delete them before "uninstalling" them properly first. More details may be found below.

# RegisterURI

RegisterURI associates "xfxbgapp" with the action to open XboxBackgroundApp, by adding a new "key" into the system registry.

In order to edit the system registry, RegisterURI needs administrator previliges, so you should grant them when prompeted to do so.

Associating the URI is absolutely mandatory if you want to be able to launch games directly from the browser.

This process can be reversed through the use of the "UNINSTALL.bat" file from the "Scripts" folder, provided you first copied both XboxBackgroundApp and RegisterURI's executables to that same "Scripts" folder.

Note that the "UNINSTALL.bat" folder doesn't delete any files, instead it only removes the associated "key" from the system registry, if it exists.

After the "uninstall" process is completed, and only after that, you may now manually delete the executables without worry.

# XboxBackgroundApp

XboxBackgroundApp takes care of launching the game, if any is found and/or compatible, by processing some command-line arguments.

It could also be possible to run it without RegisterURI, but this functionality is currently not implemented, and won't be for the foreseeable future, either.

# LICENSE

XboxBackgroundApp and RegisterURI (collectively referred to as "the software" or "this software") is licensed under the ISC License.

You may find a copy of such license inside the LICENSE file.

By using, distributing, modifying, copying code, or interacting with this software, or pieces of this software, you automatically agree to abide by the terms of the license.

# Disclaimer

XboxFocus is not affiliated nor associated with Xbox, GitHub, Google, Alphabet, Microsoft, Telegram or any of their subsidiaries.

Proceed at your own risk.
