using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Client
{
    // Singleton class
    internal static class ScreenCapturer
    {
        // The queue in which the image will be enqueued after
        // capturing it
        private static Queue<Bitmap> _capturedFrame;

        public static Queue<Bitmap> CapturedFrame
        {
            // get: pops and return the first frame
            get;
            private set;
        }

        // The threads that will be started to capture the screen
        // and to send the packets to the network
        // Created in Capture function
        private static Thread _captureThread;

        // Called by controller
        // Initialize queue
        static ScreenCapturer() { }

        // Only called when client starts screen sharing
        // Called by ScrenController
        // Will have a lambda function - Captures and pushes to the queue
        // Run the lambda function on capture_thread
        // Suspend the thread immediately
        public static void StartCapture();

        // Called when the server asks to stop
        // Suspend the thread
        // Empty the queue
        public static void SuspendCapture();

        // Called when the server asks to send
        // Resume the thread
        public static void ResumeCapture();

        // Only called when client stops screen sharing
        // Will be called by the controller
        // kill the capture thread and make the capture_thread variable null
        // Empty the Queue
        public static void StopCapture();
    }
}
