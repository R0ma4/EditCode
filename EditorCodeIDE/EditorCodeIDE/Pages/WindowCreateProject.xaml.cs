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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EditorCodeIDE.Pages
{
    /// <summary>
    /// Логика взаимодействия для WindowCreateProject.xaml
    /// </summary>
    public partial class WindowCreateProject : Window
    {

        protected string type_projectName;

        /// <summary> Типизация Проектов </summary>
        public enum TypeProject 
        {
            // Консольные приложения 
            /// <summary> Консольное приложение C# </summary>
               Classikc_CSharp_Console = 0,
            /// <summary> Консольное приложение C++ </summary>
                Classikc_CPP_Console = 1,
            /// <summary> Консольное приложение Pyhton </summary>
                Classikc_Pyhton_Console = 2,

            //  Боты и ИИ
            /// <summary> Консольный котролер TelegramBot - Pyhton</summary>
                Telegram_Bot_Pyhton = 3,
            /// <summary> Консольный котролер VoisBot - Pyhton - </summary>
                Vois_Bot_Pyhton = 4,

            // 
            // 
            // 
            // 
            // 
        }


        public WindowCreateProject()
        {
            InitializeComponent();

            CreateConsoleAppTemplateCard(TypeProject.Classikc_CSharp_Console,"C#");
            CreateConsoleAppTemplateCard(TypeProject.Classikc_CPP_Console,"C#");
        }

        
        public ListBoxItem CreateConsoleAppTemplateCard(TypeProject type_Project, string lengech)
        {
           
            ListBoxItem CSharplistBoxItem = new ListBoxItem();

            // Настройка блока проекта 
            switch (type_Project) 
            {
                case TypeProject.Classikc_CSharp_Console:  CSharplistBoxItem.Content = ConsoleSharp(); break;

            }
           return CSharplistBoxItem;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Чтение и настройка C# Консольного проекта
        /// </summary>
        /// <returns></returns>
        protected Border ConsoleSharp()
        {
            // Создаем основной Border
            Border mainBorder = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFrom("#FF252424"),
                Padding = new Thickness(12),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF3E3E40"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Margin = new Thickness(0, 0, 0, 8)
            };

            // Создаем StackPanel как содержимое Border
            StackPanel CSharpstackPanel = new StackPanel();

            // Заголовок
            TextBlock CSharptitleTextBlock = new TextBlock
            {
                Text = "Консольное C# приложение",
                Foreground = Brushes.White,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold
            };

            // Описание
            TextBlock CSharpdescriptionTextBlock = new TextBlock
            {
                Text = "Стандартное - консольное C# приложение",
                Foreground = (Brush)new BrushConverter().ConvertFrom("#A0A0A0"),
                FontSize = 12,
                Margin = new Thickness(0, 4, 0, 4)
            };

            // WrapPanel для тегов
            WrapPanel CSharpwrapPanel = new WrapPanel
            {
                Margin = new Thickness(0, 8, 0, 0)
            };

            // Фон (временно удалён)
            Border csharpTagBorder = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFrom("#2A5B8C"),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(4, 2, 0, 0),
                Margin = new Thickness(0, 0, 4, 0)
            };

            // Теги
            TextBlock csharpTextBlock = new TextBlock
            {
                Text = "[C#]",
                Foreground = Brushes.White,
                FontSize = 10
            };

            TextBlock dotnetTextBlock = new TextBlock
            {
                Text = "\t[.NET 6+]",
                Foreground = (Brush)new BrushConverter().ConvertFrom("#A0A0A0"),
                FontSize = 10
            };

            // dotnetTagBorder.Child = dotnetTextBlock;

            CSharpwrapPanel.Children.Add(dotnetTextBlock);

            // Добавляем элементы в StackPanel
            CSharpstackPanel.Children.Add(CSharptitleTextBlock);
            CSharpstackPanel.Children.Add(CSharpdescriptionTextBlock);
            CSharpstackPanel.Children.Add(CSharpwrapPanel);

            // Устанавливаем StackPanel как содержимое Border
            mainBorder.Child = CSharpstackPanel;

            // Добавляем эффекты для интерактивности
            mainBorder.MouseEnter += (sender, e) =>
            {
                mainBorder.Background = (Brush)new BrushConverter().ConvertFrom("#FF2D2D2D");
                mainBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF007ACC");
                mainBorder.Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    ShadowDepth = 2,
                    Opacity = 0.3,
                    BlurRadius = 4
                };
            };

            mainBorder.MouseLeave += (sender, e) =>
            {
                mainBorder.Background = (Brush)new BrushConverter().ConvertFrom("#FF252424");
                mainBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF3E3E40");
                mainBorder.Effect = null;
            };

            // Добавляем обработчик клика
            mainBorder.MouseLeftButtonDown += (sender, e) =>
            {
                // Можно добавить логику выбора шаблона
                Console.WriteLine("Выбран шаблон: Консольное приложение C#");
            };

            // Включаем возможность фокуса
            mainBorder.Focusable = true;

            return mainBorder;
        }
    }
}
