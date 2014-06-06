using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO;

namespace WumpusXNA
{
    class Sound
    {
        public Sound()
        {
        }

        public void playEffect(String name)
        {
            WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
            string path = Directory.GetCurrentDirectory() + @"\" + name + ".wav";
            player.URL = path;
        }

        public void playLoop(String name)
        {
            SoundPlayer player = new SoundPlayer(Directory.GetCurrentDirectory() + @"\" + name + ".wav");
            player.PlayLooping();
        }
    }
}
