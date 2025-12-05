using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace EditorCodeIDE.Pack_EditCode.RegisterSetteng
{
    public class RegisterSetteng
    {
        public bool Initialize { get; protected set; }

        #region перемнные хронящие данные реестра.
        public double version_programm_reestor = 0.01;
        public double version_dvijock_reestor = 0.01;
        #endregion

        #region перемнные хронящие данные git - контроля версии.
        public double version_programm_gitversioncontrol =  0.01;
        public double version_dvijock_gitversioncontrol = 0.01;
        #endregion
        
        public bool IsAddReistorOpenContexsMenu { protected get; set; } = false;

        public RegisterSetteng() 
        {
            
            if (version_programm_reestor < version_programm_gitversioncontrol) 
            {
                // Выдать сообщение, что версия программы устарела!
                return;
            }

            if (version_programm_reestor < version_programm_gitversioncontrol)
            {
                // Выдать сообщение, что версия программы устарела!
                return;
            }

            InitializeProgramm();
        }

        void InitializeProgramm()
        {
            Initialize = true;
            ///
            // HKEY_CLASSES_ROOT\exefile\shell\EditorCodeIDE
            // directory\shell
            // HKEY_CLASSES_ROOT\directory\background\shell
            // HKEY_CURRENT_USER\SOFTWARE\Classes
            string info_prog = @"SOFTWARE\Classes";
            string pathexeregcontol = @"directory\background\shell\EditorCodeIDE";

            // string pathexeregcontol = @"\SOFTWARE\Classes\EditorCodeIDE";

            using (RegistryKey menyKey = Registry.CurrentUser.CreateSubKey(info_prog))
            {
                if (menyKey != null)
                {
                    menyKey.SetValue("", "Alfa: 0.0.1");
                }
            }

            if (IsAddReistorOpenContexsMenu)
            {
                using (RegistryKey menyKey = Registry.ClassesRoot.CreateSubKey(pathexeregcontol))
                {
                    string comand = @"D:\EditCode\EditorCodeIDE\EditorCodeIDE\bin\Debug\EditorCodeIDE.exe";
                    if (menyKey != null)
                    {
                        menyKey.SetValue("", "Открыть с помощью EditCodeIDE");
                    }
                }

                using (RegistryKey menyKey = Registry.ClassesRoot.CreateSubKey(pathexeregcontol + @"\command"))
                {
                    string comand = @"D:\EditCode\EditorCodeIDE\EditorCodeIDE\bin\Debug\EditorCodeIDE.exe";
                    if (menyKey != null)
                    {
                        menyKey.SetValue("", comand);
                    }
                }
                MessageBox.Show("Теперь вы можете открывать файлы при помощи контекстного меню. выбров соотвествующий пункт.", "УРА!");
            }
        }


        protected double VersionGitDvoj() 
        {
            if(!Initialize) { return version_programm_reestor; }
            return version_programm_gitversioncontrol;
        }
    }
}
