using System;
using System.Collections.Generic;
using System.Linq;
using App1.PageModels;
using App1.Pages;
using Xamarin.Forms;

namespace App1.Utilities
{
    public class NavigationService
    {
        private NavigationService()
        { }

        public static Page Init(BasePageModel model)
        {
            var navigation = new NavigationService();
            var page = navigation.GetPage(model);
            return new NavigationPage(page);
        }

        public void Pop()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public void Push(BasePageModel model)
        {
            var page = GetPage(model);
            Application.Current.MainPage.Navigation.PushAsync(page, false);
        }
        
        public void Remove(BasePageModel model)
        {
            var page = Application.Current.MainPage.Navigation.NavigationStack.FirstOrDefault(p => p.BindingContext.Equals(model));
            if (page != null)
            {
                Application.Current.MainPage.Navigation.RemovePage(page);
            }
        }

        private Page GetPage(BasePageModel model)
        {
            model.Navigation = this;
            var pageTypeName = model.GetType().AssemblyQualifiedName.Replace("PageModel", "Page").Replace("ViewModel", "View");
            var pageType = Type.GetType(pageTypeName);
            var page = (ContentPage)Activator.CreateInstance(pageType);
            page.Appearing += model.OnPageAppearing;
            page.BindingContext = model;
            return page;
        }
    }
}