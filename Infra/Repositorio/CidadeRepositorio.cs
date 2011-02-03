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
    public class CidadeRepositorio : IRepositorio<Cidade,sgphdbEntities>
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Cidade).Name);

                return _entitySetName;
            }
        }

        public CidadeRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Cidade entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Cidade entidade)
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

        public void Excluir(Cidade entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Cidade ObtemUm(Expression<Func<Cidade, bool>> condicao)
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Cidade> ObtemTodos()
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).ToList();
        }

        public IList<Cidade> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Cidade> ObtemTodos(Expression<Func<Cidade, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Cidade> ConsultaTodos()
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).AsQueryable();
        }

        public IQueryable<Cidade> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Cidade, bool>> condicao)
        {
            return Contexto.CreateQuery<Cidade>(EntitySetName).Where(condicao).Count();
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