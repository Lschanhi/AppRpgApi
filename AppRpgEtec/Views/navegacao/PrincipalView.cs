using AppRpgEtec.Views.Armas;
using AppRpgEtec.Views.Personagens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppRpgEtec.Views.navegacao
{
    public class PrincipalView : TabbedPage
    {

        public PrincipalView()
        {
            Title = "App RPG Etec";

            Children.Add(new NavigationPage(new ListagemView())
            {
                Title = "Personagens"
            });

            Children.Add(new NavigationPage(new ListagemArmaView())
            {
                Title = "Armas"
            });
        }
    }
}
