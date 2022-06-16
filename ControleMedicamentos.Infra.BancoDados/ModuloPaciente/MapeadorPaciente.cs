using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class MapeadorPaciente : IMapeavel<Paciente>
    {
        public Paciente ConverterParaRegistro(SqlDataReader leitorRegistro)
        {

            int idPaciente = Convert.ToInt32(leitorRegistro["ID"]);
            string nomePaciente = Convert.ToString(leitorRegistro["NOME"]);
            string cartaoSUS = Convert.ToString(leitorRegistro["CARTAOSUS"]);




            var Paciente = new Paciente(nomePaciente, cartaoSUS);
            Paciente.Id = idPaciente;

            return Paciente;
        }

        public void ConfigurarParametrosRegistro(Paciente registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("NOME", registro.Nome);
            cmdInserir.Parameters.AddWithValue("CARTAOSUS", registro.CartaoSUS);

        }
    }
}
