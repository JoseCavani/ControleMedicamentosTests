using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloMedicamento;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlefornecedors.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
        private const string sqlExcluirMedicamento =
          @"  DELETE FROM TBREQUISICAO  DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0) DELETE FROM TBMEDICAMENTO  DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)";

        private const string sqlExcluirFornecedor =
          @"DELETE FROM TBFORNECEDOR  DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)";


        private const string enderecoBanco =
       "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMed;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";



        public RepositorioFornecedorEmBancoDadosTest()
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
        public void Deve_inserir_fornecedor()
        {
            RepositorioFornecedorEmBancoDados repositorio = new();

            Fornecedor fornecedor = new("a", "b", "c", "d", "e");

            repositorio.Inserir(fornecedor);

            Fornecedor fornecedor2 = repositorio.SelecionarPorID(fornecedor.Id);

            Assert.AreEqual(fornecedor, fornecedor2);

        }
        [TestMethod]
        public void Deve_excluir_fornecedor()
        {

            RepositorioFornecedorEmBancoDados repositorio = new();

            Fornecedor fornecedor = new("a", "b", "c", "d", "e");

            repositorio.Inserir(fornecedor);

            ValidationResult result = repositorio.Excluir(fornecedor);


            Assert.AreEqual(result.Errors.Count, 0);

        }
        [TestMethod]
        public void Deve_selecionar_todos_fornecedores()
        {
            RepositorioFornecedorEmBancoDados repositorio = new();
            List<Fornecedor> fornecedors = new List<Fornecedor>();


            for (int i = 0; i < 10; i++)
            {
                Fornecedor fornecedor = new(i.ToString(), "b", "c", "d", "e");
                repositorio.Inserir(fornecedor);
                fornecedors.Add(fornecedor);
            }

            List<Fornecedor> fornecedorsDoBanco = repositorio.SelecionarTodos();

            for (int i = 0; i < fornecedorsDoBanco.Count; i++)
            {
                Assert.AreEqual(fornecedorsDoBanco[i], fornecedors[i]);
            }


        }


            [TestMethod]
        public void Deve_editar_fornecedor()
        {

            RepositorioFornecedorEmBancoDados repositorio = new();

            Fornecedor fornecedor = new("a", "b", "c", "d", "e");
      
            repositorio.Inserir(fornecedor);

            fornecedor.Nome = "ssssss";

            repositorio.Editar(fornecedor);

            Fornecedor fornecedor2 = repositorio.SelecionarPorID(fornecedor.Id);


            Assert.AreEqual(fornecedor2, fornecedor);

        }

        [TestMethod]
        public void Nao_Deve_excluir_fornecedor_que_tem_Medicamento()
        {


            RepositorioMedicamentoEmBancoDados repositorioMedicamento = new();
            RepositorioFornecedorEmBancoDados repositorioFornecedor = new();

            Fornecedor fornecedor = new("a", "b", "c", "d", "e");

            repositorioFornecedor.Inserir(fornecedor);


            Medicamento med = new("medicamento1", "descricao1", "lote1", System.DateTime.Now);

            med.Fornecedor = fornecedor;


            Requisicao requ1 = new(med, new Paciente("a", "b"), 5, System.DateTime.Now, new Funcionario("a", "v", "c"));
            Requisicao requ2 = new(med, new Paciente("a", "b"), 5, System.DateTime.Now, new Funcionario("a", "v", "c"));
            Requisicao requ3 = new(med, new Paciente("a", "b"), 5, System.DateTime.Now, new Funcionario("a", "v", "c"));

            med.Requisicoes.Add(requ1);
            med.Requisicoes.Add(requ2);
            med.Requisicoes.Add(requ3);

            repositorioMedicamento.Inserir(med);



            ValidationResult result = repositorioFornecedor.Excluir(fornecedor);


            Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"));

        }


    }
}
