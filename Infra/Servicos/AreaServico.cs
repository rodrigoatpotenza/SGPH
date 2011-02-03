using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class AreaServico : IServico<Area>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Area, sgphdbEntities> _repositorio;

        public AreaServico(IDicionarioDeValidacao dicionarioDeValidacao)
            : this(dicionarioDeValidacao, new AreaRepositorio()) { }

        public AreaServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Area, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Area cargo)
        {
            if (cargo.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao", "Descricao é campo obrigatório.");

            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico

        public bool Inserir(Area entidade)
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

        public bool Atualizar(Area entidade)
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

        public bool Excluir(Area entidade)
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

        public Area ObtemUm(Expression<Func<Area, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Area> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Area> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Area> ObtemTodos(Expression<Func<Area, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Area> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Area> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Area, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}