﻿using LibraryManagement.Data;
using LibraryManagement.Models.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagement.Models.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private LibraryContext _db;
        internal DbSet<T> dbSet;

        public Repository(LibraryContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task RemoveAsync(Expression<Func<T, bool>> propertyName)
        {
            T entity = await _db.Set<T>().FirstOrDefaultAsync(propertyName);
            if(entity != null)
            {
                _db.Set<T>().Remove(entity);
                await SaveAsync();
            }
        }
        public async Task<bool> CheckDuplicateAtCreation(Expression<Func<T, bool>> propertyName)
        {
            return await dbSet.AnyAsync(propertyName);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


    }
}
