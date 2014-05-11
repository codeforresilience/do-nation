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
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Do_Nation
{
    public partial class OrgKeys : PhoneApplicationPage
    {
        public OrgKeys()
        {
            InitializeComponent();
            loadKeys();
        }

        private async void loadKeys()
        {
            var keyList = await App.MobileService.GetTable<OrganizationKeyList>().ToListAsync();
            foreach (OrganizationKeyList key in keyList)
            {
                if (key.organizationid.Equals(MainPage.userID) && key.status.Equals("APPROVED"))
                {
                    addtoList(key);
                }
            }
            if (keyListPanel.Children.Count == 0)
                keyListPanel.Children.Add(new noneChild("No available volunteers."));

        }

        private async void generateKey(object sender, System.Windows.RoutedEventArgs e)
        {
            OrganizationKeyList key = new OrganizationKeyList();
            key.volunteerid = "";
            key.organizationid = MainPage.userID;
            await App.MobileService.GetTable<OrganizationKeyList>().InsertAsync(key);

            addtoList(key);
        }

        private async void addtoList(OrganizationKeyList key)
        {
            /*        		
             * <Grid Background="#FF3E728B">
            <TextBlock Margin="10,10,10,0" TextWrapping="Wrap" VerticalAlignment="Top" Foreground="#FFC2FFEE" 
             * HorizontalAlignment="Center" Text="0E085FAC-D5EC-44A7-AD82-CDEEEE54504F"/>
            <TextBlock HorizontalAlignment="Center" Margin="0,42,0,10" TextWrapping="Wrap" Text="NOT YET TAKEN" 
             * VerticalAlignment="Bottom" Foreground="#FFD9E9F1" FontWeight="Bold"/>
            </Grid>
             * */
            Grid grid = new Grid();
            if (keyListPanel.Children.Count % 2 == 1)
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 62, 114, 139));
            else
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 58, 128, 162));
            grid.Margin = new Thickness(10, 10, 10, 10);
            
            TextBlock txt = new TextBlock();
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            txt.Margin = new Thickness(10, 10, 10, 10);
            txt.TextWrapping = TextWrapping.Wrap;
            txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 217, 233, 241));

            var accountList = await App.MobileService.GetTable<Account>().ToListAsync();
            foreach (Account a in accountList)
            {
                System.Diagnostics.Debug.WriteLine(a.volunteerid + " = " + key.volunteerid);
                if (a.id.Equals(key.volunteerid))
                {
                    txt.Inlines.Add(new Run()
                    {
                        FontSize = 30,
                        FontWeight = FontWeights.Bold,
                        Text = a.name
                    });
                    txt.Inlines.Add(new Run()
                    {
                        Text = "\nContact No: " + a.contactnumber + "\nEmail: " + a.email
                    });
                
                    break;
                }
            }
            grid.Children.Add(txt);
            keyListPanel.Children.Add(grid);
            scrollView.ScrollToVerticalOffset(keyListPanel.ActualHeight);

            if (keyListPanel.Children.Contains(new noneChild("No available volunteers.")))
                keyListPanel.Children.Remove(new noneChild("No available volunteers."));
        }

        private async Task<Account> getUserName(string volid)
        {
            var accountList = await App.MobileService.GetTable<Account>().ToListAsync();
            foreach (Account a in accountList)
            {
                if (a.volunteerid.Equals(volid))
                {
                    return a;
                }
            }
            return null;
        }
    }
}