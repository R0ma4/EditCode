using EditorCodeIDE.Pack_EditCode;
using EditorCodeIDE.Pack_EditCode.languageRedactors;
using EditorCodeIDE.Pack_EditCode.languageRedactors.CSarp;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.Win32;
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
using System.Windows.Threading;

namespace EditorCodeIDE.Pages
{

    public class Market
    {
        private DispatcherTimer _errorCheckTimer;
        public string TextLoader;
        int Index = 0, Count = 0;
        public Market()
        {
            _errorCheckTimer = new DispatcherTimer();
            _errorCheckTimer.Interval = TimeSpan.FromMilliseconds(3000);
            _errorCheckTimer.Tick += UpdateEditor;
            _errorCheckTimer.Start();
        }

        public void UpdareTextInfi(TreeViewItem treeViewItem)
        {
            if(Index == 0) { treeViewItem.Header = "Загрузка данных"; Index = 1; }
            if(Index == 1) { treeViewItem.Header = "Загрузка данных."; Index = 2; }
            if(Index == 2) { treeViewItem.Header = "Загрузка данных.."; Index = 3; }
            if(Index == 3) { treeViewItem.Header = "Загрузка данных..."; Index = 4; }
            if(Index == 4) { treeViewItem.Header = "Загрузка данных...."; Index = 0; }
            
        }
        private static void UpdateEditor(object sender, EventArgs e) { }


    }

    public static class TreeFile
    {
        

        static TreeViewItem TreeFileDirect()
        {

            return new TreeViewItem();
        }
    }

    /// <summary>
    /// Логика взаимодействия для WindowEditCodeIDE.xaml
    /// </summary>
    /// 

    public partial class WindowEditCodeIDE : Window
    {
        #region Поля для системы ошибок

        private DispatcherTimer _errorCheckTimer;
        private string _lastCheckedCode = "";
        private string _currentError = null; // null - нет ошибок, "12 7" - строка 12, символ 7, пока только линия на которой ошибка

        #endregion

        #region Стартовные настройки для компелятора
        public MainWindow mainWindow = new MainWindow();
        static int SecendNamber = 2;

        string ImgPathDir = @"D:\EditCode\EditorCodeIDE\EditorCodeIDE\Pack-EditCode\img";
        private bool StartScrept = false;
        private double _currentZoom = 0.20;
        public Pack_EditCode.CodeExecutionService.Language CODElanguage;
        public string lan;
        public string PathFile;
        public string PathDir;
        string CeshPathFile;
        public string FileFinalEdit_Data;
        #endregion

        Market market = new Market();


        public WindowEditCodeIDE()
        {
            InitializeComponent();
            InitializeEditor();
           
        }

        ~WindowEditCodeIDE()
        {
            MessageBox.Show("");
            _errorCheckTimer.Stop();
        }

