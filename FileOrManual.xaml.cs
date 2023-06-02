using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace CST238_Final_Project
{
    public partial class FileOrManual : Page
    {
        public Game SelectedGame { get; }

        public FileOrManual(Game game)
        {
            InitializeComponent();
            SelectedGame = game;
            DataContext = SelectedGame;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Game Info", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Current Settings", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // File selected, do something with the selected file
                string selectedFilePath = openFileDialog.FileName;
                // Perform the necessary operations with the selected file
                // Example: Display the selected file path
                MessageBox.Show($"Selected File: {selectedFilePath}");
            }
        }

        private void ManualEditButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ManualEdit(SelectedGame));
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            GameSelect gameSelectPage = new GameSelect();
            NavigationService?.Navigate(gameSelectPage);
        }
    }
}
