using Microsoft.AspNetCore.Identity;
using WebStore.DomainNew.Entities;

namespace WebStore.Interfaces.Api
{
    public interface IUsersClient : IUserRoleStore<User>,
        IUserClaimStore<User>,
        IUserPasswordStore<User>,
        IUserTwoFactorStore<User>,
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>,
        IUserLoginStore<User>,
        IUserLockoutStore<User>
    { }
}
