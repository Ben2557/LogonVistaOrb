/*
 * Copyright (C) 2024 Marshall Lalonde (AKA xxxman360)
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace LogonVistaOrb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process LogonUI =  new();
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal;
        bool isAdmin = false;
        string[] args = Environment.GetCommandLineArgs();
        int imagesLoadedCount = 0;
        int totalImages = 5; //This could change in the future if a skin creator is made. Adding this here to make future updates easier.
        string backgroundColor = "#FF000000";
        bool[] settings = [true, true, true, true, true, true, true]; //Startup sound, Logon Sound, Audio Srv check, Sound Scheme, Custom Startup, ORB Animation, App Enabled
        bool passedAllChecks = false;
        byte[] wavDataStartup = null;
        byte[] wavDataCustomStartup = null;
        byte[] wavDataLogonScheme = null;
        byte[] wavDataLogon = null;
        byte[] dummySound = {   0x52, 0x49, 0x46, 0x46, 0x24, 0x00, 0x00, 0x00, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20,
                                0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x40, 0x1F, 0x00, 0x00, 0x80, 0x3E, 0x00, 0x00,
                                0x02, 0x00, 0x10, 0x00, 0x64, 0x61, 0x74, 0x61, 0x00, 0x00, 0x00, 0x00  }; //A silent and extremely short WAV file used to get audiodg.exe to start (If it hasn't already)


        private void FinishedInit(object sender, RoutedEventArgs e)
        {
            if (settings[2])
            {
                //Ensure we're ready to play sound. Testing has shown that the sound does not sync well on slower devices
                WaitForAudioServices();
            }
            else { 
                passedAllChecks = true;
            }
        }

        public MainWindow()
        {
            /* 
             In response to an update I got mid development (KB5041580), I had to add a new method of prepping the app for loading at the login screen, 
             as the update had caused the animation to completely skip. LogonVistaOrb will still work fine on systems without the update.
            */

            //Declare SID of SYSTEM account for running animation
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            string systemSid = "S-1-5-18";

            if (args.Length == 1) //No actual args, just generate a blank array to prevent crashing 
            {
                args = new string[5];
            }
            if (identity.User != null && identity.User.Value != systemSid && args[1] != "-testAnim")
            {
                MessageBox.Show("Only SYSTEM account can play the animation directly.\nIf you want to test as a standard user, use the -testAnim argument.", "Invalid User account", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            if (Directory.GetCurrentDirectory() == @"C:\Windows\System32" && isAdmin == false)
            {
                MessageBox.Show("Impossible to run the program from System32 without administrator privileges !", "Access denied", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            if (isAdmin)
            {
                LoadSettings();
            }

            var dummyWindow = new Window1(); 
            dummyWindow.Loaded += FinishedInit;
            dummyWindow.Show(); 
        }

        public void WaitForAudioServices()
        {
            SoundPlayer player = new SoundPlayer(new MemoryStream(dummySound)); //Need a sound to get audiodg running
            player.PlaySync();

            Debug.Print("Waiting for Audiosrv (Windows Audio) service...");
            while (true)
            {
                ServiceController sc = new ServiceController("Audiosrv");
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    Debug.Print("Audiosrv service is running!");
                    passedAllChecks = true;
                    break;
                }
                Task.Delay(500); // Wait 500ms before checking again
            }
            
        }

        private void Image_Loaded(object sender, RoutedEventArgs e) //By loading all the content into RAM, we have ensured the animation will not lag on slower devices or HDDs
        {
            imagesLoadedCount++;

            //Declare SID of SYSTEM account for running animation
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            string systemSid = "S-1-5-18";

            if (imagesLoadedCount == totalImages)
            {
                // All images are loaded
                if (identity.User != null && identity.User.Value == systemSid)
                { //Remove the debugger key
                    string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe";
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
                    {
                        if (key != null)
                        {
                            key.DeleteValue("Debugger");
                        }
                    }

                }

                //Load the audio file too!
                string audioPathStartup = Directory.GetCurrentDirectory() + "\\LogonVistaOrb\\Sounds\\Startup.wav"; //Make sure the file exists, then load the sound
                if (File.Exists(audioPathStartup))
                {
                    wavDataStartup = File.ReadAllBytes(audioPathStartup);
                }
                // Retrieve Startup sound scheme path from the registry
                string audioPathStartupScheme = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                string audioPathStartupSchemeSound = null;
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(audioPathStartupScheme, false))
                audioPathStartupSchemeSound = key.GetValue("StartupSound") as string;

                if (File.Exists(audioPathStartupSchemeSound))
                {
                    wavDataCustomStartup = File.ReadAllBytes(audioPathStartupSchemeSound);
                }

                string audioPathLogon = Directory.GetCurrentDirectory() + "\\LogonVistaOrb\\Sounds\\Logon.wav";
                if (File.Exists(audioPathLogon))
                {
                    wavDataLogon = File.ReadAllBytes(audioPathLogon);
                }
                // Retrieve Logon sound scheme path from the registry
                string audioPathLogonScheme = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                string audioPathLogonSchemeSound = null;
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(audioPathLogonScheme, false))
                audioPathLogonSchemeSound = key.GetValue("LogonSound") as string;

                if (File.Exists(audioPathLogonSchemeSound))
                {
                    wavDataLogonScheme = File.ReadAllBytes(audioPathLogonSchemeSound);
                }

                while (!passedAllChecks)
                {
                    Debug.Print("Waiting for all checks to finish... (" + Environment.TickCount + ")");
                }
                Debug.Print("Ready to start animation!");
                StartAnimation();
            }
        }

        private async void StartAnimation()
        {
            SolidColorBrush brush = (SolidColorBrush)new BrushConverter().ConvertFromString(backgroundColor);//Set background color (if user has set one)
            this.Background = brush;

            //Declare SYSTEM account SID
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            string systemSid = "S-1-5-18";

            if (identity.User != null && identity.User.Value == systemSid)
            {
                // Reload Registry settings during Startup
                LoadSettings();

                //Attempt to launch LogonUI
                LogonUI.StartInfo.FileName = Environment.SystemDirectory + "\\LogonUI.exe";
                LogonUI.StartInfo.WorkingDirectory = Environment.SystemDirectory; //Make sure LogonUI is where it recognizes itself
                LogonUI.StartInfo.Arguments = args[2] + " " + args[3] + " " + args[4]; //Meaningful args start at 2 here, [0] is the path to LogonVistaOrb, and [1] is "LogonUI.exe"
                LogonUI.StartInfo.UseShellExecute = true;
                LogonUI.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                LogonUI.Start();
            }
            PlayStartupSound();

            if (settings[5]) // Condition to play ORB animation if box is checked
            {
                await Task.Delay(100);
                await FadeInImage(Image1, 0.10, 0, 1);
                await Task.Delay(700);
                await FadeInImage(Image2, 0.20, 0, 0.5); //Image2 has a little "blip" synced with the startup sound, hence why it has two different fade intervals
                await FadeInImage(Image2, 0.60, 0.5, 1);
                await FadeInImage(Image3, 0.35, 0, 1);
                await FadeInImage(Image4, 0.600, 0, 1);
                await FadeInImage(Image5, 0.5, 0, 1);
            }
            else
            {
                await Task.Delay(800); // Don't play ORB animation if box is unchecked, 800 value is the addition related to previous values
            }

            if (identity.User != null && identity.User.Value == systemSid)
            {
                await FadeOutWindow(1000);
                Hide();
                LogonUI.WaitForExit();
                //PlayLogonSound(); // No more used since the addition of "-logon" argument
            }
            else {
                await FadeOutWindow(1600);
                //PlayLogonSound(); // No more used since the addition of "-logon" argument
            }
            Debug.Print("Sequence complete! Quitting now...");
            Environment.Exit(1337);
        }

        private void PlayStartupSound()
        {
            if (settings[6]) //Is App enabled? Added this condition to prevent playing sounds if app is completely disabled
            {
                if (!settings[0])
                {
                    Debug.Print("User has disabled the Startup sound...");
                    return;
                }
                if (settings[4])
                {
                    Debug.Print("Playing user's custom Startup sound scheme...");
                    using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataCustomStartup)))
                    {
                        player.Play();
                    }
                    return;
                }
                Debug.Print("Playing the default Startup sound...");
                using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataStartup)))
                {
                    player.Play();
                }
            }
        }

        private void PlayLogonSound() //For now, no more used to prevent duplicated logon sound
        {
            if (settings[6]) //Is App enabled ? Added this condition to prevent playing sounds if app is completely disabled
            {
                if (!settings[1])
                {
                    Debug.Print("User has disabled the Logon sound...");
                    return;
                }
                if (settings[3])
                {
                    Debug.Print("Playing user's custom Logon sound scheme...");
                    using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataLogonScheme)))
                    {
                        player.PlaySync();
                    }
                    return;
                }
                Debug.Print("Playing the default Logon sound...");
                using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataLogon)))
                {
                    player.PlaySync();
                }
            }
        }

        private async Task FadeInImage(System.Windows.Controls.Image image, double duration, double startOpacity, double endOpacity) //Fade each image into view
        {
            var fadeIn = new DoubleAnimation(startOpacity, endOpacity, TimeSpan.FromSeconds(duration))
            {
                AccelerationRatio = 0.0,
                DecelerationRatio = 0.0
            };
            image.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            await Task.Delay(TimeSpan.FromSeconds(duration));
        }

        private async Task FadeOutWindow(int time) //Fade out the window (Presumably to the LogonUI)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1))
            {
                AccelerationRatio = 0.5,
                DecelerationRatio = 0.5
            };
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            await Task.Delay(time);
        }

        private void ProgramClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void LoadSettings() { //Load the settings if they exist to replace the default values
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                Debug.Print("hi");
                //Background color
                if (key.GetValueNames().Contains("backgroundColor"))
                {
                    backgroundColor = key.GetValue("backgroundColor").ToString();
                    Debug.Print(backgroundColor);
                }
                //Play startup sound?
                if (key.GetValueNames().Contains("noStartupSound"))
                {
                    settings[0] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noStartupSound").ToString()));
                }
                //Play logon sound?
                if (key.GetValueNames().Contains("noLogonSound"))
                {
                    settings[1] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noLogonSound").ToString()));
                }
                //Wait for audio services?
                if (key.GetValueNames().Contains("awaitAudioServices"))
                {
                    settings[2] = Convert.ToBoolean(Int32.Parse(key.GetValue("awaitAudioServices").ToString()));
                }
                //Custom Sound scheme?
                if (key.GetValueNames().Contains("SchemeSound"))
                {
                    settings[3] = Convert.ToBoolean(Int32.Parse(key.GetValue("SchemeSound").ToString()));
                }
                //Custom Startup Sound?
                if (key.GetValueNames().Contains("CustomStartupSound"))
                {
                    settings[4] = Convert.ToBoolean(Int32.Parse(key.GetValue("CustomStartupSound").ToString()));
                }
                //ORB Animation Enabled?
                if (key.GetValueNames().Contains("animationEnabled"))
                {
                    settings[5] = Convert.ToBoolean(Int32.Parse(key.GetValue("animationEnabled").ToString()));
                }
                //App Enabled?
                if (key.GetValueNames().Contains("Enabled"))
                {
                    settings[6] = Convert.ToBoolean(Int32.Parse(key.GetValue("Enabled").ToString()));
                }
            }
        }
    }
}
