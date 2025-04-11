using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SunidhiV2_0.Code.Common.Authentication
{
    public class MyStore : IUserStore<MyUser>
    {
        public System.Threading.Tasks.Task CreateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<MyUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<MyUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(MyUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}