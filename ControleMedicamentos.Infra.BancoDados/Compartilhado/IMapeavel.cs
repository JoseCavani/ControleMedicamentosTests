using ControleMedicamentos.Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Compartilhado
{
    public interface IMapeavel<T> where T : EntidadeBase<T>
    {
        abstract T ConverterParaRegistro(SqlDataReader leitorRegistro);

        abstract void ConfigurarParametrosRegistro(T registro, SqlCommand cmdInserir);

    }
}
