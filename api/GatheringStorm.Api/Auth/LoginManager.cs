using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models.DB;

namespace GatheringStorm.Api.Auth
{
    public interface ILoginManager
    {
        User LoggedInUser { get; }

        Task SetLoggedInUser(string mail, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class LoginManager : ILoginManager
    {
        private readonly AppDbContext dbContext;

        public LoginManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User LoggedInUser { get; private set; }

        public async Task SetLoggedInUser(string mail, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await dbContext.Users.FindAsync(mail);
            if (user == null)
            {
                user = new User
                {
                    Mail = mail
                };
                await this.dbContext.Users.AddAsync(user, cancellationToken);
                await this.dbContext.SaveChangesAsync(cancellationToken);
            }
            this.LoggedInUser = user;
        }
    }
}
