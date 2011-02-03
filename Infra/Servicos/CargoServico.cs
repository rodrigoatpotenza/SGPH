using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class CargoServico : IServico<Cargo>
    {
        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IRepositorio<Cargo, sgphdbEntities> _repositorio;

        public CargoServico(IDicionarioDeValidacao dicionarioDeValidacao) 
            : this(dicionarioDeValidacao,new CargoRepositorio()){}

        public CargoServico(IDicionarioDeValidacao dicionarioDeValidacao, IRepositorio<Cargo, sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Cargo cargo)
        {
            if(cargo.Descricao.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Descricao","Descricao é campo obrigatório.");
           
            return _dicionarioDeValidacao.EhValido;
        }

        #region Implementação IServico
        
        public bool Inserir(Cargo entidade)
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

        public bool Atualizar(Cargo entidade)
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

        public bool Excluir(Cargo entidade)
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

        public Cargo ObtemUm(Expression<Func<Cargo, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Cargo> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Cargo> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Cargo> ObtemTodos(Expression<Func<Cargo, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Cargo> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Cargo> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Cargo, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}