using proiect_paw_2.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace proiect_paw_2.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        public Client _client;

        public ClientViewModel(Client client)
        {
            _client = client;
        }

        public int Client_Id
        {
            get => _client.Client_Id;
            set
            {
                if (_client.Client_Id != value)
                {
                    _client.Client_Id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _client.Name;
            set
            {
                if (_client.Name != value)
                {
                    _client.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Birthday
        {
            get => _client.Birthday;
            set
            {
                if (_client.Birthday != value)
                {
                    _client.Birthday = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
