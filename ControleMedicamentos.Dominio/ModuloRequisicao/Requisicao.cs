using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase<Requisicao>
    {
        public Requisicao(Medicamento medicamento, Paciente paciente, int qtdMedicamento, DateTime data, Funcionario funcionario)
        {
            Medicamento = medicamento;
            Paciente = paciente;
            QtdMedicamento = qtdMedicamento;
            Data = data;
            Funcionario = funcionario;
        }

        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }
        public ModuloFuncionario.Funcionario Funcionario { get; set; }

        public override void Atualizar(Requisicao registro)
        {
            Medicamento = registro.Medicamento;
            Paciente = registro.Paciente;
            QtdMedicamento = registro.QtdMedicamento;
            Data = registro.Data;
            Funcionario = registro.Funcionario;
        }

        public override bool Equals(object obj)
        {
            return obj is Requisicao requisicao &&
                   Id == requisicao.Id &&
                   Medicamento.Id == requisicao.Medicamento.Id &&
                   Medicamento.Lote == requisicao.Medicamento.Lote &&
                   Medicamento.Validade == requisicao.Medicamento.Validade &&
                   Medicamento.Nome == requisicao.Medicamento.Nome &&
                   Medicamento.Descricao == requisicao.Medicamento.Descricao &&
                   Medicamento.QuantidadeRequisicoes == requisicao.Medicamento.QuantidadeRequisicoes &&
                   Medicamento.Fornecedor.Equals(requisicao.Medicamento.Fornecedor) &&
                   Medicamento.QuantidadeDisponivel == requisicao.Medicamento.QuantidadeDisponivel &&
                   EqualityComparer<Paciente>.Default.Equals(Paciente, requisicao.Paciente) &&
                   QtdMedicamento == requisicao.QtdMedicamento &&
                   Data.TrimMilliseconds() == requisicao.Data.TrimMilliseconds() &&
                   EqualityComparer<Funcionario>.Default.Equals(Funcionario, requisicao.Funcionario);
        }
    }
}
