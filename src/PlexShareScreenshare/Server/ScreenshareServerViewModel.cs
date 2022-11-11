﻿/// <author>Mayank Singla</author>
/// <summary>
/// Defines the "ScreenshareServerViewModel" class which represents the
/// view model for screen sharing on the server side machine.
/// </summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace PlexShareScreenshare.Server
{
    /// <summary>
    /// Represents the view model for screen sharing on the server side machine.
    /// </summary>
    public class ScreenshareServerViewModel :
        INotifyPropertyChanged, // Notifies the UX that a property value has changed.
        IMessageListener        // Notifies the UX that subscribers list has been updated.
    {
        /// <summary>
        /// The maximum number of tiles of the shared screens
        /// on a single page that will be shown to the server.
        /// </summary>
        public static readonly int MaxTiles = 9;

        /// <summary>
        /// Acts as a map from the number of screens on the current window to
        /// the number of rows and columns of the grid displayed on the screen.
        /// </summary>
        public static readonly List<(int Row, int Column)> NumRowsColumns = new()
        {
            (1, 1),  // 0 Total Screen
            (1, 1),  // 1 Total Screen
            (1, 2),  // 2 Total Screens
            (1, 3),  // 3 Total Screens
            (2, 2),  // 4 Total Screens
            (2, 3),  // 5 Total Screens
            (2, 3),  // 6 Total Screens
            (3, 3),  // 7 Total Screens
            (3, 3),  // 8 Total Screens
            (3, 3)   // 9 Total Screens
        };

        /// <summary>
        /// Acts as a map from the number of screens on the current window
        /// to the resolution of each screen image inside the grid cell
        /// displayed on the screen.
        /// </summary>
        public static readonly List<(int Height, int Width)> Resolution = new()
        {
            (0, 0),      // 0 Total Screen
            (100, 100),  // 1 Total Screen
            (100, 100),  // 2 Total Screens
            (100, 100),  // 3 Total Screens
            (100, 100),  // 4 Total Screens
            (100, 100),  // 5 Total Screens
            (100, 100),  // 6 Total Screens
            (100, 100),  // 7 Total Screens
            (100, 100),  // 8 Total Screens
            (100, 100)   // 9 Total Screens
        };

        /// <summary>
        /// The only singleton instance for this class.
        /// </summary>
        private static ScreenshareServerViewModel? _instance;

        /// <summary>
        /// Underlying data model.
        /// </summary>
        private readonly ScreenshareServer _model;

        /// <summary>
        /// The current page number that the server is viewing.
        /// </summary>
        private int _currentPage;

        /// <summary>
        /// List of all the clients sharing their screens. This list first contains
        /// the clients which are marked as pinned and then the rest of the clients
        /// in lexicographical order of their name.
        /// </summary>
        private List<SharedClientScreen> _subscribers;

        /// <summary>
        /// Creates an instance of the "ScreenshareServerViewModel" which represents the
        /// view model for screen sharing on the server side. It also instantiates the instance
        /// of the underlying data model.
        /// </summary>
        protected ScreenshareServerViewModel()
        {
            // Get the instance of the underlying data model
            _model = ScreenshareServer.GetInstance(this);

            // Always display the first page initially
            _currentPage = 1;

            // Initialize rest of the fields
            _subscribers = new List<SharedClientScreen>();
            this.CurrentWindowClients = new ObservableCollection<SharedClientScreen>();

            (this.CurrentPageRows, this.CurrentPageColumns) = NumRowsColumns[this.CurrentPage];
            this.CurrentPageResolution = Resolution[this.CurrentPage];

            Trace.WriteLine(Utils.GetDebugMessage("Successfully created an instance for the view model", withTimeStamp: true));
        }

        /// <summary>
        /// Property changed event raised when a property is changed on a component.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies that subscribers list has been changed.
        /// This will happen when a client either starts or stops screen sharing.
        /// </summary>
        /// <param name="subscribers">
        /// Updated list of clients
        /// </param>
        public void OnSubscribersChanged(List<SharedClientScreen> subscribers)
        {
            Debug.Assert(subscribers != null, Utils.GetDebugMessage("Received null subscribers list"));

            // Sort the subscribers in lexicographical order of their name
            List<SharedClientScreen> sortedSubscribers = subscribers
                                                            .OrderBy(subscriber => subscriber.Name)
                                                            .ToList();

            // Move the subscribers marked as pinned to the front of the list
            // keeping the lexicographical order of their name
            _subscribers = MovePinnedSubscribers(sortedSubscribers);

            // Recompute the current window clients to notify the UX
            RecomputeCurrentWindowClients();

            Trace.WriteLine(Utils.GetDebugMessage($"Successfully updated the subscribers list", withTimeStamp: true));
        }

        /// <summary>
        /// Gets the dispatcher to the main thread. In case it is not available
        /// (such as during unit testing) the dispatcher associated with the
        /// current thread is returned.
        /// </summary>
        private static Dispatcher ApplicationMainThreadDispatcher =>
            (Application.Current?.Dispatcher != null) ?
                    Application.Current.Dispatcher :
                    Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Gets the clients which are on the current page.
        /// </summary>
        public ObservableCollection<SharedClientScreen> CurrentWindowClients { get; private set; }

        /// <summary>
        /// Gets the current page that the server is viewing.
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;

            set
            {
                // Update the current page field and notify the UX
                _ = ApplicationMainThreadDispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action<int>((newPage) =>
                        {
                            lock (this)
                            {
                                _currentPage = newPage;
                                this.OnPropertyChanged(nameof(this.CurrentPage));
                            }
                        }),
                        value);

                // Recompute the current window clients to notify the UX
                RecomputeCurrentWindowClients();
            }
        }

        /// <summary>
        /// Gets the current number of rows of the grid displayed on the screen.
        /// </summary>
        public int CurrentPageRows { get; private set; }

        /// <summary>
        /// Gets the current number of columns of the grid displayed on the screen.
        /// </summary>
        public int CurrentPageColumns { get; private set; }

        /// <summary>
        /// Gets the current resolution of the screen image in each
        /// grid cell displayed on the screen.
        /// </summary>
        public (int, int) CurrentPageResolution { get; private set; }

        /// <summary>
        /// Gets a singleton instance of "ScreenshareServerViewModel" class.
        /// </summary>
        /// <returns>
        /// A singleton instance of "ScreenshareServerViewModel"
        /// </returns>
        public static ScreenshareServerViewModel GetInstance()
        {
            // Create a new instance if it was null before
            _instance ??= new();
            return _instance;
        }

        /// <summary>
        /// Moves the subscribers marked as pinned to the front of the list
        /// keeping the lexicographical order of their name.
        /// </summary>
        /// <param name="subscribers">
        /// Input list of subscribers.
        /// </param>
        /// <returns>
        /// List of subscribers with pinned subscribers in front.
        /// </returns>
        private static List<SharedClientScreen> MovePinnedSubscribers(List<SharedClientScreen> subscribers)
        {
            Debug.Assert(subscribers != null, Utils.GetDebugMessage("Received null subscribers list"));

            // Separate pinned and unpinned subscribers
            List<SharedClientScreen> pinnedSubscribers = new();
            List<SharedClientScreen> unpinnedSubscribers = new();

            foreach (SharedClientScreen subscriber in subscribers)
            {
                if (subscriber.Pinned)
                {
                    pinnedSubscribers.Add(subscriber);
                }
                else
                {
                    unpinnedSubscribers.Add(subscriber);
                }
            }

            // Join both the lists with pinned subscribers followed by the unpinned ones
            return pinnedSubscribers.Concat(unpinnedSubscribers).ToList();
        }

        /// <summary>
        /// Recomputes current window clients using the pagination logic
        /// and notifies the UX. It also notifies the old and new clients
        /// about the new status of sending image packets.
        /// </summary>
        public void RecomputeCurrentWindowClients()
        {
            Debug.Assert(_subscribers != null, Utils.GetDebugMessage("_subscribers is found null"));

            // Total count of all the subscribers sharing screen
            int totalCount = _subscribers.Count;

            // Get the count of subscribers to skip for the current page
            int countToSkip = GetCountToSkip();

            int remainingCount = totalCount - countToSkip;

            // Number of subscribers that will show up on the current page
            int limit = _subscribers[countToSkip].Pinned ? 1 : Math.Min(remainingCount, MaxTiles);

            // Get the new window clients to be displayed on the current page
            List<SharedClientScreen> newWindowClients = _subscribers.GetRange(countToSkip, limit);
            int numNewWindowClients = newWindowClients.Count;

            // Save the previous window clients which are not there in the current window
            List<SharedClientScreen> previousWindowClients = this.CurrentWindowClients.ToList();
            previousWindowClients = previousWindowClients
                                    .Where(client => newWindowClients.FindIndex(n => n.Id == client.Id) == -1)
                                    .ToList();

            // The new number of rows and columns to be displayed based on new number of clients
            var (newNumRows, newNumCols) = NumRowsColumns[numNewWindowClients];

            // The new resolution of screen image to be displayed based on new number of clients
            (int, int) newResolution = Resolution[numNewWindowClients];

            // Update all the fields and notify the UX
            _ = ApplicationMainThreadDispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action<List<SharedClientScreen>, int, int, (int, int)>((newCurrentWindowClients, numNewRows, numNewCols, newRes) =>
                        {
                            lock (this)
                            {
                                // Note, to update the whole list, we can't simply assign it equal
                                // to the new list. We need to clear the list first and add new elements
                                // into the list to be able to see the effect on the UI.
                                this.CurrentWindowClients.Clear();
                                foreach (SharedClientScreen screen in newCurrentWindowClients)
                                {
                                    this.CurrentWindowClients.Add(screen);
                                }

                                this.CurrentPageRows = numNewRows;
                                this.CurrentPageColumns = numNewCols;
                                this.CurrentPageResolution = newRes;

                                this.OnPropertyChanged(nameof(this.CurrentWindowClients));
                                this.OnPropertyChanged(nameof(this.CurrentPageRows));
                                this.OnPropertyChanged(nameof(this.CurrentPageColumns));
                                this.OnPropertyChanged(nameof(this.CurrentPageResolution));
                            }
                        }),
                        newWindowClients, newNumRows, newNumCols, newResolution);

            // Notifies the old and new clients about the status of sending image packets.
            NotifySubscribers(previousWindowClients, newWindowClients, newResolution);

            Trace.WriteLine(Utils.GetDebugMessage($"Successfully recomputed current window clients for the page {this.CurrentPage}", withTimeStamp: true));
        }

        /// <summary>
        /// Mark the client as pinned and switch to the page of that client.
        /// </summary>
        /// <param name="clientId">
        /// Id of the client which is marked as pinned
        /// </param>
        public void OnPin(string clientId)
        {
            Debug.Assert(clientId != null, Utils.GetDebugMessage("Received null client id"));
            Debug.Assert(_subscribers != null, Utils.GetDebugMessage("_subscribers is found null"));

            // Find the index of the client
            int pinnedScreenIdx = _subscribers.FindIndex(subscriber => subscriber.Id == clientId);

            Debug.Assert(pinnedScreenIdx != -1, Utils.GetDebugMessage($"Client Id: {clientId} not found in the subscribers list"));

            if (pinnedScreenIdx != -1)
            {
                // Mark the client as pinned and switch to the page of the client
                SharedClientScreen pinnedScreen = _subscribers[pinnedScreenIdx];
                pinnedScreen.Pinned = true;
                this.CurrentPage = GetClientPage(pinnedScreen.Id);

                Trace.WriteLine(Utils.GetDebugMessage($"Successfully pinned the client with id: {clientId}", withTimeStamp: true));
            }
        }

        /// <summary>
        /// Mark the client as pinned and switch to the previous (or the first) page.
        /// </summary>
        /// <param name="clientId">
        /// Id of the client which is marked as unpinned
        /// </param>
        public void OnUnpin(string clientId)
        {
            Debug.Assert(clientId != null, Utils.GetDebugMessage("Received null client id"));
            Debug.Assert(_subscribers != null, Utils.GetDebugMessage("_subscribers is found null"));

            // Find the index of the client
            int unpinnedScreenIdx = _subscribers.FindIndex(subscriber => subscriber.Id == clientId);

            Debug.Assert(unpinnedScreenIdx != -1, Utils.GetDebugMessage($"Client Id: {clientId} not found in the subscribers list"));

            if (unpinnedScreenIdx != -1)
            {
                // Mark the client as unpinned and switch to the previous (or the first) page
                SharedClientScreen unpinnedScreen = _subscribers[unpinnedScreenIdx];
                unpinnedScreen.Pinned = false;
                this.CurrentPage = Math.Max(1, this.CurrentPage - 1);

                Trace.WriteLine(Utils.GetDebugMessage($"Successfully unpinned the client with id: {clientId}", withTimeStamp: true));
            }
        }

        /// <summary>
        /// Notify the previous/new window clients to stop/send their image packets.
        /// It also asks the previous/new window clients to stop/start their image processing.
        /// </summary>
        /// <param name="prevWindowClients">
        /// List of clients which were there in the previous window
        /// </param>
        /// <param name="currentWindowClients">
        /// List of clients which are there in the current window
        /// </param>
        /// <param name="resolution">
        /// Resolution of the image to be sent by the current window clients
        /// </param>
        private void NotifySubscribers(List<SharedClientScreen> prevWindowClients, List<SharedClientScreen> currentWindowClients, (int, int) resolution)
        {
            Debug.Assert(_model != null, Utils.GetDebugMessage("_model is found null"));

            // Ask all the current window clients to start sending image packets with the specified resolution
            _model.BroadcastClients(currentWindowClients
                                    .Select(client => client.Id)
                                    .ToList(), nameof(ServerDataHeader.Send), resolution);

            // Ask all the current window clients to start processing their images
            foreach (SharedClientScreen client in currentWindowClients)
            {
                // The lambda function takes the final image from the final image queue
                // of the client and set it as the "CurrentImage" variable for the client
                // and notify the UX about the same
                client.StartProcessing(new Action<CancellationToken>((token) =>
                {
                    // If the task was already canceled
                    token.ThrowIfCancellationRequested();

                    // Loop till the task is not canceled
                    while (!token.IsCancellationRequested)
                    {
                        // End the task when cancellation is requested
                        token.ThrowIfCancellationRequested();

                        // Update the current image of the client on the screen
                        // by taking the processed images from its final image queue
                        _ = ApplicationMainThreadDispatcher.BeginInvoke(
                                DispatcherPriority.Normal,
                                new Action<Bitmap>((image) =>
                                {
                                    lock (client)
                                    {
                                        client.CurrentImage = image;
                                        this.OnPropertyChanged(nameof(this.CurrentWindowClients));
                                    }
                                }),
                                client.GetFinalImage());
                    }
                }));
            }

            Trace.WriteLine(Utils.GetDebugMessage("Successfully notified the new current window clients", withTimeStamp: true));

            // Ask all the previous window clients to stop sending image packets
            _model.BroadcastClients(prevWindowClients
                                    .Select(client => client.Id)
                                    .ToList(), nameof(ServerDataHeader.Stop), Resolution[0]);

            // Ask all the previous window clients to stop processing their images
            foreach (SharedClientScreen client in prevWindowClients)
            {
                client.StopProcessing();
            }

            Trace.WriteLine(Utils.GetDebugMessage("Successfully notified the previous window clients", withTimeStamp: true));
        }

        /// <summary>
        /// Computes the number of subscribers to skip up to current page.
        /// </summary>
        /// <returns>
        /// Returns the number of subscribers to skip up to current page
        /// </returns>
        private int GetCountToSkip()
        {
            Debug.Assert(_subscribers != null, Utils.GetDebugMessage("_subscribers is found null"));

            int countToSkip = 0;
            for (int i = 0; i < this.CurrentPage; ++i)
            {
                // The first screen on the page "i"
                SharedClientScreen screen = _subscribers[countToSkip];
                if (screen.Pinned)
                {
                    // If the screen is pinned, skip by one
                    ++countToSkip;
                }
                else
                {
                    // If screen is not pinned, then skip by max number of tiles that are displayed on one page
                    countToSkip += MaxTiles;
                }
            }
            return countToSkip;
        }

        /// <summary>
        /// Compute the page of the client on which the client screen is displayed.
        /// </summary>
        /// <param name="clientId">
        /// The client Id whose page is to be found
        /// </param>
        /// <returns></returns>
        private int GetClientPage(string clientId)
        {
            Debug.Assert(_subscribers != null, Utils.GetDebugMessage("_subscribers is found null"));

            // Total count of all the subscribers
            int totalSubscribers = _subscribers.Count;

            // Index of the first subscriber on the page
            int startSubscriberIdx = 0;

            // Current page number
            int pageNum = 1;

            // Loop to the page of the client
            while (startSubscriberIdx < totalSubscribers)
            {
                SharedClientScreen screen = _subscribers[startSubscriberIdx];
                if (screen.Pinned)
                {
                    if (screen.Id == clientId) return pageNum;

                    // If the screen is pinned, skip by one
                    ++startSubscriberIdx;
                }
                else
                {
                    // Number of clients on the current page
                    int limit = Math.Min(MaxTiles, totalSubscribers - startSubscriberIdx);

                    // Check if the client is on the current page
                    int clientIdx = _subscribers
                                .GetRange(startSubscriberIdx, limit)
                                .FindIndex(sub => sub.Id == clientId);
                    if (clientIdx >= 0) return pageNum;

                    // If screen is not pinned, then skip by max number of tiles that are displayed on one page
                    startSubscriberIdx += MaxTiles;
                }

                // Go to next page
                ++pageNum;
            }

            Debug.Assert(false, Utils.GetDebugMessage($"Page of the client with id: {clientId} not found"));

            // Switch to the first page in case client page can not be found
            return 1;
        }

        /// <summary>
        /// Handles the property changed event raised on a component.
        /// </summary>
        /// <param name="property">
        /// The name of the property that is changed
        /// </param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
