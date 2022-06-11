using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class Medicamento : EntidadeBase<Medicamento>, IComparable<Medicamento>
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


            if (medicamento.Requisicoes.Count > 0)
                return Id == medicamento.Id &&
                       Nome == medicamento.Nome &&
                       Descricao == medicamento.Descricao &&
                       Lote == medicamento.Lote &&
                       Validade == medicamento.Validade &&
                       QuantidadeDisponivel == medicamento.QuantidadeDisponivel &&
                       CompararRequisicoes(medicamento) &&
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

        private bool CompararRequisicoes(Medicamento medicamento)
        {
            if(medicamento.Requisicoes.Count != Requisicoes.Count)
                return false;

            for (int i = 0; i < medicamento.Requisicoes.Count; i++)
            {
                if(!EqualityComparer<Requisicao>.Default.Equals(Requisicoes[i], medicamento.Requisicoes[i]))
                    return false;
            }
            return true;
        }

        public override void Atualizar(Medicamento registro)
        {
            Nome = registro.Nome;
            Descricao = registro.Descricao;
            Lote = registro.Lote;
            Validade = registro.Validade;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Nome);
            hash.Add(Descricao);
            hash.Add(Lote);
            hash.Add(Validade);
            hash.Add(QuantidadeDisponivel);
            hash.Add(Requisicoes);
            hash.Add(Fornecedor);
            hash.Add(QuantidadeRequisicoes);
            return hash.ToHashCode();
        }

        public int CompareTo(Medicamento other)
        {
            if (QuantidadeRequisicoes > other.QuantidadeRequisicoes)
                return -1;

            else if (QuantidadeRequisicoes == other.QuantidadeRequisicoes)
                return 0;
            else
                return 1;
        }
    }
}
