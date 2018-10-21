using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStore.DomainNew.Dto.User;
using WebStore.DomainNew.Entities;

namespace WebStore.Clients.Services.Users
{
    public partial class UsersClient
    {
        #region IUserLockoutStore

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/lockoutEndDate";
            var result = await PostAsync(url, user);
            return await result.Content.ReadAsAsync<DateTimeOffset?>();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            var url = $"{ServiceAddress}/lockoutEndDate/set";
            return PostAsync(url, new SetLockoutDto() { User = user, LockoutEnd = lockoutEnd });
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/accessFailedCount/increment";
            var result = await PostAsync(url, user);
            return await result.Content.ReadAsAsync<int>();
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/accessFailedCount/reset";
            return PostAsync(url, user);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/accessFailedCount";
            var result = await PostAsync(url, user);
            return await result.Content.ReadAsAsync<int>();
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/lockoutEnabled";
            var result = await PostAsync(url, user);
            return await result.Content.ReadAsAsync<bool>();
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            var url = $"{ServiceAddress}/lockoutEnabled/{enabled}";
            await PostAsync(url, user);
        }

        #endregion
    }
}
