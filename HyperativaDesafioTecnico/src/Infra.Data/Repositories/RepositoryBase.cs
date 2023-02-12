﻿using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HyperativaDesafio.Infra.Data.Repositories
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {

        protected HyperativaDesafioDbContext DbContext;

        public RepositoryBase()
        {
            DbContext = new HyperativaDesafioDbContext();
        }

        public void Add(TEntity entity, string query)
        {
            try
            {
                DbContext.Connection.Execute(query, entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Add(TEntity entity)
        {
           throw new NotImplementedException();
        }

        public void Dispose()
        {
            DbContext.Connection?.Dispose();
        }
    }
}