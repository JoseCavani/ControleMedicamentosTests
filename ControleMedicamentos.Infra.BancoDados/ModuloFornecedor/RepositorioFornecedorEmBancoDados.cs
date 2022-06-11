using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDados
    {

        private const string enderecoBanco =
 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region Sql Queries

        private const string sqlInserir =
            @"INSERT INTO [TBFORNECEDOR]
                (
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
                )    
                 VALUES
                (
                    @NOME,
                    @TELEFONE,
                    @EMAIL,
                    @CIDADE,
                    @ESTADO
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBFORNECEDOR]	
		        SET
			       [NOME] = @NOME,
                    [TELEFONE] = @TELEFONE,
                    [EMAIL] = @EMAIL,
                    [CIDADE] = @CIDADE,
                    [ESTADO] = @ESTADO
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBFORNECEDOR]
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                    [CIDADE],
					[EMAIL],
					[ESTADO],
					[ID],
					[NOME],
                    [CIDADE],
					[TELEFONE]
	            FROM 
                    [TBFORNECEDOR]
";

        private const string sqlSelecionarPorID =
           @"SELECT 
                    [CIDADE],
					[EMAIL],
					[ESTADO],
					[ID],
					[NOME],
                    [CIDADE],
					[TELEFONE]
	            FROM 
                    [TBFORNECEDOR]
		        WHERE
                    [ID] = @ID";

        #endregion

        public ValidationResult Inserir(Fornecedor registro)
        {
            var validador = new ValidadorFornecedor();

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

        public ValidationResult Editar(Fornecedor registro)
        {
            var validador = new ValidadorFornecedor();

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

        public ValidationResult Excluir(Fornecedor registro)
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
                resultadoValidacao.Errors.Add(new ValidationFailure("",ex.Message));
            }

       

            if (IDRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Fornecedor> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<Fornecedor> registros = new List<Fornecedor>();

            while (leitorRegistro.Read())
            {
                Fornecedor registro = ConverterParaRegistro(leitorRegistro);

                registros.Add(registro);
            }

            conexaoComBanco.Close();

            return registros;
        }

        public Fornecedor SelecionarPorID(int ID)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorID, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", ID);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            Fornecedor registro = null;
            if (leitorRegistro.Read())
                registro = ConverterParaRegistro(leitorRegistro);

            conexaoComBanco.Close();

            return registro;
        }

        private Fornecedor ConverterParaRegistro(SqlDataReader leitorRegistro)
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

        private static void ConfigurarParametrosRegistro(Fornecedor registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("NOME", registro.Nome);
            cmdInserir.Parameters.AddWithValue("EMAIL", registro.Email);
            cmdInserir.Parameters.AddWithValue("ESTADO", registro.Estado);
            cmdInserir.Parameters.AddWithValue("CIDADE", registro.Cidade);
            cmdInserir.Parameters.AddWithValue("TELEFONE", registro.Telefone);

        }
    }



}

