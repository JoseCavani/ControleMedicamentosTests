using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class Medicamento : EntidadeBase<Medicamento>
    {        
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public int QuantidadeDisponivel { get; set; }

        public List<Requisicao> Requisicoes { get; set; }

        public Fornecedor Fornecedor{ get; set; }

        public int QuantidadeRequisicoes { get { return Requisicoes.Count; } }

        public Medicamento(string nome, string descricao, string lote, DateTime validade)
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            Validade = validade;
            Requisicoes = new List<Requisicao>();
        }

        public override bool Equals(object obj)
        {
            Medicamento medicamento = default;
            try
            {
                 medicamento = (Medicamento)obj;
            }
            catch (Exception)
            {
                return false;
            }


            if(medicamento.Requisicoes.Count>0)
            return Id == medicamento.Id &&
                   Nome == medicamento.Nome &&
                   Descricao == medicamento.Descricao &&
                   Lote == medicamento.Lote &&
                   Validade == medicamento.Validade &&
                   QuantidadeDisponivel == medicamento.QuantidadeDisponivel &&
                   EqualityComparer<List<Requisicao>>.Default.Equals(Requisicoes, medicamento.Requisicoes) &&
                   EqualityComparer<Fornecedor>.Default.Equals(Fornecedor, medicamento.Fornecedor) &&
                   QuantidadeRequisicoes == medicamento.QuantidadeRequisicoes;

            else
               return Id == medicamento.Id &&
                   Nome == medicamento.Nome &&
                   Descricao == medicamento.Descricao &&
                   Lote == medicamento.Lote &&
                   Validade == medicamento.Validade &&
                   QuantidadeDisponivel == medicamento.QuantidadeDisponivel &&
                   EqualityComparer<Fornecedor>.Default.Equals(Fornecedor, medicamento.Fornecedor) &&
                   QuantidadeRequisicoes == medicamento.QuantidadeRequisicoes;

        }

        public override void Atualizar(Medicamento registro)
        {
            Nome = registro.Nome;
            Descricao = registro.Descricao;
            Lote = registro.Lote;
            Validade = registro.Validade;
        }
    }
}
