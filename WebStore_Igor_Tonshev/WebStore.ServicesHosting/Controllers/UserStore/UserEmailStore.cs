using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DomainNew.Entities;

namespace WebStore.ServicesHosting.Controllers
{
    public partial class UsersApiController
    {
        #region IUserEmailStore

        [HttpPost("email/{email}")]
        public async Task SetEmailAsync([FromBody]User user, string email)
        {
            await _userStore.SetEmailAsync(user, email);
        }

        [HttpPost("email")]
        public async Task<string> GetEmailAsync([FromBody]User user)
        {
            return await _userStore.GetEmailAsync(user);
        }

        [HttpPost("emailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody]User user)
        {
            return await _userStore.GetEmailConfirmedAsync(user);
        }

        [HttpPost("emailConfirmed/{confirmed}")]
        public async Task SetEmailConfirmedAsync([FromBody]User user, bool confirmed)
        {
            await _userStore.SetEmailConfirmedAsync(user, confirmed);
        }

        [HttpGet("email/{normalizedEmail}")]
        public async Task<User> FindByEmailAsync(string normalizedEmail)
        {
            return await _userStore.FindByEmailAsync(normalizedEmail);
        }

        [HttpPost("normalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody]User user)
        {
            return await _userStore.GetNormalizedEmailAsync(user);
        }

        [HttpPost("normalizedEmail/{normalizedEmail}")]
        public async Task SetNormalizedEmailAsync([FromBody]User user, string normalizedEmail)
        {
            await _userStore.SetNormalizedEmailAsync(user, normalizedEmail);
        }

        #endregion
    }
}
