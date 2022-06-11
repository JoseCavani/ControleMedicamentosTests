namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class Funcionario : EntidadeBase<Funcionario>
    {

        public Funcionario(string nome, string login, string senha)
        {
            Nome = nome;
            Login = login;
            Senha = senha;
        }

        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

        public override void Atualizar(Funcionario registro)
        {
            Nome = registro.Nome;
            Login = registro.Login;
            Senha = registro.Senha; ;
        }

        public override bool Equals(object obj)
        {
            return obj is Funcionario funcionario &&
                   Id == funcionario.Id &&
                   Nome == funcionario.Nome &&
                   Login == funcionario.Login &&
                   Senha == funcionario.Senha;
        }
    }
}
