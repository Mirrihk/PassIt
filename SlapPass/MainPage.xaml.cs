using System;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;

#if DEBUG
using System.Diagnostics;
#endif

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SlapPass
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string Final;
        public static Random RandomOutput = new Random();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            int CharCount = 0;

            bool Result = Int32.TryParse(CharNumTextBox.Text,out CharCount);
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
#if DEBUG
            Debug.WriteLine(Final);
#endif
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

        private async void ExitButton_ClickAsync(object sender, RoutedEventArgs e)
        {
#if DEBUG
            var Dialogue = new MessageDialog("Exiting now.");
            await Dialogue.ShowAsync();
#endif
            Application.Current.Exit();
        }
    }
}