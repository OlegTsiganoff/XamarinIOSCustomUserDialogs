using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CustomUserDialogsIOS.ViewModels
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                try
                {
                    eventHandler(this, new PropertyChangedEventArgs(propertyName));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("OnPropertyChanged error: " + ex.Message);
                }
            }
        }


    }
}
