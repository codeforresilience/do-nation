using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;

namespace Do_Nation
{
    public partial class addDonationV2 : PhoneApplicationPage
    {
        private string measurement = "";
        List<Button> butts = new List<Button>();

        public addDonationV2()
        {
            InitializeComponent();
            Page2.Visibility = Visibility.Collapsed;
            qtypopup.Visibility = System.Windows.Visibility.Collapsed;
            blackeffect.Visibility = System.Windows.Visibility.Collapsed;
            popup.Visibility = System.Windows.Visibility.Collapsed;

            foreach(var child in itemButts.Children)
            {
                foreach (var butt in (child as StackPanel).Children)
                {
                    butts.Add((butt as Button));
                }
            }

            btn_addItem.Click += btn_addItem_Click;
            whatItem_Copy1.Completed += whatItem_Copy1_Completed;

            List<Object> textfields = new List<Object>();
            //getTextFields(Page2, textfields);
            textfields.Add(tf_time);
            textfields.Add(tf_special_instruction);

            textfields.Add(btn_post);
            activateTextFieldJumps(textfields);
        }

        void whatItem_Copy1_Completed(object sender, EventArgs e)
        {
            blackeffect.Visibility = System.Windows.Visibility.Collapsed;
        }

        void clickedGoods(object sender, RoutedEventArgs e)
        {
            Button butt = sender as Button;
            butt.IsEnabled = false;

            qtypopup_itemtb.Text = butt.Content + "";
            qtypopup_qty.Text = "";

            qtypopup_img.Source = new BitmapImage(new Uri(new GetReliefGoodImg()[butt.Content + ""], UriKind.Relative));

            if (sender.Equals(btn_CannedGood))
            {
                measurement =  "cans";
            }
            else if (sender.Equals(btn_ReadyToEat))
            {
                measurement = "pieces";
            }
            else if (sender.Equals(btn_Noodle))
            {
                measurement = "packs";
            }
            else if (sender.Equals(btn_Rice))
            {
                measurement = "kilos";
            }
            else if (sender.Equals(btn_water))
            {
                measurement = "liters";
            }
            else if (sender.Equals(btn_drinks))
            {
                measurement = "packs";
            }
            else if (sender.Equals(btn_medsup))
            {
                measurement = "pieces";
            }
            else if (sender.Equals(btn_clothes))
            {
                measurement = "pieces";
            }
            else if (sender.Equals(btn_Utensils))
            {
                measurement = "pieces";
            }
            else if (sender.Equals(btn_soaps))
            {
                measurement = "pieces";
            }
            else if (sender.Equals(btn_conssupplies))
            {
                measurement =  "pieces";
            }
            else if (sender.Equals(btn_vehicles))
            {
                measurement =  "pieces";
            }

            qtypopup_question.Text = "How many " + measurement + "?";
            blackeffect.Visibility = System.Windows.Visibility.Visible;
            qtypopup.Visibility = System.Windows.Visibility.Visible;
            qtypopupSb.Begin();
            qtypopup_qty.Focus();
        }

        void btn_addItem_Click(object sender, RoutedEventArgs e)
        {
            blackeffect_Copy.Visibility = Visibility.Visible;
            whatItem.Begin();

        }

        private void exitWhatItem(object sender, System.Windows.RoutedEventArgs e)
        {
            /*    					
    					<Grid x:Name="Item0" Margin="10,10,0,0" HorizontalAlignment="Left" Width="446" Background="#FF194A6C">
    						<Image HorizontalAlignment="Left" Height="106" Margin="5,5,0,5" VerticalAlignment="Top" Width="106" Source="/Assets/postorgpostIcon/warning.png"/>
    						<TextBlock HorizontalAlignment="Left" Margin="120,3,0,0" TextWrapping="Wrap" Text="Vehicles / Equipment" VerticalAlignment="Top" FontSize="28" Foreground="#FF81D7FF"/>
    						<TextBox HorizontalAlignment="Right" Margin="0,0,12,5" TextWrapping="Wrap" Text="100" VerticalAlignment="Bottom" VerticalScrollBarVisibility="Disabled" TextAlignment="Center" BorderThickness="0" FontSize="44" Width="140" Background="#FF81D7FF" Foreground="#FF0D334D" SelectionBackground="#FFEFFFC9" SelectionForeground="#FF0B5716" Style="{StaticResource qtyStyle}" Height="66"/>
    						<TextBlock HorizontalAlignment="Right" Margin="0,0,13,5" TextWrapping="Wrap" Text="pcs" VerticalAlignment="Bottom" Foreground="#FF0D334D"/>
    						<Button Content="&#xE0A4;" BorderThickness="0" FontFamily="Segoe UI Symbol" Style="{StaticResource removeBtn}" FontSize="12" HorizontalAlignment="Right" VerticalAlignment="Top" Foreground="#FF3D7391" Background="#FF112D40" Width="35" Height="36" Margin="0,0,5,0"/>
    					</Grid>
             */

            Button btn = new Button()
            {
                Content="",
                BorderThickness = new Thickness(0),
                FontFamily = new FontFamily("Segoe UI Symbol"),
                Style = Resources["removeBtn"] as Style,
                FontSize= 12,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Foreground = new SolidColorBrush(Color.FromArgb(255,61, 115, 145)),
                Background=new SolidColorBrush(Color.FromArgb(255,17, 45, 64)),
                Width=35,
                Height=36,
                Margin = new Thickness(0,0,5,0),
            };

            btn.Click +=removeItemOnBtnClick;

            TextBlock msrmnt = new TextBlock()
            {
                 HorizontalAlignment=HorizontalAlignment.Right,
                 Margin= new Thickness(0,0,13,5),
                 TextWrapping = System.Windows.TextWrapping.Wrap,
                 Text=measurement,
                 VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                 Foreground= new SolidColorBrush(Color.FromArgb(255,13, 51, 77))
            };

            InputScope numericScope = new InputScope();
            InputScopeName name = new InputScopeName();

            name.NameValue = InputScopeNameValue.Number;
            numericScope.Names.Add(name);


            TextBox qty = new TextBox()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Margin = new Thickness(0,0,12,5),
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Text= qtypopup_qty.Text,
                InputScope = numericScope,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled, 
                TextAlignment = System.Windows.TextAlignment.Center,
                BorderThickness = new Thickness(0),
                FontSize=44,
                Width=140,
                Background = new SolidColorBrush(Color.FromArgb(255, 129, 215, 255)),
                Foreground=new SolidColorBrush(Color.FromArgb(255,13, 51, 77)),
                SelectionBackground= new SolidColorBrush(Color.FromArgb(255,239, 255, 201)),
                SelectionForeground=new SolidColorBrush(Color.FromArgb(255,11, 87, 22)),
                Style=this.Resources["qtyStyle"] as Style,
                Height=66
            };

