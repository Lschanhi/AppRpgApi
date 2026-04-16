using AppRpgEtec.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Services.Armas
{
    public class ArmasService
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://luizsilva12.somee.com/RpgApi/Armas";

        private string _token;

        public ArmasService(string token)
        {
            _request = new Request();
            _token = token;


        }


        public async Task<ObservableCollection<Arma>> GetArmasAsync()
        {
            string urlComplementar = string.Format("{0}", "/GetAll");
            ObservableCollection<Models.Arma> listaArmas = await
            _request.GetAsync<ObservableCollection<Models.Arma>>(apiUrlBase + urlComplementar,
            _token);
            return listaArmas;
        }

    }
}
