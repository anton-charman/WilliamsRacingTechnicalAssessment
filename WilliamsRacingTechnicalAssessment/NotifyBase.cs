using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WilliamsRacingTechnicalAssessment
{
    /// <summary>
    /// Extracted boilerplate code for implementing INotifyPropertyChanged.
    /// </summary>
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event when the input property name is changed. 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Syntactic sugar for simultaneously setting a field and raising the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <param name="val"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val))
                return false;

            member = val;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
