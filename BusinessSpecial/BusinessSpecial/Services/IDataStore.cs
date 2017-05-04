using BusinessSpecial.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessSpecial.Services
{
    public interface IDataStore<T, U, A, F>
    {
        Task<bool> AddItemAsync(T item);
        Task<U> GetUserProfileAsync(string userId);
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
        Task<ObservableRangeCollection<A>> GetAdvertsAsync();
        Task<List<F>> GetFAQAsync();
    }
}
