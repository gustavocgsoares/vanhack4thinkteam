using System;
using Farfetch.Application.Interfaces.Base;

namespace Farfetch.Application.Contexts.Base
{
    public abstract class BaseApp : IBaseApp
    {
        #region Fields | Members
        private bool disposed;
        #endregion

        #region Constructors | Destructors
        protected BaseApp()
        {
        }

        ~BaseApp()
        {
            Dispose(false);
        }
        #endregion

        #region Disposable members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
        }
        #endregion
    }
}
