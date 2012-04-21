using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Microsoft.Win32;
using System.Linq;

namespace CSXStatsViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<StatsEntry> _players;
        private string _currentFile;

        public MainWindow()
        {
            InitializeComponent();
        }

#region File Loading
        private void OpenStatsFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {Filter = "Stats File (*.dat)|*.dat|All files (*.*)|*.*"};

            if (openFileDialog.ShowDialog().Value)
            {
                if (_players != null) _players.Clear();
                _currentFile = openFileDialog.FileName;
                _players = StatsFile.ReadEntriesToList(openFileDialog.FileName);

                // Data Contexts
                playersListBox.DataContext = _players;
                MergeWithComboBox.DataContext = _players;

                // Sorting stuff
                playersListBox.Items.SortDescriptions.Add(new SortDescription("NetScore",ListSortDirection.Descending));
                MergeWithComboBox.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            }
        }
#endregion

#region Saving
        private void SaveEntries(string filename)
        {
            if (StatsFile.SaveEntriesToFile(_players, filename))
            {
                MessageBox.Show("The stats file has been saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("The stats file couldn't be saved", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveStatsButtonClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists(_currentFile))
            {
                SaveEntries(_currentFile);  
            } 
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Stats File (*.dat)|*.dat|All files (*.*)|*.*" };
                if (saveFileDialog.ShowDialog().Value)
                {
                    SaveEntries(saveFileDialog.FileName);
                }
            }
        }
#endregion        
        
#region Merging
        private void MergeButtonClick(object sender, RoutedEventArgs e)
        {
            if ((MergeWithComboBox.SelectedItem != null) 
                && (playersListBox.SelectedItem != null)
                && (MergeWithComboBox.SelectedItem != playersListBox.SelectedItem))
            {
                StatsEntry mergeWith = (StatsEntry)MergeWithComboBox.SelectedItem;
                int index = _players.IndexOf((StatsEntry) playersListBox.SelectedItem);

                _players[index] = _players[index] + mergeWith;
                StatsEntry temp = _players[index];
                _players.Remove(mergeWith);
                // Re-Selection
                playersListBox.SelectedIndex = playersListBox.Items.IndexOf(temp);
                MergeWithComboBox.SelectedIndex = playersListBox.Items.IndexOf(temp);

            } else {
                MessageBox.Show("Please select a different player to merge with the current one.",
                                    "Merge error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
            }
        }
#endregion

#region Removing Players
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (playersListBox.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Select a player to delete","Information",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                return;
            }
            
            List<StatsEntry> entries = new List<StatsEntry>(playersListBox.SelectedItems.Count);
            entries.AddRange(playersListBox.SelectedItems.Cast<StatsEntry>());
            foreach (var entry in entries)
            {
                _players.Remove(entry);
            }
        }
#endregion
        
#region Searching
        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            if ((_players != null) && (_players.Count > 0))
            {
                StatsEntry item = _players.FirstOrDefault(
                                                        r => r.Name.ToLower().Contains(SearchTextBox.Text.ToLower())
                                                    );
                if(item != null)
                {
                    int index = playersListBox.Items.IndexOf(item);
                    playersListBox.Focus();
                    playersListBox.SelectedIndex = index;
                    playersListBox.ScrollIntoView(item);
                    SearchTextBox.ClearValue(BorderBrushProperty);
                } 
                else
                {
                    SearchTextBox.BorderBrush = Brushes.Red;
                }
            }
        }

        private void SearchTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SearchButtonClick(sender, e);
                e.Handled = true;
            }
        }
#endregion
        
#region Other UI stuff
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowAboutDialog(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();

            //Just fancy stuff
            Effect = new BlurEffect();
            BeginStoryboard((Storyboard)Resources["blurElement"]);
            about.ShowDialog();
            BeginStoryboard((Storyboard)Resources["sharpenElement"]);
            Effect = null;
        }

        private void PlayersListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            detailsGrid.DataContext = playersListBox.SelectedItems.Count > 1 ? null : playersListBox.SelectedItem;
        }
#endregion

        
    }
}
