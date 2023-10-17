using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WilFitApp.Core
{
    public class ObservbleObject : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
        
    }
}
