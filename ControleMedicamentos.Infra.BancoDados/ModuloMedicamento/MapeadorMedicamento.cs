using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloMedicamento
{
    public class MapeadorMedicamento : IMapeavel<Medicamento>
    {
        public Medicamento ConverterParaRegistro(SqlDataReader leitorRegistro)
        {
            int Id = Convert.ToInt32(leitorRegistro["ID"]);
            string nome = Convert.ToString(leitorRegistro["NOME"]);
            string descricao = Convert.ToString(leitorRegistro["DESCRICAO"]);
            string lote = Convert.ToString(leitorRegistro["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorRegistro["VALIDADE"]).Date;
            int quantidade = Convert.ToInt32(leitorRegistro["QUANTIDADE"]);


            int idFornecedor = Convert.ToInt32(leitorRegistro["FORNECEDOR_ID"]);
            string nomeFornecedor = Convert.ToString(leitorRegistro["FORNECEDOR_NOME"]);
            string email = Convert.ToString(leitorRegistro["FORNECEDOR_EMAIL"]);
            string estado = Convert.ToString(leitorRegistro["FORNECEDOR_ESTADO"]);
            string cidade = Convert.ToString(leitorRegistro["FORNECEDOR_CIDADE"]);
            string telefone = Convert.ToString(leitorRegistro["FORNECEDOR_TELEFONE"]);




            var fornecedor = new Fornecedor(nomeFornecedor, telefone, email, cidade, estado);
            fornecedor.Id = idFornecedor;

            var registro = new Medicamento(nome, descricao, lote, validade);
            registro.Id = Id;
            registro.Fornecedor = fornecedor;
            registro.QuantidadeDisponivel = quantidade;

            return registro;
        }

        public void ConfigurarParametrosRegistro(Medicamento novaDisciplina, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", novaDisciplina.Id);
            cmdInserir.Parameters.AddWithValue("NOME", novaDisciplina.Nome);
            cmdInserir.Parameters.AddWithValue("DESCRICAO", novaDisciplina.Descricao);
            cmdInserir.Parameters.AddWithValue("LOTE", novaDisciplina.Lote);
            cmdInserir.Parameters.AddWithValue("VALIDADE", novaDisciplina.Validade);
            cmdInserir.Parameters.AddWithValue("QUANTIDADEDISPONIVEL", novaDisciplina.QuantidadeDisponivel);
            cmdInserir.Parameters.AddWithValue("FORNECEDOR_ID", novaDisciplina.Fornecedor.Id);

        }
    }
}
