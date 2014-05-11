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
    public partial class ListOfOrgs : PhoneApplicationPage
    {
        public ListOfOrgs()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int x = orgList.Children.Count;
            for (int i = 0; i < x; i++)
                orgList.Children.RemoveAt(0);

            loadOrganizations();
            loadingBar.Visibility = Visibility.Collapsed;
        }

        private async void loadOrganizations()
        {
            var orgs = await App.MobileService.GetTable<OrgAccount>().ToListAsync();
            foreach (OrgAccount org in orgs)
            {
                if(org.isverified == "VERIFIED")
                {
                    Button b = new Button();
                    b.Height = 150;
                    b.Width = 450;
                    b.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                    b.Content = org.name + "\n" + org.officelocation + "\n" + org.contactnum;
                    //b
                    //orgList.Children.Add(b);
                    /*
                     *         		
                     * <Button Background="#FFE03F3F" BorderBrush="#FFBF2727">
                        <Grid Height="100" Width="394">
                        </Grid>
                    </Button>
                     * */

                    Button button = new Button();
                    button.Background = new SolidColorBrush(Color.FromArgb(255, 224, 63, 63));
                    button.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 191, 39, 39));
                    Grid grid = new Grid();
                    grid.Height = 100;
                    grid.Width = 394;
                    button.Style = this.Resources["Btn"] as Style;

                    /*
                     * <TextBlock TextWrapping="Wrap" Text="Zebra Foundation" Foreground="White" Margin="127,0,0,0" VerticalAlignment="Top" HorizontalAlignment="Left" 
                     * FontSize="29.333"/>

                            <Image HorizontalAlignment="Left" Height="100" VerticalAlignment="Top" Width="122"/>
                            <TextBlock TextWrapping="Wrap" Text="Office loc.:" Foreground="#FFF1C2C2" Margin="127,44,0,0" VerticalAlignment="Top" HorizontalAlignment="Left" 
                     * FontSize="12"/>
                            <TextBlock TextWrapping="Wrap" Foreground="#FFF1C2C2" Margin="201,44,0,0" VerticalAlignment="Top" FontSize="12" Width="193" Height="33" HorizontalAlignment="Right">
                                <Run Text="2401 Taft Ave, Manila, "/>
                                <Run Text="Malacanang Palace, "/>
                                <Run Text="Philippines"/>
                            </TextBlock>
                            <TextBlock TextWrapping="Wrap" Text="Contact no:" Foreground="#FFF1C2C2" Margin="127,79,0,0" VerticalAlignment="Top" HorizontalAlignment="Left" FontSize="12"/>
                            <TextBlock TextWrapping="Wrap" Foreground="#FFF1C2C2" Margin="201,79,0,0" VerticalAlignment="Top" HorizontalAlignment="Right" FontSize="12" Width="193" Height="16" Text="09229231469"/>
                        */

                    TextBlock name = new TextBlock();
                    name.TextWrapping = TextWrapping.Wrap;
                    name.Foreground = new SolidColorBrush(Colors.White);
                    name.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    name.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    name.FontSize = 29.333;
                    name.Text = org.name;

                    TextBlock offLocTb = new TextBlock();
                    offLocTb.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 194, 194));
                    offLocTb.Text = "Office loc.:";
                    offLocTb.Margin = new Thickness(10, 44, 0, 0);
                    offLocTb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    offLocTb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    offLocTb.FontSize = 12;

                    TextBlock contactTb = new TextBlock();
                    contactTb.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 194, 194));
                    contactTb.Text = "Contact no.:";
                    contactTb.Margin = new Thickness(10, 79, 0, 0);
                    contactTb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    contactTb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    contactTb.FontSize = 12;

                    TextBlock offLoc = new TextBlock();
                    offLoc.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 194, 194));
                    offLoc.Text = org.officelocation;
                    offLoc.Margin = new Thickness(84, 44, 0, 0);
                    offLoc.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    offLoc.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    offLoc.FontSize = 12;
                    offLoc.Width = 193;

                    TextBlock contactno = new TextBlock();
                    contactno.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 194, 194));
                    contactno.Text = org.contactnum;
                    contactno.Margin = new Thickness(84, 79, 0, 0);
                    contactno.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    contactno.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    contactno.FontSize = 12;
                    contactno.Width = 193;

                    Image logo = new Image();
                    logo.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    logo.Height = 100;
                    logo.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    logo.Width = 122;

                    grid.Children.Add(name);
                    grid.Children.Add(offLocTb);
                    grid.Children.Add(offLoc);
                    grid.Children.Add(contactTb);
                    grid.Children.Add(contactno);

                    button.Content = grid;

                    button.Click += (s, e) =>
                    {
                        NavigationService.Navigate(new Uri("/OrgPage.xaml?param0=" + org.id, UriKind.Relative));
                    };

                    orgList.Children.Add(button);
                }
            }

            if (orgList.Children.Count == 0)
            {
                orgList.Children.Add(new noneChild("No available organizations."));
            }
        }
    }
}