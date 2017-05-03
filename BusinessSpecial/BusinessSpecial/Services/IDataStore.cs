using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessSpecial.Services
{
    public interface IDataStore<T, U, A>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> SignUpUserAsync(U user);
        Task<U> LoginUserAsync(U user);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        Task InitializeAsync();
        Task<bool> PullLatestAsync();
        Task<bool> SyncAsync();

        Task<bool> AddAdvertAsync(A advert);
        Task<IEnumerable<A>> GetAdvertsAsync(bool forceRefresh = false);
    }
}
