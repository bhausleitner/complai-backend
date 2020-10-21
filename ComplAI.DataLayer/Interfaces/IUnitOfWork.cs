using System.Threading.Tasks;

namespace ComplAI.DataLayer.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
