using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ScreenShare.Server
{
    internal class SharedClientScreen
    {
        // The screen stitcher object
        private ScreenStitcher _stitcher;

        // The images received from the clients as packets
        private Queue<Frame> _imageQueue;

        // The images which will be received after patching the previous
        // screen image of the client with the new image and
        // ready to be displayed
        private Queue<Image> _finalImageQueue;

        private Task _imageSendTask;

        // Initialize the ScreenSticher object
        // Initialize the timer
        SharedClientScreen(ip, name)
        {
            _stitcher = ScreenStitcher(this);
        }

        // The IP of the client sharing this screen
        public string IP { private get; set; }

        // The name of the client sharing this screen
        public string Name { private get; set; }

        // The current screen image of the client displayed
        public Image CurrentImage { get; set; }

        // Whether the client is pinned or not
        public bool Pinned { get; set; }

        // Timer which keeps track of the time the confirmation packet was
        // received that the client is presenting the screen
        public Timer Timer { get; set; }

        // Pops and returns the image from the `_imageQueue`
        public Bitmap GetImage() { }

        // Insert the image into the `_imageQueue`
        public void PutImage() { }

        // Pops and returns the image from the `_finalImageQueue`
        public Bitmap GetFinalImage() { }

        // Insert the image into the `_finalImageQueue`
        public void PutFinalImage(Bitmap image) { }

        // Calls `sticher.StartStiching`
        // Create (if not exist) and start the task `ImageSendTask` with the lambda function
        // The `CurrentImage` variable will be bind to the xaml file
        public void StartProcessing(lambdaFunction) { }

        // Calls `sticher.StopStiching`
        // Kills the task `ImageSendTask` and mark it as null
        // Empty both the queues
        public void StopProcessing() { }
    }
}
