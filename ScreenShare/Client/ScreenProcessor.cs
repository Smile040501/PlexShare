using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Client
{
    // Singleton class
    internal class ScreenProcessor
    {
        // The queue in which the image will be enqueued after
        // processing it
        /*
         * Frame ->
         * After Processing : send ->
         * list of : ( (new_res) , list of : {x, y, (R,G,B)} )
         *
         */
        private Queue<Frame> _processedFrame;

        // Processing task
        private Task _processorTask;

        // The screen capturer object
        private ScreenCapturer _capturer;

        private Pair<int, int> OldRes { get; set; }   // Do we need this?
        public Pair<int, int> NewRes { private get; set; }

        // Called by ScreenShareClient
        // Initialize queue, oldRes, newRes
        // Called by ScreenShareController
        ScreenProcessor(ScreenCapturer capturer) { }

        // Pops and return the image from the queue
        public Bitmap GetImage() { }

        // Called by ScreenShareClient when the client starts screen sharing
        // Will have a lambda function - Process and pushes to the queue
        // Create the task for the lambda function
        public void StartProcessing();

        // Called when the server asks to stop
        // Kill the task
        // Empty the queue
        public void SuspendProcessing();

        // Called when the server asks to send
        // Resume the thread
        public void ResumeProcessing();

        // Called by ScreenShareClient when the client stops screen sharing
        // kill the processor task and make the processor task variable null
        // Empty the Queue
        public void StopProcessing();

        // Called by StartProcessing
        // run the compression algorithm and returns list of changes in pixels
        // Does it need to be public?
        public void Compress();
    }
}
