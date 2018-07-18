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
        private List<Page> _pages;

        public Page CurrentPage => _pages.LastOrDefault();

        public NavigationService(BasePageModel model)
        {
            _pages = new List<Page>();
            BasePageModel.Navigation = this;
            Push(model);
        }

        public void Pop()
        {
            _pages.RemoveAt(_pages.Count - 1);
        }

        public void Push(BasePageModel model)
        {
            var pageTypeName = model.GetType().AssemblyQualifiedName.Replace("PageModel", "Page").Replace("ViewModel", "View");
            var pageType = Type.GetType(pageTypeName);
            var page = (ContentPage) Activator.CreateInstance(pageType);
            page.Appearing += model.OnPageAppearing;
            page.BindingContext = model;
            _pages.Add(page);
            
            // https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/weak-event-patterns
            // page.OnAppearing += new WeakEventHandler<EventArgs>(PageAppearing).Handler;

            //Application.Current.MainPage.Navigation.PushAsync(new PageModifyQty((Model)obj), false);
        }
        
        public void Remove(BasePageModel model)
        {
            _pages.RemoveAll(p => p.BindingContext.Equals(model));
        }
    }
}