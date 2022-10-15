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
        // all the active subscribers
        public Map<IP, SharedScreen> subscribers;

        // The constructor for this class. It will instantiate the
        // Subscribe the networking by calling `_communicator.Subscribe()`
        public ScreenShareServer() { ...}

        // This method will be invoked by the networking team
        // This will be the response packets from the clients
        // Based on the header in the packet received, do further
        // processing as follows:
        /*
            REGISTER    --> RegisterClient(ip)
            DEREGISTER  --> DeregisterClient(ip)
            IMAGE        --> Stitch(ip, converted_image)
            CONFIRMATION --> UpdateTimer(ip)
        */
        public void OnMessageReceived(packet) {}

        // Add this client to the map
        private void RegisterClient(ip) {}

        // Remove this client from the map
        private void DeregisterClient(ip) {}

        // Tell the clients the information about the resolution
        // of the image to be sent and whether to send the image or not
        // Functionality Ambiguity?
        public void BroadcastClients(CurrentWindowClients) {}

        // Start/Reset the timer for the client with the `OnTimeOut()`
        private void UpdateTimer(ip) {}

        // Callback method for the timer. It will unsubscribe the client.
        private void OnTimeOut(ip) {}
    }
}
