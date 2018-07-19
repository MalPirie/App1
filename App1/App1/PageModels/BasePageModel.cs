using System;
using App1.Utilities;

namespace App1.PageModels
{
    public class BasePageModel : ObservableObject
    {
        public NavigationService Navigation { get; set; }

        public virtual void OnPageAppearing(object sender, EventArgs e)
        { }
    }
}