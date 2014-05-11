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
using System.Windows.Documents;

namespace Do_Nation
{
    public partial class myPickUpList : PhoneApplicationPage
    {
        public myPickUpList()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int x = readyList.Children.Count;
            for (int i = 0; i < x; i++)
                readyList.Children.RemoveAt(0);
            x = pendingList.Children.Count;
            for (int i = 0; i < x; i++)
                pendingList.Children.RemoveAt(0);
            x = pickedupList1.Children.Count;
            for (int i = 0; i < x; i++)
                pickedupList1.Children.RemoveAt(0);

            loadDonations();

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PanoramaMenuPage.xaml", UriKind.Relative));
            e.Cancel = true;
        }

        Account donator;

        private async void loadDonations()
        {
            var donations = await App.MobileService.GetTable<Post>().ToListAsync();
            foreach (Post p in donations)
            {
                if (p.status == "WAITING FOR PICKUP")
                {
                    donator = await getUserName(p.userid);
                    /*
                     * <Grid Margin="10,0,0,0" HorizontalAlignment="Left">
									<TextBlock TextWrapping="Wrap" Text="Office of Queson City" FontSize="26.667" Foreground="#FF234E66" Margin="0,5,0,0" HorizontalAlignment="Left" VerticalAlignment="Center"/>
								</Grid>*/
                    
                    Grid title = new Grid()
                    {
                        Margin = new Thickness(10,0,0,0),
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                    };

                    TextBlock titleloc = new TextBlock()
                    {
                        TextWrapping = TextWrapping.Wrap,
                        Text = donator.address,
                        FontSize= 26.667,
                        Foreground = new SolidColorBrush(Color.FromArgb(255,35, 78, 102)),
                        Margin = new Thickness(0,5,0,0),
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

                    /*
                     * 
                     * <StackPanel VerticalAlignment="Bottom" Margin="0,6,0,0">
                            <TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" Text="Time Available" VerticalAlignment="Top" Margin="10,0,0,-5" Width="407" Foreground="#FF8EF1DF" FontSize="18.667"/>
                            <TextBlock x:Name="availableTime" HorizontalAlignment="Left" TextWrapping="Wrap" Text="" VerticalAlignment="Top" Margin="26,0,0,10" Width="391" Foreground="#FF8EF1DF" FontSize="32"/>
                        </StackPanel>
                     * */
                    

                    //<TextBlock HorizontalAlignment="Right" TextWrapping="Wrap" FontSize="12" Foreground="#FFA4D7F9" Margin="0,6,10,0" 
                    //VerticalAlignment="Top" Width="97">

                    TextBlock donatortb = new TextBlock();
                    donatortb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    donatortb.TextWrapping = TextWrapping.Wrap;
                    donatortb.FontSize = 14;
                    donatortb.Margin = new Thickness(0, 6, 10, 0);
                    donatortb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    donatortb.Text = "donated by " + donator.name;

                    /*
                     * 	<Grid Height="70">
										<Button Content="More Details" BorderThickness="2" Background="#FF104061" VerticalAlignment="Top" BorderBrush="#FF0C2536" Margin="0" HorizontalAlignment="Left"/>
										<Button Content="Mark as Picked Up" BorderThickness="2" Background="#FF104061" VerticalAlignment="Top" BorderBrush="#FF0C2536" Margin="0" HorizontalAlignment="Right" Width="243"/>
									</Grid>
                     */

                    Grid buttContainer = new Grid()
                    {
                        Height = 70
                    };

                    Button moreDet = new Button()
                    {
                        Content="More Details",
                        BorderThickness = new Thickness(2),
                        Background = new SolidColorBrush(Color.FromArgb(255,16, 64, 97)),
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255,12, 37, 54)),
                        Margin = new Thickness(0),
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                    };

                    Button mark = new Button()
                    {
                        Content="Mark as Picked Up",
                        BorderThickness = new Thickness(2),
                        Background = new SolidColorBrush(Color.FromArgb(255,16, 64, 97)),
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255,12, 37, 54)),
                        Margin = new Thickness(0),
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Right, 
                        Width=243,
                        Style = Resources["pickupstyle"] as Style
                    };

                    buttContainer.Children.Add(moreDet);
                    buttContainer.Children.Add(mark);
                    
                    //b.Content = "Donation by: " + donatorName + "\nQuantity: " + donateQty;
                    moreDet.Click += (s, e) =>
                    {
                        NavigationService.Navigate(new Uri("/DonationPage.xaml?param0=" + p.id + "&param1=" + donator.id, UriKind.Relative));
                    };

                    mark.Click += (s, e) =>
                    {
                        updatePostStatus("DELIVERED", p.id);
                        mark.IsEnabled = false;
                    };

                    StackPanel postPanel = new postStackPanel(items);
                    postPanel.Children.Add(new myPickUpListInfoPanel("Time Available:", p.availabletime));
                    postPanel.Children.Add(new myPickUpListInfoPanel("Special Instruction:", p.specialinstruction));
                    postPanel.Children.Add(new myPickUpListInfoPanel("Contact no.:", donator.contactnumber));
                    postPanel.Children.Add(donatortb);
                    postPanel.Children.Add(buttContainer);

