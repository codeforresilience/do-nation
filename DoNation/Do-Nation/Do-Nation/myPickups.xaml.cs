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
using System.Windows.Media.Imaging;

namespace Do_Nation
{
    public partial class myPickups : PhoneApplicationPage
    {
        public myPickups()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int x = pickupList.Children.Count;
            for (int i = 0; i < x; i++)
                pickupList.Children.RemoveAt(0);
            loadDonations();
        }

        Account donator;
        string donateQty;

        private async void loadDonations()
        {
            int count = 0;
            var donations = await App.MobileService.GetTable<Post>().ToListAsync();
            foreach (Post p in donations)
            {
                if (p.status == "WAITING FOR PICKUP")
                {
                    count++;
                    donator = await getUserName(p.userid);
                    donateQty = await getTotalQuantity(p.id);

                    var x = donator.name;

                    //<StackPanel HorizontalAlignment="Left" VerticalAlignment="Top" Width="427" Background="#FF3D6F91" Margin="10,0,0,20">

                    StackPanel postPanel = new StackPanel();
                    postPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    postPanel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    postPanel.Width = 427;
                    postPanel.Background = new SolidColorBrush(Color.FromArgb(255, 61, 111, 145));
                    postPanel.Margin = new Thickness(10, 0, 0, 20);

                    /*
                                <Grid>
                                    <TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" Text="NEW" FontSize="29.333" Foreground="#FF79F9E4"
                     * Margin="10,5,0,0" VerticalAlignment="Top" FontWeight="Bold"/>
                                    <TextBlock TextWrapping="Wrap" Text="Office of Queson City" FontSize="20" Foreground="#FFBEE5FF" 
                     * Margin="0,5,20,0" HorizontalAlignment="Right" VerticalAlignment="Center"/>
                                </Grid>
                     * */

                    Grid grid = new Grid();

                    TextBlock statusVal = new TextBlock();
                    statusVal.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    statusVal.TextWrapping = TextWrapping.Wrap;
                    statusVal.Text = p.status;
                    statusVal.FontSize = 29.333;
                    statusVal.Margin = new Thickness(10, 5, 0, 0);
                    statusVal.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    statusVal.FontWeight = FontWeights.Bold;

                    TextBlock addressVal = new TextBlock();
                    addressVal.TextWrapping = TextWrapping.Wrap;
                    addressVal.Text = donator.address;
                    addressVal.FontSize = 20;
                    addressVal.Margin = new Thickness(0, 5, 20, 0);
                    addressVal.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    addressVal.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                    grid.Children.Add(statusVal);
                    grid.Children.Add(addressVal);

                    //<StackPanel Orientation="Horizontal" Margin="0,0,29,0">
                    StackPanel columns = new StackPanel();
                    columns.Orientation = System.Windows.Controls.Orientation.Horizontal;
                    columns.Margin = new Thickness(0, -5, 0, -15);

                    //<StackPanel x:Name="col5" Margin="12,10,0,10" Width="196" VerticalAlignment="Top" HorizontalAlignment="Center">

                    StackPanel col1 = new StackPanel();
                    col1.Margin = new Thickness(12, 10, 0, 10);
                    col1.Width = 196;
                    col1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    col1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                    StackPanel col2 = new StackPanel();
                    col2.Margin = new Thickness(12, 10, 0, 10);
                    col2.Width = 196;
                    col2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    col2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                    columns.Children.Add(col1);
                    columns.Children.Add(col2);

                    /*
                     *  <Grid Width="196" VerticalAlignment="Top" Height="68" Background="#FFC0E666" Margin="0,0,0,5">
                            <Image HorizontalAlignment="Left" VerticalAlignment="Top" Source="/Assets/postorgpostIcon/annuncement.png" 
                     * Margin="5,5,0,5"/>
                            <TextBlock HorizontalAlignment="Left" Margin="92,0,0,0" TextWrapping="Wrap" VerticalAlignment="Top" 
                     * \Width="104" FontSize="16" Text="Food:" Foreground="#FF0B5716"/>
                            <TextBlock HorizontalAlignment="Right" Margin="0,21,9,0" TextWrapping="Wrap" Text="18" VerticalAlignment="Top" 
                     * FontSize="29.333" Foreground="#FF0B5716"/>
                        </Grid>
                     * */

                    var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
                    List<Item> items = new List<Item>();
                    foreach (Item item in itemList)
                    {
                        if (p.id.Equals(item.postid))
                            items.Add(item);
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        Grid entry = new Grid();
                        entry.Width = 196;
                        entry.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        entry.Height = 68;
                        if ((i / 2) % 2 == 0)
                            entry.Background = new SolidColorBrush(Color.FromArgb(255, 192, 230, 102));
                        else
                            entry.Background = new SolidColorBrush(Color.FromArgb(255, 209, 236, 147));
                        entry.Margin = new Thickness(0, 0, 0, 5);


                        TextBlock category = new TextBlock();
                        category.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        category.Margin = new Thickness(92, 0, 0, 0);
                        category.TextWrapping = TextWrapping.Wrap;
                        category.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        category.FontSize = 16;
                        category.Foreground = new SolidColorBrush(Color.FromArgb(255, 11, 87, 22));
                        category.Text = items.ElementAt(i).category + ":";

                        Image img = new Image();
                        img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        img.Margin = new Thickness(5, 5, 0, 5);
                        try
                        {
                            img.Source = new BitmapImage(new Uri(new GetReliefGoodImg()[items.ElementAt(i).category], UriKind.Relative));
                        }
                        catch (Exception)
                        {
                            img.Source = new BitmapImage(new Uri("/Assets/icons_goods/others.png", UriKind.Relative));
                        }

                        TextBlock qty = new TextBlock();
                        qty.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        qty.Margin = new Thickness(0, 21, 9, 0);
                        qty.TextWrapping = TextWrapping.Wrap;
                        qty.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        qty.FontSize = 29.333;
                        qty.Foreground = new SolidColorBrush(Color.FromArgb(255, 11, 87, 22));
                        qty.Text = items.ElementAt(i).quantity + "";

                        entry.Children.Add(img);
                        entry.Children.Add(category);
                        entry.Children.Add(qty);

                        if (i % 2 == 0)
                        {
                            col1.Children.Add(entry);
                        }
                        else
                        {
                            col2.Children.Add(entry);
                        }
                    }

                    //<TextBlock HorizontalAlignment="Right" TextWrapping="Wrap" FontSize="12" Foreground="#FFA4D7F9" Margin="0,6,10,0" 
                    //VerticalAlignment="Top" Width="97">

                    TextBlock donatortb = new TextBlock();
                    donatortb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    donatortb.TextWrapping = TextWrapping.Wrap;
                    donatortb.FontSize = 14;
                    donatortb.Margin = new Thickness(0, 6, 10, 0);
                    donatortb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    donatortb.Text = "donated by " + donator.name;

                    //<Button Content="View Details" BorderThickness="2" Background="#FF104061" VerticalAlignment="Top" 
                    //BorderBrush="#FF0C2536" Margin="0,-4,0,0"/>


                    Button viewBtn = new Button();
                    viewBtn.BorderThickness = new Thickness(2);
                    viewBtn.Background = new SolidColorBrush(Color.FromArgb(255, 16, 64, 97));
                    viewBtn.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    viewBtn.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 12, 37, 54));
                    viewBtn.Margin = new Thickness(0, -4, 0, 0);
                    viewBtn.Content = "View Details";

                    //b.Content = "Donation by: " + donatorName + "\nQuantity: " + donateQty;
                    viewBtn.Click += (s, e) =>
                    {
                        NavigationService.Navigate(new Uri("/PickupPage.xaml?param0=" + p.id + "&param1=" + donator.id, UriKind.Relative));
                    };

                    if (statusVal.Text.Equals("NEW"))
                    {
                        statusVal.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 241, 52));
                        addressVal.Foreground = new SolidColorBrush(Color.FromArgb(255, 190, 229, 255));
                        donatortb.Foreground = new SolidColorBrush(Color.FromArgb(255, 164, 215, 249));
                    }
                    else
                    {
                        statusVal.Foreground = new SolidColorBrush(Color.FromArgb(255, 31, 81, 116));
                        addressVal.Foreground = new SolidColorBrush(Color.FromArgb(255, 31, 81, 116));
                        donatortb.Foreground = new SolidColorBrush(Color.FromArgb(255, 31, 81, 116));
                    }
                    postPanel.Children.Add(grid);
                    postPanel.Children.Add(columns);
                    postPanel.Children.Add(donatortb);
                    postPanel.Children.Add(viewBtn);
                    pickupList.Children.Add(postPanel);
                }

            }
            if (count == 0)
            {
                TextBlock notif = new TextBlock();
                notif.Text = "No available pickups.";
                notif.HorizontalAlignment = HorizontalAlignment.Center;
                pickupList.Children.Add(notif);
            }
        }

        private async Task<string> getTotalQuantity(string postid)
        {
            int count = 0;
            var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
            foreach (Item i in itemList)
            {
                if (i.postid.Equals(postid))
                {
                    count++;
                }
            }
            return count.ToString();
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
    }
}