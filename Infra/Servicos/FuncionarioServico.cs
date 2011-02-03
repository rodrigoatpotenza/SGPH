using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Infra.Interfaces;
using Infra.Repositorio;

namespace Infra.Servicos
{
    public class FuncionarioServico : IFuncionarioServico<Funcionario>
    {
        #region [ Membros Repositorios ]

        private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        private readonly IFuncionarioRepositorio<Infra.Funcionario, sgphdbEntities> _repositorio;
        private IRepositorio<ExperienciaProfissional,sgphdbEntities> _experienciaProfissional;
        private IRepositorio<Formacao,sgphdbEntities> _formacao;
        private IRepositorio<Funcionario_Idioma,sgphdbEntities> _funcionarioIdioma;
        private IRepositorio<CursoExtensao,sgphdbEntities> _cursoExtensao;

        #endregion

        //private readonly IDicionarioDeValidacao _dicionarioDeValidacao;
        //private readonly IFuncionarioRepositorio<Funcionario, sgphdbEntities> _repositorio;

        public FuncionarioServico(IDicionarioDeValidacao dicionarioDeValidacao) 
            : this(dicionarioDeValidacao,new FuncionarioRepositorio()){}

        public FuncionarioServico(IDicionarioDeValidacao dicionarioDeValidacao, IFuncionarioRepositorio<Funcionario,sgphdbEntities> repositorio)
        {
            _dicionarioDeValidacao = dicionarioDeValidacao;
            _repositorio = repositorio;
        }

        public bool ValidaFuncionario(Funcionario funcionario)
        {
            if(funcionario.Nome.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Nome","Nome é campo obrigatório.");
            if(funcionario.Matricula.Trim().Length == 0)
                _dicionarioDeValidacao.AdicionaErro("Matricula", "Nome é campo obrigatório.");
            if(funcionario.Email.Length > 0 && !Regex.IsMatch(funcionario.Email,@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                _dicionarioDeValidacao.AdicionaErro("Email","Endereço de e-mail inválido");

            return _dicionarioDeValidacao.EhValido;
        }


        #region Implementação IServico

        public bool Inserir(Funcionario entidade)
        {
            //if (!ValidaFuncionario(entidade))
            //    return false;

            //try
            //{
            //    _repositorio.Inserir(entidade);
            //}
            //catch
            //{
            //    return false;
            //}

            //return true;

            if (!ValidaFuncionario(entidade))
                return false;

            var funcionario = new Infra.Funcionario
            {
                Bairro = entidade.Bairro,
                Cargos_Id = entidade.Cargos_Id,
                Cep = entidade.Cep,
                Cidades_Estados_Id = entidade.Cidades_Estados_Id,
                Cidades_Id = entidade.Cidades_Id,
                Complemento = entidade.Complemento,
                DataNascimento = entidade.DataNascimento,
                Email = entidade.Email,
                Logradouro = entidade.Logradouro,
                Matricula = entidade.Matricula,
                Nome = entidade.Nome,
                Numero = entidade.Numero,
                Perfis_Id = entidade.Perfis_Id,
                Ramal = entidade.Ramal,
                Setores_Id = entidade.Setores_Id,
                Telefone = entidade.Telefone,
                Unidades_Id = entidade.Unidades_Id,
            };

            _repositorio.Inserir(funcionario);

            foreach (var experienciaProfissional in entidade.ExperienciasProfissionais)
            {
                experienciaProfissional.Funcionarios_Id = funcionario.Id;
                _experienciaProfissional.Inserir(experienciaProfissional);
            }

            foreach (var formacao in entidade.Formacoes)
            {
                formacao.Funcionarios_Id = funcionario.Id;
                _formacao.Inserir(formacao);
            }

            foreach (var funcionarioIdioma in entidade.Funcionarios_Idiomas)
            {
                funcionarioIdioma.Funcionarios_Id = funcionario.Id;
                _funcionarioIdioma.Inserir(funcionarioIdioma);
            }

            foreach (var cursoExtensao in entidade.CursosExtensao)
            {
                cursoExtensao.Funcionarios_Id = funcionario.Id;
                _cursoExtensao.Inserir(cursoExtensao);
            }


            return true;
        }

        public bool Atualizar(Funcionario entidade)
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

        public bool Excluir(Funcionario entidade)
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

        public Funcionario ObtemUm(Expression<Func<Funcionario, bool>> condicao)
        {
            return _repositorio.ObtemUm(condicao);
        }

        public IList<Funcionario> ObtemTodos()
        {
            return _repositorio.ObtemTodos();
        }

        public IList<Funcionario> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(maximoDelinhas, linhaInicial);
        }

        public IList<Funcionario> ObtemTodos(Expression<Func<Funcionario, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return _repositorio.ObtemTodos(condicao, maximoDeLinhas, linhaInicial);
        }

        public IQueryable<Funcionario> ConsultaTodos()
        {
            return _repositorio.ConsultaTodos();
        }

        public IQueryable<Funcionario> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return _repositorio.ConsultaTodos(maximoDelinhas, linhaInicial);
        }

        public int Quantidade()
        {
            return _repositorio.Quantidade();
        }

        public int Quantidade(Expression<Func<Funcionario, bool>> condicao)
        {
            return _repositorio.Quantidade(condicao);
        }

        #endregion
    }
}