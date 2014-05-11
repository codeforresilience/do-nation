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
    public partial class PickupPage : PhoneApplicationPage
    {
        string donationid = "";
        string donatorID = "";
        string donatorName = "";
        string donatorContact = "";
        string donatorEmail = "";
        Post post;

        public PickupPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                donationid = NavigationContext.QueryString["param0"];
            }
            catch (Exception) { }
            loadContactDetails();
            loadDonationDetails();
            loadItems();
        }



        private async void loadContactDetails()
        {
            var accountList = await App.MobileService.GetTable<Account>().ToListAsync();
            foreach (Account a in accountList)
            {
                if (a.id.Equals(donatorID))
                {
                    donatorName = a.name;
                    contactNum.Text = a.contactnumber;
                    email.Text = a.email;
                    donatorContact = a.contactnumber;
                    donatorEmail = a.email;
                    break;
                }
            }
        }

        private async void loadDonationDetails()
        {
            var donationList = await App.MobileService.GetTable<Post>().ToListAsync();
            foreach (Post p in donationList)
            {
                if (p.id.Equals(donationid))
                {
                    statustb.Text = p.status;
                    postDetails.Text = "by " + donatorName + " posted on " + p.dateposted;

                    availableTime.Text = p.availabletime;
                    specialInstruction.Text = p.specialinstruction;
                    location.Text = p.address;
                    contactNum.Text = donatorContact;
                    email.Text = donatorEmail;
                    post = p;
                    break;
                }
            }
        }

        private async void loadItems()
        {
            //var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
            //foreach (Item i in itemList)
            //{
            //    if (i.postid.Equals(donationid))
            //    {
            //        TextBlock txt = new TextBlock();
            //        txt.Text = i.category + " " + i.quantity.ToString();
            //        itemPanel.Children.Add(txt);
            //    }
            //}
            var itemList = await App.MobileService.GetTable<Item>().ToListAsync();
            List<Item> items = new List<Item>();
            foreach (Item item in itemList)
            {
                if (item.postid.Equals(donationid))
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

                TextBlock category = new TextBlock();
                category.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                category.Margin = new Thickness(92, 0, 0, 0);
                category.TextWrapping = TextWrapping.Wrap;
                category.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                category.FontSize = 16;
                category.Foreground = new SolidColorBrush(Color.FromArgb(255, 11, 87, 22));
                category.Text = items.ElementAt(i).category + ":";

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
        }

        private async void donationDelivered(object sender, System.Windows.RoutedEventArgs e)
        {
            post.status = "DELIVERED";
            await App.MobileService.GetTable<Post>().UpdateAsync(post);
        }
    }
}