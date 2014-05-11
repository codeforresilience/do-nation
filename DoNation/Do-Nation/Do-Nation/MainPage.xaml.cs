using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Do_Nation.Resources;
using Microsoft.WindowsAzure.MobileServices;


namespace Do_Nation
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static string userID = "";
        public static string userType = "";
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            tf_email.KeyUp += nextField;
            tf_password.KeyUp += hideKeyboard;
            //Item item = new Item { Text = "Mamattew2324211" };
            //Categories cat = new Categories();
            //cat.categoryID = 1;   
            //cat.categoryName = "haha";
            //App.MobileService.GetTable<Item>().InsertAsync(item);

           // App.MobileService.GetTable<Categories>().InsertAsync(cat);
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(NavigationService != null)
                while (NavigationService.CanGoBack)
                    NavigationService.RemoveBackEntry();
        }

        void hideKeyboard(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                this.Focus();
        }

        void nextField(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                tf_password.Focus();
                if (tf_password.Password.Equals("Password"))
                {
                    tf_password.Password = "";
                }
            }
        }

        private void tf_password_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
			/*
        	// TODO: Add event handler implementation here.
			if (String.IsNullOrEmpty(tf_password.Text))
			{
				//set default message in normal chars
				tf_password.Text = "Password...";
				tf_password.PasswordChar = Char.MinValue;
			}
			//if the new text is different from the default message
			//but the passwordchar is not yet on
			if (tf_password.PasswordChar.Equals(Char.MinValue) && tf_password.Text.Equals("Password...").Equals(false))
			{
				//take the newly added letter
				tf_password.Text = textBox1.Text[0].ToString();
				//set caret at the end of the textbox
				tf_password.SelectionStart = 1;
				//set the passwordchar
				tf_password.PasswordChar = '*';
			}
			*/
        }

        private async void login(object sender, System.Windows.RoutedEventArgs e)
        {
            int count = 0;
            btn_login.IsEnabled = false;
            errorText.Text = "Incorrect Username or Password";
            errorText.Visibility = System.Windows.Visibility.Collapsed;
            this.Focus();

            try
            {
                var results = App.MobileService.GetTable<Account>();
                List<Account> filter = await results.ToListAsync();
                foreach (Account a in filter)
                {
                    if (a.email.Equals(tf_email.Text) && a.password.Equals(tf_password.Password))
                    {
                        userID = a.id;

                        if (a.email.Equals("admin"))
                        {
                            userType = "ADMIN";
                            break;
                        }
                        
                        if(a.volunteerid.Equals(""))
                            userType = "USER";
                        else
                            userType = "VOLUNTEER";
                        System.Diagnostics.Debug.WriteLine("NEW USER " + userID);
                        count++;
                        break;
                    }   
                }
                if(userType == "ADMIN")
                    NavigationService.Navigate(new Uri("/AdminPage.xaml", UriKind.Relative));
                else 
                {
                    if (count != 0)
                        NavigationService.Navigate(new Uri("/PanoramaMenuPage.xaml", UriKind.Relative));
                    else
                    {
                        var results2 = App.MobileService.GetTable<OrgAccount>();
                        List<OrgAccount> filter2 = await results2.ToListAsync();
                        foreach (OrgAccount a in filter2)
                        {
                            if (a.email.Equals(tf_email.Text) && a.password.Equals(tf_password.Password) && a.isverified.Equals("VERIFIED"))
                            {
                                    userID = a.id;
                                    userType = "ORG";
                                    System.Diagnostics.Debug.WriteLine("NEW USER ORG " + userID);
                                    count++;
                                    break;
                            }
                            else if (a.email.Equals(tf_email.Text) && a.password.Equals(tf_password.Password) && a.isverified.Equals("REJECTED"))
                            {
                                errorText.Text = "Account has been rejected";
                                break;
                            }
                            else if (a.email.Equals(tf_email.Text) && a.password.Equals(tf_password.Password) && a.isverified.Equals("NEW"))
                            {
                                errorText.Text = "Account pending for verification";
                                break;
                            }
                        }

                        if (count != 0)
                            NavigationService.Navigate(new Uri("/PanoramaMenuPage.xaml", UriKind.Relative));
                        else
                        {
                            errorText.Visibility = Visibility.Visible;
                            btn_login.IsEnabled = true;
                        }
                    }
                }
                
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("NO");
            }
            
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBlock)sender).Visibility = Visibility.Collapsed;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text))
                {
                    ((TextBlock)sender).Visibility = Visibility.Visible;
                }
            }
        }

        private void tf_password_LostFocus(object sender, RoutedEventArgs e)
        {
            if(tf_password.Password.Equals(""))
            {
                tf_password.Password = "Password";
            }

        }

        private void tf_password_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(tf_password.Password.Equals("Password"))
            {
                tf_password.Password = "";
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}