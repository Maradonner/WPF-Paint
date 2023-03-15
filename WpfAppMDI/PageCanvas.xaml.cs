using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfAppMDI
{
    /// <summary>
    /// Логика взаимодействия для PageCanvas.xaml
    /// </summary>
    public partial class PageCanvas : UserControl
    {
        public string Filepath { get; set; } = String.Empty;
        public PageCanvas()
        {
            InitializeComponent();
        }


        //void onDragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    double yadjust = myCanvasStretch.Height + e.VerticalChange;
        //    double xadjust = myCanvasStretch.Width + e.HorizontalChange;
        //    if ((xadjust >= 0) && (yadjust >= 0))
        //    {
        //        myCanvasStretch.Width = xadjust;
        //        myCanvasStretch.Height = yadjust;
        //        myThumb.Margin = new Thickness(xadjust, yadjust, 0, 0);
        //    }
        //}

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            double minScale = 0.1;
            double maxScale = 10.0;
            double scale = zoomSlider.Value;

            if (scale >= minScale && scale <= maxScale)
            {
                if (e.Delta > 0)
                {
                    zoomSlider.Value += 0.1;

                    //ScaleTransform scaleTransform = new ScaleTransform(scale, scale);
                    //Contentik.LayoutTransform = scaleTransform;
                }

                else if (e.Delta < 0)
                {
                    zoomSlider.Value -= 0.1;

                    //ScaleTransform scaleTransform = new ScaleTransform(scale, scale);
                    //Contentik.LayoutTransform = scaleTransform;
                }
            }

        }

    }
}
