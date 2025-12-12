using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MojeDzwieki
{
    public class Album(string artist, string title, int songsNumber, int year, int downloadNumber)
    {
        public string Artist { get; set; } = artist;
        public string Title { get; set; } = title;
        public int SongsNumber { get; set; } = songsNumber;
        public int Year { get; set; } = year;
        public int DownloadNumber { get; set; } = downloadNumber;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Album> _albums;

        public Album CurrentAlbum { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _albums = LoadData();
            CurrentAlbum = _albums.First();

            SetLabels();
        }

        private void SetLabels()
        {
            artistLbl.Content = CurrentAlbum.Artist;
            downloadNumberLbl.Content = CurrentAlbum.DownloadNumber;
            songsNumberLbl.Content = CurrentAlbum.SongsNumber + " utworów";
            titleLbl.Content = CurrentAlbum.Title;
            yearLbl.Content = CurrentAlbum.Year;
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
            SetLabels();
        }

        private void PrevCommand(object sender, RoutedEventArgs e)
        {
            int index = _albums.IndexOf(CurrentAlbum);

            index--;

            if (index == -1)
                index = _albums.Count - 1;

            CurrentAlbum = _albums[index];
            SetLabels();
        }

        private void DownloadCommand(object sender, RoutedEventArgs e)
        {
            _albums.First(x => x == CurrentAlbum).DownloadNumber++;
            SetLabels();
        }

    }
}