            TextBlock item = new TextBlock()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Margin = new Thickness(120,3,0,0),
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Text = qtypopup_itemtb.Text,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                FontSize=28,
                Foreground= new SolidColorBrush(Color.FromArgb(255,129, 215, 255))
            };

            
            Image img = new Image()
            {
                HorizontalAlignment= System.Windows.HorizontalAlignment.Left,
                Height= 106,
                Margin= new Thickness(5,5,0,5),
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Width=106,
                Source = qtypopup_img.Source
            };

            Grid itemGrid = new Grid()
            {
                Name = qtypopup_itemtb.Text,
                Margin = new Thickness(10,10,10,0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Width = 446,
                Background = new SolidColorBrush(Color.FromArgb(255,25, 74, 108))
            };

            itemGrid.Children.Add(img);
            itemGrid.Children.Add(item);
            itemGrid.Children.Add(qty);
            itemGrid.Children.Add(msrmnt);
            itemGrid.Children.Add(btn);

            itemList.Children.Add(itemGrid);

            qtypopupSbExit.Begin();
            blackeffect_Copy.Visibility = Visibility.Collapsed;
            whatItem_Copy1.Begin();
        }

        void removeItemOnBtnClick(object sender, RoutedEventArgs e)
        {
            Grid parent = (sender as Button).Parent as Grid;
            foreach (var butt in butts)
            {
                if (parent.Name.ToString() == butt.Content.ToString())
                {
                    butt.IsEnabled = true;
                    break;
                }
            }

            itemList.Children.Remove(parent);
        }

        private void qtypopup_qty_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                qtypopup_btn.Focus();
            }
        }

        private void page2(object sender, System.Windows.RoutedEventArgs e)
        {
            if (itemList.Children.Count == 0)
            {
                blackeffect.Visibility = System.Windows.Visibility.Visible;
                popuperr.Visibility = System.Windows.Visibility.Visible;
            }
            else
                Page2.Visibility = Visibility.Visible;
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
            (sender as Button).Focus();
        }

        private async void btn_agree_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //btn_agree.IsEnabled = false;
            rulestb.Visibility = System.Windows.Visibility.Collapsed;
            //gettingReadytb.Visibility = System.Windows.Visibility.Visible;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            this.Focus();

            var panels = itemList.Children;
            //List<String> itemCat = new List<String>();
            //List<int> itemQty = new List<int>();

            Post p = new Post();
            p.userid = MainPage.userID;
            p.status = "NEW";
            p.address = tf_address.Text;
            p.dateposted = DateTime.Now;
            p.specialinstruction = tf_special_instruction.Text;
            p.availabletime = tf_time.Text;
            p.volunteerid = "";

            await App.MobileService.GetTable<Post>().InsertAsync(p);

            var results = await App.MobileService.GetTable<Post>().ToListAsync();

            foreach (var panel in panels)
            {
                if (panel.GetType().Equals(typeof(Grid)))
                {
                    var ppanel = panel as Grid;
                    if (ppanel.Visibility == Visibility.Visible)
                    {
                        TextBlock btn = ppanel.Children.ElementAt(1) as TextBlock;
                        TextBox b = ppanel.Children.ElementAt(2) as TextBox;

                        if (btn != null)
                        {
                            Item i = new Item();
                            i.category = btn.Text;
                            i.quantity = int.Parse(b.Text);
                            i.postid = p.id;
                            await App.MobileService.GetTable<Item>().InsertAsync(i);
                            NavigationService.Navigate(new Uri("/PostSuccess.xaml", UriKind.Relative));
                        }
                    }
                }
            }
        }

        private void btn_post_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            blackeffect.Visibility = Visibility.Visible;
            popup.Visibility = Visibility.Visible;
        }

        private void popuperrclose(object sender, System.Windows.RoutedEventArgs e)
        {
            popuperr.Visibility = System.Windows.Visibility.Collapsed;
            blackeffect.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void loadAddress(object sender, System.Windows.RoutedEventArgs e)
        {
            tf_address.IsEnabled = false;
            var accounts = await App.MobileService.GetTable<Account>().ToListAsync();
            foreach (Account a in accounts)
            {
                if (a.id.Equals(MainPage.userID))
                {
                    tf_address.Text = a.address;
                    break;
                }
            }
        }

        private void clearAddressField(object sender, System.Windows.RoutedEventArgs e)
        {
            tf_address.Text = "";
            tf_address.IsEnabled = true;
        }

    }
}