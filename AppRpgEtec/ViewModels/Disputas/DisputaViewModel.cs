using System;
using System.Collections.Generic;
using System.Text;

using AppRpgEtec.Models;
using AppRpgEtec.Services.Disputas;
using AppRpgEtec.Services.PersonagemHabilidades;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Disputas
{
    public class DisputaViewModel : BaseViewModel
    {
        private readonly PersonagemService pService;
        private readonly DisputaService dService;
        private readonly PersonagemHabilidadeService phService;

        private Personagem atacante;
        private Personagem oponente;
        private Personagem personagemSelecionado;
        private PersonagemHabilidade habilidadeSelecionada;
        private string textoBuscaDigitado;

        public ObservableCollection<Personagem> PersonagensEncontrados { get; set; }
        public ObservableCollection<PersonagemHabilidade> Habilidades { get; set; }

        public Disputa DisputaPersonagens { get; set; }

        public ICommand PesquisarPersonagensCommand { get; set; }
        public ICommand DisputaComArmaCommand { get; set; }
        public ICommand DisputaComHabilidadeCommand { get; set; }
        public ICommand DisputaGeralCommand { get; set; }

        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);

            pService = new PersonagemService(token);
            dService = new DisputaService(token);
            phService = new PersonagemHabilidadeService(token);

            atacante = new Personagem();
            oponente = new Personagem();

            PersonagensEncontrados = new ObservableCollection<Personagem>();
            Habilidades = new ObservableCollection<PersonagemHabilidade>();
            DisputaPersonagens = new Disputa();

            PesquisarPersonagensCommand =
                new Command<string>(async (string pesquisa) =>
                {
                    await PesquisarPersonagens(pesquisa);
                });

            DisputaComArmaCommand =
                new Command(async () => await ExecutarDisputaArmada());

            DisputaComHabilidadeCommand =
                new Command(async () => await ExecutarDisputaHabilidades());

            DisputaGeralCommand =
                new Command(async () => await ExecutarDisputaGeral());
        }

        public string TextoBuscaDigitado
        {
            get => textoBuscaDigitado;
            set
            {
                textoBuscaDigitado = value;
                OnPropertyChanged();

                if (!string.IsNullOrWhiteSpace(value))
                    _ = PesquisarPersonagens(value);
                else
                    PersonagensEncontrados.Clear();
            }
        }

        public string DescricaoPersonagemAtacante
        {
            get => atacante?.Nome;
        }

        public string DescricaoPersonagemOponente
        {
            get => oponente?.Nome;
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

                    _ = SelecionarPersonagem(personagemSelecionado);
                    PersonagensEncontrados.Clear();
                }
            }
        }

        public PersonagemHabilidade HabilidadeSelecionada
        {
            get => habilidadeSelecionada;
            set
            {
                habilidadeSelecionada = value;
                OnPropertyChanged();
            }
        }

        public async Task PesquisarPersonagens(string textoPesquisaPersonagem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textoPesquisaPersonagem))
                {
                    PersonagensEncontrados.Clear();
                    return;
                }

                PersonagensEncontrados =
                    await pService.GetByNomeAproximadoAsync(textoPesquisaPersonagem);

                OnPropertyChanged(nameof(PersonagensEncontrados));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        public async Task SelecionarPersonagem(Personagem p)
        {
            try
            {
                string tipoCombatente =
                    await Application.Current.MainPage.DisplayActionSheet(
                        "Atacante ou Oponente?",
                        "Cancelar",
                        null,
                        "Atacante",
                        "Oponente");

                if (tipoCombatente == "Atacante")
                {
                    atacante = p;

                    await ObterHabilidadesAsync(p.Id);

                    OnPropertyChanged(nameof(DescricaoPersonagemAtacante));
                }
                else if (tipoCombatente == "Oponente")
                {
                    oponente = p;

                    OnPropertyChanged(nameof(DescricaoPersonagemOponente));
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

        public async Task ObterHabilidadesAsync(int personagemId)
        {
            try
            {
                Habilidades =
                    await phService.GetPersonagemHabilidadesAsync(personagemId);

                OnPropertyChanged(nameof(Habilidades));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        private async Task ExecutarDisputaArmada()
        {
            try
            {
                if (atacante.Id == 0 || oponente.Id == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Atenção",
                        "Selecione um atacante e um oponente.",
                        "Ok");
                    return;
                }

                DisputaPersonagens.AtacanteId = atacante.Id;
                DisputaPersonagens.OponenteId = oponente.Id;

                DisputaPersonagens =
                    await dService.PostDisputaComArmaAsync(DisputaPersonagens);

                await Application.Current.MainPage.DisplayAlert(
                    "Resultado",
                    DisputaPersonagens.Narracao,
                    "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        private async Task ExecutarDisputaHabilidades()
        {
            try
            {
                if (atacante.Id == 0 || oponente.Id == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Atenção",
                        "Selecione um atacante e um oponente.",
                        "Ok");
                    return;
                }

                if (HabilidadeSelecionada == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Atenção",
                        "Selecione uma habilidade.",
                        "Ok");
                    return;
                }

                DisputaPersonagens.AtacanteId = atacante.Id;
                DisputaPersonagens.OponenteId = oponente.Id;
                DisputaPersonagens.HabilidadeId = HabilidadeSelecionada.HabilidadeId;

                DisputaPersonagens =
                    await dService.PostDisputaComHabilidadesAsync(DisputaPersonagens);

                await Application.Current.MainPage.DisplayAlert(
                    "Resultado",
                    DisputaPersonagens.Narracao,
                    "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops",
                    ex.Message + " Detalhes: " + ex.InnerException,
                    "Ok");
            }
        }

        private async Task ExecutarDisputaGeral()
        {
            try
            {
                ObservableCollection<Personagem> lista =
                    await pService.GetPersonagensAsync();

                DisputaPersonagens.ListaIdPersonagens =
                    lista.Select(p => p.Id).ToList();

                DisputaPersonagens =
                    await dService.PostDisputaGeralAsync(DisputaPersonagens);

                string mensagem = string.Empty;

                if (!string.IsNullOrWhiteSpace(DisputaPersonagens.Narracao))
                    mensagem += DisputaPersonagens.Narracao + "\n\n";

                if (DisputaPersonagens.Resultados != null &&
                    DisputaPersonagens.Resultados.Count > 0)
                    mensagem += string.Join("\n", DisputaPersonagens.Resultados);

                if (string.IsNullOrWhiteSpace(mensagem))
                    mensagem = "A API executou a disputa, mas não retornou narração nem resultados.";

                await Application.Current.MainPage.DisplayAlert(
                    "Resultado",
                    mensagem,
                    "Ok");
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
