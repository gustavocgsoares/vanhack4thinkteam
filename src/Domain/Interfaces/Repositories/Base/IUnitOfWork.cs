using System;
using System.Threading;
using System.Threading.Tasks;

namespace Farfetch.Application.Interfaces.Base
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync(CancellationToken ct);

        Task CommitAndRefreshChangesAsync(CancellationToken ct);

        void RollbackChanges();

        void SetConnection(string connectionString);
    }
}