using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CST238_Final_Project
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public partial class ManualEdit : Page, INotifyPropertyChanged
    {
        public class Tile : INotifyPropertyChanged
        {
            private BitmapImage tileImage;
            public BitmapImage TileImage
            {
                get { return tileImage; }
                set
                {
                    tileImage = value;
                    OnPropertyChanged(nameof(TileImage));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Game selectedGame;
        public Game SelectedGame
        {
            get { return selectedGame; }
            set
            {
                selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }

        private List<Tile> tiles;
        public List<Tile> Tiles
        {
            get { return tiles; }
            set
            {
                tiles = value;
                OnPropertyChanged(nameof(Tiles));
            }
        }

        private Tile currentSelection;
        public Tile CurrentSelection
        {
            get { return currentSelection; }
            set
            {
                currentSelection = value;
                OnPropertyChanged(nameof(CurrentSelection));
            }
        }

        private List<List<Tile>> map;
        public List<List<Tile>> Map 
        { 
            get { return map; } 
            set
            {
                map = value; 
                OnPropertyChanged(nameof(Map));
            }
        }



        public ICommand DrawTileCommand { get; }
        public ICommand ChangeCellTileCommand { get; }


        public ManualEdit(Game selectedGame)
        {
            InitializeComponent();
            SelectedGame = selectedGame;
            DataContext = this;

            // Initialize tiles and set the default current selection
            Tiles = new List<Tile>
            {
                new Tile { TileImage = new BitmapImage(new Uri("pack://application:,,,/Images/tree_tile.png")) },
                new Tile { TileImage = new BitmapImage(new Uri("pack://application:,,,/Images/stone_tile.png")) },
                new Tile { TileImage = new BitmapImage(new Uri("pack://application:,,,/Images/water_tile.png")) },
                new Tile { TileImage = new BitmapImage(new Uri("pack://application:,,,/Images/grass_tile.png")) }
            };
            CurrentSelection = Tiles[0];

            // Initialize the command for drawing a tile
            DrawTileCommand = new RelayCommand(TileClicked);

            // Initialize the command for changing a cell tile
            ChangeCellTileCommand = new RelayCommand(ChangeCellTile);

            // Initialize the map with buttons
            Map = new List<List<Tile>>();
            for (int row = 0; row < 10; row++)
            {
                List<Tile> rowButtons = new List<Tile>();
                for (int col = 0; col < 10; col++)
                {
                    Tile tile = new Tile();
                    tile.TileImage = new BitmapImage(new Uri("pack://application:,,,/Images/grass_tile.png"));
                    rowButtons.Add(tile);
                }
                Map.Add(rowButtons);
            }

        }

        private void OptimizeForButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Select a resource to produce an optimized output for!"); // Placeholder for button functionality
        }

        private void ChangeMapSizeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Change Map Size Here"); // Placeholder for button functionality
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            OptimizedOutput optimizedOutputPage = new OptimizedOutput();
            Window optimizedOutputWindow = new Window
            {
                Content = optimizedOutputPage,
                Title = "Optimized Output",
                Width = 1050,
                Height = 525
            };
            optimizedOutputWindow.Show();
        }

        private void TileClicked(object parameter)
        {
            if (parameter is Tile selectedTile)
            {
                CurrentSelection = selectedTile;
            }
        }

        private void ChangeCellTile(object parameter)
        {
            if (parameter is Tile tile)
            {
                // Find the clicked tile in the Map and update its image
                foreach (var row in Map)
                {
                    foreach (var cell in row)
                    {
                        if (cell == tile)
                        {
                            cell.TileImage = CurrentSelection.TileImage;
                            return;
                        }
                    }
                }
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            GameSelect gameSelectPage = new GameSelect();
            NavigationService?.Navigate(gameSelectPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
