using CodeLineCounter.Counter;
using CodeLineCounter.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CodeLineCounter
{
    /// <summary>
    /// Логика взаимодействия для SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow(LineCounter lineCounter)
        {
            Counter = lineCounter;
            InitializeComponent();
        }

        private LineCounter Counter { get; set; } = default!;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Настройки";

            ProjectPath_UpdateBlock();
        }

        #region ProjectPath

        private void ProjectPath_UpdateBlock()
        {
            ProjectPathTextBox.Text = Counter.ProjectPath;
        }

        private void ChangeProjectPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker dialog = new();
            dialog.InputPath = Counter.ProjectPath ?? @"C:\Windows\system32";

            if (dialog.ShowDialog(this) == true)
            {
                Counter.ProjectPath = dialog.ResultPath;
            }

            ProjectPath_UpdateBlock();
        }

        #endregion

        #region IgnorableExtensions

        private void IgnorableExtensions_UpdateBlock()
        {

        }

        #endregion

        #region IgnorableFolders

        private void IgnorableFolders_UpdateBlock()
        {

        }

        #endregion

        #region IgnorableFiles

        private void IgnorableFiles_UpdateBlock()
        {

        }

        #endregion        
    }
}
