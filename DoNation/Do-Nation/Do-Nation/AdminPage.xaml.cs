using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace Do_Nation
{
    public partial class AdminPage : PhoneApplicationPage
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int x = notifList.Children.Count;
            for (int i = 0; i < x; i++)
                notifList.Children.RemoveAt(0);
            loadNotifications();
            
        }

        private async void loadNotifications()
        {
            int count = 0;
            var notifs = await App.MobileService.GetTable<OrgAccount>().ToListAsync();
            foreach (OrgAccount n in notifs)
            {
                if (n.isverified.Equals("NEW"))
                {
                    addNotification(n);
                    count++;
                }
            }
            if (count == 0)
            {
                Grid grid = new Grid();
                grid.Height = 100;
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 33, 104, 153));
                grid.Margin = new Thickness(0, 0, 0, 15);
                TextBlock notif = new TextBlock();
                notif.Text = "No available notifications";
                notif.HorizontalAlignment = HorizontalAlignment.Right;
                notif.TextWrapping = TextWrapping.Wrap;
                notif.FontSize = 18.667;
                notif.Margin = new Thickness(0, 10, 154, 10);
                notif.Width = 241;
                notif.Foreground = new SolidColorBrush(Colors.White);
                grid.Children.Add(notif);
                notifList.Children.Add(grid);
            }
        }

        private void addNotification(OrgAccount n)
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 33, 104, 153));
            grid.Margin = new Thickness(0, 0, 0, 5);

            TextBlock notif = new TextBlock();
            notif.Name = n.id;
            notif.Text = n.name + "\n" + n.contactnum +"\n"+ n.email ;
            notif.HorizontalAlignment = HorizontalAlignment.Right;
            notif.TextWrapping = TextWrapping.Wrap;
            notif.FontSize = 18.667;
            notif.Margin = new Thickness(0, 10, 154, 10);
            notif.Width = 241;
            notif.Foreground = new SolidColorBrush(Colors.White);
            Button yes = new Button();
            yes.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            yes.Margin = new Thickness(322, 10, -2, 0);
            yes.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            yes.Height = 75;
            yes.Width = 85;
            yes.Background = new SolidColorBrush(Color.FromArgb(255, 52, 219, 82));
            yes.Content = "";
            yes.FontFamily = new FontFamily("Segoe UI Symbol");
            yes.Click += async (s, e) =>
            {
                n.isverified = "VERIFIED";
                await App.MobileService.GetTable<OrgAccount>().UpdateAsync(n);
                notifList.Children.Remove(grid);
            };
            Button no = new Button();
            no.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            no.Margin = new Thickness(251, 10, 0, 0);
            no.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            no.Height = 75;
            no.Content = "";
            no.FontFamily = new FontFamily("Segoe UI Symbol");
            no.Width = 85;
            no.Click += async (s, e) =>
            {
                n.isverified = "REJECTED";
                await App.MobileService.GetTable<OrgAccount>().UpdateAsync(n);
                notifList.Children.Remove(grid);
            };
            grid.Children.Add(notif);
            grid.Children.Add(no);
            grid.Children.Add(yes);
            notifList.Children.Add(grid);
        }

        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void refreshPostsNotifs(object sender, System.EventArgs e)
        {
            int x = notifList.Children.Count;
            for (int i = 0; i < x; i++)
                notifList.Children.RemoveAt(0);

            loadNotifications();
        }

    }
}