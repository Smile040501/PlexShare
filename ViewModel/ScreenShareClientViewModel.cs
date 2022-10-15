using System;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

/** ScreenShareClientPage.xaml.cs
 *
 * // It will be called when the client clicks on the start sharing button
 * // Sets the `SharingScreen` state in the view model to true
 * private void StartSharingButton_Click();
 *
 * // It will be called when the client clicks on the stop sharing button
 * // Sets the `SharingScreen` state in the view model to false
 * private void StopSharingButton_Click();
 */

namespace ViewModel
{
    public class ScreenShareClientViewModel :
        INotifyPropertyChanged
    {
        // Whether the client is sharing screen or not
        private bool _sharingScreen;

        // Underlying data model
        private ScreenShareClient _model;

        // Property changed event raised when a property is changed on a component
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the dispatcher to the main thread. In case it is not available
        /// (such as during unit testing) the dispatcher associated with the
        /// current thread is returned.
        /// </summary>
        private Dispatcher ApplicationMainThreadDispatcher =>
            (Application.Current?.Dispatcher != null) ?
                    Application.Current.Dispatcher :
                    Dispatcher.CurrentDispatcher;

        public bool SharingScreen
        {
            get => _sharingScreen;

            set
            {
                // Execute the call on the application's main thread.
                //
                // Also note that we may execute the call asynchronously as the calling
                // thread is not dependent on the callee thread finishing this method call.
                // Hence we may call the dispatcher's BeginInvoke method which kicks off
                // execution asynchronously as opposed to Invoke which does it synchronously.

                _ = this.ApplicationMainThreadDispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                lock (this)
                                {
                                    this._sharingScreen = value;
                                    this.OnPropertyChanged("SharingScreen");
                                }
                            }));

                if (value)
                {
                    _model.StartScreenSharing();
                }
                else
                {
                    _model.StopScreenSharing();
                }
            }
        }

        // Called by `.xaml.cs` file
        // Initializes the `ScreenShareClient` model
        ScreenShareClientViewModel() { }

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