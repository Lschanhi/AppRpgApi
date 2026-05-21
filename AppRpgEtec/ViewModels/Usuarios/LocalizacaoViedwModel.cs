using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class LocalizacaoViewModel : BaseViewModel
    {
        private Map meuMapa;

        private UsuarioService uService;
        public Map MeuMapa
        {
            get => meuMapa;
            set
            {
                if (meuMapa != value)
                {
                    meuMapa = value;
                    OnPropertyChanged();
                }
            }
        }

        public LocalizacaoViewModel()
        {
            InicializarMapa();

            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
        }

        public async void InicializarMapa()
        {
            try
            {
                Location location = new Location(-23.5200241d, -46.596498d);

                Pin pinEtec = new Pin
                {
                    Type = PinType.Place,
                    Label = "Etec Horácio",
                    Address = "Rua Alcantara, 113, Vila Guilherme",
                    Location = location
                };

                Map map = new Map
                {
                    MapType = MapType.Street,
                    IsShowingUser = true,
                    IsZoomEnabled = true
                };

                MapSpan mapSpan = MapSpan.FromCenterAndRadius(
                    location,
                    Distance.FromKilometers(5)
                );

                map.Pins.Add(pinEtec);
                map.MoveToRegion(mapSpan);

                MeuMapa = map;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
        }

        public async void ExibirUsuariosNoMapa()
        {
            try
            {
                ObservableCollection<Usuario> ocUsuarios = 
                    await uService.GetUsuariosAsync();
                List<Usuario> listaUsuarios = new List<Usuario>(ocUsuarios);
                Map map = new Map();

                foreach(Usuario u in listaUsuarios)
                {
                    if(u.Latitude != null && u.Longitude != null)
                    {
                        double latitude = (double)u.Latitude;
                        double longitude = (double)u.Longitude;
                        Location location = new Location(latitude, longitude);

                        Pin pinAtual = new Pin()
                        {
                                Type = PinType.Place,
                                Label = u.Username,
                                Address = $"E-Mail: {u.Email}",
                                Location = location
                        };

                        map.Pins.Add(pinAtual);
                    }
                }

                MeuMapa = map;
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Erro", ex.Message, "Ok");
            }
        }
    }
}