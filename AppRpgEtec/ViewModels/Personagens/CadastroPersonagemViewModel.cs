using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    [QueryProperty(nameof(PersonagemSelecionadoId), "pId")]
    public class CadastroPersonagemViewModel : BaseViewModel
    {
        private readonly PersonagemService pService;

        private int id;
        private string nome;
        private int pontosVida;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;
        private string personagemSelecionadoId;

        private ObservableCollection<TipoClasse> listaTiposClasse;
        private TipoClasse tipoClasseSelecionado;

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; set; }

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);

            pService = new PersonagemService(token);

            ListaTiposClasse = new ObservableCollection<TipoClasse>();
            _ = ObterClasses();

            SalvarCommand =
                new Command(async () => await SalvarPersonagem(), () => ValidarCampos());

            CancelarCommand =
                new Command(async () => await CancelarCadastro());
        }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        public int PontosVida
        {
            get => pontosVida;
            set
            {
                pontosVida = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CadastroHabilitado));
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        public int Forca
        {
            get => forca;
            set
            {
                forca = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        public int Defesa
        {
            get => defesa;
            set
            {
                defesa = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        public int Inteligencia
        {
            get => inteligencia;
            set
            {
                inteligencia = value;
                OnPropertyChanged();
            }
        }

        public int Disputas
        {
            get => disputas;
            set
            {
                disputas = value;
                OnPropertyChanged();
            }
        }

        public int Vitorias
        {
            get => vitorias;
            set
            {
                vitorias = value;
                OnPropertyChanged();
            }
        }

        public int Derrotas
        {
            get => derrotas;
            set
            {
                derrotas = value;
                OnPropertyChanged();
            }
        }

        public string PersonagemSelecionadoId
        {
            get => personagemSelecionadoId;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    personagemSelecionadoId = Uri.UnescapeDataString(value);
                    _ = CarregarPersonagem();
                }
            }
        }

        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get => listaTiposClasse;
            set
            {
                listaTiposClasse = value;
                OnPropertyChanged();
            }
        }

        public TipoClasse TipoClasseSelecionado
        {
            get => tipoClasseSelecionado;
            set
            {
                tipoClasseSelecionado = value;
                OnPropertyChanged();
            }
        }

        public bool CadastroHabilitado
        {
            get => PontosVida > 0;
        }

        public bool ValidarCampos()
        {
            return !string.IsNullOrEmpty(Nome)
                   && CadastroHabilitado
                   && Forca != 0
                   && Defesa != 0;
        }

        public async Task ObterClasses()
        {
            try
            {
                ListaTiposClasse.Clear();

                ListaTiposClasse.Add(new TipoClasse { Id = 1, Descricao = "Cavaleiro" });
                ListaTiposClasse.Add(new TipoClasse { Id = 2, Descricao = "Mago" });
                ListaTiposClasse.Add(new TipoClasse { Id = 3, Descricao = "Clerigo" });

                OnPropertyChanged(nameof(ListaTiposClasse));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        private async Task CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task SalvarPersonagem()
        {
            try
            {
                if (TipoClasseSelecionado == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Atenção",
                        "Selecione uma classe.",
                        "Ok");
                    return;
                }

                Personagem model = new Personagem
                {
                    Nome = Nome,
                    PontosVida = PontosVida,
                    Defesa = Defesa,
                    Derrotas = Derrotas,
                    Disputas = Disputas,
                    Forca = Forca,
                    Inteligencia = Inteligencia,
                    Vitorias = Vitorias,
                    Id = Id,
                    Classe = (ClasseEnum)TipoClasseSelecionado.Id
                };

                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);
                else
                    await pService.PutPersonagemAsync(model);

                await Application.Current.MainPage.DisplayAlert(
                    "Mensagem",
                    "Dados salvos com sucesso.",
                    "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "OPS",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        public async Task CarregarPersonagem()
        {
            try
            {
                if (!int.TryParse(PersonagemSelecionadoId, out int personagemId))
                    return;

                Personagem p = await pService.GetPersonagemAsync(personagemId);

                Nome = p.Nome;
                PontosVida = p.PontosVida;
                Defesa = p.Defesa;
                Derrotas = p.Derrotas;
                Disputas = p.Disputas;
                Forca = p.Forca;
                Inteligencia = p.Inteligencia;
                Vitorias = p.Vitorias;
                Id = p.Id;

                TipoClasseSelecionado =
                    ListaTiposClasse.FirstOrDefault(tClasse => tClasse.Id == (int)p.Classe);
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