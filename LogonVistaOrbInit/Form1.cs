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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogonVistaOrbInit
{
    public partial class Configuration : Form
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal;
        bool isAdmin = false;
        string color;
        bool[] settings = new bool[11];
        string[] args = Environment.GetCommandLineArgs();
        byte[] wavDataShutdownScheme = null;
        byte[] wavDataShutdown = null;
        byte[] wavDataLogoffScheme = null;
        byte[] wavDataLogoff = null;
        byte[] wavDataUnlockScheme = null;
        byte[] wavDataUnlock = null;
        byte[] wavDataLockScheme = null;
        byte[] wavDataLock = null;
        byte[] wavDataLogon = null;
        byte[] wavDataLogonScheme = null;

        public Configuration()
        {
            if (args.Length == 1) {
                args = new string[6];
            }
            principal = new WindowsPrincipal(identity);
            isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin && (args[1] == "-config" || args[1] == "-shutdown" || args[1] == "-logoff" || args[1] == "-unlock" || args[1] == "-lock" || args[1] == "-logon")) //Yup, this app is also used as the settings for LogonVistaOrb! 
            {
                InitializeComponent();
                if (args[1] == "-config")
                {
                    ReloadSettings();
                    CopyRegistryValueForLogonSound();
                    CopyRegistryValueForShutdownSound();
                    CopyRegistryValueForLogoffSound();
                    CopyRegistryValueForUnlockSound();
                    Debug.Print("Current color: " + color + "\nEnabled? " + settings[0] + "\nStartup Sound? " + settings[1] + "\nLogon Sound? " + settings[2] + "\nShutdown Sound? " + settings[3] + "\nAudio service check? " + settings[4] + "\nCustom scheme check? " + settings[5] + "\nCustom startup check? " + settings[6] + "\nLogoff Sound? " + settings[7] + "\nUnlock Sound? " + settings[8] + "\nLock Sound? " + settings[9] + "\nAnimation Enabled? " + settings[10]);

                    //Set the window accordingly

                    colorText.Text = color;
                    enableCheck.Checked = settings[0];
                    startupCheck.Checked = settings[1];
                    logonCheck.Checked = settings[2];
                    shutdownCheck.Checked = settings[3];
                    audioCheck.Checked = settings[4];
                    schemeCheck.Checked = settings[5];
                    customstartupCheck.Checked = settings[6];
                    logoffCheck.Checked = settings[7];
                    unlockCheck.Checked = settings[8];
                    lockCheck.Checked = settings[9];
                    animationCheck.Checked = settings[10];
                }
                else if (args[1] == "-shutdown")
                {
                    ReloadSettings();
                    CopyRegistryValueForLogonSound(); //In anticipation before next login
                    CopyRegistryValueForShutdownSound();

                    if (settings[0])
                    { //App enabled
                        string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe";
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
                        {
                            key.SetValue("Debugger", @"C:\WINDOWS\system32\LogonVistaOrb.exe", RegistryValueKind.String);
                        }
                    }
                    if (settings[3] && settings[0]) //Shutdown sound enabled and App enabled
                    {
                        string audioPathShutdown = Directory.GetCurrentDirectory() + @"\Sounds\Shutdown.wav";
                        if (File.Exists(audioPathShutdown))
                        {
                            wavDataShutdown = File.ReadAllBytes(audioPathShutdown);
                        }
                        // Retrieve Logon sound scheme path from the registry
                        string audioPathShutdownScheme = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                        string audioPathShutdownSchemeSound = null;
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(audioPathShutdownScheme, false))
                            audioPathShutdownSchemeSound = key.GetValue("ShutdownSound") as string;

                        if (File.Exists(audioPathShutdownSchemeSound))
                        {
                            wavDataShutdownScheme = File.ReadAllBytes(audioPathShutdownSchemeSound);
                        }
                        PlayShutdownSound();
                    }
                    Environment.Exit(0);
                }

                else if (args[1] == "-logoff")
                {
                    ReloadSettings();

                    // Check if argument has SID = S-1-5-18 (LocalSystem)
                    SecurityIdentifier sid = new SecurityIdentifier("S-1-5-18");
                    bool isLocalSystem = identity.User.Equals(sid);
                    if (!isLocalSystem) //Prevent from crashing because SYSTEM account doesn't have access to HKEY_CURRENT_USER
                    {
                        CopyRegistryValueForLogoffSound();
                    }

                    if (settings[7] && settings[0]) //Logoff sound enabled and App enabled
                    {
                        string audioPathLogoff = Directory.GetCurrentDirectory() + @"\Sounds\Logoff.wav";
                        if (File.Exists(audioPathLogoff))
                        {
                            wavDataLogoff = File.ReadAllBytes(audioPathLogoff);
                        }
                        // Retrieve Logoff sound scheme path from the registry
                        string audioPathLogoffScheme = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                        string audioPathLogoffSchemeSound = null;
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(audioPathLogoffScheme, false))
                            audioPathLogoffSchemeSound = key.GetValue("LogoffSound") as string;

                        if (File.Exists(audioPathLogoffSchemeSound))
                        {
                            wavDataLogoffScheme = File.ReadAllBytes(audioPathLogoffSchemeSound);
                        }
                        PlayLogoffSound();
                    }
                    Environment.Exit(0);
                }

                else if (args[1] == "-unlock")
                {
                    ReloadSettings();
                    CopyRegistryValueForUnlockSound();
                    if (settings[8] && settings[0]) //Unlock sound enabled and App enabled
                    {
                        string audioPathUnlock = Directory.GetCurrentDirectory() + @"\Sounds\Logon.wav";
                        if (File.Exists(audioPathUnlock))
                        {
                            wavDataUnlock = File.ReadAllBytes(audioPathUnlock);
                        }
                        // Retrieve Unlock sound scheme path from the registry
                        string audioPathUnlockScheme = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                        string audioPathUnlockSchemeSound = null;
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(audioPathUnlockScheme, false))
                        audioPathUnlockSchemeSound = key.GetValue("UnlockSound") as string;

                        if (File.Exists(audioPathUnlockSchemeSound))
                        {
                            wavDataUnlockScheme = File.ReadAllBytes(audioPathUnlockSchemeSound);
                        }
                        PlayUnlockSound();
                    }
                    Environment.Exit(0);
                }

                else if (args[1] == "-lock")
                {
                    ReloadSettings();
                    CopyRegistryValueForLogoffSound();
                    if (settings[9] && settings[0]) //Lock sound enabled and App enabled
                    {
                        string audioPathLock = Directory.GetCurrentDirectory() + @"\Sounds\Logoff.wav";
                        if (File.Exists(audioPathLock))
                        {
                            wavDataLock = File.ReadAllBytes(audioPathLock);
                        }
                        // Retrieve Lock sound scheme path from the registry
                        string audioPathLockScheme = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                        string audioPathLockSchemeSound = null;
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(audioPathLockScheme, false))
                        audioPathLockSchemeSound = key.GetValue("LogoffSound") as string;

                        if (File.Exists(audioPathLockSchemeSound))
                        {
                            wavDataLockScheme = File.ReadAllBytes(audioPathLockSchemeSound);
                        }
                        PlayLockSound();
                    }
                    Environment.Exit(0);
                }

                else if (args[1] == "-logon")
                {
                    ReloadSettings();
                    CopyRegistryValueForLogonSound();
                    if (settings[2] && settings[0]) //Logon sound enabled and App enabled
                    {
                        string audioPathLogon = Directory.GetCurrentDirectory() + @"\Sounds\Logon.wav";
                        if (File.Exists(audioPathLogon))
                        {
                            wavDataLogon = File.ReadAllBytes(audioPathLogon);
                        }
                        // Retrieve Lock sound scheme path from the registry
                        string audioPathLogonScheme = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                        string audioPathLogonSchemeSound = null;
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(audioPathLogonScheme, false))
                        audioPathLogonSchemeSound = key.GetValue("LogonSound") as string;

                        if (File.Exists(audioPathLogonSchemeSound))
                        {
                            wavDataLogonScheme = File.ReadAllBytes(audioPathLogonSchemeSound);
                        }
                        PlayLogonSound();
                    }
                    Environment.Exit(0);
                }
            }
            else
            {
                MessageBox.Show("If you're looking for settings, check your start menu shortcuts. If you do not have a start menu shortcut, create a shortcut to this program with \"-config\" as an argument, and grant administrative privileges.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }
        private void ReloadSettings() {
            //Open the registry and check for settings
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                //Background color
                if (key.GetValueNames().Contains("backgroundColor"))
                {
                    color = key.GetValue("backgroundColor").ToString();
                }
                else
                {
                    color = "#FF000000";
                }
                //Is app enabled?
                if (key.GetValueNames().Contains("Enabled"))
                {
                    settings[0] = Convert.ToBoolean(Int32.Parse(key.GetValue("Enabled").ToString()));
                }
                else
                {
                    settings[0] = true;
                }
                //Play startup sound?
                if (key.GetValueNames().Contains("noStartupSound"))
                {
                    settings[1] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noStartupSound").ToString()));
                }
                else
                {
                    settings[1] = true;
                }
                //Play logon sound?
                if (key.GetValueNames().Contains("noLogonSound"))
                {
                    settings[2] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noLogonSound").ToString()));
                }
                else
                {
                    settings[2] = true;
                }
                //Play shutdown sound?
                if (key.GetValueNames().Contains("noShutdownSound"))
                {
                    settings[3] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noShutdownSound").ToString()));
                }
                else
                {
                    settings[3] = true;
                }
                //Wait for audio services?
                if (key.GetValueNames().Contains("awaitAudioServices"))
                {
                    settings[4] = Convert.ToBoolean(Int32.Parse(key.GetValue("awaitAudioServices").ToString()));
                }
                else
                {
                    settings[4] = false;
                }
                //Use custom sound scheme?
                if (key.GetValueNames().Contains("SchemeSound"))
                {
                    settings[5] = Convert.ToBoolean(Int32.Parse(key.GetValue("SchemeSound").ToString()));
                }
                else
                {
                    settings[5] = false;
                }
                //Use custom startup sound?
                if (key.GetValueNames().Contains("CustomStartupSound"))
                {
                    settings[6] = Convert.ToBoolean(Int32.Parse(key.GetValue("CustomStartupSound").ToString()));
                }
                else
                {
                    settings[6] = false;
                }
                //Play logoff sound?
                if (key.GetValueNames().Contains("noLogoffSound"))
                {
                    settings[7] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noLogoffSound").ToString()));
                }
                else
                {
                    settings[7] = true;
                }
                //Play unlock sound?
                if (key.GetValueNames().Contains("noUnlockSound"))
                {
                    settings[8] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noUnlockSound").ToString()));
                }
                else
                {
                    settings[8] = true;
                }
                //Play lock sound?
                if (key.GetValueNames().Contains("noLockSound"))
                {
                    settings[9] = !Convert.ToBoolean(Int32.Parse(key.GetValue("noLockSound").ToString()));
                }
                else
                {
                    settings[9] = true;
                }
                //Is ORB animation enabled?
                if (key.GetValueNames().Contains("animationEnabled"))
                {
                    settings[10] = Convert.ToBoolean(Int32.Parse(key.GetValue("animationEnabled").ToString()));
                }
                else
                {
                    settings[10] = true;
                }
            } 
        }
        private static String ToHex(System.Drawing.Color c) => $"#FF{c.R:X2}{c.G:X2}{c.B:X2}";

        //Main handling of settings change
        private void colorPicker_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            colorText.Text = ToHex(colorDialog1.Color);
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("backgroundColor", colorText.Text, RegistryValueKind.String);
            }
        }

        private void enableCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("Enabled", enableCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void startupCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("noStartupSound", !startupCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void logonCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("noLogonSound", !logonCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void shutdownCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("noShutdownSound", !shutdownCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void audioCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("awaitAudioServices", audioCheck.Checked, RegistryValueKind.DWord);
            }
        }

        private void schemeCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("SchemeSound", schemeCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void customstartupCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            string defaultPath = @"C:\Windows\System32\LogonVistaOrb\Sounds\Startup.wav";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                // Update or create "CustomStartupSound" key
                key.SetValue("CustomStartupSound", customstartupCheck.Checked, RegistryValueKind.DWord);

                if (customstartupCheck.Checked)
                {
                    // Update or create "StartupSound" key with default path
                    key.SetValue("StartupSound", defaultPath, RegistryValueKind.String);
                }
                else
                {
                    // Delete "StartupSound" key if exits
                    if (key.GetValueNames().Contains("StartupSound"))
                    {
                        key.DeleteValue("StartupSound");
                    }
                }
            }
            // Update controls
            UpdateControlStates();
        }

        private void logoffCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("noLogoffSound", !logoffCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void unlockCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("noUnlockSound", !unlockCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void lockCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("noLockSound", !lockCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }

        private void animationCheck_CheckedChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                key.SetValue("animationEnabled", animationCheck.Checked, RegistryValueKind.DWord);
            }
            UpdateControlStates();
        }


        private void AttemptedClose(object sender, FormClosingEventArgs e) //Prevent the app from being immediately terminated during shutdown
        {
            if (args[1] == "-shutdown") {
                e.Cancel = true;
            }
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            UpdateControlStates();
            InitializeStartupSoundFilePath();
        }

        private void CopyRegistryValueForLogonSound()
        {
            const string sourceKeyPath = @"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\WindowsLogon\.Current";
            const string destinationKeyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";

            // Read source value
            object sourceValue = Registry.GetValue(sourceKeyPath, "", null);

            // Open destination key
            using (RegistryKey destinationKey = Registry.LocalMachine.OpenSubKey(destinationKeyPath, true))
            {
               // Copy value with new name "LogonSound"
               destinationKey.SetValue("LogonSound", sourceValue, RegistryValueKind.String);
               Debug.Print("Registry value successfully copied and renamed to 'LogonSound'.");
            }         
        }

        private void CopyRegistryValueForShutdownSound()
        {
            const string sourceKeyPath = @"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\SystemExit\.Current";
            const string destinationKeyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";

            // Read source value
            object sourceValue = Registry.GetValue(sourceKeyPath, "", null);

            // Open destination key
            using (RegistryKey destinationKey = Registry.LocalMachine.OpenSubKey(destinationKeyPath, true))
            {
                // Copy value with new name "ShutdownSound"
                destinationKey.SetValue("ShutdownSound", sourceValue, RegistryValueKind.String);
                Debug.Print("Registry value successfully copied and renamed to 'ShutdownSound'.");
            }
        }

        private void CopyRegistryValueForLogoffSound() // Also used for Lock Sound !
        {
            const string sourceKeyPath = @"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\WindowsLogoff\.Current";
            const string destinationKeyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";

            // Read source value
            object sourceValue = Registry.GetValue(sourceKeyPath, "", null);

            // Open destination key
            using (RegistryKey destinationKey = Registry.LocalMachine.OpenSubKey(destinationKeyPath, true))
            {
                // Copy value with new name "LogoffSound"
                destinationKey.SetValue("LogoffSound", sourceValue, RegistryValueKind.String);
                Debug.Print("Registry value successfully copied and renamed to 'LogoffSound'.");
            }
        }

        private void CopyRegistryValueForUnlockSound()
        {
            const string sourceKeyPath = @"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\WindowsUnlock\.Current";
            const string destinationKeyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";

            // Read source value
            object sourceValue = Registry.GetValue(sourceKeyPath, "", null);

            // Open destination key
            using (RegistryKey destinationKey = Registry.LocalMachine.OpenSubKey(destinationKeyPath, true))
            {
                // Copy value with new name "UnlockSound"
                destinationKey.SetValue("UnlockSound", sourceValue, RegistryValueKind.String);
                Debug.Print("Registry value successfully copied and renamed to 'UnlockSound'.");
            }
        }

        private void PlayShutdownSound()
        {
            if (!settings[3])
            {
                Debug.Print("User has disabled the Shutdown sound...");
                return;
            }
            if (settings[5])
            {
                Debug.Print("Playing user's custom Shutdown sound scheme...");
                using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataShutdownScheme)))
                {
                    player.PlaySync();
                }
                return;
            }
            Debug.Print("Playing the default Shutdown sound...");
            using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataShutdown)))
            {
                player.PlaySync();
            }
        }

        private void PlayLogoffSound()
        {
            if (!settings[7])
            {
                Debug.Print("User has disabled the Logoff sound...");
                return;
            }
            if (settings[5])
            {
                Debug.Print("Playing user's custom Logoff sound scheme...");
                using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataLogoffScheme)))
                {
                    player.PlaySync();
                }
                return;
            }
            Debug.Print("Playing the default Logoff sound...");
            using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataLogoff)))
            {
                player.PlaySync();
            }
        }

        private void PlayUnlockSound()
        {
            if (!settings[8])
            {
                Debug.Print("User has disabled the Unlock sound...");
                return;
            }
            if (settings[5])
            {
                Debug.Print("Playing user's custom Unlock sound scheme...");
                using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataUnlockScheme)))
                {
                    player.PlaySync();
                }
                return;
            }
            Debug.Print("Playing the default Unlock sound...");
            using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataUnlock)))
            {
                player.PlaySync();
            }
        }

        private void PlayLockSound()
        {
            if (!settings[9])
            {
                Debug.Print("User has disabled the Lock sound...");
                return;
            }
            if (settings[5])
            {
                Debug.Print("Playing user's custom Lock sound scheme...");
                using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataLockScheme)))
                {
                    player.PlaySync();
                }
                return;
            }
            Debug.Print("Playing the default Lock sound...");
            using (SoundPlayer player = new SoundPlayer(new MemoryStream(wavDataLock)))
            {
                player.PlaySync();
            }
        }

        private void PlayLogonSound()
        {
            if (!settings[2])
            {
                Debug.Print("User has disabled the Logon sound...");
                return;
            }
            if (settings[5])
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

        private void colorText_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateControlStates()
        {
            // Grey out checkBoxes depending on Check states
            bool AppEnabled = enableCheck.Checked;

            startupCheck.Enabled = AppEnabled;
            logonCheck.Enabled = AppEnabled;
            shutdownCheck.Enabled = AppEnabled;
            audioCheck.Enabled = AppEnabled;
            customstartupCheck.Enabled = AppEnabled;
            logoffCheck.Enabled = AppEnabled;
            unlockCheck.Enabled = AppEnabled;
            lockCheck.Enabled = AppEnabled;
            animationCheck.Enabled = AppEnabled;
            bkgClrLabel.Enabled = AppEnabled;
            colorText.Enabled = AppEnabled;
            colorPicker.Enabled = AppEnabled;
            StartupSoundFilePath.Enabled = AppEnabled;
            StartupTest.Enabled = AppEnabled;
            BrowseStartupFileButton.Enabled = AppEnabled;

            // Grey out custom startup button if Startup Sound is unchecked
            bool StartupSoundEnabled = startupCheck.Checked && enableCheck.Checked;

            customstartupCheck.Enabled = StartupSoundEnabled; // Enable or disable "Custom startup" button

            // Specific for customstartupCheck
            bool customStartupEnabled = customstartupCheck.Checked && startupCheck.Checked && enableCheck.Checked;

            BrowseStartupFileButton.Enabled = customStartupEnabled; // Enable or disable "Browse" button
            StartupSoundFilePath.Enabled = customStartupEnabled;   // Enable or disable textbox
            StartupTest.Enabled = customStartupEnabled;            // Enable or disable test sound button

            // Grey out "Use sounds from my Sound scheme" if enableCheck is unchecked OR all action sounds are unchecked
            bool ActionSounds = logonCheck.Checked || logoffCheck.Checked || unlockCheck.Checked || lockCheck.Checked || shutdownCheck.Checked;

            schemeCheck.Enabled = AppEnabled && ActionSounds;
        }

        private void BrowseStartupFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Define dialog properties
                openFileDialog.Filter = "Audio Files (*.wav;*.mp3;*.ogg;*.flac)|*.wav;*.mp3;*.ogg;*.flac";
                openFileDialog.Title = "Select an Audio File";
                openFileDialog.Multiselect = false; // Allow to select a single file

                // Display the dialog and check whether the user has selected a file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get full path of selected file
                    string selectedFilePath = openFileDialog.FileName;

                    // Update textbox with file's path
                    StartupSoundFilePath.Text = selectedFilePath;

                    // Save path on registry
                    string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
                    {
                        key.SetValue("StartupSound", selectedFilePath, RegistryValueKind.String);
                    }
                }
            }
        }

        private void InitializeStartupSoundFilePath()
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";

            // Default Startup sound path
            string defaultPath = @"%SystemRoot%\System32\LogonVistaOrb\Sounds\Startup.wav";

            // Open registry key
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, false))
            {
                if (key != null)
                {
                    // Read "StartupSound" value
                    string registryValue = key.GetValue("StartupSound") as string;

                    // Update textbox with the value read, or default path if it's empty
                    StartupSoundFilePath.Text = !string.IsNullOrEmpty(registryValue) ? registryValue : defaultPath;
                }
                else
                {
                    // If key doesn't exist, display default path
                    StartupSoundFilePath.Text = defaultPath;
                }
            }
        }

        private void StartupSoundFilePath_TextChanged(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";

            // Check if path of the file inside TextBox isn't empty
            if (!string.IsNullOrWhiteSpace(StartupSoundFilePath.Text))
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
                {
                    if (key != null)
                    {
                        // Update registry key with file's path inside TextBox
                        key.SetValue("StartupSound", StartupSoundFilePath.Text, RegistryValueKind.String);
                    }
                }
            }
            UpdateControlStates();
        }


        private void StartupTest_Click(object sender, EventArgs e)
        {
            // Open registry key
            string keyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\LogonUI.exe\LogonVistaOrb";
            string valueName = "StartupSound";

            // Open registry key in read only
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, false))
            {
                if (key != null)
                {
                   // Read "LogonSound" value
                   object value = key.GetValue(valueName);
                   string soundPath = value.ToString();

                   if (System.IO.File.Exists(soundPath)) // Check if file exists
                   {
                      // Read and play sound
                      SoundPlayer player = new SoundPlayer(soundPath);
                      player.Play(); // Using PlaySync() to wait until the end
                   }
                }
            }
            UpdateControlStates();
        }

        private void VistaORB(object sender, EventArgs e)
        {

        }

        private void bkgClrLabel_Click(object sender, EventArgs e)
        {

        }
    }
}