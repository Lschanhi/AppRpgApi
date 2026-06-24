using System;
using System.Collections.Generic;
using System.Text;

namespace AppRpgEtec.Models
{
    public class PersonagemHabilidade
    {
        public int Id { get; set; }
        public int PersonagemId { get; set; }
        public int HabilidadeId { get; set; }
        public string HabilidadeNome { get; set; } = string.Empty;
        public int HabilidadeDano { get; set; }
    }
}
