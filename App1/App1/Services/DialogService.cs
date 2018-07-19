using System;
using System.Threading.Tasks;

namespace App1.Services
{
    public class DialogService 
    {
        public async Task<bool> ShowMessage(
            string message,
            string title,
            string buttonConfirmText,
            string buttonCancelText)
        {
            return await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, buttonConfirmText, buttonCancelText);
        }
    }
}