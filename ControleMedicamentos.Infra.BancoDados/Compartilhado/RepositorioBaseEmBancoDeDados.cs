using ControleMedicamentos.Dominio;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Compartilhado
{
    public abstract class RepositorioBaseEmBancoDeDados<T,TValidador,TMapeamento> 
        where T : EntidadeBase<T>
        where TValidador : AbstractValidator<T>
        where TMapeamento : IMapeavel<T>
    {
        #region abstract sqls

        protected const string enderecoBanco =
 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected abstract string sqlInserir { get; }
        protected abstract string sqlEditar { get; }
        protected abstract string sqlExcluir { get; }
        protected abstract string sqlSelecionarTodos { get; }
        protected abstract string sqlSelecionarPorID { get; }
        #endregion


        #region variaveis
        TValidador validador;
        TMapeamento mapeador;
        #endregion


        #region construtor
        public RepositorioBaseEmBancoDeDados(AbstractValidator<T> validationRules, IMapeavel<T> mapeavel)
        {
            this.validador = (TValidador)validationRules;
            this.mapeador = (TMapeamento)mapeavel;
        }
        #endregion

        public ValidationResult Inserir(T registro)
        {

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexao = new SqlConnection(enderecoBanco);
            SqlCommand cmdInserir = new SqlCommand(sqlInserir, conexao);

            mapeador.ConfigurarParametrosRegistro(registro, cmdInserir);
            conexao.Open();

            var ID = cmdInserir.ExecuteScalar();

            registro.Id = Convert.ToInt32(ID);
            conexao.Close();
            return resultadoValidacao;

        }

        public ValidationResult Editar(T registro)
        {

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            mapeador.ConfigurarParametrosRegistro(registro, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(T registro)
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

        public List<T> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            List<T> registros = new List<T>();

            while (leitorRegistro.Read())
            {
                T registro = mapeador.ConverterParaRegistro(leitorRegistro);

                registros.Add(registro);
            }

            conexaoComBanco.Close();

            return registros;
        }

        public T SelecionarPorID(int ID)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorID, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", ID);

            conexaoComBanco.Open();
            SqlDataReader leitorRegistro = comandoSelecao.ExecuteReader();

            T registro = null;
            if (leitorRegistro.Read())
                registro = mapeador.ConverterParaRegistro(leitorRegistro);

            conexaoComBanco.Close();

            return registro;
        }

    }
}
