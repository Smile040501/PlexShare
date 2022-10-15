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

        // The threads to send the packets to the network
        // Will be made by `SendImagePacket()` and `SendConfirmationPacket` methods
        private Thread _sendImageThread, _sendConfirmationThread;

        // Called by view model
        // Subscribe the networking by calling `_communicator.Subscribe()`
        ScreenShareClient() {}

        // This method will be called by the ViewModel
        // Sends register request to the server by calling `_communicator.SendMessage()`
        // Call the methods `SendImage`, `SendConfirmationPacket` and `Contoller.StartSharing`
        public void StartScreenSharing() {}

        // This method will be called by the ViewModel
        // Sends de-register request to the server by calling `_communicator.SendMessage()`
        // Calls the method `StopSending` and `Controller.StopSharing()`
        // Kill the `sendConfirmationThread`
        public void StopScreenSharing() {}

        // From the interface
        // Receive Packets from the networking
        // This method will be invoked by the networking team
        // This will be the request/response packets from the server
        /*
         * STOP: Controller.SuspendSharing and SuspendSending
         * SEND: Controller.ResumeSharing and ResumeSending
         */
        void OnMessageReceived(packet) {}

        // This method will start the thread `sendImageThread` for the lambda function
        // The lambda function will read the processed frames from that queue
        // and start sending those images to the server using `_communicator.SendMessage`
        // After creating the thread, Suspend the thread immediately
        private void SendImage() {}

        // Suspend the `sendImageThread`
        private void SuspendSending() { }

        // Resume the thread `sendImageThread`
        private void ResumeSending() { }

        // Stop the thread `sendImageThread`
        private void StopSending() { }

        // This method will start the thread which it will keep on
        // sending the packet having the header as confirmation using `_communicator.SendMessage`
        // to let the server and the dashboard know that the client is not
        // disconnected and is still presenting his screen.
        public void SendConfirmationPacket() {}
    }
}
