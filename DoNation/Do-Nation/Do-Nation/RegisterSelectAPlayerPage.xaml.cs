using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Do_Nation
{
    public partial class RegisterSelectAPlayerPage : PhoneApplicationPage
    {
        public RegisterSelectAPlayerPage()
        {
            InitializeComponent();

            btn_user.Tap += btn_user_Tap;
            btn_orgh.Tap += btn_user_Tap;
        }

        void btn_user_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int id = 0;
            if (sender.Equals(btn_user))
                id = 1;
            else if (sender.Equals(btn_orgh))
                id = 3;
            NavigationService.Navigate(new Uri("/RegisterPage.xaml?id="+id, UriKind.Relative));
        }
    }
}