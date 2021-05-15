using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Redis;
using System;
using System.Linq;
using System.Net;

namespace Shared.Services
{
    public interface IAuthCodeService
    {
        void AddOrRefresh(string email, Cookie cookie);
        void Remove(string email);
        bool TryGetSid(string email, out Cookie sid);
    }

    public class AuthCodeService : IAuthCodeService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<AuthCodeService> _logger;

        public AuthCodeService(IDistributedCache distributedCache, ILogger<AuthCodeService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public void AddOrRefresh(string email, Cookie cookie)
        {
            try
            {
                if (_distributedCache == null) return;

                var authCode = _distributedCache.Get(email)?
                    .FromByteArray<AuthCode>();

                if (authCode != null)
                {
                    authCode.Sid = cookie;
                }
                else
                {
                    authCode = new AuthCode {
                        Email = email,
                        Sid = cookie
                    };
                }

                _distributedCache.Set(email, authCode.ToByteArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception adding/refreshing the auth data.");
            }

        }

        public void Remove(string email)
        {
            try
            {
                if (_distributedCache == null) return;

                _distributedCache.Remove(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception removing the auth data.");
            }
        }

        public bool TryGetSid(string email, out Cookie sid)
        {
            try
            {
                var authCode = _distributedCache.Get(email)?
                    .FromByteArray<AuthCode>();
                sid = authCode?.Sid;

                return authCode != null;
            }
            catch (Exception ex)
            {
                sid = null;
                _logger.LogError(ex, "Exception getting the Sid.");

                return false;
            }
        }
    }
}
