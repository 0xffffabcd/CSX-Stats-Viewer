using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CSXStatsViewer
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void WindowMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void OpenGithubPage(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/nacereddine/CSX-Stats-Viewer");
        }

    }
}
