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
    public class EstadoRepositorio : IRepositorio<Estado,sgphdbEntities>
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Estado).Name);

                return _entitySetName;
            }
        }

        public EstadoRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Estado entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Estado entidade)
        {
            var key = entidade.EntityKey ?? Contexto.CreateEntityKey(EntitySetName, entidade);

            object original;
            if (Contexto.TryGetObjectByKey(key, out original))
                if (original is EntityObject &&
                    ((EntityObject)original).EntityState != EntityState.Added)
                {
                    Contexto.ApplyCurrentValues(key.EntitySetName, entidade);
                    GravarNoBanco();
                }
        }

        public void Excluir(Estado entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Estado ObtemUm(Expression<Func<Estado, bool>> condicao)
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Estado> ObtemTodos()
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).ToList();
        }

        public IList<Estado> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Estado> ObtemTodos(Expression<Func<Estado, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Estado> ConsultaTodos()
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).AsQueryable();
        }

        public IQueryable<Estado> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Estado, bool>> condicao)
        {
            return Contexto.CreateQuery<Estado>(EntitySetName).Where(condicao).Count();
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