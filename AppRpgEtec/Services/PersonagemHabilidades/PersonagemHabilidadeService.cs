using System;
using System.Collections.Generic;
using System.Text;

using AppRpgEtec.Models;
using System.Collections.ObjectModel;

namespace AppRpgEtec.Services.PersonagemHabilidades
{
    public class PersonagemHabilidadeService : Request
    {
        private readonly Request _request;
        private readonly string _token;

        private const string apiUrlBase =
            "http://luizsilva12.somee.com/RpgApi/PersonagemHabilidades";

        public PersonagemHabilidadeService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<PersonagemHabilidade>>
            GetPersonagemHabilidadesAsync(int personagemId)
        {
            string urlComplementar = $"/{personagemId}";

            return await _request.GetAsync<ObservableCollection<PersonagemHabilidade>>(
                apiUrlBase + urlComplementar,
                _token);
        }
    }
}
