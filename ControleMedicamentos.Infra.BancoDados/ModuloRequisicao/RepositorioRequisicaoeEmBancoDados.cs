using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoeEmBancoDados
    {

        private const string enderecoBanco =
 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region Sql Queries

        private const string sqlInserir =
            @"INSERT INTO [TBREQUISICAO]
                (
                    [QUANTIDADEMEDICAMENTO],
                    [DATA],
                    [FUNCIONARIO_ID],
                    [PACIENTE_ID],
                    [MEDICAMENTO_ID]
                )    
                 VALUES
                (
                    @QUANTIDADEMEDICAMENTO,
                    @DATA,
                    @FUNCIONARIO_ID,
                    @PACIENTE_ID,
                    @MEDICAMENTO_ID
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBREQUISICAO]	
		        SET
                    [QUANTIDADEMEDICAMENTO] = @QUANTIDADEMEDICAMENTO,
                    [DATA] = @DATA,
                    [FUNCIONARIO_ID]= @FUNCIONARIO_ID,
                    [PACIENTE_ID] = @PACIENTE_ID,
                    [MEDICAMENTO_ID] = @MEDICAMENTO_ID
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBREQUISICAO]
		        WHERE
			        [ID] = @ID";


        //from here
        private const string sqlSelecionarTodos =
            @"SELECT 
					R.[Data] AS REQUISICAO_DATA,
					R.[ID] AS REQUISICAO_ID,
					R.[QuantidadeMedicamento] AS REQUISICAO_QUANTIDADE,
                    M.[ID] AS MEDICAMENTO_ID,
		            M.[NOME] AS MEDICAMENTO_NOME,
                    M.[DESCRICAO] AS MEDICAMENTO_DESCRICAO,
                    M.[LOTE] AS MEDICAMENTO_LOTE,
                    M.[VALIDADE] AS MEDICAMENTO_VALIDADE,
                    M.[QUANTIDADEDISPONIVEL] AS MEDICAMENTO_QUANTIDADE,
					F.[EMAIL] AS FORNECEDOR_EMAIL,
					F.[ESTADO] AS FORNECEDOR_ESTADO,
					F.[ID] AS FORNECEDOR_ID,
					F.[NOME] AS FORNECEDOR_NOME,
                    F.[CIDADE] AS FORNECEDOR_CIDADE,
					F.[TELEFONE] AS FORNECEDOR_TELEFONE,
					P.[Id] AS PACIENTE_ID,
					P.[CartaoSUS] AS PACIENTE_CARTAOSUS,
					P.[Nome] AS PACIENTE_NOME,
					FU.[Id] AS FUNCIONARIO_ID,
					FU.[Login] AS FUNCIONARIO_LOGIN,
					FU.[Nome] AS FUNCIONARIO_NOME,
					FU.[Senha] AS FUNCIONARIO_SENHA
	            FROM 
		            [TBMEDICAMENTO] AS M LEFT JOIN
                    [TBFORNECEDOR] AS F
                ON
                    M.[FORNECEDOR_ID] = F.ID INNER JOIN
					[TBRequisicao] AS R
				ON
					R.Medicamento_Id = M.Id INNER JOIN
					[TBPaciente] AS P
				ON
					R.Paciente_Id = P.Id INNER JOIN
					[TBFuncionario] AS FU
				ON
					R.Funcionario_Id = FU.Id
