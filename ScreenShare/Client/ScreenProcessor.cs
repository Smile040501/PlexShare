using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Client
{
    // Singleton class
    internal static class ScreenProcessor
    {
        // The queue in which the image will be enqueued after
        // processing it
        /*
         * Frame ->
         * After Processing : send ->
         * list of : ( (new_res) , list of : {x, y, (R,G,B)} )
         *
         */
        private static Queue<Frame> _processedFrame;

        public static Queue<Frame> ProcessedFrame
        {
            // Get pops and return the first frame
            get;
            private set;
        }

        // Processing thread
        private static Thread _processorThread;

        public static Pair<int, int> OldRes { get; set; }   // Do we need this?
        public static Pair<int, int> NewRes { get; set; }

        // Called by controller
        // Initialize queue, oldRes, newRes
        // Called by ScreenShareController
        static ScreenProcessor() { }

        // Called by controller when the client starts screen sharing
        // Will have a lambda function - Process and pushes to the queue
        // Run the lambda function on processed_thread
        // Suspend the thread immediately
        public static void StartProcessing();

        // Called when the server asks to stop
        // Suspend the thread
        // Empty the queue
        public static void SuspendProcessing();

        // Called when the server asks to send
        // Resume the thread
        public static void ResumeProcessing();

        // Called by controller when the client stops screen sharing
        // kill the processor thread and make the processor_thread variable null
        // Empty the Queue
        public static void StopProcessing();

        // Called by StartProcessing
        // run the compression algorithm and returns list of changes in pixels
        // Does it need to be public?
        public static void Compress();
    }
}
