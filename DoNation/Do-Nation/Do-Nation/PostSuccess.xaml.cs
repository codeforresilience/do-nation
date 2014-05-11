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
    public partial class PostSuccess : PhoneApplicationPage
    {
        public PostSuccess()
        {
            InitializeComponent();
            Storyboard1.Begin();
        }

        // display message, "Your donation has successfully been posted! Thank you for your help!"
        // then button sa baba, "Click here to return to Main Menu"

        private void click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PanoramaMenuPage.xaml", UriKind.Relative));
        }
    }
}