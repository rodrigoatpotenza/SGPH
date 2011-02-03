using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infra.Interfaces
{
    public interface  IRepositorio<TE,TC>
    {
        TC Contexto { get; set; }

        void Inserir(TE entidade);

        void Atualizar(TE entidade);

        void Excluir(TE entidade);

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