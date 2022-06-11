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

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        [TestMethod]
        public void Deve_selecionar_todos_requisicoes()
        {
            RepositorioRequisicaoeEmBancoDados repositorio = new();

            List<Requisicao> registros = new List<Requisicao>();

            Paciente paciente = CriarEInserirPaciente();
            Medicamento med = CriarEInserirMedicamento();

            Funcionario funcionario = CriarEInserirFuncionario();

            for (int i = 0; i < 10; i++)
            {


                Requisicao requisicao = new(med, paciente, i, DateTime.Now, funcionario);

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
        public void Deve_inserir_Requisicao()
        {
            Paciente paciente = CriarEInserirPaciente();
            Medicamento med = CriarEInserirMedicamento();

            Funcionario funcionario = CriarEInserirFuncionario();


            RepositorioRequisicaoeEmBancoDados repositorio = new();

            Requisicao requisicao = new(med, paciente,5,DateTime.Now,funcionario);

            repositorio.Inserir(requisicao);

            Requisicao requisicao2 =  repositorio.SelecionarPorID(requisicao.Id);




            Assert.AreEqual(requisicao, requisicao2);

        }
        [TestMethod]
        public void Deve_excluir_Requisicao()
        {
            Paciente paciente = CriarEInserirPaciente();
            Medicamento med = CriarEInserirMedicamento();

            Funcionario funcionario = CriarEInserirFuncionario();



            RepositorioRequisicaoeEmBancoDados repositorio = new();

            Requisicao requisicao = new(med, paciente, 5, DateTime.Now, funcionario);

            repositorio.Inserir(requisicao);

            ValidationResult result = repositorio.Excluir(requisicao);


            Assert.AreEqual(result.Errors.Count, 0);

        }

        [TestMethod]
        public void Deve_editar_Requsiscao()
        {

            Paciente paciente = CriarEInserirPaciente();

            Medicamento med = CriarEInserirMedicamento();

            Funcionario funcionario = CriarEInserirFuncionario();


            RepositorioRequisicaoeEmBancoDados repositorio = new();

            Requisicao requisicao = new(med, paciente, 5, DateTime.Now, funcionario);

            repositorio.Inserir(requisicao);

            requisicao.QtdMedicamento = 9;


            repositorio.Editar(requisicao);

            Requisicao requisicao2 = repositorio.SelecionarPorID(requisicao.Id);




            Assert.AreEqual(requisicao, requisicao2);


        }


        [TestMethod]
        public void Nao_deve_excluir_paceinte_ou_funcionario_ou_medicamento_com_requisicao()
        {
            Paciente paciente = CriarEInserirPaciente();
            Medicamento med = CriarEInserirMedicamento();

            Funcionario funcionario = CriarEInserirFuncionario();


            RepositorioRequisicaoeEmBancoDados repositorio = new();

            Requisicao requisicao = new(med, paciente, 5, DateTime.Now, funcionario);

            repositorio.Inserir(requisicao);

         
            
            
            
            RepositorioFuncionarioEmBancoDados repositorioFuncionario = new();

           ValidationResult result = repositorioFuncionario.Excluir(funcionario);

            Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));

          
            
            
            
            RepositorioPacienteEmBancoDados repositorioPaciente = new();

            ValidationResult result2 = repositorioPaciente.Excluir(paciente);

            Assert.IsTrue(result2.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));

         
            
            
            
            
            RepositorioMedicamentoEmBancoDados repositorioMedicamento = new();

            ValidationResult result3 = repositorioMedicamento.Excluir(med);

            Assert.IsTrue(result3.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));

        }


        private  Funcionario CriarEInserirFuncionario()
        {
            RepositorioFuncionarioEmBancoDados repositorioFuncionario = new();

            Funcionario funcionario = new("a", "b", "c");

            repositorioFuncionario.Inserir(funcionario);
            return funcionario;
        }

        private  Medicamento CriarEInserirMedicamento()
        {
            RepositorioMedicamentoEmBancoDados repositorioMedicamento = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();


            Fornecedor fornecedor = new("a", "b", "c", "d", "e");
            repositorioFornecedor.Inserir(fornecedor);



            Medicamento med = new("medicamento1", "descricao1", "lote1", System.DateTime.Today);

            med.Fornecedor = fornecedor;


            repositorioMedicamento.Inserir(med);
            return med;
        }

        private Paciente CriarEInserirPaciente()
        {
            RepositorioPacienteEmBancoDados repositorioPaciente = new();

            Paciente paciente = new("a", "b");
            repositorioPaciente.Inserir(paciente);
            return paciente;
        }


    }
}
