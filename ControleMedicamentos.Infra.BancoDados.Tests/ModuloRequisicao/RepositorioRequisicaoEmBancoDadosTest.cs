using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        RepositorioRequisicaoeEmBancoDados repositorio;
        Random random = new Random();
        private const string sqlExcluir =
      @"
        DELETE FROM TBREQUISICAO  DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)
        DELETE FROM TBPACIENTE  DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)
        DELETE FROM TBFuncionario  DBCC CHECKIDENT (TBFuncionario, RESEED, 0) 
        DELETE FROM TBMEDICAMENTO  DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0) 
        DELETE FROM TBFORNECEDOR  DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0) ";



        private const string enderecoBanco =
       "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";



        public RepositorioRequisicaoEmBancoDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);


            repositorio = new();

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        [TestMethod]
        public void Deve_selecionar_todos_requisicoes()
        {

            List<Requisicao> registros = new List<Requisicao>();


            for (int i = 0; i < 10; i++)
            {


                Requisicao requisicao = CriarRequisicao();

                repositorio.Inserir(requisicao);

                registros.Add(requisicao);
            }

            List<Requisicao> registrosDoBanco = repositorio.SelecionarTodos();

            for (int i = 0; i < registrosDoBanco.Count; i++)
            {
                Assert.AreEqual(registrosDoBanco[i], registros[i]);
            }


        }
        [TestMethod]
        public void Deve_selecionar_por_id()
        {
            Requisicao req = CriarRequisicao();
            repositorio.Inserir(req);

            Requisicao req2 = repositorio.SelecionarPorID(req.Id);

            Assert.AreEqual(req2, req);
        }

        [TestMethod]
        public void Deve_inserir_Requisicao()
        {

            Requisicao requisicao = CriarRequisicao();

            repositorio.Inserir(requisicao);

            Requisicao requisicao2 =  repositorio.SelecionarPorID(requisicao.Id);

            Assert.AreEqual(requisicao, requisicao2);

        }
        [TestMethod]
        public void Deve_excluir_Requisicao()
        {
          

            Requisicao requisicao = CriarRequisicao();

            repositorio.Inserir(requisicao);

            ValidationResult result = repositorio.Excluir(requisicao);


            Assert.AreEqual(result.Errors.Count, 0);

        }

        [TestMethod]
        public void Deve_editar_Requsiscao()
        {

            Requisicao requisicao = CriarRequisicao();

            repositorio.Inserir(requisicao);

            requisicao.QtdMedicamento = 9;


            repositorio.Editar(requisicao);

            Requisicao requisicao2 = repositorio.SelecionarPorID(requisicao.Id);


            Assert.AreEqual(requisicao, requisicao2);


        }

        [TestMethod]
        public void Nao_deve_excluir_funcionario_com_requisicao()
        {

            RepositorioFuncionarioEmBancoDados repositorioFuncionario = new();


            Requisicao requisicao = CriarRequisicao();

            repositorio.Inserir(requisicao);

            ValidationResult result = repositorioFuncionario.Excluir(requisicao.Funcionario);

            Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));


        }

        [TestMethod]
        public void Nao_deve_excluir_paceinte_com_requisicao()
        {

            Requisicao requisicao = CriarRequisicao();

            repositorio.Inserir(requisicao);


            RepositorioPacienteEmBancoDados repositorioPaciente = new();

            ValidationResult result2 = repositorioPaciente.Excluir(requisicao.Paciente);

            Assert.IsTrue(result2.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));

        }


        [TestMethod]
        public void Nao_deve_excluir_medicamento_com_requisicao()
        {

            Requisicao requisicao = CriarRequisicao();

            repositorio.Inserir(requisicao);

            
            RepositorioMedicamentoEmBancoDados repositorioMedicamento = new();

            ValidationResult result3 = repositorioMedicamento.Excluir(requisicao.Medicamento);

            Assert.IsTrue(result3.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));

        }

        private Requisicao CriarRequisicao()
        {
            Paciente paciente = CriarEInserirPaciente();
            Medicamento med = CriarEInserirMedicamento();

            Funcionario funcionario = CriarEInserirFuncionario();


           return new(med, paciente, random.Next(), DateTime.Now, funcionario);
        }


        private  Funcionario CriarEInserirFuncionario()
        {
            RepositorioFuncionarioEmBancoDados repositorioFuncionario = new();

            Funcionario funcionario = new(random.Next().ToString(), "b", "c");

            repositorioFuncionario.Inserir(funcionario);
            return funcionario;
        }

        private  Medicamento CriarEInserirMedicamento()
        {
            RepositorioMedicamentoEmBancoDados repositorioMedicamento = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();


            Fornecedor fornecedor = new(random.Next().ToString(), "b", "c", "d", "e");
            repositorioFornecedor.Inserir(fornecedor);



            Medicamento med = new(random.Next().ToString(), "descricao1", "lote1", System.DateTime.Today);

            med.Fornecedor = fornecedor;


            repositorioMedicamento.Inserir(med);
            return med;
        }

        private Paciente CriarEInserirPaciente()
        {
            RepositorioPacienteEmBancoDados repositorioPaciente = new();

            Paciente paciente = new(random.Next().ToString(), "b");
            repositorioPaciente.Inserir(paciente);
            return paciente;
        }


    }
}
