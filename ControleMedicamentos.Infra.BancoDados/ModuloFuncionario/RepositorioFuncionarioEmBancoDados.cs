using ControleMedicamentos.Dominio.ModuloFuncionario;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class RepositorioFuncionarioEmBancoDados
    {

        private const string enderecoBanco =
 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region Sql Queries

        private const string sqlInserir =
            @"INSERT INTO [TBFUNCIONARIO]
                (
                    [NOME],
                    [LOGIN],
                    [SENHA]                    
                )    
                 VALUES
                (
                    @NOME,
                    @LOGIN,
                    @SENHA
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBFUNCIONARIO]	
		        SET
			       [NOME] = @NOME,
                    [LOGIN] = @LOGIN,
                    [SENHA] = @SENHA
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBFUNCIONARIO]
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                    [NOME],
					[LOGIN],
					[SENHA],
					[ID]
	            FROM 
                    [TBFUNCIONARIO]
";

        private const string sqlSelecionarPorID =
           @"SELECT 
                    [NOME],
					[LOGIN],
					[SENHA],
					[ID]
	            FROM 
                    [TBFUNCIONARIO]
		        WHERE
                    [ID] = @ID";

        #endregion

        public ValidationResult Inserir(Funcionario registro)
        {
            var validador = new ValidadorFuncionario();

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexao = new SqlConnection(enderecoBanco);
            SqlCommand cmdInserir = new SqlCommand(sqlInserir, conexao);

            ConfigurarParametrosRegistro(registro, cmdInserir);
            conexao.Open();

            var ID = cmdInserir.ExecuteScalar();

            registro.Id = Convert.ToInt32(ID);
            conexao.Close();
            return resultadoValidacao;

        }

        public ValidationResult Editar(Funcionario registro)
        {
            var validador = new ValidadorFuncionario();

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosRegistro(registro, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Funcionario registro)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", registro.Id);

            conexaoComBanco.Open();

            var resultadoValidacao = new ValidationResult();

            int IDRegistrosExcluidos = 0;

            try
            {
                IDRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                resultadoValidacao.Errors.Add(new ValidationFailure("", ex.Message));
            }


            if (IDRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Funcionario> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<Funcionario> registros = new List<Funcionario>();

            while (leitorRegistro.Read())
            {
                Funcionario registro = ConverterParaRegistro(leitorRegistro);

                registros.Add(registro);
            }

            conexaoComBanco.Close();

            return registros;
        }

        public Funcionario SelecionarPorID(int ID)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorID, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", ID);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            Funcionario registro = null;
            if (leitorRegistro.Read())
                registro = ConverterParaRegistro(leitorRegistro);

            conexaoComBanco.Close();

            return registro;
        }

        private Funcionario ConverterParaRegistro(SqlDataReader leitorRegistro)
        {

            int id = Convert.ToInt32(leitorRegistro["ID"]);
            string nome = Convert.ToString(leitorRegistro["NOME"]);
            string login = Convert.ToString(leitorRegistro["LOGIN"]);
            string senha = Convert.ToString(leitorRegistro["SENHA"]);


            var Funcionario = new Funcionario(nome, login, senha);
            Funcionario.Id = id;

            return Funcionario;
        }

        private static void ConfigurarParametrosRegistro(Funcionario registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("NOME", registro.Nome);
            cmdInserir.Parameters.AddWithValue("LOGIN", registro.Login);
            cmdInserir.Parameters.AddWithValue("SENHA", registro.Senha);

        }
    }



}

