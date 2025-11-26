using ICSharpCode.AvalonEdit;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EditorCodeIDE
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double _currentZoom = 1.0;
        private const double BaseFontSize = 14;
        public MainWindow()
        {
            Pages.PageMain pageMain = new Pages.PageMain();
            
            InitializeComponent();
            
            pageMain.MainWindow = this;
            MainControlPage.Content = new Pages.PageMain();

        }

        // Масштабирование
        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //_currentZoom = e.NewValue / 100.0;
            //ZoomText.Text = $"{e.NewValue}%";
            //
            //// Применяем масштаб к редактору
            //TextEditor.FontSize = BaseFontSize * _currentZoom;
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
          //  ZoomSlider.Value = System.Math.Min(ZoomSlider.Maximum, ZoomSlider.Value + 10);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
          //  ZoomSlider.Value = System.Math.Max(ZoomSlider.Minimum, ZoomSlider.Value - 10);
        }

        private void ZoomReset_Click(object sender, RoutedEventArgs e)
        {
          //  ZoomSlider.Value = 100;
        }


        
    }
}
