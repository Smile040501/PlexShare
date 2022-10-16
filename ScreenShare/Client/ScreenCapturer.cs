using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Client
{
    internal class ScreenCapturer
    {
        // The queue in which the image will be enqueued after
        // capturing it
        private Queue<Bitmap> _capturedFrame;

        // The task that will be started to capture the screen
        // and to send the packets to the network
        // Created in Capture function
        private Task _captureTask;

        // Called by ScreenShareClient
        // Initialize
        ScreenCapturer() { }

        // Pops and return the image from the queue
        public Bitmap GetImage() { }

        // Only called when client starts screen sharing
        // Called by ScreenShareClient
        // Will have a lambda function - Captures and pushes to the queue
        // Create the task for the lambda function
        public void StartCapture();

        // Called when the server asks to send
        // Create a new task again if not already there
        public void ResumeCapture();

        // Called when the server asks to stop
        // Kill the task and make the variable null
        // Empty the queue
        public void SuspendCapture();

        // Only called when client stops screen sharing
        // Will be called by the ScreenShareClient
        // kill the task and make the task variable null
        // Empty the queue
        public void StopCapture();
    }
}
