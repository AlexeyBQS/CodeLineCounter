using CodeLineCounter.Counter;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CodeLineCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private LineCounter Counter { get; set; } = null!;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Counter = new(Counter_Change);

            //Counter.ProjectPath = @"C:\Examwork\Product";
            //Counter.IgnorableExtensions = new string[] { ".csproj", ".user", ".png", ".jpg", ".ico", ".jpeg", ".db" };
            //Counter.IgnorableFiles = new string[] { "\\GenTestData\\AssemblyInfo.cs", "\\Schedule\\AssemblyInfo.cs" };
            //Counter.IgnorableFolders = new string[] { "\\.vs", "\\GenTestData\\bin", "\\GenTestData\\obj", "\\Schedule\\bin", "\\Schedule\\obj", };

            Counter.ProjectPath = @"E:\Projects\VisualStudio\С#\3)Other\CodeLineCounter\CodeLineCounter";
            Counter.IgnorableExtensions = new string[] { ".csproj", ".user", ".png", ".jpg", ".ico", ".jpeg", ".db" };
            Counter.IgnorableFiles = new string[] { "\\AssemblyInfo.cs" };
            Counter.IgnorableFolders = new string[] { "\\bin", "\\obj" };

            Counter.StartAsync();
        }

        private void Counter_Change(object sender, LineCounterEventArgs e)
        {
            LineCounter counter = (LineCounter)sender;

            Dispatcher.Invoke(() =>
            {
                LineCountTextBlock.Text = counter.LineCount.ToString("N0");
                CharacterCountTextBlock.Text = counter.CharacterCount.ToString("N0");
            });
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new(Counter);
            settingWindow.Owner = this;
            
            settingWindow.Show();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Counter.Stop();
        }
    }
}
