using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Models.ServiceModels.Home;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Services
{
    public class HomeService : IHomeService
    {
        #region Instance Fields

        private readonly ILogger<HomeService> _logger;

        private readonly IApplicationCache _cache;

        #endregion

        #region Constructor

        public HomeService(
            ILogger<HomeService> logger,
            IApplicationCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        #endregion

        #region Public Methods

        public async Task<GetHomeResponse> GetHome()
        {
            var response = new GetHomeResponse();
            var config = await _cache.Configuration();

            response.DisplayPromoBanner = config.Home_Promo_Banner_Is_Enabled;

            return response;
        }

        #endregion
    }
}
