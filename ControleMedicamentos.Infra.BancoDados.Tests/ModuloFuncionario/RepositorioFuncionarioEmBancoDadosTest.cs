using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        private const string sqlExcluir =
          @"DELETE FROM TBFuncionario  DBCC CHECKIDENT (TBFuncionario, RESEED, 0)";



        private const string enderecoBanco =
       "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";



        public RepositorioFuncionarioEmBancoDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        [TestMethod]
        public void Deve_inserir_Funcionario()
        {
            RepositorioFuncionarioEmBancoDados repositorio = new();

            Funcionario funcionario = new("a", "b", "c");

            repositorio.Inserir(funcionario);

            Funcionario Funcionario2 = repositorio.SelecionarPorID(funcionario.Id);

            Assert.AreEqual(funcionario, Funcionario2);

        }
        [TestMethod]
        public void Deve_excluir_Funcionario()
        {

            RepositorioFuncionarioEmBancoDados repositorio = new();

            Funcionario Funcionario = new("a", "b", "c");

            repositorio.Inserir(Funcionario);

            ValidationResult result = repositorio.Excluir(Funcionario);


            Assert.AreEqual(result.Errors.Count, 0);

        }
        [TestMethod]
        public void Deve_selecionar_todos_funcionarios()
        {
            RepositorioFuncionarioEmBancoDados repositorio = new();
            List<Funcionario> registros = new List<Funcionario>();


            for (int i = 0; i < 10; i++)
            {
                Funcionario funcionario = new(i.ToString(), "b", "c");

                repositorio.Inserir(funcionario);
                registros.Add(funcionario);
            }

            List<Funcionario> registrosDoBanco = repositorio.SelecionarTodos();

            for (int i = 0; i < registrosDoBanco.Count; i++)
            {
                Assert.AreEqual(registrosDoBanco[i], registros[i]);
            }


        }

        [TestMethod]
        public void Deve_editar_Funcionario()
        {

            RepositorioFuncionarioEmBancoDados repositorio = new();

            Funcionario Funcionario = new("a", "b", "c");

            repositorio.Inserir(Funcionario);

            Funcionario.Nome = "ssssss";

            repositorio.Editar(Funcionario);

            Funcionario Funcionario2 = repositorio.SelecionarPorID(Funcionario.Id);


            Assert.AreEqual(Funcionario2, Funcionario);

        }

    }
}


