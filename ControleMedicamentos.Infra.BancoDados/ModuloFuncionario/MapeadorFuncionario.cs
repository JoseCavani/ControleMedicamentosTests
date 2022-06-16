using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class MapeadorFuncionario : IMapeavel<Funcionario>
    {
        public Funcionario ConverterParaRegistro(SqlDataReader leitorRegistro)
        {

            int id = Convert.ToInt32(leitorRegistro["ID"]);
            string nome = Convert.ToString(leitorRegistro["NOME"]);
            string login = Convert.ToString(leitorRegistro["LOGIN"]);
            string senha = Convert.ToString(leitorRegistro["SENHA"]);


            var Funcionario = new Funcionario(nome, login, senha);
            Funcionario.Id = id;

            return Funcionario;
        }

        public void ConfigurarParametrosRegistro(Funcionario registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("NOME", registro.Nome);
            cmdInserir.Parameters.AddWithValue("LOGIN", registro.Login);
            cmdInserir.Parameters.AddWithValue("SENHA", registro.Senha);

        }
    }
}
