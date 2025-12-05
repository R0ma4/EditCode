using System;
using System.Collections.Generic;
using System.IO;
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
using EditorCodeIDE.Pack_EditCode.RegisterSetteng;
using System.Diagnostics;

namespace EditorCodeIDE.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        public MainWindow _MainWindow;
        public PageMain()
        {
            InitializeComponent();
        }

        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            MainWindow MainWindowShow = new MainWindow();
            //_MainWindow = MainWindowShow;

            if (button != null)
            {
                try
                {
                    if (button.Name == "OpenFileBtn")
                    {
                        System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                        openFileDialog.DefaultExt = @"D:\EditorCodeIDE\TestProgect";
                        openFileDialog.ShowDialog();

                        if (openFileDialog.FileName != null)
                        {
                            string lan = "txt";
                            Pages.WindowEditCodeIDE windowEditCodeIDE = new Pages.WindowEditCodeIDE();

                            if (openFileDialog.FileName.Contains(".py"))
                            {
                                // Настройка окна
                                windowEditCodeIDE.Title = openFileDialog.FileName;

                                // Насткройка текстового полотна
                                windowEditCodeIDE.TextEditor.Uid = openFileDialog.FileName; // Зависим от пути к файлу
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName); // Текст Скрипта
                                windowEditCodeIDE.TextEditor.Tag = "pyhton"; // Язык Скрипта

                                // Настройка Компилятора
                                windowEditCodeIDE.MainDirect.Header = openFileDialog.InitialDirectory;
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.Python;
                                windowEditCodeIDE.InitializeEditor();

                                windowEditCodeIDE.Show();
                            }
                            else if (openFileDialog.FileName.Contains(".cs"))
                            {
                                // Настройка окна
                                windowEditCodeIDE.Title = openFileDialog.FileName;

                                // Насткройка текстового полотна
                                windowEditCodeIDE.TextEditor.Uid = openFileDialog.FileName; // Зависим от пути к файлу
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName); // Текст Скрипта
                                windowEditCodeIDE.TextEditor.Tag = "pyhton"; // Язык Скрипта

                                // Настройка Компилятора
                                windowEditCodeIDE.MainDirect.Header = openFileDialog.InitialDirectory;
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.CSharp;
                                windowEditCodeIDE.InitializeEditor();

                                windowEditCodeIDE.Show();

                            }
                            else if (openFileDialog.FileName.Contains(".cpp"))
                            {
                                windowEditCodeIDE.Title = openFileDialog.FileName;
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                windowEditCodeIDE.MainDirect.Header = openFileDialog.InitialDirectory;
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.Cpp;
                                windowEditCodeIDE.InitializeEditor();

                                windowEditCodeIDE.Show();

                            }
                            else if (openFileDialog.FileName.Contains(".js"))
                            {
                                windowEditCodeIDE.Title = openFileDialog.FileName;
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                windowEditCodeIDE.MainDirect.Header = openFileDialog.InitialDirectory;
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.JavaScript;
                                windowEditCodeIDE.InitializeEditor();

                                windowEditCodeIDE.Show();

                            }
                            else if (openFileDialog.FileName.Contains(".sql"))
                            {
                                lan = "\'sql\'";


                            }
                            else if (openFileDialog.FileName.Contains(".txt"))
                            {
                                lan = "\'txt\'";
                                windowEditCodeIDE.Title = openFileDialog.FileName;
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                windowEditCodeIDE.MainDirect.Header = openFileDialog.InitialDirectory;
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.Text;
                                windowEditCodeIDE.InitializeEditor();

                                windowEditCodeIDE.Show();

                            }
                            else 
                            {
                                MessageBox.Show("Открываеммый вами формат - не изсвестен нашей программе, его можно открыть в обычном .txt формате с правилами отлатки .txt формата","Не возможно зарегестрировать формат",MessageBoxButton.OKCancel,MessageBoxImage.Warning);
                            }
                        }
                        else { MessageBox.Show($"Отмена открытия", "Не известное имя оброботки", MessageBoxButton.OKCancel, MessageBoxImage.Stop); }
                    }
                    else if (button.Name == "OpenEditorBtn")
                    {
                        Pages.WindowEditCodeIDE windowEditCodeIDE = new Pages.WindowEditCodeIDE();
                        windowEditCodeIDE.Title = "Новый файл пользователя ";

                        windowEditCodeIDE.TextEditor.Text = "Новый файл!";
                        windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.Text;
                        windowEditCodeIDE.InitializeEditor();

                        windowEditCodeIDE.Show();
                    }
                    else if(button.Name == "NewFileBtn")
                    {
                        Pages.WindowCreateProject windowCreateProject = new Pages.WindowCreateProject();
                        windowCreateProject.Show();
                    }
                    else if(button.Name == "SettingsBtn")
                    {
                        Pages.SettengsWindow settengsWindow = new Pages.SettengsWindow();
                        settengsWindow.Show();
                    }
                    else { MessageBox.Show($"{button.Name} -> Не известное имя оброботки.", "Не известное имя оброботки", MessageBoxButton.OKCancel, MessageBoxImage.Stop); }                
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Программе не удалось обработать событие:\n" + ex.Message, "Error-[PE: '002']", MessageBoxButton.OK, MessageBoxImage.Error);
                }   
            }

            // _MainWindow.Close();
        }

        private void MouseKeys(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "GitHub": Process.Start(@"https://github.com/R0ma4/EditCode"); break;
            }

        }
    }
}
