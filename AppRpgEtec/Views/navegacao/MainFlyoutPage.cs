namespace AppRpgEtec.Views.navegacao;

public class MainFlyoutPage : FlyoutPage
{
	public MainFlyoutPage()
	{
		Flyout = new MenuPage();

		Detail = new NavigationPage(new PrincipalView());
		
	}
}