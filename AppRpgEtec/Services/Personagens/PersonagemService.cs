using AppRpgEtec.Models;
using System.Collections.ObjectModel;

namespace AppRpgEtec.Services.Personagens
{
    public class PersonagemService : Request
    {
        private readonly Request _request;

        private const string apiUrlBase =
            "http://luizsilva12.somee.com/RpgApi/Personagens";

        private readonly string _token;

        public PersonagemService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<int> PostPersonagemAsync(Personagem p)
        {
            return await _request.PostReturnIntAsync(apiUrlBase, p, _token);
        }

        public async Task<ObservableCollection<Personagem>> GetPersonagensAsync()
        {
            string urlComplementar = "/GetAll";

            return await _request.GetAsync<ObservableCollection<Personagem>>(
                apiUrlBase + urlComplementar,
                _token);
        }

        public async Task<Personagem> GetPersonagemAsync(int personagemId)
        {
            string urlComplementar = $"/{personagemId}";

            return await _request.GetAsync<Personagem>(
                apiUrlBase + urlComplementar,
                _token);
        }

        public async Task<ObservableCollection<Personagem>> GetByNomeAproximadoAsync(string nome)
        {
            string urlComplementar = $"/GetByNomeAproximado/{nome}";

            return await _request.GetAsync<ObservableCollection<Personagem>>(
                apiUrlBase + urlComplementar,
                _token);
        }

        public async Task<int> PutPersonagemAsync(Personagem p)
        {
            return await _request.PutAsync(apiUrlBase, p, _token);
        }

        public async Task<int> DeletePersonagemAsync(int personagemId)
        {
            string urlComplementar = $"/{personagemId}";

            return await _request.DeleteAsync(
                apiUrlBase + urlComplementar,
                _token);
        }

        public async Task<int> PutRestaurarPontosAsync(Personagem p)
        {
            string urlComplementar = "/RestaurarPontosVida";

            return await _request.PutAsync(
                apiUrlBase + urlComplementar,
                p,
                _token);
        }

        public async Task<int> PutZerarRankingAsync(Personagem p)
        {
            string urlComplementar = "/ZerarRanking";

            return await _request.PutAsync(
                apiUrlBase + urlComplementar,
                p,
                _token);
        }

        public async Task<int> PutZerarRankingRestaurarVidasGeralAsync()
        {
            string urlComplementar = "/ZerarRankingRestaurarVidas";

            return await _request.PutAsync(
                apiUrlBase + urlComplementar,
                new Personagem(),
                _token);
        }
    }
}