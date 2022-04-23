using CodeLineCounter.Counter;
using CodeLineCounter.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            IgnorableExtensions_UpdateBlock();
            IgnorableFolders_UpdateBlock();
            IgnorableFiles_UpdateBlock();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Counter.ForceUpdate();
        }

        #region ProjectPath

        private void ProjectPath_UpdateBlock()
        {
            ProjectPathTextBox.Text = Counter.ProjectPath;
        }

        private void ChangeProjectPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker dialog = new();
            dialog.InputPath = Counter.ProjectPath ?? Directory.GetCurrentDirectory();

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
            IgnorableExtensionsListBox.ItemsSource = Counter.IgnorableExtensions.OrderBy(x => x);
        }

        private void IgnorableExtensionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnorableExtensionsListBox.SelectedItems.Count > 0)
            {
                RemoveIgnoreExtensionButton.IsEnabled = true;
            }
            else
            {
                RemoveIgnoreExtensionButton.IsEnabled = false;
            }
        }

        private void AddIgnoreExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            string value = IgnoreExtensionTextBox.Text;

            if (!string.IsNullOrEmpty(value) && value[0] == '.' && value.Count(x => x == '.') == 1)
            {
                if (!Counter.IgnorableExtensions.Where(x => x == value).Any())
                {
                    Counter.IgnorableExtensions.Add(value);
                    IgnorableExtensions_UpdateBlock();
                }

                IgnoreExtensionTextBox.Text = string.Empty;
            }
        }

        private void RemoveIgnoreExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            if (IgnorableExtensionsListBox.SelectedItem != null)
            {
                string removeValue = (string)IgnorableExtensionsListBox.SelectedItem;

                Counter.IgnorableExtensions.Remove(removeValue);

                IgnorableExtensions_UpdateBlock();
            }
        }

        #endregion

        #region IgnorableFolders

        private void IgnorableFolders_UpdateBlock()
        {
            IgnorableFoldersListBox.ItemsSource = Counter.IgnorableFolders.OrderBy(x => x);
        }

        private void IgnorableFoldersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnorableFoldersListBox.SelectedItems.Count > 0)
            {
                RemoveIgnoreFolderButton.IsEnabled = true;
            }
            else
            {
                RemoveIgnoreFolderButton.IsEnabled = false;
            }
        }

        private void AddIgnoreFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker dialog = new();
            dialog.InputPath = Counter.ProjectPath ?? Directory.GetCurrentDirectory();

            if (dialog.ShowDialog(this) == true)
            {
                if (dialog.ResultPath.Contains(Counter.ProjectPath ?? string.Empty))
                {
                    string ignoreFolder = dialog.ResultPath.Replace(Counter.ProjectPath ?? string.Empty, "");

                    if (!Counter.IgnorableFolders.Contains(ignoreFolder))
                    {
                        Counter.IgnorableFolders.Add(ignoreFolder);
                        IgnorableFolders_UpdateBlock();
                    }
                }
            }
        }

        private void RemoveIgnoreFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (IgnorableFoldersListBox.SelectedItem != null)
            {
                string removeIgnoreFolder = (string)IgnorableFoldersListBox.SelectedItem;

                Counter.IgnorableFolders.Remove(removeIgnoreFolder);

                IgnorableFolders_UpdateBlock();
            }
        }

        #endregion

        #region IgnorableFiles

        private void IgnorableFiles_UpdateBlock()
        {
            IgnorableFilesListBox.ItemsSource = Counter.IgnorableFiles.OrderBy(x => x);
        }

        private void IgnorableFilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnorableFilesListBox.SelectedItems.Count > 0)
            {
                RemoveIgnoreFileButton.IsEnabled = true;
            }
            else
            {
                RemoveIgnoreFileButton.IsEnabled = false;
            }
        }

        private void AddIgnoreFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.InitialDirectory = Counter.ProjectPath ?? Directory.GetCurrentDirectory();
            dialog.Multiselect = false;

            if (dialog.ShowDialog(this) == true)
            {
                string ignoreFile = dialog.FileName.Replace(Counter.ProjectPath ?? string.Empty, "");

                if (!Counter.IgnorableFiles.Contains(ignoreFile))
                {
                    Counter.IgnorableFiles.Add(ignoreFile);
                    IgnorableFiles_UpdateBlock();
                }
            }
        }

        private void RemoveIgnoreFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (IgnorableFilesListBox.SelectedItem != null)
            {
                string removeIgnoreFile = (string)IgnorableFilesListBox.SelectedItem;

                Counter.IgnorableFiles.Remove(removeIgnoreFile);

                IgnorableFiles_UpdateBlock();
            }
        }

        #endregion
    }
}
