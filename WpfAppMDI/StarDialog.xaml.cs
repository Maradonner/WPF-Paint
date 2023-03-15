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
using System.Windows.Shapes;

namespace WpfAppMDI
{
    /// <summary>
    /// Логика взаимодействия для StarDialog.xaml
    /// </summary>
    public partial class StarDialog : Window
    {
        public StarDialog()
        {
            InitializeComponent();
        }

        public int PointsNumber
        {
            get { return Convert.ToInt32(pointsNumber.Text); }
            set { pointsNumber.Text = value.ToString(); }
        }
        public double OuterRadius
        {
            get { return Convert.ToDouble(outerRadius.Text); }
            set { outerRadius.Text = value.ToString(); }
        }
        public double InnerRadius
        {
            get { return Convert.ToDouble(innerRadius.Text); }
            set { innerRadius.Text = value.ToString(); }
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
