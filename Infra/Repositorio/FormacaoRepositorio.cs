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
    public class FormacaoRepositorio : IRepositorio<Formacao,sgphdbEntities>, IDisposable
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Formacao).Name);

                return _entitySetName;
            }
        }

        public FormacaoRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Formacao entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Formacao entidade)
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

        public void Excluir(Formacao entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Formacao ObtemUm(Expression<Func<Formacao, bool>> condicao)
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Formacao> ObtemTodos()
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).ToList();
        }

        public IList<Formacao> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Formacao> ObtemTodos(Expression<Func<Formacao, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Formacao> ConsultaTodos()
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).AsQueryable();
        }

        public IQueryable<Formacao> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Formacao, bool>> condicao)
        {
            return Contexto.CreateQuery<Formacao>(EntitySetName).Where(condicao).Count();
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