                    readyList.Children.Add(title);
                    readyList.Children.Add(postPanel);
                }
                else if (p.status == "PENDING FOR APPROVAL")
                {
                    donator = await getUserName(p.userid);

                    var x = donator.name;

                    //<StackPanel HorizontalAlignment="Left" VerticalAlignment="Top" Width="427" Background="#FF3D7391" Margin="10,0,0,20">

                    var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
                    List<Item> items = new List<Item>();
                    foreach (Item item in itemList)
                    {
                        if (p.id.Equals(item.postid))
                            items.Add(item);
                    }

                    TextBlock donatortb = new TextBlock();
                    donatortb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    donatortb.TextWrapping = TextWrapping.Wrap;
                    donatortb.FontSize = 14;
                    donatortb.Margin = new Thickness(0, 6, 10, 0);
                    donatortb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    donatortb.Text = "donated by " + donator.name;


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
                    postPanel.Children.Add(new myPickUpListInfoPanel("Contact no.:", donator.contactnumber));
                    postPanel.Children.Add(new myPickUpListInfoPanel("Location:", donator.address));
                    postPanel.Children.Add(donatortb);
                    postPanel.Children.Add(buttContainer);

                    pendingList.Children.Add(postPanel);
                }
                else if (p.status == "DELIVERED")
                {
                    postPickedup(p);
                }

            }
            loadingBar1.Visibility = System.Windows.Visibility.Collapsed;
            loadingBar2.Visibility = System.Windows.Visibility.Collapsed;
            loadingBar3.Visibility = System.Windows.Visibility.Collapsed;
           // if (readyList.Children.Count == 0)
             //   readyList.Children.Add(new noneChild("No available pickups."));
           // if (pendingList.Children.Count == 0)
             //   pendingList.Children.Add(new noneChild("No pending requests."));
            if (pickedupList1.Children.Count == 0)
                pickedupList1.Children.Add(new noneChild("No donations have been picked up."));
        }

        private async void postPickedup(Post p)
        {
            donator = await getUserName(p.userid);

            var x = donator.name;

            //<StackPanel HorizontalAlignment="Left" VerticalAlignment="Top" Width="427" Background="#FF3D7391" Margin="10,0,0,20">

            var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
            List<Item> items = new List<Item>();
            foreach (Item item in itemList)
            {
                if (p.id.Equals(item.postid))
                    items.Add(item);
            }

            /*
             * 									<Grid Margin="10,0,0,0" HorizontalAlignment="Left">
                                <TextBlock TextWrapping="Wrap" FontSize="26.667" Foreground="#FF234E66" Margin="0,5,0,0" HorizontalAlignment="Left" VerticalAlignment="Center">
                                    <Run FontSize="14.667" Text="on "/>
                                    <Run Text="April 17, 2014"/>
                                </TextBlock>
                            </Grid>
             * */

            Grid title = new Grid()
            {
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left
            };

            TextBlock titleVal = new TextBlock()
            {
                TextWrapping = System.Windows.TextWrapping.Wrap,
                FontSize = 26.667,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 35, 78, 102)),
                Margin = new Thickness(0, 5, 0, 0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
            };

            titleVal.Inlines.Add(new Run()
            {
                FontSize = 14.667,
                Text = "on"
            });
            titleVal.Inlines.Add(new Run()
            {
                Text = p.datepickedup.ToString("MMMM d, yyyy")
            });

            TextBlock donatortb = new TextBlock();
            donatortb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            donatortb.TextWrapping = TextWrapping.Wrap;
            donatortb.FontSize = 14;
            donatortb.Margin = new Thickness(0, 6, 10, 0);
            donatortb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            donatortb.Text = "donated by " + donator.name;


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

            title.Children.Add(titleVal);
            StackPanel postPanel = new postStackPanel(items);
            postPanel.Children.Add(donatortb);
            postPanel.Children.Add(buttContainer);

            pickedupList1.Children.Add(title);
            pickedupList1.Children.Add(postPanel);
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

        private async void updatePostStatus(String status, String postID)
        {
            var results = await App.MobileService.GetTable<Post>().ToListAsync();

            foreach (Post a in results)
            {
                if (a.id.Equals(postID))
                {
                    a.status = status;
                    if (status.Equals("DELIVERED"))
                    {
                        a.datepickedup = DateTime.Now;
                        foreach (var child in pickedupList1.Children)
                        {
                            if(child as StackPanel != null)
                            if ((child as StackPanel).Name.Equals("nonechild"))
                            {

                                pickedupList1.Children.Remove(child);
                                break;
                            }

                        }
                        postPickedup(a);
                    }
                    await App.MobileService.GetTable<Post>().UpdateAsync(a);
                    break;
                }
            }
        }


    }
}