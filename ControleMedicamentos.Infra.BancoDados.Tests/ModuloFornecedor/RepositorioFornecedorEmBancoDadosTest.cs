using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Tests.ModuloCompartilhado;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FluentAssertions;

namespace Controlefornecedors.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest :BaseTestRepositorio
    {
    
        RepositorioMedicamentoEmBancoDados repositorioMedicamento = new();
        RepositorioFornecedorEmBancoDados repositorio = new();

        Random random = new Random();



        [TestMethod]
        public void Deve_selecionar_por_id()
        {
            Fornecedor registro = CriarFornecedor();
            repositorio.Inserir(registro);

            Fornecedor registro2 = repositorio.SelecionarPorID(registro.Id);

            registro2.Should().NotBeNull();
            registro2.Should().Be(registro);
        }



        [TestMethod]
        public void Deve_inserir_fornecedor()
        {
            Fornecedor fornecedor = CriarFornecedor();

            repositorio.Inserir(fornecedor);

            Fornecedor fornecedor2 = repositorio.SelecionarPorID(fornecedor.Id);

            fornecedor2.Should().NotBeNull();
            fornecedor2.Should().Be(fornecedor);

        }

        private Fornecedor CriarFornecedor()
        {
            return new(random.Next().ToString(), "b", "c", "d", "e");
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {

            Fornecedor fornecedor = CriarFornecedor();

            repositorio.Inserir(fornecedor);

             repositorio.Excluir(fornecedor);

            repositorio.SelecionarPorID(fornecedor.Id)
                          .Should().BeNull();

        }
        [TestMethod]
        public void Deve_selecionar_todos_fornecedores()
        {
            List<Fornecedor> fornecedors = new List<Fornecedor>();


            for (int i = 0; i < 10; i++)
            {
                Fornecedor fornecedor = CriarFornecedor();
                repositorio.Inserir(fornecedor);
                fornecedors.Add(fornecedor);
            }

            List<Fornecedor> fornecedorsDoBanco = repositorio.SelecionarTodos();

            for (int i = 0; i < fornecedorsDoBanco.Count; i++)
            {

                fornecedorsDoBanco[i].Should().NotBeNull();
                fornecedorsDoBanco[i].Should().Be(fornecedors[i]);
            }


        }


        [TestMethod]
        public void Deve_editar_fornecedor()
        {

            Fornecedor fornecedor = CriarFornecedor();
      
            repositorio.Inserir(fornecedor);

            fornecedor.Nome = "ssssss";

            repositorio.Editar(fornecedor);

            Fornecedor fornecedor2 = repositorio.SelecionarPorID(fornecedor.Id);


            fornecedor2.Should().NotBeNull();
            fornecedor2.Should().Be(fornecedor);

        }

        [TestMethod]
        public void Nao_Deve_excluir_fornecedor_que_tem_Medicamento()
        {



            Fornecedor fornecedor = CriarFornecedor();

            repositorio.Inserir(fornecedor);


            Medicamento med = new(random.Next().ToString(), "descricao1", "lote1", System.DateTime.Now);

            med.Fornecedor = fornecedor;


            Requisicao requ1 = new(med, new Paciente("a", "b"), 5, System.DateTime.Now, new Funcionario("a", "v", "c"));
            Requisicao requ2 = new(med, new Paciente("a", "b"), 5, System.DateTime.Now, new Funcionario("a", "v", "c"));
            Requisicao requ3 = new(med, new Paciente("a", "b"), 5, System.DateTime.Now, new Funcionario("a", "v", "c"));

            med.Requisicoes.Add(requ1);
            med.Requisicoes.Add(requ2);
            med.Requisicoes.Add(requ3);

            repositorioMedicamento.Inserir(med);



            ValidationResult result = repositorio.Excluir(fornecedor);


            Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));

        }


    }
}
