using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDados : RepositorioBaseEmBancoDeDados<Fornecedor, ValidadorFornecedor, MapeadorFornecedor>
    {
        public RepositorioFornecedorEmBancoDados() : base(new ValidadorFornecedor(), new MapeadorFornecedor())
        {

        }


        #region Sql Queries

        protected override string sqlInserir {

            get =>
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
    }
        protected override string sqlEditar
        {
            get =>
            @"UPDATE [TBFORNECEDOR]	
		        SET
			       [NOME] = @NOME,
                    [TELEFONE] = @TELEFONE,
                    [EMAIL] = @EMAIL,
                    [CIDADE] = @CIDADE,
                    [ESTADO] = @ESTADO
		        WHERE
			        [ID] = @ID";
        }

        protected override string sqlExcluir
        {
            get =>
            @"DELETE FROM [TBFORNECEDOR]
		        WHERE
			        [ID] = @ID";
        }

        protected override string sqlSelecionarTodos
        {
            get =>
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
        }

        protected override string sqlSelecionarPorID
        {
            get =>
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
        }

        #endregion


    
    }



}

