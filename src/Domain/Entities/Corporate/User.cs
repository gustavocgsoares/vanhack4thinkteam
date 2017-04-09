using System;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.Domain.Entities.Base;
using Farfetch.Domain.Enums.Corporate;

namespace Farfetch.Domain.Entities.Corporate
{
    public class User : Entity<User, Guid>
    {
        #region Constructors | Destructors
        public User()
        {
        }
        #endregion

        #region Properties
        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Email { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Password { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual DateTime BirthDate { get; set; }

        public virtual string ProfileImage { get; set; }

        public virtual short AccessAttempts { get; set; }

        public virtual DateTime LastAccessDate { get; set; }

        public virtual DateTime LastAcceptanceTermsDate { get; set; }

        public virtual bool Blocked { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<User, Guid> repository)
        {
            await repository.SaveAsync(this);
        }
        #endregion
    }
}
