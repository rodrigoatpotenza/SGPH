using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class CidadeServico :IServico<Cidade>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Cidade, sgphdbEntities> _repositorio;

        public CidadeServico(IDicionarioDeValidacao dicionarioDeValidacao) 
            : this(dicionarioDeValidacao,new CidadeRepositorio()){}

        public CidadeServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Cidade, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Cidade cidade)
        {
            if(cidade.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao","Descricao é campo obrigatório.");
           
            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico
        
        public bool Inserir(Cidade entidade)
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

        public bool Atualizar(Cidade entidade)
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

        public bool Excluir(Cidade entidade)
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

        public Cidade ObtemUm(Expression<Func<Cidade, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Cidade> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Cidade> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Cidade> ObtemTodos(Expression<Func<Cidade, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Cidade> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Cidade> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Cidade, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}