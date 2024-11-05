using System.Collections.ObjectModel;

namespace WSSTest.InterfacesLib
{
    public interface IRepository<T> : IDisposable
        where T : class
    {

        Task<List<T>> GetList();
        Task<T> GetAsync(int itemId);
        Task CreateAsync(T item);
        Task CreateAsync(List<T> items);
        Task DeleteAsync(T item);
        Task UpdateAsync(T item);
    }
}
