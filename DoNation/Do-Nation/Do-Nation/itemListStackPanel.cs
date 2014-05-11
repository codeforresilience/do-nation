using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Do_Nation
{
    class itemListStackPanel : StackPanel
    {
        public itemListStackPanel(List<Item> items)
        {
            //<StackPanel Orientation="Horizontal" Margin="0,0,29,0">
            Orientation = System.Windows.Controls.Orientation.Horizontal;
            if (items.Count == 12)
                Margin = new Thickness(0, 0, 0, 0);
            else
            Margin = new Thickness(0, 0, 0, -15);

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

            Children.Add(col1);
            Children.Add(col2);

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
        }
    }
}
