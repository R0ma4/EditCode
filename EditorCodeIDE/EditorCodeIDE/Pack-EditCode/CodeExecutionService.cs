using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using IronPython.Hosting;
using Jint;

// Добавляем необходимые using для компиляции
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

// оброботчики
using EditorCodeIDE.Pack_EditCode.language_redactors.CSarp;

namespace EditorCodeIDE.Pack_EditCode
{
    public class CodeExecutionService
    {
        languageCShaerp languageCShaerp = new languageCShaerp();
        public enum Language
        {
            Text,
            SQL,
            CSharp,
            Cpp,
            Python,
            JavaScript
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
                    return await ExecutePythonAsync(code);
                case Language.JavaScript:
                    return await ExecuteJavaScriptAsync(code);
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
    }
}