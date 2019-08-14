using System;
using System.Threading.Tasks;

namespace TemplateV2.Infrastructure.Session.Contracts
{
    public interface ISessionProvider
    {
        Task<T> Get<T>(string key);

        Task<T> Set<T>(string key, T value);

        Task Remove(string key);

        Task Clear();
    }
}