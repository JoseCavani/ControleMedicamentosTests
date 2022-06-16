using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class RepositorioFuncionarioEmBancoDados : RepositorioBaseEmBancoDeDados<Funcionario, ValidadorFuncionario, MapeadorFuncionario>
    {
        public RepositorioFuncionarioEmBancoDados() : base(new ValidadorFuncionario(), new MapeadorFuncionario())
        {

        }


        #region Sql Queries

         protected override string sqlInserir
        {
            get =>
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
        }
         protected override string sqlEditar
        {
            get =>
            @"UPDATE [TBFUNCIONARIO]	
		        SET
			       [NOME] = @NOME,
                    [LOGIN] = @LOGIN,
                    [SENHA] = @SENHA
		        WHERE
			        [ID] = @ID";
        }
         protected override string sqlExcluir
        {
            get =>
            @"DELETE FROM [TBFUNCIONARIO]
		        WHERE
			        [ID] = @ID";
        }
         protected override string sqlSelecionarTodos
        {
            get =>
            @"SELECT 
                    [NOME],
					[LOGIN],
					[SENHA],
					[ID]
	            FROM 
                    [TBFUNCIONARIO]
";
        }
         protected override string sqlSelecionarPorID
        {
            get =>
                   @"SELECT 
                    [NOME],
					[LOGIN],
					[SENHA],
					[ID]
	            FROM 
                    [TBFUNCIONARIO]
		        WHERE
                    [ID] = @ID";
        }
        #endregion


    }

}

