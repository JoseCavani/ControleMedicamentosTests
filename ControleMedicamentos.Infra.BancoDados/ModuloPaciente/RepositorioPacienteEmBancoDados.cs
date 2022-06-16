using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDados : RepositorioBaseEmBancoDeDados<Paciente, ValidadorPaciente, MapeadorPaciente>
    {
        public RepositorioPacienteEmBancoDados() : base(new ValidadorPaciente(), new MapeadorPaciente())
        {

        }

        #region Sql Queries

         protected override string sqlInserir
        {
            get =>
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
        }
         protected override string sqlEditar 
        {
            get =>
            @"UPDATE [TBPACIENTE]	
		        SET
			       [NOME] = @NOME,
                    [CARTAOSUS] = @CARTAOSUS
		        WHERE
			        [ID] = @ID";
        }
         protected override string sqlExcluir
        {
            get =>
            @"DELETE FROM [TBPACIENTE]
		        WHERE
			        [ID] = @ID";
        }
         protected override string sqlSelecionarTodos
        {
            get =>
@"SELECT 
                    [NOME],
					[CARTAOSUS],
					[ID]
	            FROM 
                    [TBPACIENTE]
";
        }
         protected override string sqlSelecionarPorID
        {
            get =>
@"SELECT 
                    [NOME],
					[CARTAOSUS],
					[ID]
	            FROM 
                    [TBPACIENTE]
		        WHERE
                    [ID] = @ID";
        }
        #endregion

     
    }



}
