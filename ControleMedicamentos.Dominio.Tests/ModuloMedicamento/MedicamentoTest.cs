using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class MedicamentoTest
    {
        [TestMethod]
        public void Deve_Mostrar_Quantidade_Requsicoes()
        {
            Medicamento med = new("medicamento1","descricao1","lote1",System.DateTime.Now);


            Requisicao requ1 = new(med,new Paciente("a","b"),5, System.DateTime.Now,new Funcionario("a","v","c"));
            Requisicao requ2 = new(med, new Paciente("a", "b"), 5, System.DateTime.Now, new Funcionario("a", "v", "c"));
            Requisicao requ3 = new(med,new Paciente("a","b"),5, System.DateTime.Now,new Funcionario("a","v","c"));

            med.Requisicoes.Add(requ1);
            med.Requisicoes.Add(requ2);
            med.Requisicoes.Add(requ3);

            Assert.AreEqual(3, med.QuantidadeRequisicoes);

        }


        [TestMethod]
        public void Deve_Ser_Objetos_Iguais()
        {
            Medicamento med = new("medicamento1", "descricao1", "lote1", System.DateTime.Now);

            Medicamento medIgual = med;

            Assert.AreEqual(medIgual, med);

        }

        [TestMethod]
        public void Deve_Ser_Fornecedor_Igual()
        {
            Fornecedor fornecedor = new("a","b","c","d","e");

            Medicamento med = new("medicamento1", "descricao1", "lote1", System.DateTime.Now);

            med.Fornecedor = fornecedor;

            Assert.AreEqual(med.Fornecedor, fornecedor);

        }



    }
}
