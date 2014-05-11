using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

namespace Do_Nation
{
    public partial class OrgPostForm : PhoneApplicationPage
    {
        private ObservableCollection<string> categories = new ObservableCollection<string>();
        private BitmapImage announcement = new BitmapImage(new Uri("/Assets/postorgpostIcon/annuncement.png", UriKind.Relative));
        private BitmapImage request = new BitmapImage(new Uri("/Assets/postorgpostIcon/request.png", UriKind.Relative));
        private BitmapImage update = new BitmapImage(new Uri("/Assets/postorgpostIcon/update.png", UriKind.Relative));
        private BitmapImage warning = new BitmapImage(new Uri("/Assets/postorgpostIcon/warning.png", UriKind.Relative));
        private BitmapImage announcement2 = new BitmapImage(new Uri("/Assets/postorgpostIcon/annuncement2.png", UriKind.Relative));
        private BitmapImage request2 = new BitmapImage(new Uri("/Assets/postorgpostIcon/request2.png", UriKind.Relative));
        private BitmapImage update2 = new BitmapImage(new Uri("/Assets/postorgpostIcon/update2.png", UriKind.Relative));
        private BitmapImage warning2 = new BitmapImage(new Uri("/Assets/postorgpostIcon/warning2.png", UriKind.Relative));

        public OrgPostForm()
        {
            InitializeComponent();
            //categories.Add("Announcement");
            //categories.Add("Update");
            //categories.Add("Warning");
            //categories.Add("Request");

            //tf_category.ItemsSource = categories;
            postPanel.Visibility = System.Windows.Visibility.Collapsed;
            btn_announce.Click += btn_announce_Click;
            btn_request.Click += btn_request_Click;
            btn_update.Click += btn_update_Click;
            btn_warning.Click += btn_warning_Click;

            List<TextBox> textfields = new List<TextBox>();
            getTextFields(LayoutRoot, textfields);
            activateTextFieldJumps(textfields);
            btn_PostAnother.Click += btn_PostAnother_Click;
        }

        void btn_PostAnother_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/OrgPostForm.xaml" + "?no-cache=" + Guid.NewGuid(), UriKind.Relative));
        }

        void btn_warning_Click(object sender, RoutedEventArgs e)
        {
            tb_Content.Text = "Warning";
            img_anc.Source = announcement2;
            img_upd.Source = update2;
            img_req.Source = request2;
            img_wrn.Source = warning;

            postPanel.Visibility = System.Windows.Visibility.Visible;
            Storyboard x = new Storyboard()
            {
            };
        }

        void btn_update_Click(object sender, RoutedEventArgs e)
        {
            tb_Content.Text = "Update";
            img_anc.Source = announcement2;
            img_upd.Source = update;
            img_req.Source = request2;
            img_wrn.Source = warning2;
            postPanel.Visibility = System.Windows.Visibility.Visible;
        }

        void btn_request_Click(object sender, RoutedEventArgs e)
        {
            tb_Content.Text = "Request";
            img_anc.Source = announcement2;
            img_upd.Source = update2;
            img_req.Source = request;
            img_wrn.Source = warning2;
            postPanel.Visibility = System.Windows.Visibility.Visible;
        }

        void btn_announce_Click(object sender, RoutedEventArgs e)
        {
            tb_Content.Text = "Announcement";
            img_anc.Source = announcement;
            img_upd.Source = update2;
            img_req.Source = request2;
            img_wrn.Source = warning2;
            postPanel.Visibility = System.Windows.Visibility.Visible;
        }

        private async void postContent(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tf_content.Text.Trim() == "" || tf_content.Text.Trim() == "Cannot be empty!")
            {
                popup2.Visibility = System.Windows.Visibility.Visible;
                blackeffect.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                blackeffect.Visibility = Visibility.Visible;
                progressBar.Visibility = System.Windows.Visibility.Visible;

                OrgPost post = new OrgPost();
                post.orgid = MainPage.userID;
                post.postcontent = tf_content.Text;
                post.category = tb_Content.Text;

                await App.MobileService.GetTable<OrgPost>().InsertAsync(post);

                progressBar.Visibility = System.Windows.Visibility.Collapsed;
                popup.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void getTextFields(Object panel, List<TextBox> tfs)
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

        private void activateTextFieldJumps(List<TextBox> tfs)
        {
            System.Diagnostics.Debug.WriteLine("tfs.Count():" + tfs.Count);
            for (int i = 0; i < tfs.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("tfs[" + i + "] : " + tfs.ElementAt(i).Name);
                if (i + 1 < tfs.Count)
                {
                    tfs.ElementAt(i).KeyUp += (s, e) =>
                    {
                        if (e.Key == System.Windows.Input.Key.Enter)
                        {
                            System.Diagnostics.Debug.WriteLine("going to : " + i);
                            tfs[tfs.IndexOf(s as TextBox) + 1].Focus();
                        }
                    };
                }
                else
                {
                    tfs.ElementAt(i).KeyUp += (s, e) =>
                    {
                        if (e.Key == System.Windows.Input.Key.Enter)
                            this.Focus();
                    };
                }
            }
        }

        private void btn_agree_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            blackeffect.Visibility = System.Windows.Visibility.Collapsed;
            popup2.Visibility = System.Windows.Visibility.Collapsed;
        }
    }

}