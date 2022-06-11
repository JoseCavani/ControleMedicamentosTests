using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDados
    {

        private const string enderecoBanco =
 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region Sql Queries

        private const string sqlInserir =
            @"INSERT INTO [TBPACIENTE]
                (
                    [NOME],
                    [CARTAOSUS]
                )    
                 VALUES
                (
                    @NOME,
                    @CARTAOSUS
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBPACIENTE]	
		        SET
			       [NOME] = @NOME,
                    [CARTAOSUS] = @CARTAOSUS
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBPACIENTE]
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                    [NOME],
					[CARTAOSUS],
					[ID]
	            FROM 
                    [TBPACIENTE]
";

        private const string sqlSelecionarPorID =
           @"SELECT 
                    [NOME],
					[CARTAOSUS],
					[ID]
	            FROM 
                    [TBPACIENTE]
		        WHERE
                    [ID] = @ID";

        #endregion

        public ValidationResult Inserir(Paciente registro)
        {
            var validador = new ValidadorPaciente();

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

        public ValidationResult Editar(Paciente registro)
        {
            var validador = new ValidadorPaciente();

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

        public ValidationResult Excluir(Paciente registro)
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

        public List<Paciente> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<Paciente> registros = new List<Paciente>();

            while (leitorRegistro.Read())
            {
                Paciente registro = ConverterParaRegistro(leitorRegistro);

                registros.Add(registro);
            }

            conexaoComBanco.Close();

            return registros;
        }

        public Paciente SelecionarPorID(int ID)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorID, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", ID);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            Paciente registro = null;
            if (leitorRegistro.Read())
                registro = ConverterParaRegistro(leitorRegistro);

            conexaoComBanco.Close();

            return registro;
        }

        private Paciente ConverterParaRegistro(SqlDataReader leitorRegistro)
        {

            int idPaciente = Convert.ToInt32(leitorRegistro["ID"]);
            string nomePaciente = Convert.ToString(leitorRegistro["NOME"]);
            string cartaoSUS = Convert.ToString(leitorRegistro["CARTAOSUS"]);




            var Paciente = new Paciente(nomePaciente, cartaoSUS);
            Paciente.Id = idPaciente;

            return Paciente;
        }

        private static void ConfigurarParametrosRegistro(Paciente registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("NOME", registro.Nome);
            cmdInserir.Parameters.AddWithValue("CARTAOSUS", registro.CartaoSUS);

        }
    }



}
