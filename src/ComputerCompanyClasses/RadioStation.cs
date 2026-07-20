using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerCompanyClasses
{
    public class RadioStation
    {
        private readonly List<Song> songs = new List<Song>();

        public List<Song> Songs { get => songs;}

        public RadioStation(List<Song> songs)
        {
            this.songs = songs;
        }

        public static RadioStation Vibe = new RadioStation(
            new List<Song>
            {
                new Song("I love you sou", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/ILoveUSou.mp3"),
                new Song("Oh my little baby boy", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/OhMyLittleBabyBoy.mp3"),
                new Song("Но он не знает ничего", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/NoOnNeZnaetNichego.mp3"),
                new Song("А пока мне 16", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/PokaMne16.mp3"),
                new Song("Солнышко", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/Solnyshko.mp3")
            });

        public static RadioStation Cool = new RadioStation(
            new List<Song>
            {
               new Song("Monster", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/Monster.mp3"),
               new Song("BFG Division", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/BFGDivision.mp3"),
               new Song("Turbo Killer", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/TurboKiller.mp3")
            });

        public static RadioStation MLG = new RadioStation(
            new List<Song>
            {
                new Song("House", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/House.mp3"),
                new Song("Get blazed", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/GetBlazed.mp3"),
                new Song("My hope will never die", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/MyHopeWillNeverDie.mp3"),
            });

        public static RadioStation Vokaloid = new RadioStation(
            new List<Song>
            {
                new Song("Popipopipopipo", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/Popipopipopipo.mp3"),
                new Song("Stop nagging me", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/StopNaggingMe.mp3"),
                new Song("Around the world", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/AroundTheWorld.mp3"),
                new Song("Triple baka", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/TripleBaka.mp3"),
                new Song("Ochame Kinou", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/OchameKinou.mp3"),
                new Song("Young girl A", "pack://application:,,,/ComputerCompany;component/Resources/Sounds/YoungGirlA.wav")
            });

        public void AddToPlaylist(ObservableCollection<Song> Songs)
        {
            foreach (Song song in this.Songs)
            {
                Songs.Add(song);
            }
        }

        public void RemoveFromPlaylist(ObservableCollection<Song> Songs)
        {
            foreach (Song song in this.Songs)
            {
                Songs.Remove(song);
            }
        }


    }
}
