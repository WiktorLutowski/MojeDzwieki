using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MojeDzwieki
{
    public class Album(string artist, string title, int songsNumber, int year, int downloadNumber) : INotifyPropertyChanged
    {
        public string Artist { get; set; } = artist;
        public string Title { get; set; } = title;
        public int SongsNumber { get; set; } = songsNumber;
        public int Year { get; set; } = year;
        public int DownloadNumber
        {
            get => downloadNumber;
            set
            {
                downloadNumber = value;
                OnPropertyChanged(nameof(DownloadNumber));
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly List<Album> _albums;

        public event PropertyChangedEventHandler? PropertyChanged;

        private Album currentAlbum;

        public Album CurrentAlbum
        {
            get { return currentAlbum; }
            set
            {
                currentAlbum = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _albums = LoadData();
            currentAlbum = _albums.First();

            DataContext = this;
        }

        private static List<Album> LoadData()
        {
            StreamReader sr = new("Data.txt");

            List<Album> result = [];

            while (!sr.EndOfStream)
            {
                string artist = sr.ReadLine()!;
                string title = sr.ReadLine()!;
                int songsNumber = int.Parse(sr.ReadLine()!);
                int year = int.Parse(sr.ReadLine()!);
                int downloadNumber = int.Parse(sr.ReadLine()!);
                result.Add(new(artist, title, songsNumber, year, downloadNumber));

                sr.ReadLine(); // Skip empty line
            }

            return result;
        }

        private void NextCommand(object sender, RoutedEventArgs e)
        {
            int index = _albums.IndexOf(CurrentAlbum);

            index++;

            if (index == _albums.Count)
                index = 0;

            CurrentAlbum = _albums[index];
        }

        private void PrevCommand(object sender, RoutedEventArgs e)
        {
            int index = _albums.IndexOf(CurrentAlbum);

            index--;

            if (index == -1)
                index = _albums.Count - 1;

            CurrentAlbum = _albums[index];
        }

        private void DownloadCommand(object sender, RoutedEventArgs e)
        {
            _albums.First(x => x == CurrentAlbum).DownloadNumber++;
            OnPropertyChanged("DownloadNumber");
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null!)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}