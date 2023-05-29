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

        private ObservableCollection<ObservableCollection<Tile>> map;
        public ObservableCollection<ObservableCollection<Tile>> Map 
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

            // Initialize the map with cells
            Map = new ObservableCollection<ObservableCollection<Tile>>();
            for (int row = 0; row < 10; row++)
            {
                ObservableCollection<Tile> rowCells = new ObservableCollection<Tile>();
                for (int col = 0; col < 10; col++)
                {
                    rowCells.Add(Tiles[3]);
                }
                Map.Add(rowCells);
            }
        }

        private void OptimizeForButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Optimize For clicked"); // Placeholder for button functionality
        }

        private void ChangeMapSizeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Change Map Size clicked"); // Placeholder for button functionality
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the final page (e.g., OptimizedOutput)
            NavigationService?.Navigate(new OptimizedOutput());
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
                int rowIndex = -1;
                int colIndex = -1;

                // Retrieve the row and column indices from the command parameter
                if (parameter is Tuple<int, int> indices)
                {
                    rowIndex = indices.Item1;
                    colIndex = indices.Item2;
                }

                if (rowIndex >= 0 && colIndex >= 0 && rowIndex < Map.Count && colIndex < Map[rowIndex].Count)
                {
                    // Update the corresponding cell with the selected tile
                    Map[rowIndex][colIndex] = tile;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
