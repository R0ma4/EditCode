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

namespace EditorCodeIDE.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        public MainWindow MainWindow;
        public PageMain()
        {
            InitializeComponent();
        }

        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            MainWindow MainWindowShow = new MainWindow();

            if (button != null)
            {
                try
                {
                    if (button.Name == "OpenFileBtn")
                    {
                        System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                        openFileDialog.DefaultExt = @"D:\EditorCodeIDE\TestProgect";
                        Pages.WindowEditCodeIDE windowEditCodeIDE = new Pages.WindowEditCodeIDE();

                        openFileDialog.ShowDialog();

                        if (openFileDialog.FileName != null)
                        {
                            string lan = "txt";

                            if (openFileDialog.FileName.Contains(".py"))
                            {
                                windowEditCodeIDE.Title = openFileDialog.FileName;
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.Python;
                                windowEditCodeIDE.InitializeEditor();
                                
                                windowEditCodeIDE.Show();
                            }
                            else if (openFileDialog.FileName.Contains(".cs"))
                            {
                                windowEditCodeIDE.Title = openFileDialog.FileName;
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.CSharp;
                                windowEditCodeIDE.InitializeEditor();

                                windowEditCodeIDE.Show();

                            }
                            else if (openFileDialog.FileName.Contains(".cpp"))
                            {
                                windowEditCodeIDE.Title = openFileDialog.FileName;
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                windowEditCodeIDE.CODElanguage = Pack_EditCode.CodeExecutionService.Language.Cpp;
                                windowEditCodeIDE.InitializeEditor();

                                windowEditCodeIDE.Show();

                            }
                            else if (openFileDialog.FileName.Contains(".js"))
                            {
                                windowEditCodeIDE.Title = openFileDialog.FileName;
                                windowEditCodeIDE.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
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
                              
                            }

                        }
                    }
                    else if (button.Name == "OpenEditorBtn")
                    {
                        Pages.WindowEditCodeIDE windowEditCodeIDE = new Pages.WindowEditCodeIDE();
                        windowEditCodeIDE.Title = "Новый файл пользователя *";



                        windowEditCodeIDE.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error<02>", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
