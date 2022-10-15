using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ScreenShare.Server
{
    internal class SharedScreen
    {
        // The IP of the client sharing this screen
        public string IP { get; set; }

        // The name of the client sharing this screen
        public string Name { get; set; }

        // The images received from the clients as packets
        public Queue<Frame> FrameQueue { get; set; }

        // The images which will be received after patching the previous
        // screen image of the client with the new image and
        // ready to be displayed
        public Queue<Image> FinalImageQueue { get; set; }

        // The current screen image of the client displayed
        public Image CurrentImage { get; set; }

        // Whether the client is pinned or not
        public bool Pinned { get; set; }

        // Timer which keeps track of the time the confirmation packet was
        // received that the client is presenting the screen
        public Timer Timer { get; set; }
    }
}
