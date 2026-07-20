using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerCompanyClasses
{
    public class Song
    {
        private readonly string title;
        private readonly string uri;
        private string playOrPause;

        public string Title { get => title; }

        public string URI { get => uri; }
        public string PlayOrPause { get => playOrPause; }

        public Song(string Title, string URI)
        {
            playOrPause = "▷";

            this.title = Title;
            this.uri = URI;
        }

        public void ChangePlayStatus()
        {
            playOrPause = PlayOrPause == "▷" ? "⏸" : "▷";
        }

        public void ChangePlayStatus(bool status)
        {
            playOrPause = status ? "⏸" : "▷";
        }
    }
}
