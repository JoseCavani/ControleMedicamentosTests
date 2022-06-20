using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Tests.ModuloCompartilhado;
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
    public class RepositorioFuncionarioEmBancoDadosTest : BaseTestRepositorio
    {
     

        Random  random = new Random();

        RepositorioFuncionarioEmBancoDados repositorio = new();



        [TestMethod]
        public void Deve_selecionar_por_id()
        {
            Funcionario registro = CriarFuncionario();
            repositorio.Inserir(registro);

            Funcionario registro2 = repositorio.SelecionarPorID(registro.Id);

            Assert.AreEqual(registro2, registro);
        }



        [TestMethod]
        public void Deve_inserir_Funcionario()
        {

            Funcionario funcionario = CriarFuncionario();

            repositorio.Inserir(funcionario);

            Funcionario Funcionario2 = repositorio.SelecionarPorID(funcionario.Id);

            Assert.AreEqual(funcionario, Funcionario2);

        }

        private  Funcionario CriarFuncionario()
        {
            return new(random.Next().ToString(), "b", "c");
        }

        [TestMethod]
        public void Deve_excluir_Funcionario()
        {


            Funcionario Funcionario = CriarFuncionario();

            repositorio.Inserir(Funcionario);

            ValidationResult result = repositorio.Excluir(Funcionario);


            Assert.AreEqual(result.Errors.Count, 0);

        }
        [TestMethod]
        public void Deve_selecionar_todos_funcionarios()
        {
            List<Funcionario> registros = new List<Funcionario>();

            for (int i = 0; i < 10; i++)
            {
                Funcionario funcionario = CriarFuncionario();

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


            Funcionario Funcionario = CriarFuncionario();

            repositorio.Inserir(Funcionario);

            Funcionario.Nome = "ssssss";

            repositorio.Editar(Funcionario);

            Funcionario Funcionario2 = repositorio.SelecionarPorID(Funcionario.Id);


            Assert.AreEqual(Funcionario2, Funcionario);

        }

    }
}


