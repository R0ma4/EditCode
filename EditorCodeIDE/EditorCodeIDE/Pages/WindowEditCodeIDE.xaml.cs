using EditorCodeIDE.Pack_EditCode;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EditorCodeIDE.Pages
{
    /// <summary>
    /// Логика взаимодействия для WindowEditCodeIDE.xaml
    /// </summary>
    /// 

    
    public partial class WindowEditCodeIDE : Window
    {
        #region Поля для системы ошибок

        private DispatcherTimer _errorCheckTimer;
        private string _lastCheckedCode = "";
        private string _currentError = null; // null - нет ошибок, "12 7" - строка 12, символ 7

        #endregion
        public WindowEditCodeIDE()
        {
            InitializeComponent();

            _errorCheckTimer = new DispatcherTimer();
            _errorCheckTimer.Interval = TimeSpan.FromMilliseconds(1000); // 1 секунда
            _errorCheckTimer.Tick += ErrorCheckTimer_Tick;
            _errorCheckTimer.Start();
        }

        #region Система обновления
        private void ErrorCheckTimer_Tick(object sender, EventArgs e)
        {

            #region обработка ошибок - временно она тут)
            // Проверяем, изменился ли код с последней проверки
            string currentCode = TextEditor.Text;
            if (currentCode == _lastCheckedCode)
                return;

            _lastCheckedCode = currentCode;

            // Запускаем проверку ошибок в фоновом режиме
            Dispatcher.BeginInvoke(new Action(() =>
            {
               // CheckForErrorsBackground();
            }), DispatcherPriority.Background);
            #endregion
        }

        #endregion
        private double _currentZoom = 1.0;
        public MainWindow mainWindow = new MainWindow();
        public Pack_EditCode.CodeExecutionService.Language CODElanguage;
        public string lan;
        public string PathFile;
        public string PathDir;
        string CeshPathFile;

        void SaveSytem()
        {
            if (CeshPathFile == TextEditor.Text) { mainWindow.Title = PathFile + " *"; }
            else { mainWindow = new MainWindow() { Title = PathFile }; }
        }
        /// <summary>
        /// Иницаилизация редактора, без него редактор забускаеться в базовых настройках
        /// </summary>
        public void InitializeEditor()
        {
            switch (CODElanguage)
            {
                case CodeExecutionService.Language.CSharp:
                    TextEditor.Text = StandaertCode.CodeSShart();
                    SetlanguageIDECode("\'c#\'");
                    break;

                case CodeExecutionService.Language.JavaScript: TextEditor.Text = @"console.log(""Hello, World!"")."; SetlanguageIDECode("\'js\'"); break;
                case CodeExecutionService.Language.Python: TextEditor.Text = @"print(""Hello, World!"")."; SetlanguageIDECode("\'py\'"); break;
                case CodeExecutionService.Language.Cpp: TextEditor.Text = "#include <iostream>  \r\nint main() {  \r\n    std::cout << \"Hello, World!\" << std::endl;  \r\n    return 0;  \r\n}  "; SetlanguageIDECode("c++"); break;
                default: TextEditor.Text = ""; break;
            }
            ProgecrRedact();
            // Подписка на события для отслеживания позиции каретки
            TextEditor.TextArea.Caret.PositionChanged += Caret_PositionChanged;
        }
        private void Caret_PositionChanged(object sender, System.EventArgs e)
        {
            // Обновление позиции в статус баре
            var caret = TextEditor.TextArea.Caret;
            CursorPosition.Text = ($"Ln {caret.Line}, Col {caret.Column}");
        }

        protected void ProgecrRedact()
        {

            switch (CODElanguage)
            {
                case CodeExecutionService.Language.CSharp:
                    Framework.Text = "Framework: .NET 6.0";
                    TypeProject.Text = "Тип: Стондарт <Console>";
                    language.Text = $"Язык: {CODElanguage.ToString()}";
                    break;
                case CodeExecutionService.Language.JavaScript:
                    Framework.Text = "Framework: WebJs";
                    TypeProject.Text = "Тип: WEB";
                    language.Text = $"Язык: {CODElanguage.ToString()}";
                    break;
                case CodeExecutionService.Language.Python:
                    Framework.Text = "Framework: Python";
                    TypeProject.Text = "Тип: Стондарт <Console>";
                    language.Text = $"Язык: {CODElanguage.ToString()}";
                    break;
                case CodeExecutionService.Language.Cpp:
                    Framework.Text = "Framework: NULL";
                    TypeProject.Text = "Тип: Стондарт <Console>";
                    language.Text = $"Язык: {CODElanguage.ToString()}";
                    break;
                default:
                    Framework.Text = "Framework: NULL";
                    TypeProject.Text = "Тип: Стондарт <NonPad>";
                    language.Text = $"Язык: Text";
                    break;
            }


        }
        // Масштабирование
        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                _currentZoom = e.NewValue / 100.0;

                // // Применяем масштаб к редактору
                TextEditor.FontSize = 14 * _currentZoom;
                // ZoomText.Text = $"{TextEditor.FontSize = 14 * _currentZoom}%";
            }
            catch
            {
                // ZoomText.Visibility = Visibility.Collapsed;
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Работа с Подсветкой и Языками. 
        private string CheckSetSyntaxHighlighting(string language)
        {
            string l = string.Empty;
            if (language != null)
            {
                switch (language.ToLower())
                {
                    case "cs":
                    case "csharp":
                        l = ("C#");
                        break;
                    case "js":
                    case "javascript":
                        l = ("JavaScript");
                        break;
                    case "html":
                        l = ("HTML");
                        break;
                    case "css":
                        l = ("CSS");
                        break;
                    case "xml":
                        l = ("XML");
                        break;
                    case "json":
                        l = ("JSON");
                        break;
                    case "sql":
                        l = ("SQL");
                        break;
                    case "python":
                    case "py":
                        l = ("Python");
                        break;
                    case "cpp":
                    case "c++":
                        l = ("C++");
                        break;
                    default:
                        l = null; // Без подсветки
                        break;
                }
            }
            return l;
        }
     
        private void SetSyntaxHighlighting(string language)
        {
            try
            {
                var highlightingManager = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance;

                if (language == null) { language = "XML"; }
                TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition(language);
                languageType.Text = $"Язык Проекта: {language}";
            }
            catch (ArgumentNullException argument)
            {
                MessageBox.Show($"Не вышло установить тип языка/проекта {language}. Попробуйте устоновить сами в настройках", "Ошибка при открытии.", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {

            }
        }
        private string SetlanguageIDECode(string language)
        {
            Console.Write("Язык Проекта: " + language);
            string ReluesComand = null;
            var highlightingManager = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance;

            if (language == "\'c#\'" || language == "\'csarp\'" || language == "\'cs\'" || language == "\'.cs\'")
            {
                TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("C#");
                languageType.Text = $"Язык Проекта: C#";
                CODElanguage = Pack_EditCode.CodeExecutionService.Language.CSharp;
            }
            else if (language == "\'.cpp\'" || language == "\'cpp\'" || language == "\'c++\'" || language == "\'cpulsplus\'")
            {
                TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("C++");
                languageType.Text = $"Язык Проекта: C++";
                CODElanguage = Pack_EditCode.CodeExecutionService.Language.Cpp;
            }
            else if (language == "\'py\'" || language == "\'pyhton\'")
            {
                TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("Python");
                languageType.Text = $"Язык Проекта: Python";
                CODElanguage = Pack_EditCode.CodeExecutionService.Language.Python;
            }
            else if (language == "\'js\'" || language == "\'javascript\'")
            {
                TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("JavaScript");
                languageType.Text = $"Язык Проекта: JavaScript";
                CODElanguage = Pack_EditCode.CodeExecutionService.Language.JavaScript;
            }

            else if (language == "\'sql\'" || language == "\'db\'") { TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("SQL"); languageType.Text = $"Язык Проекта: SQL"; }
            else if (language == "\'yml\'" || language == "\'yaml\'") { TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("YAML"); languageType.Text = $"Язык Проекта: YAML"; }
            else if (language == "\'yml\'" || language == "\'yaml\'") { TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("YAML"); languageType.Text = $"Язык Проекта: JavaScript"; }
            else if (language == "\'txt\'" || language == "\'text\'") { languageType.Text = $"Язык Проекта: Текстовый"; }
            else if (language == "\'xml\'" || language == "\'xmal\'") { TextEditor.SyntaxHighlighting = highlightingManager.GetDefinition("XML"); languageType.Text = $"Язык Проекта: XMAL"; }
            else { ReluesComand = language + $"не известный - {language}"; }
            ProgecrRedact();
            return ReluesComand;
        }
        private string SystemConsole(string comand)
        {
            try
            {
                string ReluesComand = null;
                comand = comand.ToLower();
                comand = comand.Trim();

                string[] LastList = comand.Split('\n');
                LastList[LastList.Length - 1] = LastList[LastList.Length - 1].Replace("> ", null);
                string[] StringComand = LastList[LastList.Length - 1].Split(' ');

                if (StringComand[0] == "set") // каманда на устоновление чего либо
                {
                    if (StringComand[1] == "language")
                    {
                        ReluesComand = SetlanguageIDECode(StringComand[2]);


                    }
                    else if (StringComand[1] == "create")
                    {
                        if (StringComand[2] == "-project") { }
                        else if (StringComand[2] == "-file")
                        {
                            string full_path = @"D:\EditorCodeIDE\TestProgect\" + StringComand[3];
                            File.Create(full_path);
                            ReluesComand = $"файл \'{full_path}\' - успешно создан!";
                        }
                        else { }

                    }
                    else if (StringComand[1] == "get") { }
                    else if (StringComand[1] == "run")
                    {
                        
                    }
                    else { ReluesComand = $"{StringComand[1]} - ни известная команда!"; }
                }
                else if (StringComand[0] == "open")
                {
                    if (StringComand[1] == "-project") { }
                    else if (StringComand[1] == "-file")
                    {
                        
                    }

                }
                else if (StringComand[0] == "get")
                {
                    if (StringComand[1] == "write")
                    {
                        if (StringComand[2] == "texteditor")
                        {

                            if (!File.Exists(StringComand[3]) && StringComand[3] == "-standaer_scrept")
                            {
                                switch (CODElanguage)
                                {
                                    case CodeExecutionService.Language.CSharp:
                                        TextEditor.Text =
                                            @"using System;
                                                 class Program
                                                 {
                                                     static void Main()
                                                     {
                                                         Console.WriteLine(""Hello, World!"");
                                                     }
                                                 }";
                                        break;

                                    case CodeExecutionService.Language.JavaScript: TextEditor.Text = "console.log(\"Hello, World!\")."; break;
                                    case CodeExecutionService.Language.Python: TextEditor.Text = "print(\"Hello, World!\")."; break;
                                    case CodeExecutionService.Language.Cpp: TextEditor.Text = "#include <iostream>  \r\nint main() {  \r\n    std::cout << \"Hello, World!\" << std::endl;  \r\n    return 0;  \r\n}  "; break;
                                    default: TextEditor.Text = ""; break;
                                }
                            }
                            else
                            {
                                string full_path = StringComand[3];
                                TextEditor.Text = File.ReadAllText(StringComand[3]);
                                ReluesComand = $"===== звершенно ====";
                            }
                        }

                    }
                }
                else if (StringComand[0] == "run") { }
                else if (StringComand[0] == "echo")
                {


                }
                else { }
                return ReluesComand + "\n> ";
            }
            catch (Exception ex) { return $"-< C# -> Exception:<{ex.Message} | {ex.InnerException} | {ex.Data}> \n\n> \""; }
        }
        #endregion
        private void EventKeyDowonDater(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.Key.ToString());
            // if (e.Key == Key.Enter) { ConsoleOutput.Text += $"\n{SystemConsole(ConsoleOutput.Text)}"; }

            //if(e.Key == Key.Back) { ConsoleOutput.Text = ConsoleOutput.Text; }
        }

        private async void RunScrept(object sender, RoutedEventArgs e)
        {
            try
            {
                string code = TextEditor.Text;

                // Создаем экземпляр сервиса прямо здесь
                var service = new Pack_EditCode.CodeExecutionService();
                var language = CODElanguage;

                string result = await service.ExecuteCodeAsync(code, language);
            //    ConsoleOutput.Text += $"\nРезультат:\n{result}\n> ";
            }
            catch (Exception ex)
            {
                // ConsoleOutput.Text += $"\nОшибка: {ex.Message}\n> ";
            }
        }



        #region Terminal_Control 



        #endregion

    }
}


class StandaertCode
{
    public static string CodeSShart()
    {
        return
        @"/*
	        Стандартное консольное приложение. 
	        Что выводит сообщение в консоль ""Heloll, World!"".
	        
	        Выполняеться по средтву высозва Windows Консоли.
	        Открываясь в нём.
	
        */



        using System; // используем, как основную библиотку System.dll
        
        namespace StandaetProgramm
        {
        	class Programm
        	{
        		static void Main()
        		{
        			Console.WriteLine(""Heloll, World!"");
        		}
        	}	
        }
        
        ";
    }
}
