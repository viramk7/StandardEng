using StandardEng.Common;
using StandardEng.Data.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.Repository
{
    public class GenericRepository<T> where T : class
    {
        /// <summary>
        /// Function to get entites
        /// </summary>
        /// <returns>DbSet result of entities</returns>
        public DbSet<T> GetEntities()
        {
            StandardEngEntities context = BaseContext.GetDbContext();
            return context.Set<T>();
        }

        /// <summary>
        /// Function to create entity instance
        /// </summary>
        /// <returns>New instance of entity</returns>
        public T CreateEntity()
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                DbSet<T> table = context.Set<T>();
                return table.Create();
            }
        }

        /// <summary>
        /// Function to insert entity.
        /// </summary>
        /// <param name="entity">entity to be inserted</param>
        public string Insert(T entity)
        {
            try
            {
                using (StandardEngEntities context = BaseContext.GetDbContext())
                {
                    DbSet<T> table = context.Set<T>();
                    table.Add(entity);
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string messages = String.Empty;
                foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError error in entityErr.ValidationErrors)
                    {
                        //Console.WriteLine("Error Property Name {0} : Error Message: {1}",
                        //                    error.PropertyName, error.ErrorMessage);
                        messages = messages + "<br/>" + string.Join("<br/>", error.ErrorMessage);

                    }
                }
                return messages;
            }
            catch (Exception ex)
            {
                return CommonHelper.GetErrorMessage(ex);
            }

        }


        /// <summary>
        /// Function to update entity.
        /// </summary>
        /// <param name="entity">entity to be updated</param>
        public string Update(T entity)
        {
            try
            {
                using (StandardEngEntities context = BaseContext.GetDbContext())
                {
                    DbSet<T> table = context.Set<T>();
                    table.Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return CommonHelper.GetErrorMessage(ex);
            }
        }

        /// <summary>
        /// Function to select entity by primary key
        /// </summary>
        /// <param name="pk">Primary key of entity to fetch</param>
        /// <returns></returns>
        public T SelectById(object pk)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                DbSet<T> table = context.Set<T>();
                return table.Find(pk);
            }
        }
        //public int SelectId(object pk)
        //{
        //    using (CosmosEntities context = BaseContext.GetDbContext())
        //    {
        //        return Convert.ToInt32(pk);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk">Primary key of entity to be deleted</param>
        public string Delete(object pk)
        {
            try
            {
                using (StandardEngEntities context = BaseContext.GetDbContext())
                {


                    DbSet<T> table = context.Set<T>();
                    T existing = table.Find(pk);
                    context.Entry(existing).State = EntityState.Deleted;
                    context.SaveChanges();

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return CommonHelper.GetDeleteException(ex);
            }
        }


        /// <summary>
        /// Function to insert entity.
        /// </summary>
        /// <param name="entity">entity to be inserted</param>
        /// <param name="context">context to maintain transaction</param>
        public void Insert(T entity, StandardEngEntities context)
        {
            DbSet<T> table = context.Set<T>();
            table.Add(entity);
        }

        /// <summary>
        /// Function to update entity.
        /// </summary>
        /// <param name="entity">entity to be updated</param>
        /// <param name="context">context to maintain transaction</param>
        public void Update(T entity, StandardEngEntities context)
        {
            DbSet<T> table = context.Set<T>();
            table.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Function to delete entity whose primary key is passes.
        /// </summary>
        /// <param name="pk">Primary key of entity to be deleted</param>
        /// <param name="context">context to maintain transaction</param>
        public void Delete(object pk, StandardEngEntities context)
        {
            DbSet<T> table = context.Set<T>();
            T existing = table.Find(pk);
            context.Entry(existing).State = EntityState.Deleted;
        }

        public IEnumerable<TElement> ExecuteSQL<TElement>(string commandText, params object[] parameters)
        {
            try
            {
                StandardEngEntities context = BaseContext.GetDbContext();

                if (parameters != null && parameters.Length > 0)
                {
                    for (int i = 0; i <= parameters.Length - 1; i++)
                    {
                        var p = parameters[i] as DbParameter;
                        if (p == null)
                        {
                            throw new Exception("Not support parameter type");
                        }

                        commandText += i == 0 ? " " : ", ";

                        commandText += "@" + p.ParameterName;
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                        {
                            ////output parameter
                            commandText += " output";
                        }
                    }
                }


                return context.Database.SqlQuery<TElement>(commandText, parameters);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
