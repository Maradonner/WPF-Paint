using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Themes;

namespace WpfAppMDI
{
    /// <summary>
    /// Логика взаимодействия для ResizeDialog.xaml
    /// </summary>
    public partial class ResizeDialog : Window
    {
        public ResizeDialog()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        public int Height
        {
            get { return Convert.ToInt32(height.Text); }
            set { height.Text = value.ToString(); }
        }
        public int Width
        {
            get { return Convert.ToInt32(width.Text); }
            set { width.Text = value.ToString(); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
