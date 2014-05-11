using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Documents;

namespace Do_Nation
{
    public partial class myVolunteers : PhoneApplicationPage
    {
        public myVolunteers()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int x = readyList.Children.Count;
            for (int i = 0; i < x; i++)
                readyList.Children.RemoveAt(0);

            x = pickedupList1.Children.Count;
            for (int i = 0; i < x; i++)
                pickedupList1.Children.RemoveAt(0);

            loadDonations();
        }

        Account donator;
        Account volunteer;

        private async void loadDonations()
        {
            Dictionary<string, Dictionary<string, int>> dates = new Dictionary<string, Dictionary<string, int>>();

            var donations = await App.MobileService.GetTable<Post>().ToListAsync();
            foreach (Post p in donations)
            {
                donator = await getUserName(p.userid);
                if (p.status == "WAITING FOR PICKUP")
                {
                    donator = await getUserName(p.userid);
                    volunteer = await getVolunteer(p.volunteerid);
                    if (volunteer.orgid == MainPage.userID)
                    {
                        Grid title = new Grid()
                        {
                            Margin = new Thickness(10, 0, 0, 0),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                        };

                        TextBlock titleloc = new TextBlock()
                        {
                            TextWrapping = TextWrapping.Wrap,
                            Text = donator.address,
                            FontSize = 26.667,
                            Foreground = new SolidColorBrush(Color.FromArgb(255, 35, 78, 102)),
                            Margin = new Thickness(0, 5, 0, 0),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                            VerticalAlignment = System.Windows.VerticalAlignment.Center
                        };

                        title.Children.Add(titleloc);

                        var x = donator.name;

                        //<StackPanel HorizontalAlignment="Left" VerticalAlignment="Top" Width="427" Background="#FF3D7391" Margin="10,0,0,20">

                        var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
                        List<Item> items = new List<Item>();
                        foreach (Item item in itemList)
                        {
                            if (p.id.Equals(item.postid))
                                items.Add(item);
                        }

                        Grid grid = new Grid()
                        {
                            Margin = new Thickness(10,5,10,0)
                        };

                        TextBlock tbVol = new TextBlock()
                        {
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                            TextWrapping = TextWrapping.Wrap,
                            FontSize = 16,
                            Foreground = new SolidColorBrush(Color.FromArgb(255, 164, 215, 249)),
                            VerticalAlignment = System.Windows.VerticalAlignment.Top,
                            Text = "Volunteer: " + volunteer.name
                        };

                        TextBlock tbDon = new TextBlock()
                        {
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            TextWrapping = TextWrapping.Wrap,
                            FontSize = 16,
                            Foreground = new SolidColorBrush(Color.FromArgb(255, 164, 215, 249)),
                            VerticalAlignment = System.Windows.VerticalAlignment.Top,
                            Text = "donated by: " + donator.name
                        };

                        grid.Children.Add(tbVol);
                        grid.Children.Add(tbDon);
                        
                        Grid buttContainer = new Grid()
                        {
                            Height = 70
                        };

                        Button moreDet = new Button()
                        {
                            Content = "More Details",
                            BorderThickness = new Thickness(2),
                            Background = new SolidColorBrush(Color.FromArgb(255, 16, 64, 97)),
                            VerticalAlignment = System.Windows.VerticalAlignment.Top,
                            BorderBrush = new SolidColorBrush(Color.FromArgb(255, 12, 37, 54)),
                            Margin = new Thickness(0),
                        };

                        buttContainer.Children.Add(moreDet);

                        //b.Content = "Donation by: " + donatorName + "\nQuantity: " + donateQty;
                        moreDet.Click += (s, e) =>
                        {
                            NavigationService.Navigate(new Uri("/DonationPage.xaml?param0=" + p.id + "&param1=" + donator.id, UriKind.Relative));
                        };
                    
                            StackPanel postPanel = new postStackPanel(items);
                            postPanel.Children.Add(grid);
                            postPanel.Children.Add(buttContainer);

                            readyList.Children.Add(title);
                            readyList.Children.Add(postPanel);
                    }
                }
                else if (p.status == "DELIVERED")
                {
                    volunteer = await getVolunteer(p.volunteerid);

                    if (volunteer.orgid == MainPage.userID)
                    {
                        if (!dates.ContainsKey(p.datepickedup.ToString("MMMM dd yyyy")))
                        {
                            Dictionary<string, int> tally = new Dictionary<string, int>();
                            GetReliefGoodImg goods = new GetReliefGoodImg();
                            foreach (var thing in goods.Keys)
                            {
                                tally.Add(thing, 0);
                            }

                            dates.Add(p.datepickedup.ToString("MMMM dd yyyy"), tally);
                        }

                        //<StackPanel HorizontalAlignment="Left" VerticalAlignment="Top" Width="427" Background="#FF3D7391" Margin="10,0,0,20">

                        var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
                        foreach (Item item in itemList)
                        {
                            if (p.id.Equals(item.postid))
                            {
                                dates[p.datepickedup.ToString("MMMM dd yyyy")][item.category] += item.quantity;//dates[p.datepickedup.ToString("MMMM dd yyyy")][item.category];
                                
                            }
                        }

                    }
                }
            }
            postPickedups(dates);

            loadingBar1.Visibility = System.Windows.Visibility.Collapsed;
            loadingBar2.Visibility = System.Windows.Visibility.Collapsed;
            //if (readyList.Children.Count == 0)
            //    readyList.Children.Add(new noneChild("No available pickups."));
            if (pickedupList1.Children.Count == 0)
                pickedupList1.Children.Add(new noneChild("No donations have been picked up."));
        }

        private void postPickedups(Dictionary<string, Dictionary<string, int>> dates)
        {
            foreach (var day in dates.Values)
            {
                List<Item> items = new List<Item>();
                foreach (var temp in day.Keys)
                {
                    items.Add(new Item()
                    {
                        quantity = day[temp],
                        category = temp
                    });
                }
                Grid title = new Grid()
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                };

                TextBlock titleloc = new TextBlock()
                {
                    Text = dates.Where(date => date.Value == day).First().Key + "",
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 26.667,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 35, 78, 102)),
                    Margin = new Thickness(0, 5, 0, 0),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };

                title.Children.Add(titleloc);
                StackPanel postPanel = new postStackPanel(items);

                pickedupList1.Children.Add(title);
                pickedupList1.Children.Add(postPanel);
            }
        }

        private async Task<Account> getUserName(string userid)
        {
            var accountList = await App.MobileService.GetTable<Account>().ToListAsync();
            System.Diagnostics.Debug.WriteLine("entering getUserName");
            foreach (Account a in accountList)
            {
                System.Diagnostics.Debug.WriteLine("comparing " + userid + " & " + a.id);
                if (a.id.Equals(userid))
                {
                    //donator = a;
                    return a;
                }
            }

            System.Diagnostics.Debug.WriteLine("DID NOT FIND " + userid);
            return null;
        }

        private async Task<Account> getVolunteer(string userid)
        {
            var accountList = await App.MobileService.GetTable<Account>().ToListAsync();
            System.Diagnostics.Debug.WriteLine("entering getVolunteer");
            foreach (Account a in accountList)
            {
                System.Diagnostics.Debug.WriteLine("comparing " + userid + " & " + a.id);
                if (a.id.Equals(userid))
                {
                    //donator = a;
                    return a;
                }
            }

            System.Diagnostics.Debug.WriteLine("DID NOT FIND " + userid);
            return null;
        }

    }
}