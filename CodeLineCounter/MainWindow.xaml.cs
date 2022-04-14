using CodeLineCounter.Counter;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            Counter.ProjectPath = @"E:\Projects\VisualStudio\С#\3)Other\CodeLineCounter\CodeLineCounter";
            Counter.Extentions = new string[] { "*.cs", "*.xaml" };
            Counter.Ignorable = new string[] { "\\obj", "\\bin", "\\.vs" };

            Counter.Start();
        }

        private void Counter_Change(object sender, LineCounterEventArgs e)
        {
            // Какой-то комментарий
            LineCounter counter = (LineCounter)sender;

            Dispatcher.Invoke(() =>
            {
                LineCountTextBlock.Text = $"{counter.LineCount}";
                CharacterCountTextBlock.Text = $"{counter.CharacterCount}";
            });
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