        #region Иницилизация Редактора, настройка его повидения, проектирование, открисовка.
        public void InitializeEditor()
        {

            TextRender("Начало компиляции");
            TextEditor.FontSize = _currentZoom;
            ZoomSlider.Value = _currentZoom;
            ZoomText.Text = $"{_currentZoom}%";

            _errorCheckTimer = new DispatcherTimer();
            _errorCheckTimer.Interval = TimeSpan.FromMilliseconds(SecendNamber * 1000);
            _errorCheckTimer.Tick += UpdateEditor;
            _errorCheckTimer.Start();

            switch (CODElanguage)
            {
                case CodeExecutionService.Language.CSharp:
                    TextEditor.Text = StandaertCode.CodeSShart();
                    SetlanguageIDECode("\'c#\'");
                    break;

                case CodeExecutionService.Language.JavaScript: TextEditor.Text = @"console.log(""Hello, World!"")."; SetlanguageIDECode("\'js\'"); break;
                case CodeExecutionService.Language.Python: TextEditor.Text = @"print(""Hello, World!"")."; SetlanguageIDECode("\'py\'"); break;
                case CodeExecutionService.Language.Cpp: TextEditor.Text = "#include <iostream>  \r\nint main() {  \r\n    std::cout << \"Hello, World!\" << std::endl;  \r\n    return 0;  \r\n}  "; SetlanguageIDECode("c++"); break;
                case CodeExecutionService.Language.XMAL: SetlanguageIDECode("\'xmal\'"); break;
                default: TextEditor.Text = ""; break;
            }

           // if (CODElanguage != CodeExecutionService.Language.CSharp || CODElanguage != CodeExecutionService.Language.Text || CODElanguage != CodeExecutionService.Language.SQL)
           // {
           //     MessageBox.Show("К сожилению, компеляторы к данному языку, сейчас не вктивны", "Не вышло подключиться к компилятору", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
           //     this.Close();
           //     return;
           // }

            ProgecrRedact();
            // Подписка на события для отслеживания позиции каретки
            TextEditor.TextArea.Caret.PositionChanged += Caret_PositionChanged;
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

        protected void ProgecrRedact()
        {
            // Такие элементы как 
            // CointLineCode
            // FileCoint
            // FilnalRedacrt 
            // Работают и обновляються в 
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
                 case CodeExecutionService.Language.Text:
                    Framework.Text = "Framework: [NULL]";
                    TypeProject.Text = "Тип: .txt";
                    language.Text = $"Обычный Текст";

                    break;
                case CodeExecutionService.Language.XMAL:
                    Framework.Text = "Framework: [NULL]";
                    TypeProject.Text = "Тип: .txt";
                    language.Text = $"Обычный Текст";

                    break;
                default:
                    Framework.Text = "Framework: NULL";
                    TypeProject.Text = "Тип: Стондарт <NonPad>";
                    language.Text = $"Язык: Text";
                    FileCoint.Text = "1";
                    break;
            }
            TextRender("Готово");
        }

        #endregion


        #region Система обновления
        DrawingContext drawingContext;
        CodeExecutionService codeExecutionService = new CodeExecutionService(); // Оброботчик помпелятора
        languageCShaerp languageCShaerp = new languageCShaerp(); // Компилятор C#
        int erroline = -112;
        string TitleNameProject;
        

        private void UpdateEditor(object sender, EventArgs e)
        {
            market.UpdareTextInfi(UpdaetInfoLoad);
            
            for(int i = 0; i < TextEditor.LineCount; i++) 
            {
                LineBackgroundRenderer.HighlightLine(TextEditor, i, Brushes.Cyan);
            }

           // TextRender("Начало обновления компиляции");
            if (TitleNameProject == null) { TitleNameProject = $"{MainWindowEdit.Title}"; }

            if (_lastCheckedCode != TextEditor.Text) { MainWindowEdit.Title = TitleNameProject + " *"; }

            
            UpdateInformatinProjecrt();
            Console.WriteLine($"Строка ошибки '{erroline}' ");
            ZoomText.Text = $"{Math.Round(_currentZoom)}%";

            #region обработка ошибок - временно она тут)
            Dispatcher.BeginInvoke(new Action(() => {

                codeExecutionService.CompilatorErrorInfo(TextEditor.Text, CODElanguage);
                erroline = codeExecutionService.LineError;
            }),
            DispatcherPriority.Background);


            if (erroline == -1)
            {
                MessageBox.Show("Произошла ошибка при обработке кода: \nКод ошибки POPS: 00-2", "POPS: 00-2");
            }
            else if (erroline > 0)
            {
                ErrorList.Items.Clear();
                // Добавляем информацию об ошибке
                ErrorList.Items.Add($"Ошибка: [{codeExecutionService.ErrorText}] Строка ^{erroline}^ ");
                LineBackgroundRenderer.HighlightLine(TextEditor, erroline, Brushes.Red);
                Console.WriteLine("Ошибка в коде");
            }
            else { ErrorList.Items.Clear();}
            Console.WriteLine($"{erroline} | {CODElanguage} | {TextEditor}");
            #endregion
            // TextRender("Готово");
        }

        #endregion
        

