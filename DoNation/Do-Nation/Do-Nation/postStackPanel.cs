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
    class postStackPanel : StackPanel
    {
        public postStackPanel(List<Item> items)
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Width = 427;
            Background = new SolidColorBrush(Color.FromArgb(255, 61, 111, 145));
            Margin = new Thickness(10, 0, 0, 20);
            Children.Add(new itemListStackPanel(items));
        }
    }
}
