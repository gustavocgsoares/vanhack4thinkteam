namespace Farfetch.Application.Model.Contexts.Base
{
    public abstract class BaseModel<TModel> : Resource
        where TModel : new()
    {
        #region Static methods
        public static TModel Instance()
        {
            return new TModel();
        }
        #endregion
    }
}