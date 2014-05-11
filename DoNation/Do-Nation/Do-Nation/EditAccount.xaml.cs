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
    public partial class EditAccount : PhoneApplicationPage
    {
        Account user = new Account();
        OrgAccount org = new OrgAccount();

        public EditAccount()
        {
            InitializeComponent();
            if (MainPage.userType.Equals("USER") || MainPage.userType.Equals("VOLUNTEER") || MainPage.userType.Equals("ADMIN"))
            {
                NormalUser.Visibility = Visibility.Visible;
                OrgHead.Visibility = Visibility.Collapsed;
                loadUserDetails();
            }
            else if(MainPage.userType.Equals("ORG"))
            {
                NormalUser.Visibility = Visibility.Collapsed;
                OrgHead.Visibility = Visibility.Visible;
                loadOrgDetails();
            }

            List<Object> textfields = new List<Object>();
            getTextFields(NormalUser, textfields);
            textfields.Add(btn_updateAccount);
            activateTextFieldJumps(textfields);

            List<Object> textfields2 = new List<Object>();
            getTextFields(OrgHead, textfields2);
            textfields2.Add(btn_org_updateAccount);
            activateTextFieldJumps(textfields2);
        }

        private async void loadOrgDetails()
        {
            org.id = MainPage.userID;
            var results = await App.MobileService.GetTable<OrgAccount>().ToListAsync();

            foreach (OrgAccount a in results)
            {
                if (a.id.Equals(org.id))
                {
                    org = a;
                    break;
                }
            }

            tf_org_contacthead.Text = org.contacthead;
            tf_org_contactno.Text = org.contactnum;
            tf_org_description.Text = org.description;
            tf_org_email.Text = org.email;
            tf_org_officelocation.Text = org.officelocation;
            tf_org_reliefCenter.Text = org.reliefcenter;
            tf_orgname.Text = org.name;
        }

        private async void loadUserDetails()
        {
            user.id = MainPage.userID;
           
            var results = await App.MobileService.GetTable<Account>().ToListAsync();

            foreach (Account a in results)
            {
                if (a.id.Equals(user.id))
                {
                    user = a;
                    break;
                }
            }
            string org = "-";

            var results2 = await App.MobileService.GetTable<OrgAccount>().ToListAsync();

            foreach (OrgAccount a in results2)
            {
                if (a.id.Equals(user.orgid))
                {
                    org = a.name;
                    break;
                }
            }

            tf_organization.Text = org;
            tf_address.Text = user.address;
            tf_contactno.Text = user.contactnumber;
            tf_email.Text = user.email;
            tf_name.Text = user.name;
           
            if (!user.volunteerid.Equals(""))
            {
                tf_orgKey.Text = user.volunteerid;
                tf_orgKey.IsEnabled = false;
            }
        }

        private async void updateAccount(object sender, System.Windows.RoutedEventArgs e)
        {
            bool hasError = false;
            errorMsg.Text = "";
            if (!tf_pass1.Password.Equals(tf_pass2.Password))
            {
                hasError = true;
                errorMsg.Text = "Passwords do not match!";
            }
            if (!hasError)
            {
                user.address = tf_address.Text;
                user.contactnumber = tf_contactno.Text;
                user.email = tf_email.Text;
                user.name = tf_name.Text;
                if (!user.password.Equals(tf_pass1.Password) && !tf_pass1.Password.Equals(""))
                    if (tf_pass1.Password.Equals(tf_pass2.Password))
                        user.password = tf_pass1.Password;
                if (!tf_orgKey.Text.Equals("") && tf_orgKey.IsEnabled == true)
                {
                    var results = await App.MobileService.GetTable<OrganizationKeyList>().ToListAsync();

                    foreach (OrganizationKeyList key in results)
                    {
                        if (key.id.Equals(tf_orgKey.Text))
                        {
                            if (key.volunteerid.Equals(""))
                            {
                                user.volunteerid = tf_orgKey.Text;
                                user.orgid = key.organizationid;
                                key.volunteerid = MainPage.userID;
                                await App.MobileService.GetTable<OrganizationKeyList>().UpdateAsync(key);
                                break;
                            }
                            else
                            {
                                //May may-ari na nung nakafillup na orgKey.

                            }
                        }
                    }
                }
                errorMsg.Text = "Account has been successfully edited.";
                await App.MobileService.GetTable<Account>().UpdateAsync(user);
            }
            blackeffect.Visibility = Visibility.Visible;
            popup.Visibility = Visibility.Visible;
        }

        private async void updateOrgAccount(object sender, System.Windows.RoutedEventArgs e)
        {
            bool hasError = false;
            errorMsg.Text = "";
            if (!tf_org_pass1.Password.Equals(tf_org_pass2.Password))
            {
                hasError = true;
                errorMsg.Text = "Passwords do not match!";
            }
            if (!hasError)
            {
                org.contacthead = tf_org_contacthead.Text;
                org.contactnum = tf_org_contactno.Text;
                org.description = tf_org_description.Text;
                org.email = tf_org_email.Text;
                org.officelocation = tf_org_officelocation.Text;
                org.reliefcenter = tf_org_reliefCenter.Text;
                org.name = tf_orgname.Text;
                if (!org.password.Equals(tf_org_pass1.Password) && !tf_org_pass1.Password.Equals(""))
                    if (tf_org_pass1.Password.Equals(tf_org_pass2.Password))
                        org.password = tf_org_pass1.Password;
                errorMsg.Text = "Account has been successfully edited.";
                await App.MobileService.GetTable<OrgAccount>().UpdateAsync(org);
            }
            blackeffect.Visibility = Visibility.Visible;
            popup.Visibility = Visibility.Visible;
        }

        private void btn_agree_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (btn_agree.Content.ToString() != "ok")
                NavigationService.Navigate(new Uri("/PanoramaMenuPage.xaml", UriKind.Relative));
            blackeffect.Visibility = System.Windows.Visibility.Collapsed;
            popup.Visibility = System.Windows.Visibility.Collapsed;
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
            for (int i = 0; i < tfs.Count - 1; i++)
            {
                var temp = Content;
                if (tfs[i] as TextBox != null)
                {
                    temp = tfs[i] as TextBox;
                }
                else
                    temp = tfs[i] as PasswordBox;

                if (i + 1 < tfs.Count - 1)
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
            if (sender.Equals(btn_updateAccount))
            {
                NormalUser.ScrollToVerticalOffset(NormalUser.Height);
            }
            else if (sender.Equals(btn_org_updateAccount))
            {
                OrgHead.ScrollToVerticalOffset(OrgHead.Height);
            }
        }

    }
}