using System.Threading.Tasks;

namespace ECommerce.Cart.Cache
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        bool Set(string key, object data, int expireSeconds);
        Task<bool> SetAsync(string key, object data, int expireSeconds);
        bool Delete(string key);
        Task<bool> DeleteAsync(string key);
    }
}