using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Infra.Interfaces
{
    public interface IFuncionarioServico<TE>
    {
        bool Inserir(TE entidade);

        bool Atualizar(TE entidade);

        bool Excluir(TE entidade);

        TE ObtemUm(Expression<Func<TE, bool>> condicao);

        IList<TE> ObtemTodos();

        IList<TE> ObtemTodos(int maximoDelinhas, int linhaInicial);

        IList<TE> ObtemTodos(Expression<Func<TE, bool>> condicao, int maximoDeLinhas, int linhaInicial);

        IQueryable<TE> ConsultaTodos();

        IQueryable<TE> ConsultaTodos(int maximoDelinhas, int linhaInicial);

        int Quantidade();

        int Quantidade(Expression<Func<TE, bool>> condicao);
    }
}
