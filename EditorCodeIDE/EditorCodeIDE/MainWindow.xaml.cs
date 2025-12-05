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
            
            pageMain._MainWindow = this;
            MainControlPage.Content = new Pages.PageMain();

        }

       
        
    }
}
