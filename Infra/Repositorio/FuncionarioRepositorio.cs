using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Objects.DataClasses;
using SgphMvc.Models.Contexto;
using System.Data;
using System.Data.Metadata.Edm;
using Infra.Interfaces;

namespace Infra.Repositorio
{
    public class FuncionarioRepositorio : IFuncionarioRepositorio<Funcionario,sgphdbEntities>, IDisposable
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Funcionario).Name);

                return _entitySetName;
            }
        }

        public FuncionarioRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Funcionario entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Funcionario entidade)
        {
            var key = entidade.EntityKey ?? Contexto.CreateEntityKey(EntitySetName, entidade);

            object original;
            if (Contexto.TryGetObjectByKey(key, out original))
                if (original is EntityObject &&
                    ((EntityObject)original).EntityState != EntityState.Added)
                    Contexto.ApplyCurrentValues(key.EntitySetName, entidade);
        }

        public void Excluir(Funcionario entidade)
        {
            Contexto.DeleteObject(entidade);
        }

        public Funcionario ObtemUm(Expression<Func<Funcionario, bool>> condicao)
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Funcionario> ObtemTodos()
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).ToList();
        }

        public IList<Funcionario> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Funcionario> ObtemTodos(Expression<Func<Funcionario, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Funcionario> ConsultaTodos()
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).AsQueryable();
        }

        public IQueryable<Funcionario> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Funcionario, bool>> condicao)
        {
            return Contexto.CreateQuery<Funcionario>(EntitySetName).Where(condicao).Count();
        }

        public void Dispose()
        {
            if(Contexto != null)
                Contexto.Dispose();
        }

        private string GetEntitySetName(string entityTypeName)
        {
            return (from meta in (Contexto.MetadataWorkspace.GetEntityContainer(Contexto.DefaultContainerName, DataSpace.CSpace)).BaseEntitySets
                    where meta.ElementType.Name == entityTypeName
                    select meta.Name).FirstOrDefault();
        }

        public void GravarNoBanco()
        {
            Contexto.SaveChanges();
        }
    }

}