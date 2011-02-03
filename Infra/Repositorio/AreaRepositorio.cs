using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using SgphMvc.Models.Contexto;
using Infra.Interfaces;

namespace Infra.Repositorio
{
    public class AreaRepositorio : IRepositorio<Area, sgphdbEntities>, IDisposable
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Area).Name);

                return _entitySetName;
            }
        }

        public AreaRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Area entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Area entidade)
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

        public void Excluir(Area entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Area ObtemUm(Expression<Func<Area, bool>> condicao)
        {
            return Contexto.CreateQuery<Area>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Area> ObtemTodos()
        {
            return Contexto.CreateQuery<Area>(EntitySetName).ToList();
        }

        public IList<Area> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Area>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Area> ObtemTodos(Expression<Func<Area, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Area>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Area> ConsultaTodos()
        {
            return Contexto.CreateQuery<Area>(EntitySetName).AsQueryable();
        }

        public IQueryable<Area> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Area>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Area>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Area, bool>> condicao)
        {
            return Contexto.CreateQuery<Area>(EntitySetName).Where(condicao).Count();
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