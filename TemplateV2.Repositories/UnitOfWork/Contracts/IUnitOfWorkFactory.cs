namespace TemplateV2.Repositories.UnitOfWork.Contracts
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetUnitOfWork(bool beginTransaction = true);

        IUnitOfWork GetMySQLUnitOfWork(bool beginTransaction = true);
    }
}
