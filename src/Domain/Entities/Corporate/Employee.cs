using System;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Farfetch.Domain.Entities.Base;

namespace Farfetch.Domain.Entities.Corporate
{
    public class Employee : Entity<Employee, Guid>
    {
        #region Constructors | Destructors
        public Employee()
        {
        }
        #endregion

        #region Properties
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual string IdentityDocument { get; set; }

        public virtual string SocialSecurity { get; set; }

        public virtual Guid UpdatingEmployeeId { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<Employee, Guid> repository)
        {
            var employee = await repository.GetFirstAsync(e => e.Email == Email);
            employee.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "email"));

            Active = true;
            AddedDate = DateTime.UtcNow;

            await repository.SaveAsync(this);
        }

        public override async Task UpdateAsync(IRepository<Employee, Guid> repository)
        {
            var employee = await repository.GetAsync(Id);
            employee.IsNull().Throw<DataNotFoundException>(Id);

            if (Email.HasValue() && !employee.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase))
            {
                var employeeSameEmail = await repository.GetFirstAsync(e => e.Email == Email);
                employeeSameEmail.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "email"));

                employee.Email = Email;
            }

            if (FirstName.HasValue())
            {
                employee.FirstName = FirstName;
            }

            if (LastName.HasValue())
            {
                employee.LastName = LastName;
            }

            if (Password.HasValue())
            {
                employee.Password = Password;
            }

            if (IdentityDocument.HasValue())
            {
                employee.IdentityDocument = IdentityDocument;
            }

            if (SocialSecurity.HasValue())
            {
                employee.SocialSecurity = SocialSecurity;
            }

            employee.Active = Active;
            ModifiedDate = DateTime.UtcNow;

            await repository.SaveAsync(employee);
        }

        public override async Task DeleteAsync(IRepository<Employee, Guid> repository)
        {
            var employee = await repository.GetAsync(Id);

            if (employee.IsNotNull())
            {
                Active = false;
                DeletedDate = DateTime.UtcNow;
                await repository.SaveAsync(this);
            }
        }
        #endregion
    }
}
