using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Models;
using App1.Pages;
using App1.PageModels;
using App1.Utilities;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace App1
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            Debug.WriteLine("Hello");

            MainPage = NavigationService.Init(new AccountListPageModel(new AccountRepository()));
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
