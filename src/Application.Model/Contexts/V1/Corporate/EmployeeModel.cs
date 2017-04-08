using System;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Model.Contexts.V1.Corporate
{
    public class EmployeeModel : BaseModel<EmployeeModel>
    {
        #region Properties
        public virtual string Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }
        #endregion

        #region Converters
        public static EmployeeModel ToModel(Employee entity, string url)
        {
            var model = Instance();

            return ToModel(entity, model);
        }

        public static EmployeeModel ToModel(Employee entity, EmployeeModel model = null)
        {
            model = model ?? Instance();

            model.Id = entity.Id.ToString();
            model.FirstName = entity.Name;
            model.LastName = entity.Surname;
            model.Email = entity.Login;

            return model;
        }

        public Employee ToDomain()
        {
            return ToDomain(new Employee());
        }

        public Employee ToDomain(Employee entity)
        {
            entity = entity ?? new Employee();

            entity.Id = Id.HasValue() ? Id.To<Guid>() : default(Guid);
            entity.Name = FirstName;
            entity.Surname = LastName;
            entity.Login = Email;
            entity.Password = Password;

            return entity;
        }
        #endregion
    }
}
