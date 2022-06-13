using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
        private const string enderecoBanco =
         "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region Sql Queries

        private const string sqlInserir =
            @"INSERT INTO [TBMEDICAMENTO]
                (
                    [NOME],
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],
                    [FORNECEDOR_ID]
                )    
                 VALUES
                (
                    @NOME,
                    @DESCRICAO,
                    @LOTE,
                    @VALIDADE,
                    @QUANTIDADEDISPONIVEL,
                    @FORNECEDOR_ID
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBMEDICAMENTO]	
		        SET
			       [NOME] = @NOME,
                    [DESCRICAO] = @DESCRICAO,
                    [LOTE] = @LOTE,
                    [VALIDADE] = @VALIDADE,
                    [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,
                    [FORNECEDOR_ID] = @FORNECEDOR_ID
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBMEDICAMENTO]
		        WHERE
			        [ID] = @ID";


        private const string sqlSelecionarEmFalta =
            @"SELECT 
                    M.[ID] AS ID,
		            M.[NOME] AS NOME,
                    M.[DESCRICAO] AS DESCRICAO,
                    M.[LOTE] AS LOTE,
                    M.[VALIDADE] AS VALIDADE,
                    M.[QUANTIDADEDISPONIVEL] AS QUANTIDADE,
					F.[EMAIL] AS FORNECEDOR_EMAIL,
					F.[ESTADO] AS FORNECEDOR_ESTADO,
					F.[ID] AS FORNECEDOR_ID,
					F.[NOME] AS FORNECEDOR_NOME,
                    F.[CIDADE] AS FORNECEDOR_CIDADE,
					F.[TELEFONE] AS FORNECEDOR_TELEFONE
	            FROM 
		            [TBMEDICAMENTO] AS M LEFT JOIN
                    [TBFORNECEDOR] AS F
                ON
                    M.[FORNECEDOR_ID] = F.ID
                WHERE
                 M.[QUANTIDADEDISPONIVEL] < 5
";


        private const string sqlSelecionarTodos =
            @"SELECT 
                    M.[ID] AS ID,
		            M.[NOME] AS NOME,
                    M.[DESCRICAO] AS DESCRICAO,
                    M.[LOTE] AS LOTE,
                    M.[VALIDADE] AS VALIDADE,
                    M.[QUANTIDADEDISPONIVEL] AS QUANTIDADE,
					F.[EMAIL] AS FORNECEDOR_EMAIL,
					F.[ESTADO] AS FORNECEDOR_ESTADO,
					F.[ID] AS FORNECEDOR_ID,
					F.[NOME] AS FORNECEDOR_NOME,
                    F.[CIDADE] AS FORNECEDOR_CIDADE,
					F.[TELEFONE] AS FORNECEDOR_TELEFONE
	            FROM 
		            [TBMEDICAMENTO] AS M LEFT JOIN
                    [TBFORNECEDOR] AS F
                ON
                    M.[FORNECEDOR_ID] = F.ID
