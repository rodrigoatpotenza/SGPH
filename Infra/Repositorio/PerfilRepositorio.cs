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
    public class PerfilRepositorio : IRepositorio<Perfil,sgphdbEntities>
    {
        private string _entitySetName;

        public sgphdbEntities Contexto { get; set; }

        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(_entitySetName))
                    _entitySetName = GetEntitySetName(typeof(Perfil).Name);

                return _entitySetName;
            }
        }

        public PerfilRepositorio()
        {
            Contexto = GerenciadorDeContexto.ObtemContexto<sgphdbEntities>();
        }

        public void Inserir(Perfil entidade)
        {
            Contexto.AddObject(EntitySetName, entidade);
            GravarNoBanco();
        }

        public void Atualizar(Perfil entidade)
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

        public void Excluir(Perfil entidade)
        {
            Contexto.DeleteObject(entidade);
            GravarNoBanco();
        }

        public Perfil ObtemUm(Expression<Func<Perfil, bool>> condicao)
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).Where(condicao).FirstOrDefault();
        }

        public IList<Perfil> ObtemTodos()
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).ToList();
        }

        public IList<Perfil> ObtemTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas).ToList();
        }

        public IList<Perfil> ObtemTodos(Expression<Func<Perfil, bool>> condicao, int maximoDeLinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).Where(condicao).Skip(linhaInicial).Take(maximoDeLinhas).ToList();
        }

        public IQueryable<Perfil> ConsultaTodos()
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).AsQueryable();
        }

        public IQueryable<Perfil> ConsultaTodos(int maximoDelinhas, int linhaInicial)
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).Skip(linhaInicial).Take(maximoDelinhas);
        }

        public int Quantidade()
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).Count();
        }

        public int Quantidade(Expression<Func<Perfil, bool>> condicao)
        {
            return Contexto.CreateQuery<Perfil>(EntitySetName).Where(condicao).Count();
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