using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using EditorCodeIDE.Pack_EditCode.languageRedactors.PYHTON;
using Microsoft.CodeAnalysis.Scripting;
using IronPython.Hosting;
using Jint;

// Добавляем необходимые using для компиляции
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

// оброботчики
using EditorCodeIDE.Pack_EditCode.languageRedactors.CSarp;

namespace EditorCodeIDE.Pack_EditCode
{
    public class CodeExecutionService
    {
        #region Переменные Кампилятора.

        /// <summary> Текст совершонной Ошибки </summary>
        public string ErrorText { get; private set; }
        /// <summary> Текст совершонной Предупреждения </summary>
        public string WaringText { get; private set; }
        /// <summary> Текст совершонной Сообщения </summary>
        public string MessageText { get; private set; }

        /// <summary> Строка совершонной Ошибки </summary>
        public int LineError { get; private set; } = -1;

        /// <summary> Столбец / символ совершонной Ошибки </summary>
        public int ColumError { get; private set; } = 0;


        #endregion
        languageCShaerp languageCShaerp = new languageCShaerp();
        PyhtonComplit pyhtonComplit = new PyhtonComplit();  
        public enum Language
        {
            Text,
            SQL,
            CSharp,
            Cpp,
            Python,
            JavaScript,
            XMAL,
            YAML,
            OcScrept
        }

        public void CompilatorErrorInfo(string user_scrept, Language language) 
        {
            string ErrorInfo = string.Empty;
            switch (language)
            {
                case Language.CSharp:
                    languageCShaerp.CheckForErrorsBackground(user_scrept);
                    LineError = languageCShaerp.ErrorCoint;
                    ErrorText = languageCShaerp.ErrorMesaege;
                    Console.WriteLine($"[Компилятор]Обработана ошибка: [{LineError}] | Error^{ErrorText}");
                    break;
                case Language.Cpp:
                    ErrorText = "Код: LPCP 004. В данной нерсии, нет кампилятора [C++]";
                    break;
                case Language.Python:
                    Console.WriteLine("оброботчик => Код: LPCP 004. В данной нерсии, нет кампилятора [Python]");
                    ErrorText = "Код: LPCP 004. В данной нерсии, нет кампилятора [Python]";
                    break;
                case Language.JavaScript:
                    ErrorText = "Код: LPCP 004. В данной нерсии, нет кампилятора [JavaScript]";
                    break;
                case Language.SQL:
                    ErrorText = $"Код: LPCP 004. В данной нерсии, нет кампилятора [SQL]";
                    break;
                case Language.Text:
                    ErrorText = null;
                    LineError = 0;
                    break;
                case Language.XMAL:
                    ErrorText = $"Код: LPCP 004. В данной нерсии, нет кампилятора [XMAL]";
                     break;
                case Language.YAML:
                    ErrorText = $"Код: LPCP 004. В данной нерсии, нет кампилятора [YAML]";
                    break;
                default:
                    ErrorText = "Код: LPCP 004. (Ни известный язык оброботки)";
                    break;
            }
        }

        public async Task<string> ExecuteCodeAsync(string code, Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return await languageCShaerp.ExecuteCSharpAsync(code);
                case Language.Cpp:
                    return await ExecuteCppAsync(code);
                case Language.Python:
                    return await pyhtonComplit.ExecuteCSharpAsync(code);
                case Language.JavaScript:
                    return await ExecuteJavaScriptAsync(code);
                case Language.YAML:
                    return null;
                case Language.XMAL:
                    return null;
                case Language.SQL:
                    return null;
                default:
                    return "не изветсный пораметор рброботки!";
            }
        }

        #region Other Languages Execution - Остальные языки без изменений

        // C++ через внешний компилятор
        private async Task<string> ExecuteCppAsync(string code)
        {
            if (!IsGppAvailable())
            {
                return "C++ compiler (g++) not found. Please install MinGW-w64 or set PATH variable.";
            }

            var tempDir = Path.GetTempPath();
            var cppFile = Path.Combine(tempDir, $"temp_code_{Guid.NewGuid()}.cpp");
            var exeFile = Path.Combine(tempDir, $"temp_code_{Guid.NewGuid()}.exe");

            try
            {
                // Сохраняем код во временный файл
                File.WriteAllText(cppFile, code);

                // Компилируем
                var compileProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "g++",
                        Arguments = $"\"{cppFile}\" -o \"{exeFile}\" -static-libgcc -static-libstdc++",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                compileProcess.Start();
                string compileOutput = await ReadStreamAsync(compileProcess.StandardError);
                compileProcess.WaitForExit();

                if (compileProcess.ExitCode != 0)
                {
                    return $"Ошибки компиляции C++:\n{compileOutput}";
                }

                // Запускаем скомпилированную программу
                var runProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = exeFile,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                runProcess.Start();
                string output = await ReadStreamAsync(runProcess.StandardOutput);
                string error = await ReadStreamAsync(runProcess.StandardError);
                runProcess.WaitForExit();

                return string.IsNullOrEmpty(error) ? output : $"Ошибка: {error}\nВывод: {output}";
            }
            catch (Exception ex)
            {
                return $"Ошибка выполнения C++: {ex.Message}";
            }
            finally
            {
                languageCShaerp.CleanupTempFiles(cppFile, exeFile);
            }
        }

        // Вспомогательный метод для чтения потоков
        private async Task<string> ReadStreamAsync(StreamReader reader)
        {
            return await reader.ReadToEndAsync();
        }

        #region Старые компиляторы

        // Python через IronPython
        private Task<string> ExecutePythonAsync(string code)
        {
            return Task.Run(() =>
            {
                try
                {
                    var engine = Python.CreateEngine();
                    var scope = engine.CreateScope();

                    // Перехватываем стандартный вывод
                    var outputStream = new MemoryStream();
                    engine.Runtime.IO.SetOutput(outputStream, Encoding.UTF8);

                    engine.Execute(code, scope);

                    outputStream.Position = 0;
                    using (var reader = new StreamReader(outputStream))
                    {
                        string result = reader.ReadToEnd();
                        return string.IsNullOrEmpty(result) ? "Python код выполнен" : result;
                    }
                }
                catch (Exception ex)
                {
                    return $"Ошибка Python: {ex.Message}";
                }
            });
        }

        // JavaScript через Jint
        private Task<string> ExecuteJavaScriptAsync(string code)
        {
            return Task.Run(() =>
            {
                try
                {
                    var engine = new Engine(options =>
                    {
                        options.TimeoutInterval(TimeSpan.FromSeconds(10));
                    });

                    var output = new StringBuilder();
                    engine.SetValue("console", new
                    {
                        log = new Action<object>(msg => output.AppendLine(msg?.ToString()))
                    });

                    engine.Execute(code);
                    string result = output.ToString();
                    return string.IsNullOrEmpty(result) ? "JavaScript код выполнен" : result;
                }
                catch (Exception ex)
                {
                    return $"Ошибка JavaScript: {ex.Message}";
                }
            });
        }

        private bool IsGppAvailable()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "g++",
                        Arguments = "--version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }


        #endregion

        #endregion
    }
}