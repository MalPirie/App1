using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Utilities
{
    public class MyFab : Button
    {
        public static BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(MyFab), Color.Accent);
        public Color ButtonColor
        {
            get
            {
                return (Color)GetValue(ButtonColorProperty);
            }
            set
            {
                SetValue(ButtonColorProperty, value);
            }
        }

        public MyFab()
        {
            //InitializeComponent();
        }
    }
}
