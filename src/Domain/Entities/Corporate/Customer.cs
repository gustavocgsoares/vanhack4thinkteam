using System;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Farfetch.Domain.Entities.Base;

namespace Farfetch.Domain.Entities.Corporate
{
    public class Customer : Entity<Customer, Guid>
    {
        #region Constructors | Destructors
        public Customer()
        {
        }
        #endregion

        #region Properties
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Password { get; set; }

        public virtual string ShippingAddress { get; set; }

        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        public virtual string City { get; set; }

        public virtual string Zip { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<Customer, Guid> repository)
        {
            var customer = await repository.GetFirstAsync(e => e.Email == Email);
            customer.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "email"));

            Active = true;
            AddedDate = DateTime.UtcNow;

            await repository.SaveAsync(this);
        }

        public override async Task UpdateAsync(IRepository<Customer, Guid> repository)
        {
            var customer = await repository.GetAsync(Id);
            customer.IsNull().Throw<DataNotFoundException>(Id);

            if (Email.HasValue() && !customer.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase))
            {
                var customerSameEmail = await repository.GetFirstAsync(e => e.Email == Email);
                customerSameEmail.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "email"));

                customer.Email = Email;
            }

            if (FirstName.HasValue())
            {
                customer.FirstName = FirstName;
            }

            if (LastName.HasValue())
            {
                customer.LastName = LastName;
            }

            if (Phone.HasValue())
            {
                customer.Phone = Phone;
            }

            if (Password.HasValue())
            {
                customer.Password = Password;
            }

            if (ShippingAddress.HasValue())
            {
                customer.ShippingAddress = ShippingAddress;
            }

            if (Country.HasValue())
            {
                customer.Country = Country;
            }

            if (State.HasValue())
            {
                customer.State = State;
            }

            if (City.HasValue())
            {
                customer.City = City;
            }

            if (Zip.HasValue())
            {
                customer.Zip = Zip;
            }

            customer.Active = Active;
            ModifiedDate = DateTime.UtcNow;

            await repository.SaveAsync(customer);
        }

        public override async Task DeleteAsync(IRepository<Customer, Guid> repository)
        {
            var customer = await repository.GetAsync(Id);

            if (customer.IsNotNull())
            {
                Active = false;
                DeletedDate = DateTime.UtcNow;
                await repository.SaveAsync(this);
            }
        }
        #endregion
    }
}
