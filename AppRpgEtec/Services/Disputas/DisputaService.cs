using System;
using System.Collections.Generic;
using System.Text;

using AppRpgEtec.Models;

namespace AppRpgEtec.Services.Disputas
{
    public class DisputaService : Request
    {
        private readonly Request _request;
        private readonly string _token;

        private const string apiUrlBase =
            "http://luizsilva12.somee.com/RpgApi/Disputas";

        public DisputaService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<Disputa> PostDisputaComArmaAsync(Disputa d)
        {
            string urlComplementar = "/Arma";

            return await _request.PostAsync(
                apiUrlBase + urlComplementar,
                d,
                _token);
        }

        public async Task<Disputa> PostDisputaComHabilidadesAsync(Disputa d)
        {
            string urlComplementar = "/Habilidade";

            return await _request.PostAsync(
                apiUrlBase + urlComplementar,
                d,
                _token);
        }

        public async Task<Disputa> PostDisputaGeralAsync(Disputa d)
        {
            string urlComplementar = "/DisputaEmGrupo";

            return await _request.PostAsync(
                apiUrlBase + urlComplementar,
                d,
                _token);
        }
    }
}
