using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class UnidadeServico : IServico<Unidade>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Unidade, sgphdbEntities> _repositorio;

        public UnidadeServico(IDicionarioDeValidacao dicionarioDeValidacao) 
            : this(dicionarioDeValidacao,new UnidadeRepositorio()){}

        public UnidadeServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Unidade, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Unidade unidade)
        {
            if(unidade.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao","Descricao é campo obrigatório.");
           
            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico
        
        public bool Inserir(Unidade entidade)
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

        public bool Atualizar(Unidade entidade)
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

        public bool Excluir(Unidade entidade)
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

        public Unidade ObtemUm(Expression<Func<Unidade, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Unidade> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Unidade> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Unidade> ObtemTodos(Expression<Func<Unidade, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Unidade> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Unidade> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Unidade, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}