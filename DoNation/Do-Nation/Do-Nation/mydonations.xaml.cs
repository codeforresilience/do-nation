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

namespace Do_Nation
{
    public partial class mydonations : PhoneApplicationPage
    {
        public mydonations()
        {
            InitializeComponent();
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int x = myDonats.Children.Count;
            for (int i = 0; i < x; i++)
                myDonats.Children.RemoveAt(0);

            var postList = await App.MobileService.GetTable<Post>().ToListAsync();

            foreach (Post post in postList)
            {
                if (post.userid.Equals(MainPage.userID) && !post.status.Equals("DELIVERED"))
                {
                    var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
                    List<Item> items = new List<Item>();
                    foreach(Item item in itemList)
                    {
                        if(post.id.Equals(item.postid))
                            items.Add(item);
                    }
                    addDonation(items , post);
                }
            }

            if (myDonats.Children.Count == 0)
                myDonats.Children.Add(new noneChild("No available donations."));

        }

        private void addDonation(List<Item> items, Post post)
        {
            string datePosted = post.dateposted.ToString("MMMM d, yyyy");
            string time = post.dateposted.ToString("hh:mm tt");
            string statustext = post.status; 
            string timeAvailabletext = post.availabletime;
            string specIns = post.specialinstruction;

            /*  
             * <StackPanel Margin="10,0,19,0" Orientation="Horizontal" Height="54">
        				<TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" VerticalAlignment="Top" FontSize="40" Foreground="#FF385674">
        					<Run Text="December"/>
        					<Run Text=" 18, 2014"/>
        				</TextBlock>
        				<TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" Text="6:00PM" VerticalAlignment="Top" FontSize="21.333" Foreground="#FF385674" Margin="5,20,0,0"/>
        			</StackPanel>
             * */
            StackPanel title = new StackPanel()
            {
                Margin = new Thickness(10,0,19,0),
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                Height=54
            };
            TextBlock date = new TextBlock();
            date.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            date.TextWrapping = TextWrapping.Wrap;
            date.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            date.FontSize = 40;
            date.Foreground = new SolidColorBrush(Color.FromArgb(255, 56, 86, 116));

            TextBlock timetb = new TextBlock()
            { 
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Text=time,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                FontSize=21.333,
                Foreground=new SolidColorBrush(Color.FromArgb(255, 56, 86, 116)),
                Margin = new Thickness(10,20,0,0)
            };

            title.Children.Add(date);
            title.Children.Add(timetb);

            //<StackPanel HorizontalAlignment="Left" VerticalAlignment="Top" Width="427" Background="#FF385674" Margin="10,0,0,0">
        				
            StackPanel donatMainPanel = new StackPanel();
            donatMainPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            donatMainPanel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            donatMainPanel.Width = 427;
            donatMainPanel.Background = new SolidColorBrush(Color.FromArgb(255, 56, 86, 116));
            donatMainPanel.Margin = new Thickness(10, 0, 0, 0);

            StackPanel columns = new StackPanel();
            columns.Orientation = System.Windows.Controls.Orientation.Horizontal;


            //<TextBlock HorizontalAlignment="Center" TextWrapping="Wrap" FontSize="29.333" Foreground="#FFB9F134" Margin="0,3,0,-3" VerticalAlignment="Bottom" FontWeight="Bold">
        					
            TextBlock statusVal = new TextBlock();
            statusVal.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            statusVal.TextWrapping = TextWrapping.Wrap;
            statusVal.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            statusVal.FontSize = 29.333;
            statusVal.FontWeight = FontWeights.Bold;
            statusVal.Margin = new Thickness(0, 3, 0, -3);

            //<StackPanel x:Name="col1" Margin="12,10,0,10" Width="196" VerticalAlignment="Top" HorizontalAlignment="Center">

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
        			<Image HorizontalAlignment="Left" Height="68" VerticalAlignment="Top" Width="87"/>
        			<TextBlock HorizontalAlignment="Left" Margin="92,0,0,0" TextWrapping="Wrap" VerticalAlignment="Top" 
             * \Width="104" FontSize="16" Text="Food:" Foreground="#FF0B5716"/>
        			<TextBlock HorizontalAlignment="Right" Margin="0,21,9,0" TextWrapping="Wrap" Text="18" VerticalAlignment="Top" 
             * FontSize="29.333" Foreground="#FF0B5716"/>
        		</Grid>
             * */

            for (int i = 0; i < items.Count; i++)
            {
                Grid entry = new Grid();
                entry.Width = 196;
                entry.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                entry.Height = 68;
                if((i/2)%2 == 0)
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

            /*
                <StackPanel VerticalAlignment="Bottom" Margin="0">
        			<TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" Text="Time Available" VerticalAlignment="Top" Margin="10,0,0,3" 
             * Width="407" Foreground="#FF8EF1DF" FontSize="13.333"/>
        			<TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" Text="M - 8:00 - 12:00PM" VerticalAlignment="Top" 
             * Margin="26,0,0,10" Width="391" Foreground="#FF8EF1DF" FontSize="21.333"/>
        			<TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" Text="Special Instruction" VerticalAlignment="Top" 
             * Margin="10,0,0,3" Width="407" Foreground="#FF8EF1DF" FontSize="13.333"/>
        			<TextBlock HorizontalAlignment="Left" TextWrapping="Wrap" Margin="26,0,0,10" Width="391" Foreground="#FF8EF1DF" 
             * FontSize="21.333" VerticalAlignment="Bottom" Text="Put Balot somewhere safe"/>
                </StackPanel>
             * */

            StackPanel infoStack = new StackPanel();
            infoStack.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            infoStack.Margin = new Thickness(0);

            TextBlock timeAvailableTb = new TextBlock();
            timeAvailableTb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            timeAvailableTb.TextWrapping = TextWrapping.Wrap;
            timeAvailableTb.Text = "Time Available";
            timeAvailableTb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            timeAvailableTb.Margin = new Thickness(10, 0, 0, 3);
            timeAvailableTb.Width = 407;
            timeAvailableTb.Foreground = new SolidColorBrush(Color.FromArgb(255, 142, 241, 223));
            timeAvailableTb.FontSize = 13.333;

            TextBlock timeAvailable = new TextBlock();
            timeAvailable.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            timeAvailable.TextWrapping = TextWrapping.Wrap;
            timeAvailable.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            timeAvailable.Margin = new Thickness(26, 0, 0, 10);
            timeAvailable.Width = 391;
            timeAvailable.Foreground = new SolidColorBrush(Color.FromArgb(255, 142, 241, 223));
            timeAvailable.FontSize = 21.333;

            TextBlock specialInsTb = new TextBlock();
            specialInsTb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            specialInsTb.TextWrapping = TextWrapping.Wrap;
            specialInsTb.Text = "Special Instruction";
            specialInsTb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            specialInsTb.Margin = new Thickness(10, 0, 0, 3);
            specialInsTb.Width = 407;
            specialInsTb.Foreground = new SolidColorBrush(Color.FromArgb(255, 142, 241, 223));
            specialInsTb.FontSize = 13.333;

            TextBlock specialIns = new TextBlock();
            specialIns.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            specialIns.TextWrapping = TextWrapping.Wrap;
            specialIns.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            specialIns.Margin = new Thickness(26, 0, 0, 10);
            specialIns.Width = 391;
            specialIns.Foreground = new SolidColorBrush(Color.FromArgb(255, 142, 241, 223));
            specialIns.FontSize = 21.333;

            donatMainPanel.Children.Add(statusVal);
            donatMainPanel.Children.Add(columns);
            donatMainPanel.Children.Add(timeAvailableTb);
            donatMainPanel.Children.Add(timeAvailable);
            donatMainPanel.Children.Add(specialInsTb);
            donatMainPanel.Children.Add(specialIns);

            myDonats.Children.Add(title);
            myDonats.Children.Add(donatMainPanel);

            date.Text = datePosted;
            statusVal.Text = statustext;
            if (statustext.Equals("NEW"))
                statusVal.Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 235, 219));
            else if (statustext.Equals("WAITING FOR PICKUP"))
                statusVal.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 241, 52));
            else if (statustext.Equals("PENDING FOR APPROVAL"))
                statusVal.Foreground = new SolidColorBrush(Color.FromArgb(255, 249, 249, 77));
            timeAvailable.Text = timeAvailabletext;
            specialIns.Text = specIns;
        }
    }
}