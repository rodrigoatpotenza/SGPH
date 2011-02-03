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
    public class EscolaridadeRepositorio : IRepositorio<Escolaridade, sgphdbEntities>, IDisposable
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Escolaridade).Name);

                return _entitySetName;
            }
        }

        public EscolaridadeRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Escolaridade entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Escolaridade entidade)
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

        public void Excluir(Escolaridade entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Escolaridade ObtemUm(Expression<Func<Escolaridade, bool>> condicao)
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Escolaridade> ObtemTodos()
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).ToList();
        }

        public IList<Escolaridade> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Escolaridade> ObtemTodos(Expression<Func<Escolaridade, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Escolaridade> ConsultaTodos()
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).AsQueryable();
        }

        public IQueryable<Escolaridade> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Escolaridade, bool>> condicao)
        {
            return Contexto.CreateQuery<Escolaridade>(EntitySetName).Where(condicao).Count();
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