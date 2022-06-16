using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class MapeadorFornecedor : IMapeavel<Fornecedor>
    {
        public void ConfigurarParametrosRegistro(Fornecedor registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("NOME", registro.Nome);
            cmdInserir.Parameters.AddWithValue("EMAIL", registro.Email);
            cmdInserir.Parameters.AddWithValue("ESTADO", registro.Estado);
            cmdInserir.Parameters.AddWithValue("CIDADE", registro.Cidade);
            cmdInserir.Parameters.AddWithValue("TELEFONE", registro.Telefone);
        }

        public Fornecedor ConverterParaRegistro(SqlDataReader leitorRegistro)
        {
            int idFornecedor = Convert.ToInt32(leitorRegistro["ID"]);
            string nomeFornecedor = Convert.ToString(leitorRegistro["NOME"]);
            string email = Convert.ToString(leitorRegistro["EMAIL"]);
            string estado = Convert.ToString(leitorRegistro["ESTADO"]);
            string cidade = Convert.ToString(leitorRegistro["CIDADE"]);
            string telefone = Convert.ToString(leitorRegistro["TELEFONE"]);

            var fornecedor = new Fornecedor(nomeFornecedor, telefone, email, cidade, estado);
            fornecedor.Id = idFornecedor;

            return fornecedor;
        }
    }
}
