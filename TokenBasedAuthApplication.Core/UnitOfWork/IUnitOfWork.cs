namespace TokenBasedAuthApplication.Core.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
    bool Commit();
}