using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Client
{
    // Provided by Networking module
    internal class ScreenShareClient : ICommunicatorListener
    {

        // The Networking object to send packets and subscribe to it
        // Initialized in the constructor
        // Will have `SendMessage` and `Subscribe`
        private ICommunicator _communicator;

        // The tasks to send the packets to the network
        // Will be created by `SendImagePacket()` and `SendConfirmationPacket` methods
        private Task _sendImageTask, _sendConfirmationTask;

        private ScreenCapturer _capturer;
        private ScreenProcessor _processor;

        // Called by view model
        // Subscribe the networking by calling `_communicator.Subscribe()`
        // Creates the objects of `ScreenCapturer` and `ScreenProcessor`
        ScreenShareClient() {}

        // This method will be called by the ViewModel
        // Sends register request to the server by calling `_communicator.SendMessage()`
        // Call the methods `StartImageSending`, `SendConfirmationPacket`
        // Call the methods `_capturer.StartCapturing()` and `_processor.StartProcessing()`
        public void StartScreenSharing() {}

        // This method will be called by the ViewModel
        // Sends de-register request to the server by calling `_communicator.SendMessage()`
        // Calls the method `StopImageSending`
        // Calls the method `capturer.stop` and `processor.stop`
        // Kill the `_sendConfirmationTask` task
        public void StopScreenSharing() {}

        // From the interface
        // Receive Packets from the networking
        // This method will be invoked by the networking team
        // This will be the request/response packets from the server
        /*
         * STOP: SuspendSending()
         * SEND: ResumeSending(), SetNewResolution()
         */
        void OnMessageReceived(packet) {}

        // This method will create the task `_sendImageTask` for the lambda function
        // The lambda function will read the image from the _processor queue using GetImage
        // and start sending those images to the server using `_communicator.SendMessage`
        private void StartImageSending() {}

        // Kill the task `_sendImageTask` using cancellation token
        private void SuspendImageSending() { }

        // Create the task `_sendImageTask` only if it is null
        // Start the task
        private void ResumeImageSending() { }

        // Kill the task `_sendImageTask`
        private void StopImageSending() { }

        // This method will create and start the task which it will keep on
        // sending the packet having the header as confirmation using `_communicator.SendMessage`
        // to let the server and the dashboard know that the client is not
        // disconnected and is still presenting the screen.
        public void SendConfirmationPacket() {}

        public void SetNewResolution()
        {
            // update the new resolution to be used by the Processor
            // processor.setNewResolution()
        }
    }
}
