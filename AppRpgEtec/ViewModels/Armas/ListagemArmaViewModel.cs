using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.Threading.Tasks;

namespace AppRpgEtec.ViewModels.Armas
{
    public class ListagemArmaViewModel : BaseViewModel
    {

        private ArmasService aService;

        public ObservableCollection<Arma> Armas { get; set; }
        public ListagemArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmasService(token);
            Armas = new ObservableCollection<Arma>();
            _ = ObterArmas();
        }

        public async Task ObterArmas()
        {
            try
            {
                Armas = await aService.GetArmasAsync();
                OnPropertyChanged(nameof(Armas));// informará a View que houve Carregamento
            }
            catch (Exception ex)
            {
                //Captará o erro para exibir em tela
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message +
                        "Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
