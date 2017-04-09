using System;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Model.Contexts.V1.Corporate
{
    public class CustomerModel : BaseModel<CustomerModel>
    {
        #region Properties
        public virtual string Id { get; set; }

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

        public virtual bool? Active { get; set; }
        #endregion

        #region Converters
        public static CustomerModel ToModel(Customer entity)
        {
            if (entity.IsNull())
            {
                return null;
            }

            var model = Instance();

            return ToModel(entity, model);
        }

        public static CustomerModel ToModel(Customer entity, CustomerModel model = null)
        {
            if (entity.IsNull())
            {
                return null;
            }

            model = model ?? Instance();

            model.Id = entity.Id.ToString();
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.Email = entity.Email;
            model.Active = entity.Active;

            return model;
        }

        public Customer ToDomain()
        {
            return ToDomain(new Customer());
        }

        public Customer ToDomain(Customer entity)
        {
            entity = entity ?? new Customer();

            entity.Id = Id.HasValue() ? Id.To<Guid>() : default(Guid);
            entity.FirstName = FirstName;
            entity.LastName = LastName;
            entity.Email = Email;
            entity.Password = Password;
            entity.Active = Active.GetValueOrDefault();

            return entity;
        }
        #endregion
    }
}
