using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AppRpgEtec.ViewModels;
using AppRpgEtec.Views.Usuarios;
using AppRpgEtec.Views.Personagens;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;

        public ICommand AutenticarCommand {  get; set; }
        public ICommand RegistrarCommand { get; set; }

        public ICommand DirecionarCadastroCommand { get; set; }

        public UsuarioViewModel()
        {
            uService = new UsuarioService();
            InicializarCommands();
        }

        public void InicializarCommands()
        {
            AutenticarCommand = new Command(async() => await AutenticarUsuario());
            RegistrarCommand = new Command(async () => await RegistrarUsuario());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }

        private string login = string.Empty;
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string senha = string.Empty;
        public string Senha
        {
            get => senha;
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }
        #region
        public async Task AutenticarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = senha;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-Vindo(a){uAutenticado.Username}.";

                    //guardando os dados do usuário para uso futuro
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("usuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    Application.Current.MainPage = new Views.navegacao.PrincipalView();
                   
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação","Daods Incorretos :(","Ok");
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação", ex.Message
                        + "Detalhes" + ex.InnerException, "Ok");
            }
        }

        public async Task RegistrarUsuario()
        {
            try
            {
                if(string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Senha))
                {
                    throw new Exception("Usuário ou Senha nao pode ser Vazio");
                }

                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Senha;

                Usuario uRegistrado = await uService.PostRegistrarUsuarioAsync(u);

                if(uRegistrado.Id != 0)
                {
                  
                    string mensagem = $"Usuario Id {uRegistrado.Id} registrado com sucesso.";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    await Application.Current.MainPage.Navigation.PopAsync();//remove a página da pilha de visualização
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage .DisplayAlert("Informação", ex.Message
                        + "Detalhes" + ex.InnerException, "Ok");
            }
        }


        public async Task DirecionarParaCadastro()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação", ex.Message
                        + "Detalhes" + ex.InnerException, "Ok");
            }

        }
        #endregion
    }
}
