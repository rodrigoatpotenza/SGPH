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
    public class IdiomaRepositorio : IRepositorio<Idioma, sgphdbEntities>, IDisposable
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Idioma).Name);

                return _entitySetName;
            }
        }

        public IdiomaRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Idioma entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Idioma entidade)
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

        public void Excluir(Idioma entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Idioma ObtemUm(Expression<Func<Idioma, bool>> condicao)
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Idioma> ObtemTodos()
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).ToList();
        }

        public IList<Idioma> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Idioma> ObtemTodos(Expression<Func<Idioma, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Idioma> ConsultaTodos()
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).AsQueryable();
        }

        public IQueryable<Idioma> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Idioma, bool>> condicao)
        {
            return Contexto.CreateQuery<Idioma>(EntitySetName).Where(condicao).Count();
        }

        public void Dispose()
        {
            if (Contexto != null)
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