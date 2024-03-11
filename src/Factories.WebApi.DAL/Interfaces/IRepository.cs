﻿namespace Factories.WebApi.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>>? GetAllAsync(CancellationToken token);
        T? Get(int id);
        IEnumerable<T>? Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}