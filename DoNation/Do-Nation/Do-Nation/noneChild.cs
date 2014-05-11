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
    class noneChild : StackPanel
    {
        /*	<StackPanel HorizontalAlignment="Left" VerticalAlignment="Top" Width="427" Background="#FF3D7391" Margin="10,0,0,20">
			    <TextBlock TextWrapping="Wrap" Text="none :(" Foreground="#FFA4D7F9" HorizontalAlignment="Center" VerticalAlignment="Center" Margin="0,10"/>
			    </StackPanel>
         * */
        public noneChild(string msg)
        {
            Name = "nonechild";
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Width = 427;
            Background = new SolidColorBrush(Color.FromArgb(255, 61, 115, 145));
            Margin = new Thickness(10, 0, 0, 20);
            Children.Add(new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Text = msg,
                Foreground = new SolidColorBrush(Color.FromArgb(255,164, 215, 249)),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0,10,0,10)
            });
        }
    }
}
