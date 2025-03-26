using System;
using Tarefy.Enumeradores;

namespace Tarefy.Models
{
    public class TarefaModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Detalhes { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataLimite { get; set; }
        public PrioridadeEnum Prioridade { get; set; }
        public StatusEnum Status { get; set; }
    }
}