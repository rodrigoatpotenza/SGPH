using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class EscolaridadeServico : IServico<Escolaridade>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Escolaridade, sgphdbEntities> _repositorio;

        public EscolaridadeServico(IDicionarioDeValidacao dicionarioDeValidacao)
            : this(dicionarioDeValidacao, new EscolaridadeRepositorio()) { }

        public EscolaridadeServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Escolaridade, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Escolaridade cargo)
        {
            if (cargo.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao", "Descricao é campo obrigatório.");

            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico

        public bool Inserir(Escolaridade entidade)
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

        public bool Atualizar(Escolaridade entidade)
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

        public bool Excluir(Escolaridade entidade)
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

        public Escolaridade ObtemUm(Expression<Func<Escolaridade, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Escolaridade> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Escolaridade> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Escolaridade> ObtemTodos(Expression<Func<Escolaridade, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Escolaridade> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Escolaridade> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Escolaridade, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}