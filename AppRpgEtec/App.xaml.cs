namespace AppRpgEtec
{
    public partial class App : Application
    {
        private readonly Exception? startupException;

        public App()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                startupException = ex;
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            if (startupException is not null)
            {
                return new Window(CreateStartupErrorPage(startupException, "InitializeComponent"));
            }

            try
            {
                return new Window(new NavigationPage(new Views.Usuarios.LoginView()));
            }
            catch (Exception ex)
            {
                return new Window(CreateStartupErrorPage(ex, "NavigationPage/LoginView"));
            }
        }

        private static Page CreateStartupErrorPage(Exception ex, string stage)
        {
            return new ContentPage
            {
                Title = "Erro ao iniciar",
                Content = new ScrollView
                {
                    Content = new VerticalStackLayout
                    {
                        Padding = 24,
                        Spacing = 12,
                        Children =
                        {
                            new Label
                            {
                                Text = "Falha ao iniciar o app.",
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 22
                            },
                            new Label { Text = $"Etapa: {stage}" },
                            new Label { Text = ex.Message },
                            new Label
                            {
                                Text = ex.ToString(),
                                LineBreakMode = LineBreakMode.WordWrap
                            }
                        }
                    }
                }
            };
        }
    }
}
