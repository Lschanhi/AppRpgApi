using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using AppRpgEtec.Views.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class ListagemPersonagemViewModel : BaseViewModel
    {
        private readonly PersonagemService pService;
        private Personagem personagemSelecionado;

        public ObservableCollection<Personagem> Personagens { get; set; }

        public ICommand NovoPersonagemCommand { get; }
        public ICommand RemoverPersonagemCommand { get; set; }
        public ICommand ZerarRankingRestaurarVidasGeralCommand { get; set; }

        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);

            pService = new PersonagemService(token);
            Personagens = new ObservableCollection<Personagem>();

            _ = ObterPersonagens();

            NovoPersonagemCommand =
                new Command(async () => await ExibirCadastroPersonagem());

            RemoverPersonagemCommand =
                new Command<Personagem>(async (Personagem p) => await RemoverPersonagem(p));

            ZerarRankingRestaurarVidasGeralCommand =
                new Command(async () => await ZerarRankingRestaurarVidasGeral());
        }

        public Personagem PersonagemSelecionado
        {
            get => personagemSelecionado;
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    OnPropertyChanged();

                    _ = ExibirOpcoesAsync(personagemSelecionado);
                }
            }
        }

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

        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                await Shell.Current.GoToAsync("cadPersonagemView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        public async Task RemoverPersonagem(Personagem p)
        {
            try
            {
                if (p == null)
                    return;

                if (await Application.Current.MainPage.DisplayAlert(
                    "Confirmação",
                    $"Confirmar a remoção de {p.Nome}?",
                    "Sim",
                    "Não"))
                {
                    await pService.DeletePersonagemAsync(p.Id);

                    await Application.Current.MainPage.DisplayAlert(
                        "Mensagem",
                        "Personagem removido com sucesso.",
                        "Ok");

                    await ObterPersonagens();
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

        public async Task ExecutarRestaurarPontosPersonagem(Personagem p)
        {
            await pService.PutRestaurarPontosAsync(p);
        }

        public async Task ExecutarZerarRankingPersonagem(Personagem p)
        {
            await pService.PutZerarRankingAsync(p);
        }

        public async Task ExecutarZerarRankingRestaurarVidasGeral()
        {
            await pService.PutZerarRankingRestaurarVidasGeralAsync();
        }

        public async void ProcessarOpcaoRespondidaAsync(Personagem personagem, string result)
        {
            if (string.IsNullOrEmpty(result) || result == "Cancelar" || personagem == null)
                return;

            if (result.Equals("Editar Personagem"))
            {
                await Shell.Current.GoToAsync($"cadPersonagemView?pId={personagem.Id}");
            }
            else if (result.Equals("Remover Personagem"))
            {
                await RemoverPersonagem(personagem);
            }
            else if (result.Equals("Restaurar Pontos de Vida"))
            {
                if (await Application.Current.MainPage.DisplayAlert(
                    "Confirmação",
                    $"Restaurar os pontos de vida de {personagem.Nome.ToUpper()}?",
                    "Yes",
                    "No"))
                {
                    await ExecutarRestaurarPontosPersonagem(personagem);

                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Os pontos foram restaurados com sucesso.",
                        "Ok");

                    await ObterPersonagens();
                }
            }
            else if (result.Equals("Zerar Ranking do Personagem"))
            {
                if (await Application.Current.MainPage.DisplayAlert(
                    "Confirmação",
                    $"Zerar o ranking de {personagem.Nome.ToUpper()}?",
                    "Yes",
                    "No"))
                {
                    await ExecutarZerarRankingPersonagem(personagem);

                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "O ranking foi zerado com sucesso.",
                        "Ok");

                    await ObterPersonagens();
                }
            }
        }

        public async Task ExibirOpcoesAsync(Personagem personagem)
        {
            try
            {
                personagemSelecionado = null;
                OnPropertyChanged(nameof(PersonagemSelecionado));

                string result;

                if (personagem.PontosVida > 0)
                {
                    result = await Application.Current.MainPage.DisplayActionSheet(
                        "Opções para o personagem " + personagem.Nome,
                        "Cancelar",
                        null,
                        "Editar Personagem",
                        "Restaurar Pontos de Vida",
                        "Zerar Ranking do Personagem",
                        "Remover Personagem");
                }
                else
                {
                    result = await Application.Current.MainPage.DisplayActionSheet(
                        "Opções para o personagem " + personagem.Nome,
                        "Cancelar",
                        null,
                        "Restaurar Pontos de Vida");
                }

                ProcessarOpcaoRespondidaAsync(personagem, result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops...",
                    ex.Message,
                    "Ok");
            }
        }

        public async Task ZerarRankingRestaurarVidasGeral()
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert(
                    "Confirmação",
                    "Deseja realmente zerar todo o ranking e restaurar todas as vidas?",
                    "Yes",
                    "No"))
                {
                    await ExecutarZerarRankingRestaurarVidasGeral();

                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Ranking zerado e vidas restauradas com sucesso.",
                        "Ok");

                    await ObterPersonagens();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops...",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }
    }
}