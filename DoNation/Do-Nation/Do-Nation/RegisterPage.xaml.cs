using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;

namespace Do_Nation
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        string id = "";
        bool hasError;

        private readonly IMobileServiceTable<Account> accountTable = App.MobileService.GetTable<Account>();
        private readonly IMobileServiceTable<OrgAccount> orgAccountTable = App.MobileService.GetTable<OrgAccount>();

        public RegisterPage()
        {
            
            InitializeComponent();
            btn_reg.Click += btn_reg_Click;

            List<Object> textfields = new List<Object>();
            getTextFields(NormalUser, textfields);
            textfields.Add(btn_reg);
            activateTextFieldJumps(textfields);

            List<Object> textfields2 = new List<Object>();
            getTextFields(OrgHead, textfields2);
            textfields2.Add(btn_reg2);
            activateTextFieldJumps(textfields2);
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NormalUser.Visibility = Visibility.Collapsed;
            OrgHead.Visibility = Visibility.Collapsed;
            try
            {
                base.OnNavigatedTo(e);
                id = NavigationContext.QueryString["id"];
                switch (id)
                {
                    case "1":
                        NormalUser.Visibility = Visibility.Visible;
                        break;
                    case "3":
                        OrgHead.Visibility = Visibility.Visible;
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        async void btn_reg_Click(object sender, RoutedEventArgs e)
        {
            hasError = false;
            switch (id)
            {
                case "1":
                    if (tf_pass1.Password != tf_pass2.Password)
                    {
                        hasError = true;
                        errorMsg.Text = "Passwords do not match!";
                    }
                    else if (tf_email.Text.Trim() == "" || tf_pass1.Password == "" || tf_pass2.Password == "" || tf_name.Text.Trim() == "" || tf_address.Text.Trim() == "" || tf_contactno.Text.Trim() == "")
                    {
                        hasError = true;
                        errorMsg.Text = "Please fill up all missing fields!";
                    }
                    if (!hasError)
                    {
                        Account newAccount = new Account();
                        newAccount.email = tf_email.Text;
                        if (tf_pass1.Password.Equals(tf_pass2.Password))
                            newAccount.password = tf_pass1.Password;
                        newAccount.name = tf_name.Text;
                        newAccount.address = tf_address.Text;
                        newAccount.contactnumber = tf_contactno.Text;
                        newAccount.orgid = "";
                        newAccount.volunteerid = "";

                        try
                        {
                            await accountTable.InsertAsync(newAccount);
                            System.Diagnostics.Debug.WriteLine("Account Added");
                        }
                        catch (MobileServiceInvalidOperationException e2)
                        {
                        }
                        errorMsg.Text = "Account Created!";
                        btn_agree.Content = "Log in";
                    }
                    else
                    {
                    }
                    break;
                case "3":
                    if (tf_org_pass1.Password != tf_org_pass2.Password)
                    {
                        hasError = true;
                        errorMsg.Text = "Passwords do not match!";
                    }
                    else if (tf_orgname.Text.Trim() == "" || tf_org_email.Text.Trim() == "" || tf_org_pass1.Password == "" || tf_org_pass2.Password == "" || tf_org_contacthead.Text.Trim() == "" || tf_org_contactno.Text.Trim() == "")
                    {
                        hasError = true;
                        errorMsg.Text = "Please fill up all missing fields!";
                    }
                    if (!hasError)
                    {
                        OrgAccount orgAccount = new OrgAccount();
                        orgAccount.email = tf_org_email.Text;
                        if (tf_org_pass1.Password.Equals(tf_org_pass2.Password))
                            orgAccount.password = tf_org_pass1.Password;
                        orgAccount.name = tf_orgname.Text;
                        orgAccount.contacthead = tf_org_contacthead.Text;
                        orgAccount.contactnum = tf_org_contactno.Text;
                        orgAccount.description = "";
                        orgAccount.isverified = "VERIFIED";
                        orgAccount.message = "";
                        orgAccount.officelocation = "";
                        orgAccount.reliefcenter = "";
                        try
                        {
                            await orgAccountTable.InsertAsync(orgAccount);
                            System.Diagnostics.Debug.WriteLine("OrgAccount Added");
                        }
                        catch (MobileServiceInvalidOperationException e2)
                        {
                        }
                        errorMsg.Text = "Account Created!";
                        btn_agree.Content = "Log in";
                        blackeffect.Visibility = Visibility.Visible;
                        popup.Visibility = Visibility.Visible;
                    }
                    else
                    {
                    }
                    break;
            }
            blackeffect.Visibility = Visibility.Visible;
            popup.Visibility = Visibility.Visible;
        }


        private void getTextFields(Object panel, List<Object> tfs)
        {
            List<Object> anax = new List<object>();
            if (panel.GetType().Equals(typeof(StackPanel)))
            {
                StackPanel mother = panel as StackPanel;
                foreach (var child in mother.Children)
                {
                    anax.Add(child);
                }
            }
            else
                if (panel.GetType().Equals(typeof(Grid)))
                {
                    Grid mother = panel as Grid;
                    foreach (var child in mother.Children)
                    {
                        anax.Add(child);
                    }
                }
                else
                    if (panel.GetType().Equals(typeof(ScrollViewer)))
                    {
                        ScrollViewer mother = panel as ScrollViewer;
                        StackPanel father = mother.Content as StackPanel;
                        foreach (var child in father.Children)
                        {
                            anax.Add(child);
                        }
                    }

            System.Diagnostics.Debug.WriteLine("gettingTextBoxes from " + panel.GetType());
            foreach (var child in anax)
            {
                System.Diagnostics.Debug.WriteLine("child is a " + child.GetType());
                if (child.GetType().Equals(typeof(TextBox)))
                {
                    tfs.Add(child as TextBox);
                }
                else if (child.GetType().Equals(typeof(PasswordBox)))
                {
                    tfs.Add(child as PasswordBox);
                }
                else if (child.GetType().Equals(typeof(StackPanel)))
                {
                    getTextFields(child as StackPanel, tfs);
                }
                else if (child.GetType().Equals(typeof(Grid)))
                {
                    getTextFields(child as Grid, tfs);
                }
                else if (child.GetType().Equals(typeof(ScrollViewer)))
                {
                    getTextFields(child as ScrollViewer, tfs);
                }
            }
        }

        private void activateTextFieldJumps(List<Object> tfs)
        {
            for (int i = 0; i < tfs.Count-1; i++)
            {
                var temp = Content;
                if (tfs[i] as TextBox != null)
                {
                    temp = tfs[i] as TextBox;
                }
                else
                    temp = tfs[i] as PasswordBox;

                if (i + 1 < tfs.Count-1)
                {
                    temp.KeyUp += (s, e) =>
                    {
                        if (e.Key == System.Windows.Input.Key.Enter)
                        {
                            if (s as TextBox != null)
                            {
                                if (tfs[tfs.IndexOf(s as TextBox) + 1] as TextBox != null)
                                {
                                    TextBox temp2 = tfs[tfs.IndexOf(s as TextBox) + 1] as TextBox;
                                    temp2.Focus();
                                }
                                else
                                {
                                    PasswordBox temp2 = tfs[tfs.IndexOf(s as TextBox) + 1] as PasswordBox;
                                    temp2.Focus();
                                }
                            }
                            else
                            {
                                if (tfs[tfs.IndexOf(s as PasswordBox) + 1] as TextBox != null)
                                {
                                    TextBox temp2 = tfs[tfs.IndexOf(s as PasswordBox) + 1] as TextBox;
                                    temp2.Focus();
                                }
                                else
                                {
                                    PasswordBox temp2 = tfs[tfs.IndexOf(s as PasswordBox) + 1] as PasswordBox;
                                    temp2.Focus();
                                }
                            }
                        }
                    };
                }
                else
                {
                    temp.KeyUp += (s, e) =>
                    {
                        if (e.Key == System.Windows.Input.Key.Enter)
                        {
                            if (tfs.Last() as Button != null)
                            {
                                (tfs.Last() as Button).GotFocus += submitBtnFocus;
                                (tfs.Last() as Button).Focus();
                            }
                            else
                                this.Focus();
                        }
                    };
                }
            }
        }

        private void submitBtnFocus(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(btn_reg))
            {
                NormalUser.ScrollToVerticalOffset(NormalUser.Height);
            }
            else if (sender.Equals(btn_reg2))
            {
                OrgHead.ScrollToVerticalOffset(OrgHead.Height);
            }
        }

        private void btn_agree_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (btn_agree.Content.ToString() != "ok")
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            blackeffect.Visibility = System.Windows.Visibility.Collapsed;
            popup.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}