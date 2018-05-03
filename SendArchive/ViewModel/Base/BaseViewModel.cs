using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SendArchive
{
    /// <summary>
    /// Base class for implementation mvvm
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to implement the interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method of calling an event
        /// </summary>
        /// <param name="propertyName">Method name whence this method is called</param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}