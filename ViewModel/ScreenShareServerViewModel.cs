using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** ScreenShareServerPage.xaml.cs
 *
 * // It will be called when the client clicks on the previous page button
 * // Sets the `SharingScreen` state in the view model to true
 * private void StartSharingButton_Click();
 *
 * // It will be called when the client clicks on the stop sharing button
 * // Sets the `SharingScreen` state in the view model to false
 * private void StopSharingButton_Click();
 */

namespace ViewModel
{
    internal class ScreenShareServerViewModel
    {
        // Keeps track of the current page that the server is viewing
        public int CurrentPage
        {
            get; set;
        }

        // The maximum number of tiles of the shared screens
        // on a single page that will be shown to the server
        public const int MAX_TILES = 9;

        // For each client in the currently active window, it will start a
        // separate thread in which the final processed images of clients
        // will be dequeued and sent to the view
        private List<IP, Thread> currWinClients;

        // Called by `.xaml.cs`
        // Initializes the `ScreenShareServer` model
        ScreenShareServerViewModel() { }

        // It will compute the current window clients from the active list
        // of the subscribers using the pagination logic, and start the
        // thread for these clients using `SendImageToView()` method
        public void ReadImages() { ...}

        // It will read the final processed image of the client and send
        // them to the view. This method will run for an infinite while
        // loop and as soon as it receives an image in the queue, it
        // will dequeue it and send it to the view
        public void SendImageToView() { ...}

        // Update the current page number and call the `Broadcast` method
        // in the server class. Ask the previous clients to stop sending
        // packets and stop their threads. Ask the new clients to start
        // sending their packets
        public void OnPageChange(PageNum) { ...}

        // Mark the client as pinned. Switch to the page of that client
        // and call the `OnPageChange(newPage)` method
        public void OnPin(IP) { ...}
    }
}