";

        private const string sqlSelecionarPorID =
           @"SELECT 
					R.[Data] AS REQUISICAO_DATA,
					R.[ID] AS REQUISICAO_ID,
					R.[QuantidadeMedicamento] AS REQUISICAO_QUANTIDADE,
                    M.[ID] AS MEDICAMENTO_ID,
		            M.[NOME] AS MEDICAMENTO_NOME,
                    M.[DESCRICAO] AS MEDICAMENTO_DESCRICAO,
                    M.[LOTE] AS MEDICAMENTO_LOTE,
                    M.[VALIDADE] AS MEDICAMENTO_VALIDADE,
                    M.[QUANTIDADEDISPONIVEL] AS MEDICAMENTO_QUANTIDADE,
					F.[EMAIL] AS FORNECEDOR_EMAIL,
					F.[ESTADO] AS FORNECEDOR_ESTADO,
					F.[ID] AS FORNECEDOR_ID,
					F.[NOME] AS FORNECEDOR_NOME,
                    F.[CIDADE] AS FORNECEDOR_CIDADE,
					F.[TELEFONE] AS FORNECEDOR_TELEFONE,
					P.[Id] AS PACIENTE_ID,
					P.[CartaoSUS] AS PACIENTE_CARTAOSUS,
					P.[Nome] AS PACIENTE_NOME,
					FU.[Id] AS FUNCIONARIO_ID,
					FU.[Login] AS FUNCIONARIO_LOGIN,
					FU.[Nome] AS FUNCIONARIO_NOME,
					FU.[Senha] AS FUNCIONARIO_SENHA
	            FROM 
		            [TBMEDICAMENTO] AS M LEFT JOIN
                    [TBFORNECEDOR] AS F
                ON
                    M.[FORNECEDOR_ID] = F.ID INNER JOIN
					[TBRequisicao] AS R
				ON
					R.Medicamento_Id = M.Id INNER JOIN
					[TBPaciente] AS P
				ON
					R.Paciente_Id = P.Id INNER JOIN
					[TBFuncionario] AS FU
				ON
					R.Funcionario_Id = FU.Id
		        WHERE
                    R.[ID] = @ID";

        #endregion

        public ValidationResult Inserir(Requisicao registro)
        {
            var validador = new ValidadorRequisicao();

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

        public ValidationResult Editar(Requisicao registro)
        {
            var validador = new ValidadorRequisicao();

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

        public ValidationResult Excluir(Requisicao registro)
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

        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<Requisicao> registros = new List<Requisicao>();

            while (leitorRegistro.Read())
            {
                Requisicao registro = ConverterParaRegistro(leitorRegistro);

                registros.Add(registro);
            }

            conexaoComBanco.Close();

            return registros;
        }

        public Requisicao SelecionarPorID(int ID)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorID, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", ID);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            Requisicao registro = null;
            if (leitorRegistro.Read())
                registro = ConverterParaRegistro(leitorRegistro);

            conexaoComBanco.Close();

            return registro;
        }

        private Requisicao ConverterParaRegistro(SqlDataReader leitorRegistro)
        {

            int idPaciente = Convert.ToInt32(leitorRegistro["PACIENTE_ID"]);
            string nomePaciente = Convert.ToString(leitorRegistro["PACIENTE_NOME"]);
            string cartaoSUS = Convert.ToString(leitorRegistro["PACIENTE_CARTAOSUS"]);


            var paciente = new Paciente(nomePaciente, cartaoSUS);
            paciente.Id = idPaciente;


            int medicamentoId = Convert.ToInt32(leitorRegistro["MEDICAMENTO_ID"]);
            string medicamentoNome = Convert.ToString(leitorRegistro["MEDICAMENTO_NOME"]);
            string medicamentoDescricao = Convert.ToString(leitorRegistro["MEDICAMENTO_DESCRICAO"]);
            string medicamentoLote = Convert.ToString(leitorRegistro["MEDICAMENTO_LOTE"]);
            DateTime medicamentoValidade = Convert.ToDateTime(leitorRegistro["MEDICAMENTO_VALIDADE"]).Date;


            int fornecedorId = Convert.ToInt32(leitorRegistro["FORNECEDOR_ID"]);
            string fornecedorNome = Convert.ToString(leitorRegistro["FORNECEDOR_NOME"]);
            string fornecedorEmail = Convert.ToString(leitorRegistro["FORNECEDOR_EMAIL"]);
            string fornecedorEstado = Convert.ToString(leitorRegistro["FORNECEDOR_ESTADO"]);
            string fornecedorCidade = Convert.ToString(leitorRegistro["FORNECEDOR_CIDADE"]);
            string fornecedorTelefone = Convert.ToString(leitorRegistro["FORNECEDOR_TELEFONE"]);


            var fornecedor = new Fornecedor(fornecedorNome, fornecedorTelefone, fornecedorEmail, fornecedorCidade, fornecedorEstado);
            fornecedor.Id = fornecedorId;

            var medicamento = new Medicamento(medicamentoNome, medicamentoDescricao, medicamentoLote, medicamentoValidade);
            medicamento.Id = medicamentoId;
            medicamento.Fornecedor = fornecedor;

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

        private static void ConfigurarParametrosRegistro(Requisicao registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", registro.QtdMedicamento);
            cmdInserir.Parameters.AddWithValue("DATA", registro.Data);
            cmdInserir.Parameters.AddWithValue("FUNCIONARIO_ID", registro.Funcionario.Id);
            cmdInserir.Parameters.AddWithValue("MEDICAMENTO_ID", registro.Medicamento.Id);
            cmdInserir.Parameters.AddWithValue("PACIENTE_ID", registro.Paciente.Id);

        }
    }



}