using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Infrastructure.ExternalServices.Cache
{
    public class CachePolicyService : ICachePolicyService
    {
        private readonly Dictionary<string, int> _ttlConfig;

        public CachePolicyService(IOptions<CacheTtlOptions> options)
        {
            _ttlConfig = options.Value.Expirations;
        }

        public TimeSpan GetExpirationFor(string key)
        {
            // Match exato
            if (_ttlConfig.TryGetValue(key, out int minutes))
                return TimeSpan.FromMinutes(minutes);

            // Match parcial (ex: "pedidos_" → "pedidos_todos")
            var fallback = _ttlConfig.Keys
                .FirstOrDefault(k => key.StartsWith(k, StringComparison.OrdinalIgnoreCase));

            if (fallback != null)
                return TimeSpan.FromMinutes(_ttlConfig[fallback]);

            // Default
            return _ttlConfig.TryGetValue("default", out int defaultMinutes)
                ? TimeSpan.FromMinutes(defaultMinutes)
                : TimeSpan.FromMinutes(5);
        }
    }
}