";

        private const string sqlSelecionarPorID =
            @"SELECT 
                    M.[ID] AS ID,
    		        M.[NOME] AS NOME,
                    M.[DESCRICAO] AS DESCRICAO,
                    M.[LOTE] AS LOTE,
                    M.[VALIDADE] AS VALIDADE,
                    M.[QUANTIDADEDISPONIVEL] AS QUANTIDADE,
					F.[EMAIL] AS FORNECEDOR_EMAIL,
					F.[ESTADO] AS FORNECEDOR_ESTADO,
					F.[ID] AS FORNECEDOR_ID,
					F.[NOME] AS FORNECEDOR_NOME,
                    F.[CIDADE] AS FORNECEDOR_CIDADE,
					F.[TELEFONE] AS FORNECEDOR_TELEFONE
	            FROM 
		            [TBMEDICAMENTO] AS M LEFT JOIN
                    [TBFORNECEDOR] AS F
                ON
                    M.[FORNECEDOR_ID] = F.ID
		        WHERE
                    M.[ID] = @ID";

        #endregion

        public ValidationResult Inserir(Medicamento registro)
        {
            var validador = new ValidadorMedicamento();

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

        public ValidationResult Editar(Medicamento registro)
        {
            var validador = new ValidadorMedicamento();

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

        public ValidationResult Excluir(Medicamento registro)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", registro.Id);
            var resultadoValidacao = new ValidationResult();
            int IDRegistrosExcluidos = 0;
            conexaoComBanco.Open();
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

        public List<Medicamento> SelecionarEmFalta()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarEmFalta, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<Medicamento> registros = new List<Medicamento>();

            while (leitorRegistro.Read())
            {
                Medicamento registro = ConverterParaRegistro(leitorRegistro);

                registros.Add(registro);
            }

            conexaoComBanco.Close();

            return registros;
        }



        #region Requisicao


        private const string sqlSelecionarTodosRequisicoes =
          @"SELECT 
					R.[Data] AS REQUISICAO_DATA,
					R.[ID] AS REQUISICAO_ID,
					R.[QuantidadeMedicamento] AS REQUISICAO_QUANTIDADE,
					P.[Id] AS PACIENTE_ID,
					P.[CartaoSUS] AS PACIENTE_CARTAOSUS,
					P.[Nome] AS PACIENTE_NOME,
					FU.[Id] AS FUNCIONARIO_ID,
					FU.[Login] AS FUNCIONARIO_LOGIN,
					FU.[Nome] AS FUNCIONARIO_NOME,
					FU.[Senha] AS FUNCIONARIO_SENHA
	            FROM 
					[TBRequisicao] AS R INNER JOIN
					[TBPaciente] AS P
				ON
					R.Paciente_Id = P.Id INNER JOIN
					[TBFuncionario] AS FU
				ON
					R.Funcionario_Id = FU.Id
		        WHERE
                    R.[Medicamento_Id] = @ID";



        public void CarregarRequisicoes(Medicamento medicamento)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodosRequisicoes, conexaoComBanco);
            comandoSelecao.Parameters.AddWithValue("ID", medicamento.Id);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            while (leitorRegistro.Read())
            {
                Requisicao requisicao = ConverterParaRequisicao(leitorRegistro, medicamento);

                medicamento.Requisicoes.Add(requisicao);
            }

            conexaoComBanco.Close();

        }

        private Requisicao ConverterParaRequisicao(SqlDataReader leitorRegistro,Medicamento medicamento)
        {

            int idPaciente = Convert.ToInt32(leitorRegistro["PACIENTE_ID"]);
            string nomePaciente = Convert.ToString(leitorRegistro["PACIENTE_NOME"]);
            string cartaoSUS = Convert.ToString(leitorRegistro["PACIENTE_CARTAOSUS"]);


            var paciente = new Paciente(nomePaciente, cartaoSUS);
            paciente.Id = idPaciente;

            int funcionarioId = Convert.ToInt32(leitorRegistro["FUNCIONARIO_ID"]);
            string funcionarioNome = Convert.ToString(leitorRegistro["FUNCIONARIO_NOME"]);
            string funcionarioLogin = Convert.ToString(leitorRegistro["FUNCIONARIO_LOGIN"]);
            string funcionarioSenha = Convert.ToString(leitorRegistro["FUNCIONARIO_SENHA"]);


            var funcionario = new Funcionario(funcionarioNome, funcionarioLogin, funcionarioSenha);
            funcionario.Id = funcionarioId;

            int requisicaoId = Convert.ToInt32(leitorRegistro["REQUISICAO_ID"]);
            int requisicaoQuantidade = Convert.ToInt32(leitorRegistro["REQUISICAO_QUANTIDADE"]);
            DateTime requisicaoData = Convert.ToDateTime(leitorRegistro["REQUISICAO_DATA"]);

            var requsicao = new Requisicao(medicamento, paciente, requisicaoQuantidade, requisicaoData, funcionario);
            requsicao.Id = requisicaoId;


            return requsicao;
        }

        #endregion


        public List<Medicamento> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<Medicamento> registros = new List<Medicamento>();

            while (leitorRegistro.Read())
            {
                Medicamento registro = ConverterParaRegistro(leitorRegistro);

                registros.Add(registro);
            }

            conexaoComBanco.Close();

            return registros;
        }

        public Medicamento SelecionarPorID(int ID)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorID, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", ID);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            Medicamento registro = null;
            if (leitorRegistro.Read())
                registro = ConverterParaRegistro(leitorRegistro);

            conexaoComBanco.Close();

            return registro;
        }

        private Medicamento ConverterParaRegistro(SqlDataReader leitorRegistro)
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

        private static void ConfigurarParametrosRegistro(Medicamento novaDisciplina, SqlCommand cmdInserir)
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

