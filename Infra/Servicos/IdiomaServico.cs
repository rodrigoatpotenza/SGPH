using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class IdiomaServico : IServico<Idioma>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Idioma, sgphdbEntities> _repositorio;

        public IdiomaServico(IDicionarioDeValidacao dicionarioDeValidacao)
            : this(dicionarioDeValidacao, new IdiomaRepositorio()) { }

        public IdiomaServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Idioma, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Idioma cargo)
        {
            if (cargo.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao", "Descricao é campo obrigatório.");

            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico

        public bool Inserir(Idioma entidade)
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

        public bool Atualizar(Idioma entidade)
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

        public bool Excluir(Idioma entidade)
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

        public Idioma ObtemUm(Expression<Func<Idioma, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Idioma> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Idioma> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Idioma> ObtemTodos(Expression<Func<Idioma, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Idioma> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Idioma> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Idioma, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}