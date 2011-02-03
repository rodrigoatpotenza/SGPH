using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class EstadoServico : IServico<Estado>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Estado, sgphdbEntities> _repositorio;

        public EstadoServico(IDicionarioDeValidacao dicionarioDeValidacao)
            : this(dicionarioDeValidacao, new EstadoRepositorio()) { }

        public EstadoServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Estado, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Estado estado)
        {
            if (estado.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao", "Descricao é campo obrigatório.");

            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico

        public bool Inserir(Estado entidade)
        {
            if (!ValidaFuncionario(entidade))
                return false;

            try
            {
                _repositorio.Inserir(entidade);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Atualizar(Estado entidade)
        {
            if (!ValidaFuncionario(entidade))
                return false;

            try
            {
                _repositorio.Atualizar(entidade);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Excluir(Estado entidade)
        {
            try
            {
                _repositorio.Excluir(entidade);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Estado ObtemUm(Expression<Func<Estado, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Estado> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Estado> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Estado> ObtemTodos(Expression<Func<Estado, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Estado> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Estado> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Estado, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}