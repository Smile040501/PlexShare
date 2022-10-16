using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Server
{
    // Provided by Networking module
    internal class ScreenShareServer : ICommunicatorListener
    {
        // The Networking object to send packets and subscribe to it
        // Initialized in the constructor
        // Will have `SendMessage` and `Subscribe`
        private ICommunicator _communicator;

        // The map between each IP and the shared screen object for
        // all the active subscribers (screen sharers)
        public Map<IP, SharedClientScreen> subscribers;

        // The constructor for this class. It will instantiate the
        // Subscribe the networking by calling `_communicator.Subscribe()`
        ScreenShareServer() { ...}

        // This method will be invoked by the networking team
        // This will be the response packets from the clients
        // Based on the header in the packet received, do further
        // processing as follows:
        /*
            REGISTER     --> RegisterClient(ip)
            DEREGISTER   --> DeregisterClient(ip)
            IMAGE        --> ENQUEUE(ip_shared_screen, image)
            CONFIRMATION --> UpdateTimer(ip)
        */
        public void OnMessageReceived(packet) {}

        // Add this client to the map after calling the constructor
        // Notify the view model to recompute current window clients
        // View model will call the `StartProcessing` for current window clients
        private void RegisterClient(ip) {}

        // Remove this client from the map
        // Calls `StopProcessing` for the client
        // Notify the view model to recompute current window clients
        private void DeregisterClient(ip) {}

        // Calls the `SharedClientScreen.PutImage(image)`
        private void PutImage(ip, image) { }

        // Tell the clients the information about the resolution
        // of the image to be sent and whether to send the image or not
        public void BroadcastClients(clients, bool sendScreen, int numClients) {}

        // Reset the timer for the client with the `OnTimeOut()`
        private void UpdateTimer(ip) {}

        // Callback method for the timer
        // It will de-register the client
        private void OnTimeOut(ip) {}
    }
}
