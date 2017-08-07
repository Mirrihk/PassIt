using System;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;

/*
 * PassIt is a Universal Windows App designed to generate pseudo-random passwords.
 * Copyright (C) 2017  TuxSoft Limited <tuxsoft@tuxsoft.uk>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 */

namespace PassIt
{
    /// <summary>
    /// The Main Page of the app.  This does all of the password generation.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string Final;
        public static Random RandomOutput = new Random();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ToggleAlphaNum_Click (object sender, RoutedEventArgs e)
        {
            if (AlphaNumCheckBox.IsChecked == true)
            {
                AlphaNumCheckBox.IsChecked = false;
            }
            else
            {
                AlphaNumCheckBox.IsChecked = true;
            }
        }

        private void ToggleAmbiguous_Click(object sender, RoutedEventArgs e)
        {
            if (AmbiguousCheckBox.IsChecked == true)
            {
                AmbiguousCheckBox.IsChecked = false;
            }
            else
            {
                AmbiguousCheckBox.IsChecked = true;
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {

            bool Result = Int32.TryParse(CharNumTextBox.Text, out int CharCount);
            if (!Result)
            {
                CharCount = 16;
            }
            else if (CharCount < 4)
            {
                CharCount = 4;
            }
            else if (CharCount > 256)
            {
                CharCount = 256;
            }

            string InitialCreate = CreatePasswordOne(256);
            Final = CreatePasswordTwo(InitialCreate,CharCount);
            MessageOut();
        }

        public string CreatePasswordOne(int Length)
        {
            string ValidChars = "";
            if (AlphaNumCheckBox.IsChecked == true && AmbiguousCheckBox.IsChecked == false)
            {
                ValidChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            }
            else if (AlphaNumCheckBox.IsChecked == true && AmbiguousCheckBox.IsChecked == true)
            {
                ValidChars = @"abcdefhjkmnpqrstuvwxyzABCDEFHJKMNPQRSTUVWXYZ2345678";
            }
            else if (AlphaNumCheckBox.IsChecked == false && AmbiguousCheckBox.IsChecked == true)
            {
                ValidChars = @"abcdefhjkmnpqrstuvwxyzABCDEFHJKMNPQRSTUVWXYZ2345678()!£$%^&*{}[]\/|?#~'@;:,.<>";
            }
            else
            {
                ValidChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890()!£$%^&*{}[]\/|?#~'@;:,.<>";
            }
            string Output = "";
            int Count = Length;

            while (Count > 0)
            {
                Output = Output + (ValidChars[RandomOutput.Next(0,ValidChars.Length)]);
                Count = Count - 1;
            }
            return Output.ToString();
        }

        public string CreatePasswordTwo(string InputString,int MaxNumber)
        {
            StringBuilder Output = new StringBuilder();
            int Count = 0;

            while (Count < MaxNumber)
            {
                int RandomIndex = RandomOutput.Next(0, 255);
                Output.Append(InputString[RandomIndex]);
                Count = Count + 1;
            }
            return Output.ToString();
        }

        public void MessageOut()
        {
            ResultTextBox.Text = Final;
        }

        private async void CopyButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ResultTextBox.Text))
            {
                var Dialogue = new MessageDialog("Please generate a password.");
                await Dialogue.ShowAsync();
                return;
            }

            DataPackage ClipboardData = new DataPackage();

            ClipboardData.SetText(ResultTextBox.Text);
            Clipboard.Clear();
            Clipboard.SetContent(ClipboardData);
        }

        private void ExitButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}