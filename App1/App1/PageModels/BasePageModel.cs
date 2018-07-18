using System;
using App1.Utilities;

namespace App1.PageModels
{
    public class BasePageModel : ObservableObject
    {
        public static NavigationService Navigation { get; set; }

        protected void Pop()
        { 
            Navigation.Pop();
        }

        protected void Push(BasePageModel pageModel)
        {
            Navigation.Push(pageModel);
        }
        
        public virtual void OnPageAppearing(object sender, EventArgs e)
        { }

        protected void Remove()
        {
            Navigation.Remove(this);
        }
    }
}