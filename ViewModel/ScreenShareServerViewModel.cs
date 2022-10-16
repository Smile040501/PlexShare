using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** ScreenShareServerPage.xaml.cs
 *
 * // It will be called when the admin clicks on the previous page button
 * // Decrements the `CurrentPage` state in the view model
 * private void PreviousPageButton_Click();
 *
 * // It will be called when the admin clicks on the next page button
 * // Increments the `CurrentPage` state in the view model
 * private void NextPageButton_Click();
 *
 * // It will be called when the admin clicks on PIN button for a user
 * // Will read the `value` attribute which we will set as IP of the user or username
 * // Calls the `OnPin(ip)` method in the view model
 * private void PinButton_Click();
 *
 * // It will be called when the admin clicks on UNPIN button for a user
 * // Will read the `value` attribute which we will set as IP of the user or username
 * // Calls the `OnUnpin(ip)` method in the view model
 * private void UnpinButton_Click();
 */

namespace ViewModel
{
    internal class ScreenShareServerViewModel
    {
        // The current page number
        private int _currentPage;

        // The maximum number of tiles of the shared screens
        // on a single page that will be shown to the server
        public const int MAX_TILES = 9;

        // Underlying data model
        private ScreenShareServer _model;

        // Property changed event raised when a property is changed on a component
        public event PropertyChangedEventHandler PropertyChanged;

        // For each client in the currently active window, it will start a
        // separate thread in which the final processed images of clients
        // will be dequeued and sent to the view
        public List<ClientSharedScreen> CurrWinClients { get; private set; }

        // Keeps track of the current page that the server is viewing
        public int CurrentPage
        {
            get => _currentPage;

            set
            {
                // Update the field `_currentPage`
                // Recompute the field `currWinClients` using the pagination logic
            }
        }

        // Call Model for ->
        // Ask the previous clients to stop sending packets using `_model.BroadCastClients`
        // and call `StopProcessing` on them
        // Ask the new clients to start sending their packets using `_model.BroadCastClients`
        // and call `StartProcessing` on them with the below lambda function
        // The lambda function will take the image from the finalImageQueue
        // and set it as the `CurrentImage` variable
        public void RecomputeCurrWinClients() { }

        // Called by `.xaml.cs`
        // Initializes the `ScreenShareServer` model
        ScreenShareServerViewModel() { }

        // Mark the client as pinned. Switch to the page of that client
        // by setting the `CurrentPage` to new page of the pinned client
        public void OnPin(IP) { }

        // Mark the client as unpinned
        // Switch to the max(new last page, next page)
        public void OnUnpin(IP) { }

        /// <summary>
        /// Handles the property changed event raised on a component.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
