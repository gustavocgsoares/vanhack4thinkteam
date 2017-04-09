using System;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.Application.Model.Enums.V1.Corporate;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Model.Contexts.V1.Corporate
{
    public class UserModel : BaseModel<UserModel>
    {
        #region Properties
        public virtual string Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual Gender? Gender { get; set; }

        public virtual DateTime? BirthDate { get; set; }

        public virtual string ProfileImage { get; set; }

        public virtual bool? AlreadyExists { get; set; }

        public virtual UserLoggedWith LoggedWith { get; set; }

        public virtual DateTime? LastAccessDate { get; set; }

        public virtual DateTime? LastAcceptanceTermsDate { get; set; }

        public virtual bool? Blocked { get; set; }
        #endregion

        #region Converters
        public static UserModel ToModel(User entity, string url)
        {
            var model = Instance();

            return ToModel(entity, model);
        }

        public static UserModel ToModel(User entity, UserModel model = null)
        {
            model = model ?? Instance();

            model.Id = entity.Id.ToString();
            model.FirstName = entity.Name;
            model.LastName = entity.Surname;
            model.Email = entity.Email;
            model.Gender = (Gender)entity.Gender;
            model.BirthDate = entity.BirthDate;
            model.ProfileImage = entity.ProfileImage;
            model.LastAcceptanceTermsDate = entity.LastAcceptanceTermsDate;
            model.Blocked = entity.Blocked;

            return model;
        }

        public User ToDomain()
        {
            return ToDomain(new User());
        }

        public User ToDomain(User entity)
        {
            entity = entity ?? new User();

            entity.Name = FirstName;
            entity.Surname = LastName;
            entity.Email = Email;
            entity.Gender = (Domain.Enums.Corporate.Gender)Gender;
            entity.BirthDate = BirthDate.GetValueOrDefault();
            entity.ProfileImage = ProfileImage;
            entity.LastAcceptanceTermsDate = LastAcceptanceTermsDate.GetValueOrDefault();

            return entity;
        }
        #endregion
    }
}
