namespace AppRpgEtec.Views.navegacao;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
    }

    private void Home_Clicked(object sender, EventArgs e)
    {
        var flyout = Application.Current.MainPage as MainFlyoutPage;

        flyout.Detail =
            new NavigationPage(new PrincipalView());

        flyout.IsPresented = false;
    }

    private void Usuarios_Clicked(object sender, EventArgs e)
    {
        var flyout = Application.Current.MainPage as MainFlyoutPage;

        flyout.Detail =
            new NavigationPage(new Usuarios.ListagemView());

        flyout.IsPresented = false;
    }

    private void Personagens_Clicked(object sender, EventArgs e)
    {
        var flyout = Application.Current.MainPage as MainFlyoutPage;

        flyout.Detail =
            new NavigationPage(new Personagens.ListagemView());

        flyout.IsPresented = false;
    }

    private void Armas_Clicked(object sender, EventArgs e)
    {
        var flyout = Application.Current.MainPage as MainFlyoutPage;

        flyout.Detail =
            new NavigationPage(new Armas.ListagemArmaView());

        flyout.IsPresented = false;
    }

    private void Disputas_Clicked(object sender, EventArgs e)
    {
        var flyout = Application.Current.MainPage as MainFlyoutPage;

        flyout.Detail =
            new NavigationPage(new Disputas.ListagemView());

        flyout.IsPresented = false;
    }

    private void Sair_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage =
            new NavigationPage(new Usuarios.LoginView());
    }
}