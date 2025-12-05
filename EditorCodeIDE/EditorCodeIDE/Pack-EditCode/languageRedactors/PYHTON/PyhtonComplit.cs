using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorCodeIDE.Pack_EditCode.languageRedactors.PYHTON
{
    public class PyhtonComplit
    {

        public async Task<string> ExecuteCSharpAsync(string code)
        {
            var tempDir = Path.GetTempPath();
            var pyFile = Path.Combine(tempDir, $"temp_program_{Guid.NewGuid()}.py");
            var exeFile = Path.Combine(tempDir, $"temp_program_{Guid.NewGuid()}.exe");

            try
            {
                return await RunExeInConsole(pyFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка компиляции: {ex.Message}", "Оишбка с кодом LPCP: 002", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"Ошибка при проверке кода: {ex.Message} {ex.StackTrace}");
                return $"Ошибка выполнения: {ex.Message}";
            }
            
        }

        protected async Task<string> RunExeInConsole(string exeFile)
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

        public Task<string> ExecutePythonAsync(string code)
        {
            return Task.Run(() =>
            {
                try
                {
                    var engine = Python.CreateEngine();
                    var scope = engine.CreateScope();

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
    }
}
