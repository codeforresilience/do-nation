using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Do_Nation
{
    class myPickUpListInfoPanel : StackPanel
    {
        public myPickUpListInfoPanel(string label, string val)
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            Margin = new Thickness(0, 6, 0, 0);

            TextBlock tbloc = new TextBlock()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Text = label,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Margin = new Thickness(10, 0, 0, -5),
                Width = 407,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 142, 241, 223)),
                FontSize = 18.667
            };

            TextBlock tblocVal = new TextBlock()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Text = val,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Margin = new Thickness(26, 0, 0, 10),
                Width = 391,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 142, 241, 223)),
                FontSize = 32
            };
            Children.Add(tbloc);
            Children.Add(tblocVal);
        }
    }
}
