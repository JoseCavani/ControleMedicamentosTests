using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class MapeadorRequisicao : IMapeavel<Requisicao>
    {
        public Requisicao ConverterParaRegistro(SqlDataReader leitorRegistro)
        {

            int idPaciente = Convert.ToInt32(leitorRegistro["PACIENTE_ID"]);
            string nomePaciente = Convert.ToString(leitorRegistro["PACIENTE_NOME"]);
            string cartaoSUS = Convert.ToString(leitorRegistro["PACIENTE_CARTAOSUS"]);


            var paciente = new Paciente(nomePaciente, cartaoSUS);
            paciente.Id = idPaciente;


            int medicamentoId = Convert.ToInt32(leitorRegistro["MEDICAMENTO_ID"]);
            string medicamentoNome = Convert.ToString(leitorRegistro["MEDICAMENTO_NOME"]);
            string medicamentoDescricao = Convert.ToString(leitorRegistro["MEDICAMENTO_DESCRICAO"]);
            string medicamentoLote = Convert.ToString(leitorRegistro["MEDICAMENTO_LOTE"]);
            DateTime medicamentoValidade = Convert.ToDateTime(leitorRegistro["MEDICAMENTO_VALIDADE"]).Date;


            int fornecedorId = Convert.ToInt32(leitorRegistro["FORNECEDOR_ID"]);
            string fornecedorNome = Convert.ToString(leitorRegistro["FORNECEDOR_NOME"]);
            string fornecedorEmail = Convert.ToString(leitorRegistro["FORNECEDOR_EMAIL"]);
            string fornecedorEstado = Convert.ToString(leitorRegistro["FORNECEDOR_ESTADO"]);
            string fornecedorCidade = Convert.ToString(leitorRegistro["FORNECEDOR_CIDADE"]);
            string fornecedorTelefone = Convert.ToString(leitorRegistro["FORNECEDOR_TELEFONE"]);


            var fornecedor = new Fornecedor(fornecedorNome, fornecedorTelefone, fornecedorEmail, fornecedorCidade, fornecedorEstado);
            fornecedor.Id = fornecedorId;

            var medicamento = new Medicamento(medicamentoNome, medicamentoDescricao, medicamentoLote, medicamentoValidade);
            medicamento.Id = medicamentoId;
            medicamento.Fornecedor = fornecedor;

            int funcionarioId = Convert.ToInt32(leitorRegistro["FUNCIONARIO_ID"]);
            string funcionarioNome = Convert.ToString(leitorRegistro["FUNCIONARIO_NOME"]);
            string funcionarioLogin = Convert.ToString(leitorRegistro["FUNCIONARIO_LOGIN"]);
            string funcionarioSenha = Convert.ToString(leitorRegistro["FUNCIONARIO_SENHA"]);


            var funcionario = new Funcionario(funcionarioNome, funcionarioLogin, funcionarioSenha);
            funcionario.Id = funcionarioId;

            int requisicaoId = Convert.ToInt32(leitorRegistro["REQUISICAO_ID"]);
            int requisicaoQuantidade = Convert.ToInt32(leitorRegistro["REQUISICAO_QUANTIDADE"]);
            DateTime requisicaoData = Convert.ToDateTime(leitorRegistro["REQUISICAO_DATA"]);

            var requsicao = new Requisicao(medicamento, paciente, requisicaoQuantidade, requisicaoData, funcionario);
            requsicao.Id = requisicaoId;


            return requsicao;

        }

        public void ConfigurarParametrosRegistro(Requisicao registro, SqlCommand cmdInserir)
        {
            cmdInserir.Parameters.AddWithValue("ID", registro.Id);
            cmdInserir.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", registro.QtdMedicamento);
            cmdInserir.Parameters.AddWithValue("DATA", registro.Data);
            cmdInserir.Parameters.AddWithValue("FUNCIONARIO_ID", registro.Funcionario.Id);
            cmdInserir.Parameters.AddWithValue("MEDICAMENTO_ID", registro.Medicamento.Id);
            cmdInserir.Parameters.AddWithValue("PACIENTE_ID", registro.Paciente.Id);

        }
    }
}
