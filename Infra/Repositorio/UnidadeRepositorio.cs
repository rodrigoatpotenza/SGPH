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
    public class UnidadeRepositorio : IRepositorio<Unidade,sgphdbEntities>, IDisposable
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Unidade).Name);

                return _entitySetName;
            }
        }

        public UnidadeRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Unidade entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Unidade entidade)
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

        public void Excluir(Unidade entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Unidade ObtemUm(Expression<Func<Unidade, bool>> condicao)
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Unidade> ObtemTodos()
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).ToList();
        }

        public IList<Unidade> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Unidade> ObtemTodos(Expression<Func<Unidade, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Unidade> ConsultaTodos()
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).AsQueryable();
        }

        public IQueryable<Unidade> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Unidade, bool>> condicao)
        {
            return Contexto.CreateQuery<Unidade>(EntitySetName).Where(condicao).Count();
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