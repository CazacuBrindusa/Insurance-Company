using proiect_paw_2.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace proiect_paw_2.ViewModels
{
    public class AllClientsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ClientViewModel> Clients { get; set; }

        public AllClientsViewModel()
        {
            Clients = new ObservableCollection<ClientViewModel>();
        }

        public void LoadClients(ObservableCollection<Client> clients)
        {
            Clients.Clear();
            foreach (var client in clients)
            {
                Clients.Add(new ClientViewModel(client));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
