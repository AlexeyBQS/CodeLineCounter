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
        }

        #region ProjectPath

        private void ProjectPath_UpdateBlock()
        {

        }

        private void ChangeProjectPathButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region IgnorableExtensions



        #endregion

        #region IgnorableFolders



        #endregion

        #region IgnorableFiles



        #endregion        
    }
}
