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

namespace EditorCodeIDE.Pack_EditCode
{
    public class CodeExecutionService
    {
        public enum Language
        {
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
                    return await ExecuteCSharpAsync(code);
                case Language.Cpp:
                    return await ExecuteCppAsync(code);
                case Language.Python:
                    return await ExecutePythonAsync(code);
                case Language.JavaScript:
                    return await ExecuteJavaScriptAsync(code);
                default:
                    return "Unsupported language";
            }
        }


        // C# через Roslyn
        private async Task<string> ExecuteCSharpAsync(string code)
        {
            try
            {
                // Перехватываем вывод консоли
                var originalOutput = Console.Out;
                using (var writer = new StringWriter())
                {
                    Console.SetOut(writer);

                    var script = CSharpScript.Create(code);
                    var result = await script.RunAsync();

                    // Если есть возвращаемое значение, добавляем его к выводу
                    string output = writer.ToString();
                    if (result?.ReturnValue != null)
                    {
                        output += result.ReturnValue.ToString();
                    }

                    Console.SetOut(originalOutput);

                    return string.IsNullOrEmpty(output) ? "Код выполнен (без вывода)" : output;
                }
            }
            catch (CompilationErrorException ex)
            {
                return string.Join("\n", ex.Diagnostics);
            }
            catch (Exception ex)
            {
                return $"Ошибка выполнения: {ex.Message}";
            }
        }

        // C++ через внешний компилятор
        private async Task<string> ExecuteCppAsync(string code)
        {
            if (!IsGppAvailable())
            {
                return "C++ compiler (g++) not found. Please install MinGW-w64 or set PATH variable.";
            }

            var tempDir = Path.GetTempPath();
            var cppFile = Path.Combine(tempDir, "temp_code.cpp");
            var exeFile = Path.Combine(tempDir, "temp_code.exe");

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
                    return $"Compilation Error:\n{compileOutput}";
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

                return string.IsNullOrEmpty(error) ? output : $"Error: {error}\nOutput: {output}";
            }
            catch (Exception ex)
            {
                return $"C++ Execution Error: {ex.Message}";
            }
            finally
            {
                // Удаляем временные файлы
                try { if (File.Exists(cppFile)) File.Delete(cppFile); } catch { }
                try { if (File.Exists(exeFile)) File.Delete(exeFile); } catch { }
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
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    return $"Python Error: {ex.Message}";
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
                    return output.Length > 0 ? output.ToString() : "JavaScript executed (no output)";
                }
                catch (Exception ex)
                {
                    return $"JavaScript Error: {ex.Message}";
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
    }
}