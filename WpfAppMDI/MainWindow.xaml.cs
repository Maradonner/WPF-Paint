using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Xceed.Wpf.AvalonDock.Layout;
using Path = System.Windows.Shapes.Path;

namespace WpfAppMDI
{
    internal enum Tools
    {
        Arrow,
        Pen,
        Line,
        Ellipse,
        Eraser,
        Star
    }

    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point currentPoint;
        private Polygon currentStar;
        private Tools currentTool = Tools.Arrow;
        private Ellipse ellipse;
        private double innerRadius = 25;
        private Line line;
        private double outerRadius = 50;
        private Path path;


        private int pointsNumber = 5;
        private SolidColorBrush selectedColor = new SolidColorBrush();
        private Point startPoint;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            selectedColor = new SolidColorBrush((Color)ClrPcker_Background.SelectedColor);
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Tools_Click(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "Pen":
                    currentTool = Tools.Pen;
                    break;
                case "Line":
                    currentTool = Tools.Line;
                    break;
                case "Ellipse":
                    currentTool = Tools.Ellipse;
                    break;
                case "Eraser":
                    currentTool = Tools.Eraser;
                    break;
                case "Star":
                    currentTool = Tools.Star;
                    break;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;


            if (currentTool == Tools.Pen)
            {
                startPoint = e.GetPosition(canvas);
                path = new Path();
                path.Stroke = selectedColor;
                path.StrokeThickness = ThicknessSlider.Value;
                canvas.Children.Add(path);
            }
            else if (currentTool == Tools.Ellipse)
            {
                startPoint = e.GetPosition(canvas);
                ellipse = new Ellipse();
                ellipse.Stroke = selectedColor;
                ellipse.StrokeThickness = ThicknessSlider.Value;
                canvas.Children.Add(ellipse);
            }
            else if (currentTool == Tools.Line)
            {
                startPoint = e.GetPosition(canvas);
                line = new Line();
                line.Stroke = selectedColor;
                line.StrokeThickness = ThicknessSlider.Value;
                canvas.Children.Add(line);
            }
            else if (currentTool == Tools.Eraser)
            {
                startPoint = e.GetPosition(canvas);
                path = new Path();
                path.Stroke = canvas.Background;
                path.StrokeThickness = ThicknessSlider.Value;
                canvas.Children.Add(path);
            }
            else if (currentTool == Tools.Star)
            {
                startPoint = e.GetPosition(canvas);
                var points = new Point[2 * pointsNumber];
                var angleIncrement = Math.PI / pointsNumber;
                var currentAngle = -Math.PI / 2;
                for (var i = 0; i < 2 * pointsNumber; i++)
                {
                    var radius = i % 2 == 0 ? outerRadius : innerRadius;
                    points[i] = new Point(
                        startPoint.X + radius * Math.Cos(currentAngle),
                        startPoint.Y + radius * Math.Sin(currentAngle)
                    );
                    currentAngle += angleIncrement;
                }

                currentStar = new Polygon
                {
                    Points = new PointCollection(points),
                    Stroke = Brushes.Black,
                    Fill = selectedColor
                };
                canvas.Children.Add(currentStar);
            }

            if (e.LeftButton == MouseButtonState.Pressed) startPoint = e.GetPosition(canvas);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;
            currentPoint = e.GetPosition(canvas);

            if (currentTool == Tools.Pen && e.LeftButton == MouseButtonState.Pressed)
            {
                if (startPoint == null || startPoint.X == 0 || startPoint.Y == 0)
                {
                    startPoint = currentPoint;
                    return;
                }

                var path = new Path();
                path.Data = new LineGeometry(startPoint, currentPoint);
                path.Stroke = selectedColor;
                path.StrokeThickness = ThicknessSlider.Value;
                path.StrokeStartLineCap = PenLineCap.Round;
                path.StrokeEndLineCap = PenLineCap.Round;
                canvas.Children.Add(path);
                startPoint = e.GetPosition(canvas);
            }
            else if (currentTool == Tools.Line && e.LeftButton == MouseButtonState.Pressed && line != null)
            {
                currentPoint = e.GetPosition(canvas);
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y;
                line.X2 = currentPoint.X;
                line.Y2 = currentPoint.Y;
            }
            else if (currentTool == Tools.Ellipse && e.LeftButton == MouseButtonState.Pressed)
            {
                var width = currentPoint.X - startPoint.X;
                var height = currentPoint.Y - startPoint.Y;
                if (width > 0 && height > 0 && ellipse != null)
                {
                    ellipse.Width = width;
                    ellipse.Height = height;
                    Canvas.SetLeft(ellipse, startPoint.X);
                    Canvas.SetTop(ellipse, startPoint.Y);
                }
            }
            else if (currentTool == Tools.Eraser && e.LeftButton == MouseButtonState.Pressed)
            {
                var path = new Path();
                path.Data = new LineGeometry(startPoint, currentPoint);
                path.Stroke = canvas.Background;
                path.StrokeThickness = ThicknessSlider.Value;
                path.StrokeStartLineCap = PenLineCap.Round;
                path.StrokeEndLineCap = PenLineCap.Round;
                canvas.Children.Add(path);
                startPoint = e.GetPosition(canvas);
            }
            else if (currentTool == Tools.Star && e.LeftButton == MouseButtonState.Pressed)
            {
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (currentTool == Tools.Pen)
            {
                var canvas = (Canvas)sender;

                startPoint = new Point();
            }
        }


        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BMP Files (*.bmp)|*.bmp|JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
            openFileDialog.FilterIndex = 1;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Title = "Open Image";

            if (openFileDialog.ShowDialog() == true)
            {
                var fileExtension = System.IO.Path.GetExtension(openFileDialog.FileName).ToUpper();

                if (fileExtension == ".BMP" || fileExtension == ".JPG" || fileExtension == ".PNG")
                {
                    var newDocument = new LayoutDocument();
                    newDocument.Closing += Window_Closing;

                    var page = new PageCanvas();
                    var canvas = (Canvas)page.FindName("myCanvasStretch");

                    using (var inStream =
                           new FileStream(new Uri(openFileDialog.FileName).LocalPath, FileMode.OpenOrCreate))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            inStream.CopyTo(memoryStream);
                            memoryStream.Position = 0;

                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();
                            canvas.Height = bitmapImage.Height;
                            canvas.Width = bitmapImage.Width;
                            canvas.Background = new ImageBrush(bitmapImage);
                            bitmapImage.Freeze();

                            page.Filepath = new Uri(openFileDialog.FileName).LocalPath;

                            var image = new Image();
                            image.Source = bitmapImage;
                            image.Stretch = Stretch.None;

                            canvas.Children.Add(image);
                            canvas.Width = bitmapImage.PixelWidth;
                            canvas.Height = bitmapImage.PixelHeight;
                            canvas.Background = Brushes.White;
                            canvas.MouseDown += Canvas_MouseDown;
                            canvas.MouseMove += Canvas_MouseMove;
                            canvas.MouseLeave += Canvas_MouseLeave;

                            newDocument.ContentId = openFileDialog.SafeFileName;
                            newDocument.Title = openFileDialog.SafeFileName;
                            newDocument.Content = page;
                        }
                    }

                    documentPane.Children.Add(newDocument);
                    newDocument.IsActive = true;
                }
                else
                {
                    MessageBox.Show("Неправильный формат файла. Please select a BMP, JPG or PNG file.");
                }
            }
        }

        private void AddNewDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            var newDocument = new LayoutDocument();
            newDocument.ContentId = "New Document";
            newDocument.Title = "New Document";
            newDocument.Closing += Window_Closing;

            var page = new PageCanvas();
            var canvas = (Canvas)page.FindName("myCanvasStretch");


            canvas.Background = Brushes.White;

            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeave += Canvas_MouseLeave;


            newDocument.Content = page;
            documentPane.Children.Add(newDocument);
            newDocument.IsActive = true;
        }

        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            var activeDocument = dockingManager.ActiveContent as LayoutDocument;
            var pageCanvas = activeDocument?.Content as PageCanvas;
            var imageCanvas = (Canvas)pageCanvas.FindName("myCanvasStretch");


            var resizeDialog = new ResizeDialog();
            resizeDialog.Height = (int)imageCanvas.Height;
            resizeDialog.Width = (int)imageCanvas.Width;


            if (resizeDialog.ShowDialog() == true)
                if (resizeDialog.Height > 0 && resizeDialog.Width > 0)
                {
                    imageCanvas.Height = resizeDialog.Height;
                    imageCanvas.Width = resizeDialog.Width;
                    imageCanvas.UpdateLayout();
                }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var activeDocument = dockingManager.ActiveContent as LayoutDocument;
            SaveAsImage_Click(activeDocument, new RoutedEventArgs());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var activeDocument = dockingManager.ActiveContent as LayoutDocument;
            SaveImage_Click(activeDocument, new RoutedEventArgs());
        }


        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            var activeDocument = sender as LayoutDocument;
            var pageCanvas = activeDocument?.Content as PageCanvas;
            var imageCanvas = (Canvas)pageCanvas.FindName("myCanvasStretch");


            var transform = imageCanvas.LayoutTransform;
            imageCanvas.LayoutTransform = null;

            var saveFileDialog = new SaveFileDialog();

            BitmapEncoder encoder = null;
            var fileExtension = System.IO.Path.GetExtension(pageCanvas.Filepath).ToUpper();


            if (fileExtension == ".BMP")
            {
                encoder = new BmpBitmapEncoder();
            }
            else if (fileExtension == ".JPG")
            {
                encoder = new JpegBitmapEncoder();
            }
            else if (fileExtension == ".PNG")
            {
                encoder = new PngBitmapEncoder();
            }
            else
            {
                MessageBox.Show("Неправильный формат файла. Please select a BMP, JPG or PNG file.");
                return;
            }

            var width = (double)imageCanvas.GetValue(WidthProperty);
            var height = (double)imageCanvas.GetValue(HeightProperty);

            var renderTargetBitmap = new RenderTargetBitmap((int)imageCanvas.Width, (int)imageCanvas.Height, 96, 96,
                PixelFormats.Pbgra32);
            renderTargetBitmap.Render(imageCanvas);

            using (var stream = new FileStream(pageCanvas.Filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                encoder.Save(stream);
            }

            imageCanvas.LayoutTransform = transform;
        }

        private void SaveAsImage_Click(object sender, RoutedEventArgs e)
        {
            var activeDocument = sender as LayoutDocument;
            var pageCanvas = activeDocument?.Content as PageCanvas;
            var imageCanvas = (Canvas)pageCanvas.FindName("myCanvasStretch");

            var transform = imageCanvas.LayoutTransform;
            imageCanvas.LayoutTransform = null;

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "BMP Files (*.bmp)|*.bmp|JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Title = "Save Image";

            if (saveFileDialog.ShowDialog() == true)
            {
                activeDocument.ContentId = saveFileDialog.SafeFileName;
                activeDocument.Title = saveFileDialog.SafeFileName;

                var uri = new Uri(saveFileDialog.FileName);
                pageCanvas.Filepath = uri.LocalPath;

                NewDocument_IsActiveChanged(sender, new RoutedEventArgs());

                BitmapEncoder encoder = null;
                var fileExtension = System.IO.Path.GetExtension(saveFileDialog.FileName).ToUpper();

                if (fileExtension == ".BMP")
                {
                    encoder = new BmpBitmapEncoder();
                }
                else if (fileExtension == ".JPG")
                {
                    encoder = new JpegBitmapEncoder();
                }
                else if (fileExtension == ".PNG")
                {
                    encoder = new PngBitmapEncoder();
                }
                else
                {
                    MessageBox.Show("Неправильный формат файла. Please select a BMP, JPG or PNG file.");
                    return;
                }

                var renderTargetBitmap = new RenderTargetBitmap((int)imageCanvas.Width, (int)imageCanvas.Height, 96, 96,
                    PixelFormats.Pbgra32);
                renderTargetBitmap.Render(imageCanvas);

                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    encoder.Save(stream);
                }
            }

            imageCanvas.LayoutTransform = transform;
        }


        private void NewDocument_IsActiveChanged(object sender, EventArgs e)
        {
            var activeDocument = dockingManager.ActiveContent as LayoutDocument;

            if (activeDocument == null)
            {
                SaveAs.IsEnabled = false;
                Save.IsEnabled = false;
                Resize.IsEnabled = false;
                return;
            }

            SaveAs.IsEnabled = true;
            Resize.IsEnabled = true;


            var pageCanvas = activeDocument?.Content as PageCanvas;

            if (string.IsNullOrWhiteSpace(pageCanvas.Filepath))
            {
                Save.IsEnabled = false;
                return;
            }

            Save.IsEnabled = true;
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены?", "Закрытие документа", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.No) e.Cancel = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы хотите сохранить изменения?", "Закрытие документа",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                SaveAsImage_Click(sender, new RoutedEventArgs());
            else if (result == MessageBoxResult.Cancel) e.Cancel = true;
        }

        private void StarSettings_Click(object sender, RoutedEventArgs e)
        {
            var starDialog = new StarDialog();
            starDialog.PointsNumber = pointsNumber;
            starDialog.OuterRadius = outerRadius;
            starDialog.InnerRadius = innerRadius;

            if (starDialog.ShowDialog() == true)
                if (starDialog.PointsNumber > 0 && starDialog.OuterRadius > 0 && starDialog.InnerRadius > 0)
                {
                    pointsNumber = starDialog.PointsNumber;
                    outerRadius = starDialog.OuterRadius;
                    innerRadius = starDialog.InnerRadius;
                }
        }
    }
}