        private void ClickButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null) 
            {
                switch (button.Name) 
                {
                    case "ZoomInBtn": TextEditor.FontSize += 1; ZoomSlider.Value += 1; break;
                    case "ZoomOutBtn": if (TextEditor.FontSize > 1) { TextEditor.FontSize -= 1; ZoomSlider.Value -= 1; } break;
                    case "ZoomResetBtn": TextEditor.FontSize = 20; ZoomSlider.Value = 20; break;
                    case "Pakes": TextEditor.FontSize = 20; ZoomSlider.Value = 20; break;
                    case "RunAndDebug": EventRunScrept(); break;
                    case "Seach": TextEditor.FontSize = 20; ZoomSlider.Value = 20; break;
                    case "FileProject": TextEditor.FontSize = 20; ZoomSlider.Value = 20; break;
                    default: MessageBox.Show("Имя объекта не сущесвует в списке или не верно.", "Не вышло обработать название",MessageBoxButton.OK,MessageBoxImage.Hand); break;
                }
            }
        }

        #region Модели / Оброботка событий

        protected void SaveScrept()
        {
            try
            {
                TextRender("Сохранение...");

                // Получаем текущее имя файла из заголовка окна
                if (TextEditor.Uid == null || !File.Exists(TextEditor.Uid))
                {
                    // Пытаемся извлечь имя файла из заголовка окна
                    string title = MainWindowEdit.Title.Trim();
                    if (title.Contains(" - Ваш редактор"))
                    {
                        string fileName = title.Replace(" - Ваш редактор", "").Replace("*", "").Trim();
                        if (!string.IsNullOrEmpty(fileName) && (File.Exists(fileName) || Path.IsPathRooted(fileName)))
                        {
                            TextEditor.Uid = fileName;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(TextEditor.Uid) && File.Exists(TextEditor.Uid))
                {
                    // Сохраняем в существующий файл
                    _lastCheckedCode = TextEditor.Text;
                    File.WriteAllText(TextEditor.Uid, _lastCheckedCode);
                    MainWindowEdit.Title = $"{Path.GetFileName(TextEditor.Uid)} - Ваш редактор";
                    TextRender("Успешно сохранено");
                }
                else
                {
                    // Если файл не существует, открываем диалог сохранения
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Все файлы (*.*)|*.*|C# файлы (*.cs)|*.cs|Python файлы (*.py)|*.py|JavaScript файлы (*.js)|*.js|C++ файлы (*.cpp;*.h)|*.cpp;*.h|HTML файлы (*.html;*.htm)|*.html;*.htm|Проект EditCodeIDE (*.eciproj)|*.eciproj|Текстовые файлы (*.txt)|*.txt",
                        Title = "Сохранить файл",
                        FilterIndex = 8
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        TextEditor.Uid = saveFileDialog.FileName;

                        try
                        {
                            // Сохраняем текст в выбранный файл
                            File.WriteAllText(TextEditor.Uid, TextEditor.Text);

                            // Обновляем заголовок окна
                            MainWindowEdit.Title = $"{TextEditor.Uid}";
                            TextRender($"Файл сохранен: {Path.GetFileName(TextEditor.Uid)}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Не удалось сохранить файл: {ex.Message}", "Ошибка сохранения",
                                          MessageBoxButton.OK, MessageBoxImage.Error);
                            TextRender($"Ошибка: {ex.Message}");
                        }
                    }
                    else
                    {
                        TextRender("Сохранение отменено");
                    }
                }
            }
            catch (Exception ex)
            {
                TextRender($"Ошибка сохранения: {ex.Message}");
                MessageBox.Show($"Произошла ошибка при сохранении: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void TextRender(string textCompilait) 
        {
            if (TextEditor != null) { TextRedender.Text = textCompilait; }
        }
        protected async void EventRunScrept()
        {
            string ComandText = string.Empty;
            try
            {
                StartScrept = true;
                string imagPaly = ImgPathDir+@"\play-imag004.png";
                string imagStop = ImgPathDir+ @"\stop-imag005.png";

                TextRender("Начало работы скрипта");
                RunAndDebug.Background = new ImageBrush(new BitmapImage(new Uri(imagStop, UriKind.RelativeOrAbsolute)));
                string code = TextEditor.Text;
                string s = string.Empty;
                // Создаем экземпляр сервиса прямо здесь
                var service = new Pack_EditCode.CodeExecutionService();
                var language = CODElanguage;

                ComandText = await service.ExecuteCodeAsync(code, language);
                RunAndDebug.Background = new ImageBrush(new BitmapImage(new Uri(imagPaly, UriKind.RelativeOrAbsolute)));
                StartScrept = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вышло, запустить обробо, ошибка обработки: {ComandText}|{ex.Message}", "Ошибка - 1", MessageBoxButton.OKCancel, MessageBoxImage.Stop);
            }
        }
        #endregion
        /// <summary>
        /// Иницаилизация редактора, без него редактор забускаеться в базовых настройках
        /// </summary>
       
        private void Caret_PositionChanged(object sender, System.EventArgs e)
        {
            // Обновление позиции в статус баре
            var caret = TextEditor.TextArea.Caret;
            CursorPosition.Text = ($"Ln {caret.Line}, Col {caret.Column}");
        }
        protected void UpdateInformatinProjecrt() 
        {
            try
            {
                DateTime dateTime = File.GetLastAccessTime(PathFile);

                if (FileFinalEdit_Data == null) { FileFinalEdit_Data = "Дата не изсветсна"; }
                else { FileFinalEdit_Data = $"{dateTime.Day}.{dateTime.Month}.{dateTime.Year} / {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}"; }
                CointLineCode.Text = $"Строк кода: {TextEditor.LineCount}";
                FileCoint.Text = $"Файлов: {1}"; // как по умолчанию Строк кода: 1
                FilnalRedacrt.Text = $"Последнее изменение: {FileFinalEdit_Data}"; // Во сколько в последний раз, был изменён данный файил
            }
            catch (Exception ex)
            {
                CointLineCode.Text = $"Не вышло оброботать данный блок";
                FileCoint.Text = ex.Message;
                FilnalRedacrt.Text = ex.Source; 
            }
        }

        #region Масштабирование
        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                _currentZoom = e.NewValue;

                // // Применяем масштаб к редактору
                TextEditor.FontSize = _currentZoom;
            }
            catch
            {
                ZoomText.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
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
        bool MaxImadEditer = false,Paneles = false;
        private async void EventKeyDowonDater(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (MaxImadEditer)
                {
                    if (MainWindowEdit.WindowState == WindowState.Maximized)
                    {
                        MainWindowEdit.WindowStyle = WindowStyle.None;
                        Paneles = true; 
                    }
                    else if (MainWindowEdit.WindowState == WindowState.Normal)
                    {
                        MainWindowEdit.WindowState = WindowState.Maximized;
                        Paneles = false;
                    }

                   MainWindowEdit.WindowStyle = WindowStyle.None;
                }
                else
                {
                    if (!Paneles) 
                    { 
                        MainWindowEdit.WindowState = WindowState.Maximized;
                        MainWindowEdit.WindowStyle = WindowStyle.ThreeDBorderWindow;
                    }  
                    else 
                    { 
                        MainWindowEdit.WindowState = WindowState.Normal;
                        MainWindowEdit.WindowStyle = WindowStyle.ThreeDBorderWindow;
                    }
                }
                MaxImadEditer = !MaxImadEditer;
            }

            if (e.Key == Key.F5) {   if (!StartScrept) { EventRunScrept(); } }

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control) 
            {
                SaveScrept();
            }
            //if(e.Key == Key.Back) { ConsoleOutput.Text = ConsoleOutput.Text; }
        }

        private async void RunScrept(object sender, RoutedEventArgs e) { if(!StartScrept) { EventRunScrept(); }  }



        #region Terminal_Control 



        #endregion

        private void TabItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            TabControl tabControl =  (TabControl)e.Source;
            if (tabControl != null) 
            {
                     
            }
        }

        private void MainWindowEdit_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("Сохронить проект?","Завершение работы",MessageBoxButton.OKCancel,MessageBoxImage.Question);
           
        }

        // События во время закрытия
        private void MainWindowEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _errorCheckTimer.Stop();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

#region Обработка кастомной работы подсветки
public class LineBackgroundRenderer : IBackgroundRenderer
{
    private readonly int _lineNumber;
    private readonly Brush _backgroundBrush;

    public LineBackgroundRenderer(int lineNumber, Brush backgroundBrush)
    {
        _lineNumber = lineNumber;
        _backgroundBrush = backgroundBrush;
    }

    public KnownLayer Layer => KnownLayer.Background;
    public static void HighlightLine(TextEditor Edittext, int lineNumber, Brush brush)
    {
        var renderer = new LineBackgroundRenderer(lineNumber, brush);
        Edittext.TextArea.TextView.BackgroundRenderers.Add(renderer);
    }
    public void Draw(TextView textView, DrawingContext drawingContext)
    {
       // Очень важная херьня
    }
}
#endregion
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
