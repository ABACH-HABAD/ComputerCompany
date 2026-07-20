using ComputerCompanyClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для Music.xaml
    /// </summary>
    public partial class MusicWindow : Window
    {
        private readonly ObservableCollection<Song> Songs = [];
        private readonly List<string> tempAudioPaths = [];

        private readonly Random rnd = new();
        private readonly DispatcherTimer progressTimer;

        private int songNumber;
        private bool isSongPlaying = false;

        public MusicWindow()
        {
            InitializeComponent();

            mediaPlayer = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Stop,
                Volume = 0.5
            };

            ContentGrid.Children.Add(mediaPlayer);

            progressTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            InitializeProgressTracker();
        }

        public void LoadMediaResource(Song song)
        {
            try
            {
                // 0. Визуализируем воспроизведение
                foreach (Song s in Songs)
                {
                    s.ChangePlayStatus(false);
                }

                // 1. Создаем временный файл
                string tempAudioPath = Path.GetTempFileName() + ".wav";
                if (!File.Exists(tempAudioPath)) tempAudioPaths.Add(tempAudioPath);

                // 2. Загружаем ресурс
                var resourceUri = new Uri(song.URI, UriKind.Absolute);

                // 3. Проверка существования ресурса
                var streamInfo = Application.GetResourceStream(resourceUri);
                if (streamInfo == null)
                {
                    MessageBox.Show($"Аудио-ресурс ({song.URI}) не найден!");
                    return;
                }

                // 4. Копируем во временный файл
                using (var resourceStream = streamInfo.Stream)
                using (var fileStream = File.Create(tempAudioPath))
                {
                    resourceStream.CopyTo(fileStream);
                }


                // 5. Настраиваем воспроизведение
                SongText.Text = song.Title;
                song.ChangePlayStatus();
                mediaPlayer.Source = new Uri(tempAudioPath);
                mediaPlayer.MediaOpened += (s, args) => Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void InitializeProgressTracker()
        {

            progressTimer.Tick += (s, e) =>
            {
                if (mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    SongProgressBar.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    SongProgressBar.Value = mediaPlayer.Position.TotalSeconds;
                    Time.Text = $"{mediaPlayer.Position:mm\\:ss} / {mediaPlayer.NaturalDuration.TimeSpan:mm\\:ss}";
                }
            };

            mediaPlayer.MediaOpened += (s, e) => progressTimer.Start();
            mediaPlayer.MediaEnded += (s, e) =>
            {
                progressTimer.Stop();
                PlayNextSong();
            };
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mediaPlayer?.Stop();

            foreach (string tempAudioPath in tempAudioPaths)
            {
                try
                {
                    if (File.Exists(tempAudioPath)) File.Delete(tempAudioPath);
                }
                catch { }
            }
        }

        public void Play()
        {
            PlayOrPauseSong.Content = "⏸";
            isSongPlaying = true;
            mediaPlayer?.Play();
        }
        public void Pause()
        {
            PlayOrPauseSong.Content = "▷";
            isSongPlaying = false;
            mediaPlayer?.Pause();
        }
        public void Stop()
        {
            mediaPlayer?.Stop();
        }

        //Кнопка играющей сейчас песни
        private void PlayOrPauseSong_Click(object sender, RoutedEventArgs e)
        {
            if (isSongPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        //Кнопка для перехода к другой песне
        private void SongPlayOrPause_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is Song song)
                {
                    LoadMediaResource(song);
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = VolumeSlider.Value / 10;
        }

        private void RadioStationInitialized()
        {
            SongList.ItemsSource = Songs;
            if (!mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                PlayRandomSong();
            }
        }

        private void PlayRandomSong()
        {
            Stop();
            if (Songs.Count > 0)
            {
                songNumber = rnd.Next(0, Songs.Count);
                LoadMediaResource(Songs[songNumber]);
                Play();
            }
        }

        private void PlayNextSong()
        {
            Stop();
            if (Songs.Count > 0)
            {
                songNumber = songNumber > 0 ? songNumber - 1 : Songs.Count - 1;
                LoadMediaResource(Songs[songNumber]);
                Play();
            }
        }

        private void PlayPreviewSong_Click(object sender, RoutedEventArgs e)
        {
            Stop();
            if (Songs.Count > 0)
            {
                songNumber = songNumber < Songs.Count - 1 ? songNumber + 1 : 0;
                LoadMediaResource(Songs[songNumber]);
                Play();
            }
        }

        private void NextSong_Click(object sender, RoutedEventArgs e) => PlayNextSong();

        //Радиостанции
        private void VibeSongs_Checked(object sender, RoutedEventArgs e)
        {
            RadioStation.Vibe.AddToPlaylist(Songs);
            RadioStationInitialized();

        }

        private void VokaloidSongs_Checked(object sender, RoutedEventArgs e)
        {
            RadioStation.Vokaloid.AddToPlaylist(Songs);
            RadioStationInitialized();
        }

        private void CoolSongs_Checked(object sender, RoutedEventArgs e)
        {
            RadioStation.Cool.AddToPlaylist(Songs);
            RadioStationInitialized();
        }

        private void MLGSongs_Checked(object sender, RoutedEventArgs e)
        {
            RadioStation.MLG.AddToPlaylist(Songs);
            RadioStationInitialized();
        }

        private void VibeSongs_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioStation.Vibe.RemoveFromPlaylist(Songs);
            RadioStationInitialized();
        }

        private void VokaloidSongs_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioStation.Vokaloid.RemoveFromPlaylist(Songs);
            RadioStationInitialized();
        }

        private void CoolSongs_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioStation.Cool.RemoveFromPlaylist(Songs);
            RadioStationInitialized();
        }

        private void MLGSongs_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioStation.MLG.RemoveFromPlaylist(Songs);
            RadioStationInitialized();
        }
    }
}
