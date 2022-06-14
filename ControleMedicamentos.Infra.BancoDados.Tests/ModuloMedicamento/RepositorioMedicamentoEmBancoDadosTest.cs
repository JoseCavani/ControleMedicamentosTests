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

        RepositorioMedicamentoEmBancoDados repositorio = new();

        RepositorioFornecedorEmBancoDados repositorioFornecedor = new();


        private const string sqlExcluirMedicamento =
          @"  DELETE FROM TBREQUISICAO  DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0) DELETE FROM TBMEDICAMENTO  DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)";

        private const string sqlExcluirFornecedor =
          @"DELETE FROM TBFORNECEDOR  DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)";

        private const string sqlExcluirRequisicao =
        @"DELETE FROM TBREQUISICAO  DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)";


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

            List<Medicamento> registros = new List<Medicamento>();

         


            for (int i = 0; i < 10; i++)
            {

                Medicamento med = CriarEInserirMedicamento();


                registros.Add(med);
            }

            List<Medicamento> registrosDoBanco = repositorio.SelecionarTodos();

            for (int i = 0; i < registrosDoBanco.Count; i++)
            {
                repositorio.CarregarRequisicoes(registrosDoBanco[i]);
                Assert.AreEqual(registrosDoBanco[i], registros[i]);
            }


        }

        private Medicamento CriarEInserirMedicamento()
        {
            Medicamento medicamento = new(random.Next().ToString(), "descricao1", "lote1", System.DateTime.Today);

            Fornecedor fornecedor = CriarFornecedor();
            repositorioFornecedor.Inserir(fornecedor);


            medicamento.QuantidadeDisponivel = random.Next(1, 10);
            medicamento.Fornecedor = fornecedor;


            repositorio.Inserir(medicamento);

            CriarRequisicao(medicamento);


            repositorio.CarregarRequisicoes(medicamento);

            return medicamento;
        }

        private  Fornecedor CriarFornecedor()
        {
            return new(random.Next().ToString(), "b", "c", "d", "e");
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
        public void Deve_mostrar_medicamento_com_pouca_quantidade()
        {
            for (int i = 0; i < 3; i++)
            {

                Medicamento med = CriarEInserirMedicamento();
                int number = random.Next(1, 10);
                for (int i2 = 0; i2 < number; i2++)
                {
                    CriarRequisicao(med);
                }
            }




            List<Medicamento> medicamentos = repositorio.SelecionarEmFalta();

            foreach (var item in medicamentos)
            {
                Assert.IsTrue(item.QuantidadeDisponivel <  5);

            }


        }

        [TestMethod]
        public void Deve_selecionar_por_id()
        {


            Medicamento registro = CriarEInserirMedicamento();

            Medicamento registro2 = repositorio.SelecionarPorID(registro.Id);

            repositorio.CarregarRequisicoes(registro2);

            Assert.AreEqual(registro2, registro);
        }

        [TestMethod]
        public void Deve_inserir_medicamento()
        {

            Medicamento med = CriarEInserirMedicamento();

            Medicamento med2 = repositorio.SelecionarPorID(med.Id);
            repositorio.CarregarRequisicoes(med2);

            Assert.AreEqual(med, med2);

        }
        [TestMethod]
        public void Deve_excluir_medicamento()
        {

            Medicamento med = CriarEInserirMedicamento();

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusaoRequisicao = new SqlCommand(sqlExcluirFornecedor, conexaoComBanco);
            SqlCommand comandoExclusaoFornecedor = new SqlCommand(sqlExcluirRequisicao, conexaoComBanco);


            conexaoComBanco.Open();
            comandoExclusaoFornecedor.ExecuteNonQuery();
            conexaoComBanco.Close();

            ValidationResult result = repositorio.Excluir(med);


            Assert.AreEqual(result.Errors.Count, 0);

        }

        [TestMethod]
        public void Deve_editar_medicamento()
        {

            Medicamento med = CriarEInserirMedicamento();


            med.Nome = "qqqq";
            med.Descricao = "bbb";
            med.Lote = "ssss";


            repositorio.Editar(med);



            Medicamento med2 = repositorio.SelecionarPorID(med.Id);

            repositorio.CarregarRequisicoes(med2);

            Assert.AreEqual(med, med2);


        }

        [TestMethod]
        public void Deve_Mostrar_Mais_Requisitados()
        {

            for (int i = 0; i < 3; i++)
            {
                Medicamento med = CriarEInserirMedicamento();
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
