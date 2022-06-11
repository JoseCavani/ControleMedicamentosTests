namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class Fornecedor : EntidadeBase<Fornecedor>
    {
        public Fornecedor(string nome, string telefone, string email, string cidade, string estado)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Cidade = cidade;
            Estado = estado;
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public override void Atualizar(Fornecedor registro)
        {
            Nome = registro.Nome;
            Telefone = registro.Telefone;
            Email = registro.Email;
            Cidade = registro.Cidade;
            Estado = registro.Estado;
        }

        public override bool Equals(object obj)
        {
            return obj is Fornecedor fornecedor &&
                   Id == fornecedor.Id &&
                   Nome == fornecedor.Nome &&
                   Telefone == fornecedor.Telefone &&
                   Email == fornecedor.Email &&
                   Cidade == fornecedor.Cidade &&
                   Estado == fornecedor.Estado;
        }
    }
}
