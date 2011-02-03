namespace Infra.Interfaces
{
    public interface IDicionarioDeValidacao
    {
        void AdicionaErro(string key, string mensagemErro);
        bool EhValido { get; }
    }
}
