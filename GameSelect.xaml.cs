using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace CST238_Final_Project
{
    public class Game
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
    }

    public partial class GameSelect : Page
    {
        public List<Game> Games { get; set; }

        public GameSelect()
        {
            InitializeComponent();
            LoadGames();
        }

        private void LoadGames()
        {
            Games = new List<Game>
            {
                new Game { Name = "They Are Billions", Thumbnail = new Uri("pack://application:,,,/Images/They_Are_Billions_Video_Game_Logo.jpg").ToString() },
                new Game { Name = "Dawn of the Dukes", Thumbnail = new Uri("pack://application:,,,/Images/Dawn_of_the_Dukes_artwork.jpg").ToString() },
                new Game { Name = "Diplomacy is NOT an Option", Thumbnail = new Uri("pack://application:,,,/Images/Diplomacy is NOT an Option.jpg").ToString() },
                new Game { Name = "Factorio", Thumbnail = new Uri("pack://application:,,,/Images/Factorio.jpg").ToString() },
            };
            DataContext = this;
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            var game = (sender as Button)?.DataContext as Game;
            if (game != null)
            {
                Game selectedGame = Games.FirstOrDefault(g => g.Name == game.Name);
                if (selectedGame != null)
                {
                    FileOrManual fileOrManualPage = new FileOrManual(selectedGame);
                    NavigationService?.Navigate(fileOrManualPage);
                }
            }
        }

    }
}
