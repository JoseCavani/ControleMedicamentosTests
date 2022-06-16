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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoeEmBancoDados : RepositorioBaseEmBancoDeDados<Requisicao, ValidadorRequisicao, MapeadorRequisicao>
	{

		public RepositorioRequisicaoeEmBancoDados() : base(new ValidadorRequisicao(), new MapeadorRequisicao())
		{

		}

		#region Sql Queries

		protected override string sqlInserir
		{
			get =>
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
		}
        protected override string sqlEditar
		{
			get =>
			@"UPDATE [TBREQUISICAO]	
							SET
								[QUANTIDADEMEDICAMENTO] = @QUANTIDADEMEDICAMENTO,
								[DATA] = @DATA,
								[FUNCIONARIO_ID]= @FUNCIONARIO_ID,
								[PACIENTE_ID] = @PACIENTE_ID,
								[MEDICAMENTO_ID] = @MEDICAMENTO_ID
							WHERE
									[ID] = @ID";
		}
        protected override string sqlExcluir
		{
			get =>
			@"DELETE FROM [TBREQUISICAO]
							WHERE
								[ID] = @ID";
		}

        protected override string sqlSelecionarTodos
		{
			get =>
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
		}
        protected override string sqlSelecionarPorID
		{
			get =>
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
		}
        #endregion

   
    }



}