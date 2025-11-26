using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorCodeIDE.Pack_EditCode.language_redactors.CSarp
{
    public class languageCShaerp
    {
        #region Исправленная система компиляции C#

        /// <summary>
        /// Главный оброботчик C# кода
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> ExecuteCSharpAsync(string code)
        {
            var tempDir = Path.GetTempPath();
            var csFile = Path.Combine(tempDir, $"temp_program_{Guid.NewGuid()}.cs");
            var exeFile = Path.Combine(tempDir, $"temp_program_{Guid.NewGuid()}.exe");

            try
            {
                string wrappedCode = WrapCodeInProgramClass(code);
                // Заменяем асинхронную запись на синхронную (для C# 7.3)
                File.WriteAllText(csFile, wrappedCode);

                // Используем CodeDom вместо Roslyn для совместимости
                string compilationResult = CompileCSharpCodeLegacy(csFile, exeFile);
                if (!string.IsNullOrEmpty(compilationResult))
                {
                    return $"Ошибки компиляции:\n{compilationResult}";
                }
                return await RunExeInConsole(exeFile);
            }
            catch (Exception ex)
            {
                return $"Ошибка выполнения: {ex.Message}";
            }
            finally
            {
                CleanupTempFiles(csFile, exeFile);
            }
        }


        /// <summary>
        /// Обробоатывает код на ошибки, возрощая их
        /// </summary>
        public string ErrorComplid(string csFile)
        {
            try
            {
                using (var provider = new Microsoft.CSharp.CSharpCodeProvider())
                {
                    var parameters = new System.CodeDom.Compiler.CompilerParameters
                    {
                        GenerateExecutable = true,
                        GenerateInMemory = false,
                        TreatWarningsAsErrors = false,

                        CompilerOptions = "/optimize /langversion:11.0" // вообще должен настроить язык версии C#. Но не хочет робить как нужно)
                    };

                    // Добавляем ссылки на системные сборки
                    AddDefaultReferences(parameters);

                    // Добавляем пользовательские библиотеки
                    AddUserLibraries(parameters);

                    var results = provider.CompileAssemblyFromFile(parameters, csFile);

                    if (results.Errors.HasErrors)
                    {
                        var errors = new StringBuilder();
                        foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                        {
                            if (!error.IsWarning)
                            {
                                errors.AppendLine($"{error.ErrorText} (Строка {error.Line})");
                            }
                        }
                        return errors.ToString();
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }
        }


        /// <summary>
        /// Оборачивает код в структуру Program класса если нужно
        /// </summary>
        public string WrapCodeInProgramClass(string code)
        {
            // Проверяем, есть ли уже класс Program с Main методом
            bool hasProgramClass = ( code.Contains("class Program") && (code.Contains("static void Main") ) || code.Contains("static int Main"));

            bool hasMainMethod = code.Contains("static void Main") || code.Contains("static int Main");

            // Если код уже содержит правильную структуру, возвращаем как есть
            if (hasProgramClass && hasMainMethod)
            {
                return code;
            }

            // Если есть Main метод но нет класса Program, добавляем класс
            if (hasMainMethod && !hasProgramClass)
            {
                return $@"
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                
                {code}";
            }

            // Если нет структуры программы, оборачиваем в стандартный шаблон
            return $@"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            
            class Program
            {{
                static void Main(string[] args)
                {{
                    // Пользовательский код
                    {code}
                    
                    // Если нужно ждать нажатия клавиши перед закрытием
                    // Console.WriteLine(""Нажмите любую клавишу для выхода..."");
                    // Console.ReadKey();
                }}
            }}";
        }

        /// <summary>
        /// Запускает EXE файл в отдельном консольном окне
        /// </summary>
        public async Task<string> RunExeInConsole(string exeFile)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe", // Запускаем через cmd
                    Arguments = $"/c \"\"{exeFile}\" & pause\"", // /c - выполнить команду, & pause - ждать нажатия
                    UseShellExecute = true, // Важно: true для открытия нового окна
                    CreateNoWindow = false, // Создавать окно консоли
                    WindowStyle = ProcessWindowStyle.Normal
                };

                var process = new Process { StartInfo = processStartInfo };

                // Запускаем процесс
                process.Start();

                // Ждем завершения (асинхронно)
                await Task.Run(() => process.WaitForExit());

                return "✅ Программа выполнена.";
            }
            catch (Exception ex)
            {
                return $"❌ Ошибка запуска: {ex.Message}";
            }
        }

        /// <summary>
        /// Очищает временные файлы
        /// </summary>
        public void CleanupTempFiles(params string[] files)
        {
            foreach (var file in files)
            {
                try
                {
                    if (File.Exists(file)) { File.Delete(file); }
                        
                }
                catch
                {
                    // Игнорируем ошибки удаления
                }
            }
        }

        /// <summary>
        /// Компилирует C# код в EXE файл используя CodeDom (совместим с C# 7.3)
        /// </summary>
        private string CompileCSharpCodeLegacy(string csFile, string exeFile)
        {
            try
            {
                using (var provider = new Microsoft.CSharp.CSharpCodeProvider())
                {
                    var parameters = new System.CodeDom.Compiler.CompilerParameters
                    {
                        GenerateExecutable = true,
                        OutputAssembly = exeFile,
                        GenerateInMemory = false,
                        TreatWarningsAsErrors = false,
                        CompilerOptions = "/optimize /langversion:8.0" // вообще должен настроить язык версии C#. Но не хочет робить как нужно)
                    };

                    // Добавляем ссылки на системные сборки
                    AddDefaultReferences(parameters);

                    // Добавляем пользовательские библиотеки
                    AddUserLibraries(parameters);

                    var results = provider.CompileAssemblyFromFile(parameters, csFile);

                    if (results.Errors.HasErrors)
                    {
                        var errors = new StringBuilder();
                        foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                        {
                            if (!error.IsWarning)
                            {
                                errors.AppendLine($"{error.ErrorText} (Строка {error.Line})");
                            }
                        }
                        return errors.ToString();
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка компиляции: {ex.Message}";
            }
        }

        /// <summary>
        /// Добавляет стандартные системные ссылки
        /// </summary>
        private void AddDefaultReferences(System.CodeDom.Compiler.CompilerParameters parameters)
        {
            string[] defaultReferences = {
        "System.dll",
        "System.Core.dll",
        "System.Data.dll",
        "System.Xml.dll",
        "Microsoft.CSharp.dll",
        "System.Net.Http.dll",
        "System.Runtime.dll", // Явно добавляем System.Runtime
        "System.IO.dll",
        "System.Collections.dll"
    };

            foreach (string reference in defaultReferences)
            {
                try
                {
                    parameters.ReferencedAssemblies.Add(reference);
                }
                catch
                {
                    // Игнорируем ошибки добавления ссылок
                }
            }

            // Добавляем сборки из GAC
            try
            {
                string frameworkPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
                if (!string.IsNullOrEmpty(frameworkPath))
                {
                    parameters.ReferencedAssemblies.Add(Path.Combine(frameworkPath, "mscorlib.dll"));
                    parameters.ReferencedAssemblies.Add(Path.Combine(frameworkPath, "System.Runtime.dll"));
                }
            }
            catch
            {
                // Резервный вариант
            }
        }

        /// <summary>
        /// Добавляет пользовательские библиотеки из папок
        /// </summary>
        private void AddUserLibraries(System.CodeDom.Compiler.CompilerParameters parameters)
        {
            // Папка с библиотеками для C#
            string csharpLibsPath = Path.Combine(Directory.GetCurrentDirectory(), "Libraries", "CSharp");

            if (Directory.Exists(csharpLibsPath))
            {
                var dllFiles = Directory.GetFiles(csharpLibsPath, "*.dll");
                foreach (string dll in dllFiles)
                {
                    try
                    {
                        parameters.ReferencedAssemblies.Add(dll);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Не удалось добавить библиотеку {dll}: {ex.Message}");
                    }
                }
            }
        }

        #endregion
    }
}
