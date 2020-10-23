using System;
using KubeOps.WareHouse.Entities;

namespace KubeOps.WareHouse.TestManager
{
    public interface IManager<T>
    {
        void Created(T entity);

        TimeSpan? Updated(T entity);

        void StatusModified(T entity);

        void NotModified(T entity);

        void Deleted(T entity);

        void Finalized(T entity);
    }
}
