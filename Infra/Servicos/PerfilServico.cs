using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class PerfilServico : IServico<Perfil>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Perfil, sgphdbEntities> _repositorio;

        public PerfilServico(IDicionarioDeValidacao dicionarioDeValidacao)
            : this(dicionarioDeValidacao, new PerfilRepositorio()) { }

        public PerfilServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Perfil, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Perfil perfil)
        {
            if (perfil.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao", "Descricao é campo obrigatório.");

            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico

        public bool Inserir(Perfil entidade)
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

        public bool Atualizar(Perfil entidade)
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

        public bool Excluir(Perfil entidade)
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

        public Perfil ObtemUm(Expression<Func<Perfil, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Perfil> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Perfil> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Perfil> ObtemTodos(Expression<Func<Perfil, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Perfil> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Perfil> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Perfil, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}