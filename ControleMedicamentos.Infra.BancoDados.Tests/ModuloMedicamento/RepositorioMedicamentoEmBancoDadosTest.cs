using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleMedicamentos.Infra.BancoDados.ModuloMedicamento;
using System.Data.SqlClient;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation.Results;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using System.Collections.Generic;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using System;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {

        Random random = new Random();

        private const string sqlExcluirMedicamento =
          @"  DELETE FROM TBREQUISICAO  DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0) DELETE FROM TBMEDICAMENTO  DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)";

        private const string sqlExcluirFornecedor =
          @"DELETE FROM TBFORNECEDOR  DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)";


        private const string enderecoBanco =
       "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public RepositorioMedicamentoEmBancoDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluirMedicamento, conexaoComBanco);
            SqlCommand comandoExclusaoFornecedor = new SqlCommand(sqlExcluirFornecedor, conexaoComBanco);


            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            comandoExclusaoFornecedor.ExecuteNonQuery();
            conexaoComBanco.Close();
        }


        [TestMethod]
        public void Deve_selecionar_todos_medicamentos()
        {


            RepositorioMedicamentoEmBancoDados repositorio = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();

            List<Medicamento> registros = new List<Medicamento>();


            Fornecedor fornecedor = new("a", "b", "c", "d", "e");
            repositorioFornecedor.Inserir(fornecedor);


            for (int i = 0; i < 10; i++)
            {
                Medicamento med = new(i.ToString(), "descricao1", "lote1", System.DateTime.Today);

                med.Fornecedor = fornecedor;

                repositorio.Inserir(med);

                Requisicao req = CriarRequisicao(med);
                med.Requisicoes.Add(req);

                registros.Add(med);
            }

            List<Medicamento> registrosDoBanco = repositorio.SelecionarTodos();

            for (int i = 0; i < registrosDoBanco.Count; i++)
            {
                repositorio.CarregarRequisicoes(registrosDoBanco[i]);
                Assert.AreEqual(registrosDoBanco[i], registros[i]);
            }


        }



        private Funcionario CriarEInserirFuncionario()
        {
            RepositorioFuncionarioEmBancoDados repositorioFuncionario = new();

            Funcionario funcionario = new(random.Next().ToString(), "b", "c");

            repositorioFuncionario.Inserir(funcionario);
            return funcionario;
        }



        private Paciente CriarEInserirPaciente()
        {
            RepositorioPacienteEmBancoDados repositorioPaciente = new();

            Paciente paciente = new(random.Next().ToString(), "b");
            repositorioPaciente.Inserir(paciente);
            return paciente;
        }


        private Requisicao CriarRequisicao(Medicamento med)
        {
            Paciente paciente = CriarEInserirPaciente();

            Funcionario funcionario = CriarEInserirFuncionario();


            RepositorioRequisicaoeEmBancoDados repositorio = new();

            Requisicao requisicao = new(med, paciente, random.Next(), DateTime.Now, funcionario);

            repositorio.Inserir(requisicao);

            return requisicao;
        }



        [TestMethod]
        public void Deve_inserir_medicamento()
        {
            RepositorioMedicamentoEmBancoDados repositorio = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();


            Fornecedor fornecedor = new(random.Next().ToString(), "b","c","d","e");
            repositorioFornecedor.Inserir(fornecedor);



            Medicamento med = new("medicamento1", "descricao1", "lote1", System.DateTime.Today);

            med.Fornecedor = fornecedor;
            repositorio.Inserir(med);


           CriarRequisicao(med);

            repositorio.CarregarRequisicoes(med);



            Medicamento med2 = repositorio.SelecionarPorID(med.Id);
            repositorio.CarregarRequisicoes(med2);

            Assert.AreEqual(med, med2);

        }
        [TestMethod]
        public void Deve_excluir_medicamento()
        {

            RepositorioMedicamentoEmBancoDados repositorio = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();

            Fornecedor fornecedor = new(random.Next().ToString(), "b", "c", "d", "e");
            repositorioFornecedor.Inserir(fornecedor);



            Medicamento med = new("medicamento1", "descricao1", "lote1", System.DateTime.Today);

            med.Fornecedor = fornecedor;


            repositorio.Inserir(med);

            ValidationResult result = repositorio.Excluir(med);


            Assert.AreEqual(result.Errors.Count, 0);

        }

    

        [TestMethod]
        public void Deve_editar_medicamento()
        {

            RepositorioMedicamentoEmBancoDados repositorio = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();

            Fornecedor fornecedor = new(random.Next().ToString(), "b", "c", "d", "e");
            repositorioFornecedor.Inserir(fornecedor);


            Medicamento med = new("medicamento1", "descricao1", "lote1", System.DateTime.Today);

            med.Fornecedor = fornecedor;


            repositorio.Inserir(med);

            med.Nome = "qqqq";
            med.Descricao = "bbb";
            med.Lote = "ssss";


            repositorio.Editar(med);

            CriarRequisicao(med);

            repositorio.CarregarRequisicoes(med);


            Medicamento med2 = repositorio.SelecionarPorID(med.Id);

            repositorio.CarregarRequisicoes(med2);

            Assert.AreEqual(med, med2);


        }

        [TestMethod]
        public void Deve_Mostrar_Mais_Requisitados()
        {
            RepositorioMedicamentoEmBancoDados repositorio = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();


            Fornecedor fornecedor = new(random.Next().ToString(), "b", "c", "d", "e");
            repositorioFornecedor.Inserir(fornecedor);


            for (int i = 0; i < 3; i++)
            {
                Medicamento med = new(random.Next().ToString(), "descricao1", "lote1", System.DateTime.Today);
                med.Fornecedor = fornecedor;
                repositorio.Inserir(med);
                int number = random.Next(1, 10);
                for (int i2 = 0; i2 < number; i2++)
                {
                    CriarRequisicao(med);
                }
            }


           

          List<Medicamento> medicamentos =  repositorio.SelecionarTodos();


            foreach (var medicamento in medicamentos)
            {
                repositorio.CarregarRequisicoes(medicamento);
            }
            medicamentos.Sort();

            Assert.IsTrue(medicamentos[0].QuantidadeRequisicoes >= medicamentos[1].QuantidadeRequisicoes && medicamentos[1].QuantidadeRequisicoes >= medicamentos[2].QuantidadeRequisicoes);

        }




    }
}
