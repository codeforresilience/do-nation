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
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Do_Nation
{
    public partial class PanoramaMenuPage : PhoneApplicationPage
    {
        public PanoramaMenuPage()
        {
            InitializeComponent();
        }

        String volName = "";
        String volContactNum = "";
        private async Task getVolunteerInfo(String volID)
        {
            var results = await App.MobileService.GetTable<Account>().ToListAsync();
            foreach (Account a in results)
            {
                if (a.id.Equals(volID))
                {
                    volName = a.name;
                    volContactNum = a.contactnumber;
                }
            }
        }

        String orgID = "";
        private async Task getOrgID(String volID)
        {
            var results = await App.MobileService.GetTable<OrganizationKeyList>().ToListAsync();
            foreach (OrganizationKeyList a in results)
            {
                if (a.volunteerid == volID)
                {
                    orgID = a.organizationid;
                }
            }
            
        }

        String orgName = "";
        private async Task getOrgName()
        {

            var results = await App.MobileService.GetTable<OrgAccount>().ToListAsync();
            foreach (OrgAccount a in results)
            {
                if (a.id == orgID)
                {
                    orgName = a.name;
                }
            }
        }

       private async void updatePostStatus(String status, String postID, string volunteerID)
       {
           var results = await App.MobileService.GetTable<Post>().ToListAsync();

           foreach (Post a in results)
           {
               if (a.id.Equals(postID))
               {
                   a.status = status;
                   a.volunteerid = volunteerID;
                   await App.MobileService.GetTable<Post>().UpdateAsync(a);
                   break;
               }
           }
       }

        string donatorName;

        private async void addNotification(Notification n)
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 33, 104, 153));
            grid.Margin = new Thickness(0, 0, 0, 5);

            TextBlock notif = new TextBlock();
            notif.Name = n.id;
            await getVolunteerInfo(n.volunteerid);
            await getOrgID(n.volunteerid);
            await getOrgName();
            notif.Text = volName +" from "+ orgName + " wants to collect your donation. Contact Number: " + volContactNum;
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
                n.status = "APPROVED";
                updatePostStatus("WAITING FOR PICKUP", n.postid, n.volunteerid);
                await App.MobileService.GetTable<Notification>().UpdateAsync(n);
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
                n.status = "DECLINED";
                updatePostStatus("NEW", n.postid, "");
                await App.MobileService.GetTable<Notification>().UpdateAsync(n);
                notifList.Children.Remove(grid);
            };
            grid.Children.Add(notif);
            grid.Children.Add(no);
            grid.Children.Add(yes);
            notifList.Children.Add(grid);
        }

        private async void addOrgNotification(OrganizationKeyList key)
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 33, 104, 153));
            grid.Margin = new Thickness(0, 0, 0, 5);

            TextBlock notif = new TextBlock();
            notif.Name = key.id;
            await getVolunteerInfo(key.volunteerid);
            notif.Text = volName + " wants to apply as a volunteer under your organization. Contact Number: " + volContactNum;
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
                key.status = "APPROVED";
                updateUserToVolunteer(key.id, key.volunteerid, key.organizationid);
                await App.MobileService.GetTable<OrganizationKeyList>().UpdateAsync(key);
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
                key.status = "DECLINED";
                await App.MobileService.GetTable<OrganizationKeyList>().UpdateAsync(key);
                notifList.Children.Remove(grid);
            };
            grid.Children.Add(notif);
            grid.Children.Add(no);
            grid.Children.Add(yes);
            notifList.Children.Add(grid);

        }

        private async void updateUserToVolunteer(string key, string user, string org)
        {
            var accountList = await App.MobileService.GetTable<Account>().ToListAsync();
            foreach (Account a in accountList)
            {
                if (a.id.Equals(user))
                {
                    Account acc = a;
                    acc.volunteerid = key;
                    acc.orgid = org;
                    await App.MobileService.GetTable<Account>().UpdateAsync(acc);
                    break;
                }
            }
        }

        private async void addVolunteerNotification(Notification n)
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 33, 104, 153));
            grid.Margin = new Thickness(0, 0, 0, 5);

            TextBlock notif = new TextBlock();
            notif.Name = n.id;
            await getDonatorName(n.userid);
            if(n.status.Equals("APPROVED"))
                notif.Text = donatorName + " has approved your request.";
            else if (n.status.Equals("DECLINED"))
                notif.Text = donatorName + " has declined your request.";
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
            yes.Content = "OK";
            yes.Click += async (s, e) =>
            {
                n.isreadbyvolunteer = true;
                await App.MobileService.GetTable<Notification>().UpdateAsync(n);
                notifList.Children.Remove(grid);
            };
            grid.Children.Add(notif);
            grid.Children.Add(yes);
            notifList.Children.Add(grid);
        }

        private async void addVolunteerOrgNotification(OrganizationKeyList k)
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 33, 104, 153));
            grid.Margin = new Thickness(0, 0, 0, 5);

            TextBlock notif = new TextBlock();
            notif.Name = k.id;
            orgID = k.organizationid;
            await getOrgName();
            if (k.status.Equals("APPROVED"))
                notif.Text = orgName + " has approved your application to be a volunteer.";
            else if (k.status.Equals("DECLINED"))
                notif.Text = orgName + " has declined your application to be a volunteer.";
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
            yes.Content = "OK";
            yes.Click += async (s, e) =>
            {
                k.isreadbyvolunteer = true;
                await App.MobileService.GetTable<OrganizationKeyList>().UpdateAsync(k);
                notifList.Children.Remove(grid);
            };
            grid.Children.Add(notif);
            grid.Children.Add(yes);
            notifList.Children.Add(grid);
        }

        private async Task getDonatorName(string p)
        {
            var accountList = await App.MobileService.GetTable<Account>().ToListAsync();
            foreach (Account a in accountList)
            {
                if (a.id.Equals(p))
                {
                    donatorName = a.name;
                    break;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int x = feedPanel.Children.Count;
            for (int i = 0; i < x; i++)
                feedPanel.Children.RemoveAt(0);
            x = notifList.Children.Count;
            for (int i = 0; i < x; i++)
                notifList.Children.RemoveAt(0);
            loadFeed();
            loadNotifications();
        }

        private void loadNotifications()
        {
            if (MainPage.userType.Equals("USER"))
            {
                loadUserNotif();
                normuserStack.Visibility = System.Windows.Visibility.Visible;
            }
            else if (MainPage.userType.Equals("VOLUNTEER"))
            {
                loadVolunteerNotif();
                volunteerStack.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                loadOrgNotif();
                orgStack.Visibility = System.Windows.Visibility.Visible; 
                orgStack_Copy.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private async void loadOrgNotif()
        {
            int count = 0;
            var keys = await App.MobileService.GetTable<OrganizationKeyList>().ToListAsync();
            foreach (OrganizationKeyList key in keys)
            {
                if (key.status.Equals("PENDING"))
                {
                    addOrgNotification(key);
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

        private async void loadVolunteerNotif()
        {
            int count = 0;
            var requests = await App.MobileService.GetTable<OrganizationKeyList>().ToListAsync();
            foreach (OrganizationKeyList k in requests)
            {
                if (k.volunteerid.Equals(MainPage.userID) && k.isreadbyvolunteer == false && !k.status.Equals("PENDING"))
                {
                    addVolunteerOrgNotification(k);
                    count++;
                }
            }
            var notifs = await App.MobileService.GetTable<Notification>().ToListAsync();
            foreach (Notification n in notifs)
            {
                if (n.volunteerid.Equals(MainPage.userID) && n.isreadbyvolunteer == false && !n.status.Equals("PENDING"))
                {
                    addVolunteerNotification(n);
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

        private async void loadUserNotif()
        {
            int count = 0;
            var requests = await App.MobileService.GetTable<OrganizationKeyList>().ToListAsync();
            foreach (OrganizationKeyList k in requests)
            {
                if (k.volunteerid.Equals(MainPage.userID) && k.isreadbyvolunteer == false && !k.status.Equals("PENDING"))
                {
                    addVolunteerOrgNotification(k);
                    count++;
                }
            }
            var notifs = await App.MobileService.GetTable<Notification>().ToListAsync();
            foreach (Notification n in notifs)
            {
                if (n.userid.Equals(MainPage.userID) && n.status.Equals("PENDING"))
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

        private async void loadFeed()
        {
            string orgName = "";
            var feed = await App.MobileService.GetTable<OrgPost>().ToListAsync();
            int count = 0;
            foreach (OrgPost post in feed)
            {
                var list = await App.MobileService.GetTable<OrgAccount>().ToListAsync();
                foreach (OrgAccount acc in list)
                {
                    if (acc.id.Equals(post.orgid))
                        orgName = acc.name;
                }
                /*
                 *    <Grid x:Name="post_Copy1" Height="100" Background="#FFC0E666" Margin="10,10,10,0" Style="{StaticResource gridPostStyle}">
            				<TextBlock HorizontalAlignment="Right" TextWrapping="Wrap" Text="Lorem ipsum dolor ist tsi rolod muspi merol alvarez keryola" 
                 *          FontSize="18.667" Margin="0,10,10,10" Width="261" Foreground="#FF0B5716" Style="{StaticResource feedsTextBlockStyle}"/>
            				<Image HorizontalAlignment="Left" Height="91" VerticalAlignment="Top" Width="91" Style="{StaticResource feedsImgStyle}" Source="/Assets/postorgpostIcon/warning.png" RenderTransformOrigin="0.5,0.5" Margin="5,5,0,0"/>
            			</Grid>
                 */
                Grid grid = new Grid();
                grid.Background = new SolidColorBrush(Colors.White);
                grid.Margin = new Thickness(10, 10, 10, 10);
                TextBlock txt = new TextBlock();
                txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                txt.TextWrapping = TextWrapping.Wrap;
                txt.FontSize = 18.667;
                txt.Margin = new Thickness(0, 10, 10, 10);
                txt.Width = 261;
                txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 36, 60, 85));
                txt.Inlines.Add(new Run()
                {
                    FontSize = 25,
                    FontWeight = FontWeights.Bold,
                    Text = orgName
                });
                txt.Inlines.Add(new Run()
                {
                    Text = "\n" + post.postcontent
                });
                Image img = new Image();
                img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                img.Height = 91;
                img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                img.Width = 91;
                img.Margin = new Thickness(5, 5, 0, 0);
                string src = "";
                switch (post.category)
                {
                    case "Announcement":
                        src = "/Assets/postorgpostIcon/annuncement.png";
                        break;
                    case "Update":
                        src = "/Assets/postorgpostIcon/update.png";
                        break;
                    case "Warning":
                        src = "/Assets/postorgpostIcon/warning.png";
                        break;
                    case "Request":
                        src = "/Assets/postorgpostIcon/request.png";
                        break;
                }
                BitmapImage bi3 = new BitmapImage();
                bi3.UriSource = new Uri(src, UriKind.Relative);
                img.Source = bi3;

                grid.Children.Add(txt);
                grid.Children.Add(img);
                feedPanel.Children.Add(grid);
                count++;
            }

            if (count == 0)
            {
                Grid grid = new Grid();
                grid.Height = 100;
                grid.Background = new SolidColorBrush(Colors.White);
                grid.Margin = new Thickness(10, 10, 10, 10);
                TextBlock txt = new TextBlock();
                txt.FontSize = 18.667;
                txt.Margin = new Thickness(0, 10, 10, 10);
                txt.Width = 261;
                txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 36, 60, 85));
                txt.Text = "No posts available.";
                grid.Children.Add(txt);
                feedPanel.Children.Add(grid);
            }
        }

        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void refreshPostsNotifs(object sender, System.EventArgs e)
        {
            int x = feedPanel.Children.Count;
            for (int i = 0; i < x; i++)
                feedPanel.Children.RemoveAt(0);
            x = notifList.Children.Count;
            for (int i = 0; i < x; i++)
                notifList.Children.RemoveAt(0);
            loadFeed();
            loadNotifications();
        }
    }
}