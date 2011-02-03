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
    public class SetorRepositorio : IRepositorio<Setor, sgphdbEntities>
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Setor).Name);

                return _entitySetName;
            }
        }

        public SetorRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Setor entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Setor entidade)
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

        public void Excluir(Setor entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Setor ObtemUm(Expression<Func<Setor, bool>> condicao)
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Setor> ObtemTodos()
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).ToList();
        }

        public IList<Setor> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Setor> ObtemTodos(Expression<Func<Setor, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Setor> ConsultaTodos()
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).AsQueryable();
        }

        public IQueryable<Setor> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Setor, bool>> condicao)
        {
            return Contexto.CreateQuery<Setor>(EntitySetName).Where(condicao).Count();
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