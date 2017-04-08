namespace Farfetch.CrossCutting.ExtensionMethods
{
    public static class ClassExtensions
    {
        #region Public methods
        public static bool IsNull<TClass>(this TClass obj)
            where TClass : class
        {
            return obj == null;
        }

        public static bool IsNotNull<TClass>(this TClass obj)
            where TClass : class
        {
            return obj != null;
        }
        #endregion
    }
}
