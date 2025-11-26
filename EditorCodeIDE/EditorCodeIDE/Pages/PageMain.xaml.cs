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

                        openFileDialog.ShowDialog();

                        if (openFileDialog.FileName != null)
                        {
                            string lan = "txt";

                            if (openFileDialog.FileName.Contains(".py"))
                            {
                                lan = "\'py\'";
                                Pages.PageEditor pageEditor = new Pages.PageEditor(lan);
                                pageEditor.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);

                                pageEditor.Filer.Header = openFileDialog;
                                pageEditor.PathFile = openFileDialog.FileName;

                                pageEditor.mainWindow = MainWindowShow;

                                
                                MainWindowShow.Title = openFileDialog.FileName;
                                MainWindowShow.MainControlPage.Content = pageEditor;

                                MainWindowShow.Show();
                            }
                            else if (openFileDialog.FileName.Contains(".cs"))
                            {
                                lan = "\'c#\'";
                                Pages.PageEditor pageEditor = new Pages.PageEditor(lan);
                                pageEditor.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                MainWindowShow.Title = openFileDialog.FileName;
                                pageEditor.Filer.Header = openFileDialog.Title;

                                MainWindowShow.MainControlPage.Content = pageEditor;

                                MainWindowShow.Show();
                            }
                            else if (openFileDialog.FileName.Contains(".cpp"))
                            {
                                lan = "\'c++\'";
                                Pages.PageEditor pageEditor = new Pages.PageEditor(lan);
                                pageEditor.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                MainWindowShow.Title = openFileDialog.FileName;
                                MainWindowShow.MainControlPage.Content = pageEditor;

                                MainWindowShow.Show();
                            }
                            else if (openFileDialog.FileName.Contains(".js"))
                            {
                                lan = "\'js\'";
                                Pages.PageEditor pageEditor = new Pages.PageEditor(lan);
                                pageEditor.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                MainWindowShow.Title = openFileDialog.FileName;
                                MainWindowShow.MainControlPage.Content = pageEditor;

                                MainWindowShow.Show();
                            }
                            else if (openFileDialog.FileName.Contains(".sql"))
                            {
                                lan = "\'sql\'";
                                Pages.PageEditor pageEditor = new Pages.PageEditor(lan);
                                pageEditor.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                MainWindowShow.Title = openFileDialog.FileName;
                                MainWindowShow.MainControlPage.Content = pageEditor;

                                MainWindowShow.Show();
                            }
                            else if (openFileDialog.FileName.Contains(".txt"))
                            {
                                lan = "\'txt\'";
                                Pages.PageEditor pageEditor = new Pages.PageEditor(lan);
                                pageEditor.TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                                MainWindowShow.Title = openFileDialog.FileName;
                                MainWindowShow.MainControlPage.Content = pageEditor;

                                MainWindowShow.Show();
                            }

                        }
                    }
                    else if (button.Name == "OpenEditorBtn")
                    {
                        Pages.PageEditor pageEditor = new Pages.PageEditor();
                        MainWindowShow.MainControlPage.Content = pageEditor;
                        MainWindowShow.Show();
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
