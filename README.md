### What's new in LogonVistaOrb v2.0.0 ?

- LogonVistaOrb v2.0.0 dynamically uses the Local SYSTEM Windows Account by its common SID (S-1-5-18) instead of the declared string value which causes a crash on non-English Windows languages ! This prevents this error : https://github.com/xxxman360/LogonVistaOrb/issues/2
- BULTIN\Administrators Local Group (SID = S-1-5-32-544) are now utilised on Task Scheduler instead of SYSTEM (except for logoff) because of the lack of SYSTEM account for retrieving keys from HKEY_CURRENT_USER. As a workaround, program copies registry keys from last user. This is mandatory for custom scheme sounds !
- Added new option to play or not ORB animation while keeping other features enabled.
- Recoded "Enable LogonVistaOrb" option which in v1.0.0 some features were still starting even if the app was completely disabled.
- Support for customisation of startup sound.
- Added new system event sounds such as "Logon, Logoff, Unlock, Lock" sounds! Users can choose if they want to play Session Sounds or not.
- Recoded Logon Sound function. In v1.0.0 the logon sound only played during the first logon session. Now the sound is also played when switching sessions/users.
- Now supports SOUND SCHEMES, session sounds can be customised using Windows settings ! (it appears that some sounds, especially original sounds from Windows 7, are not supported ! "SoundAPIFormatNotSupported" returns "The Sound API only supports playback of PCM sound files"). You'll better use converters to avoid this !
- Added dependencies between boxes : depending on their state, some checkboxes disable others.
- Reorganised the agencement of the checkboxes due to the options added in the new version.
- New scheduled tasks were integrated to installer to call new arguments (-lock, -unlock, -logoff, -logon).
- Installer disables "Starting sound" and  "Fast Startup" features. It is essential to let LogonVistaOrb to start and avoid duplicate sounds !
- Installer shows hidden scheme choices (Unlock, Logon, Logoff, Shutdown) which have been removed since modern versions of Windows no longer support session sounds.


# LogonVistaOrb

LogonVistaOrb is a .NET Framework app designed to bring the iconic Windows Vista startup animation in the login screen to Windows 10 and up

<p align="center">
<img src="https://i.imgur.com/wTzRBD2.png" height=300 border="10"/>
</p>
Windows Vista is a trademark of Microsoft Corporation. I am not affiliated with Microsoft, nor does Microsoft endorse the creation of this product.

## Customizing

### Changing the Graphics and Sounds
Head over to C:\Windows\System32\LogonVistaOrb. In there, you will find two folders, named "Images" and "Sounds". You can replace them with whatever you like. If you're looking for some skins online, try some of these pages:

https://www.deviantart.com/slicedefender/gallery?q=boot+logo

https://www.deviantart.com/xantic21/gallery?q=tuneup

https://www.deviantart.com/yethzart/art/Authui-dll-Vista-RTM-and-SP1-84385962

You may need [Resource Hacker](https://www.angusj.com/resourcehacker/#download) to extract the images if the skin is offered as a DLL.

If you'd also like to use a custom background image, you can copy a PNG image to the "Images" folder, and rename it to "background.png". This image will be stretched to fill the entire screen, so make sure it matches your monitor's aspect ratio.

### Changing the program behaviour
LogonVistaOrb comes with a configuration tool to change how the program works. It features the following:
- Enabling/Disabling the program
- Toggling the startup sound
- Toggling the logon sound
- Toggling the shutdown sound
- The option to wait for the Windows audio service to start
- Changing the background color

## Compiling the software
LogonVistaOrb does not use a makefile, so you will need to manually compile it using a version of Visual Studio that supports the .NET Framework v4.8

For whatever reason you wish to compile the source code, bare in mind, that if you want to publish your own fork of the program, you must abide by the rules of the [GPL v3.0](https://github.com/xxxman360/LogonVistaOrb/blob/main/LICENSE).

### Order of solutions to compile
1. You will need to compile the LogonVistaOrbInit solution first, if you are not planning on modifying it, you can skip this step. 
    - Once you have your compiled EXE, copy it to the LogonVistaOrb solution, and paste it in the folder named "LogonVistaOrb" (Note, this is a subfolder, so "LogonVistaOrb/LogonVistaOrb")

2. Compile the LogonVistaOrb solution, when you have your build, compress the "LogonVistaOrb" folder and "LogonVistaOrb.exe" from the output directory into a ZIP file named "app.zip"
    - Copy the ZIP file to the LogonVistaOrbInstaller solution, and paste it in the "Resources" folder.

3. You can now compile the LogonVistaOrbInstaller solution. It is recommended to make a 32 bit and 64 bit build to ensure the registry is accessed correctly. 

Have fun!


Original repo : https://github.com/xxxman360/LogonVistaOrb
