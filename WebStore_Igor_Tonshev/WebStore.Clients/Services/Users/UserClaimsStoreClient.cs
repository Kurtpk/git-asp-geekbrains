using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStore.DomainNew.Dto.User;
using WebStore.DomainNew.Entities;

namespace WebStore.Clients.Services.Users
{
    public partial class UsersClient
    {
        #region IUserClaimStore
        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/claims";
            var result = await PostAsync(url, user);
            return await result.Content.ReadAsAsync<List<Claim>>();
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/claims/add";
            return PostAsync(url, new AddClaimsDto() { User = user, Claims = claims });
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/claims/replace";
            return PostAsync(url, new ReplaceClaimsDto() { User = user, Claim = claim, NewClaim = newClaim });
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/claims/remove";
            return PostAsync(url, new RemoveClaimsDto() { User = user, Claims = claims });
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var url = $"{ServiceAddress}/claims/user";
            var result = await PostAsync(url, claim);
            return await result.Content.ReadAsAsync<List<User>>();
        }

        #endregion
    }
}
