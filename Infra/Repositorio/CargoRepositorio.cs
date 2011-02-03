using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using SgphMvc.Models.Contexto;
using System.Data;
using Infra.Interfaces;

namespace Infra.Repositorio
{
    public class CargoRepositorio : IRepositorio<Cargo,sgphdbEntities>, IDisposable
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Cargo).Name);

                return _entitySetName;
            }
        }

        public CargoRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Cargo entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Cargo entidade)
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

        public void Excluir(Cargo entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Cargo ObtemUm(Expression<Func<Cargo, bool>> condicao)
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Cargo> ObtemTodos()
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).ToList();
        }

        public IList<Cargo> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Cargo> ObtemTodos(Expression<Func<Cargo, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Cargo> ConsultaTodos()
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).AsQueryable();
        }

        public IQueryable<Cargo> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Cargo, bool>> condicao)
        {
            return Contexto.CreateQuery<Cargo>(EntitySetName).Where(condicao).Count();
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