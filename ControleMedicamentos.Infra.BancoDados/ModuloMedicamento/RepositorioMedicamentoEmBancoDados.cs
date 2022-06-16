using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados : RepositorioBaseEmBancoDeDados<Medicamento, ValidadorMedicamento, MapeadorMedicamento>
    {

        MapeadorMedicamento mapeador;

        public RepositorioMedicamentoEmBancoDados() : base(new ValidadorMedicamento(), new MapeadorMedicamento())
        {
            mapeador = new MapeadorMedicamento();
        }

        #region Sql Queries

        protected override string sqlInserir
        {
            get =>
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
        }
        protected override string sqlEditar
        {
            get =>
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
        }
        protected override string sqlExcluir
        {
            get =>
            @"DELETE FROM [TBMEDICAMENTO]
		        WHERE
			        [ID] = @ID";
        }

       private const string sqlSelecionarEmFalta  =
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
        

        protected override string sqlSelecionarTodos
        {
            get =>
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
        }
        protected override string sqlSelecionarPorID
        {
            get =>
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
        }
        #endregion


        public List<Medicamento> SelecionarEmFalta()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarEmFalta, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<Medicamento> registros = new List<Medicamento>();

            while (leitorRegistro.Read())
            {
                Medicamento registro = mapeador.ConverterParaRegistro(leitorRegistro);

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


    

     
    }
}

