using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using AppRpgEtec.Views.Personagens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class ListagemPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;

        public ObservableCollection<Personagem> Personagens { get; set; }
        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            Personagens = new ObservableCollection<Personagem>();
            _ = ObterPersonagens();

            NovoPersonagemCommand = new Command(async () => { await ExibirCadastroPersonagem(); });
        }

        public ICommand NovoPersonagemCommand {  get; }

        /* public async Task ObterPersonagens()
         {
             try
             {
                 Personagens = await pService.GetPersonagensAsync();
                 OnPropertyChanged(nameof(Personagens));// informará a View que houve Carregamento
             }
             catch (Exception ex)
             {
                 //Captará o erro para exibir em tela
                 await Application.Current.MainPage.DisplayAlert("Ops", ex.Message +
                         "Detalhes: " + ex.InnerException, "Ok");
             }
         }*/
        public async Task ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync()
                             ?? new ObservableCollection<Personagem>();

                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        /*public async Task ExibirCadastroPersonagem()
        {
            try
            {
                await Shell.Current.GoToAsync("cadPersonagemView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }*/

        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                if (Application.Current?.MainPage is TabbedPage tabbedPage
                    && tabbedPage.CurrentPage is NavigationPage navPage)
                {
                    await navPage.PushAsync(new CadastroPersonagemView());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ops",
                        "Não foi possível abrir a tela de cadastro.",
                        "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

    }
}
