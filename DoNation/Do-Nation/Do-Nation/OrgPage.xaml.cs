using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Do_Nation
{
    public partial class OrgPage : PhoneApplicationPage
    {
        string orgID;

        public OrgPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        { 
            try 
            { 
                base.OnNavigatedTo(e);
                orgID = NavigationContext.QueryString["param0"];
                loadInformation();
                loadFeed();
                loadButton();
            } catch (Exception) { } 
        }

        private async void loadButton()
        {
            if (!MainPage.userType.Equals("VOLUNTEER") && !MainPage.userType.Equals("ORG"))
            {
                var keys = await App.MobileService.GetTable<OrganizationKeyList>().ToListAsync();
                int count = 0;
                foreach (OrganizationKeyList key in keys)
                {
                    if (key.volunteerid.Equals(MainPage.userID) && !key.status.Equals("DECLINED"))
                    {
                        count++;
                        break;
                    }
                }
                if (count == 0)
                    btn_volunteer.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private async void loadInformation()
        {
            var orgs = await App.MobileService.GetTable<OrgAccount>().ToListAsync();
            foreach (OrgAccount org in orgs)
            {
                if (org.id.Equals(orgID))
                {
                    organizationPage.Title = org.name;
                    orgContactNo.Text = org.contactnum;
                    orgDescription.Text = org.description;
                    orgEmail.Text = org.email;
                    orgOffice.Text = org.officelocation;
                    orgReliefCenter.Text = org.reliefcenter;
                    break;
                }
            }
        }

        private async void loadFeed()
        {
            var feed = await App.MobileService.GetTable<OrgPost>().ToListAsync();
            int count = 0;
            foreach (OrgPost post in feed)
            {
                if (post.orgid.Equals(orgID))
                {
                    Grid grid = new Grid();
                    grid.Height = 100;
                    grid.Background = new SolidColorBrush(Colors.White);
                    grid.Margin = new Thickness(10, 10, 10, 10);
                    TextBlock txt = new TextBlock();
                    txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.FontSize = 18.667;
                    txt.Margin = new Thickness(0, 10, 10, 10);
                    txt.Width = 261;
                    txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 36, 60, 85));
                    txt.Text = post.postcontent;
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
            }
            if(count == 0)
            {
                Grid grid = new Grid();
                grid.Height = 100;
                grid.Background = new SolidColorBrush(Colors.White);
                grid.Margin = new Thickness(10, 10, 10, 10);
                TextBlock txt = new TextBlock();
                txt.FontSize = 18.667;
                txt.Text = "No available posts.";
                grid.Children.Add(txt);
                feedPanel.Children.Add(grid);
            }
        }

        private async void volunteerAccount(object sender, System.Windows.RoutedEventArgs e)
        {
            OrganizationKeyList key = new OrganizationKeyList();
            key.volunteerid = MainPage.userID;
            key.organizationid = orgID;
            key.status = "PENDING";
            key.isreadbyvolunteer = false;
            await App.MobileService.GetTable<OrganizationKeyList>().InsertAsync(key);
            popup.Visibility = Visibility.Visible;
        }

        private void closePopup(object sender, System.Windows.RoutedEventArgs e)
        {
            popup.Visibility = Visibility.Collapsed;
            btn_volunteer.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}