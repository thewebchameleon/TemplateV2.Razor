using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Session.Contracts;

namespace TemplateV2.Infrastructure.Session
{
    public class SessionProvider : ISessionProvider
    {
        private readonly ISession _session;

        public SessionProvider(
            IHttpContextAccessor _httpContextAccessor
        )
        {
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public async Task<T> Get<T>(string id)
        {
            if (!_session.IsAvailable)
            {
                await _session.LoadAsync();
            }

            var storedValue = _session.GetString(id);
            if (!string.IsNullOrEmpty(storedValue))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(storedValue);
                }
                catch { }
            }
            return default(T);
        }

        public async Task<T> Set<T>(string key, T value)
        {
            if (!_session.IsAvailable)
            {
                await _session.LoadAsync();
            }

            if (value != null)
            {
                _session.SetString(key, JsonConvert.SerializeObject(value));
                return value;
            }
            return value;
        }

        public async Task Remove(string key)
        {
            if (!_session.IsAvailable)
            {
                await _session.LoadAsync();
            }
            _session.Remove(key);
        }

        public async Task Clear()
        {
            _session.Clear();
        }
    }
}
