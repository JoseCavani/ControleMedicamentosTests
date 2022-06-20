using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.Tests.ModuloCompartilhado;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest : BaseTestRepositorio
    {
      
        Random random = new Random();

        RepositorioPacienteEmBancoDados repositorio;

        public RepositorioPacienteEmBancoDadosTest()
        {
    

            repositorio = new();

  
        }

        [TestMethod]
        public void Deve_selecionar_por_id()
        {
            Paciente registro = CriarPaciente();
            repositorio.Inserir(registro);

            Paciente registro2 = repositorio.SelecionarPorID(registro.Id);

            Assert.AreEqual(registro2, registro);
        }

        [TestMethod]
        public void Deve_selecionar_todos_Pacientes()
        {

            List<Paciente> registros = new List<Paciente>();


            for (int i = 0; i < 10; i++)
            {

                Paciente paciente = CriarPaciente();

                repositorio.Inserir(paciente);
                registros.Add(paciente);
            }

            List<Paciente> registrosDoBanco = repositorio.SelecionarTodos();

            for (int i = 0; i < registrosDoBanco.Count; i++)
            {
                Assert.AreEqual(registrosDoBanco[i], registros[i]);
            }


        }

        [TestMethod]
        public void Deve_inserir_Paciente()
        {

            Paciente paciente = CriarPaciente();

            repositorio.Inserir(paciente);

            Paciente Paciente2 = repositorio.SelecionarPorID(paciente.Id);

            Assert.AreEqual(paciente, Paciente2);

        }
        [TestMethod]
        public void Deve_excluir_Paciente()
        {


            Paciente Paciente = CriarPaciente();

            repositorio.Inserir(Paciente);

            ValidationResult result = repositorio.Excluir(Paciente);


            Assert.AreEqual(result.Errors.Count, 0);

        }
        [TestMethod]
        public void Deve_editar_Paciente()
        {

            Paciente Paciente = CriarPaciente();

            repositorio.Inserir(Paciente);

            Paciente.Nome = "ssssss";

            repositorio.Editar(Paciente);

            Paciente Paciente2 = repositorio.SelecionarPorID(Paciente.Id);


            Assert.AreEqual(Paciente2, Paciente);

        }

        private Paciente CriarPaciente()
        {
           return new(random.Next().ToString(), "b");

        }

    }
}
