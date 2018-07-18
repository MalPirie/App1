using System;
using System.Threading.Tasks;

namespace App1.Services
{
    public class DialogService 
    {
        private bool DisplayAlert(
            string message,
            string title,
            string buttonConfirmText,
            string buttonCancelText)
        {
            Console.WriteLine("--- DialogService ---");
            Console.WriteLine($"  {title}");
            Console.WriteLine($"  {message}");
            Console.WriteLine($"A {buttonConfirmText}");
            if (buttonCancelText != null)
            {
                Console.WriteLine($"B {buttonCancelText}");
            }
            string input;
            do
            {
                Console.Write(">");
                input = Console.ReadLine().ToUpper();
            }
            while (input != "A" && (input != "B" || buttonCancelText == null));

            return (input == "A");
        }

        public void ShowMessage(string message, string title)
        {
            // await Application.Current.MainPage.
            DisplayAlert(title, message, "OK", null); 
        }

        public bool ShowMessage(
            string message,
            string title,
            string buttonConfirmText,
            string buttonCancelText)
        {
            var result = 
            //await Application.Current.MainPage.
            DisplayAlert(title, message, buttonConfirmText, buttonCancelText);
            return result;
        }
    }
}