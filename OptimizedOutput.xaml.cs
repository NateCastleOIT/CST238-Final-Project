using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CST238_Final_Project
{
    public class TopOrientation : INotifyPropertyChanged
    {
        private int rank;
        public int Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged(nameof(Rank));
            }
        }

        private string yield;
        public string Yield
        {
            get { return yield; }
            set
            {
                yield = value;
                OnPropertyChanged(nameof(Yield));
            }
        }

        private string efficiency;
        public string Efficiency
        {
            get { return efficiency; }
            set
            {
                efficiency = value;
                OnPropertyChanged(nameof(Efficiency));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class OptimizedOutput : Page, INotifyPropertyChanged
    {
        private string outputImageUri;
        public string OutputImageUri
        {
            get { return outputImageUri; }
            set
            {
                outputImageUri = value;
                OnPropertyChanged(nameof(OutputImageUri));
            }
        }

        private List<TopOrientation> topOrientations;
        public List<TopOrientation> TopOrientations
        {
            get { return topOrientations; }
            set
            {
                topOrientations = value;
                OnPropertyChanged(nameof(TopOrientations));
            }
        }

        public OptimizedOutput()
        {
            InitializeComponent();
            DataContext = this;

            OutputImageUri = "pack://application:,,,/Images/output_tile.png";

            TopOrientations = new List<TopOrientation>
            {
                new TopOrientation { Rank = 1,  Yield = "420x Wood/Hr", Efficiency = "69%" },
                new TopOrientation { Rank = 2,  Yield = "410x Wood/Hr", Efficiency = "65%" },
                new TopOrientation { Rank = 3,  Yield = "370x Wood/Hr", Efficiency = "60%" },
                new TopOrientation { Rank = 4,  Yield = "220x Wood/Hr", Efficiency = "45%" },
                new TopOrientation { Rank = 5,  Yield = "200x Wood/Hr", Efficiency = "39%" },
                new TopOrientation { Rank = 6,  Yield = "188x Wood/Hr", Efficiency = "36%" },
                new TopOrientation { Rank = 7,  Yield = "150x Wood/Hr", Efficiency = "30%" },
                new TopOrientation { Rank = 8,  Yield = "100x Wood/Hr", Efficiency = "21%" },
                new TopOrientation { Rank = 9,  Yield = "30x Wood/Hr",  Efficiency = "10%" },
                new TopOrientation { Rank = 10, Yield = "5x Wood/Hr",   Efficiency = "1%" },
                // Add more top orientations as needed
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RankToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rank)
            {
                double hue = (120 - rank * 12) / 360.0; // Convert rank to hue value in the range [0, 1]
                Color color = HslToRgb(hue, 1, 0.5);

                return new SolidColorBrush(color);
            }

            return Brushes.Transparent; // Default color if the value is not an integer
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        // Helper method to convert HSL color to RGB color
        private Color HslToRgb(double h, double s, double l)
        {
            double r, g, b;

            if (s == 0)
            {
                r = g = b = l; // achromatic
            }
            else
            {
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;
                r = HueToRgb(p, q, h + 1.0 / 3);
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - 1.0 / 3);
            }

            byte red = (byte)(r * 255);
            byte green = (byte)(g * 255);
            byte blue = (byte)(b * 255);

            return Color.FromRgb(red, green, blue);
        }

        private double HueToRgb(double p, double q, double t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;

            if (t < 1.0 / 6) return p + (q - p) * 6 * t;
            if (t < 1.0 / 2) return q;
            if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;

            return p;
        }
    }
}
