using SharedModule.Utils;

namespace SharedModule.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<ResponseDetail<T>> Create(T entity);
        Task<ResponseDetail<T>> Update(T entity, Guid? userId);
        Task<ResponseDetail<bool>> Delete(Guid entityId, Guid? userId);
    }
}
