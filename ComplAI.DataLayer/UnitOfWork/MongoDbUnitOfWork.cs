using System.Threading.Tasks;
using ComplAI.DataLayer.Interfaces;

namespace ComplAI.DataLayer.UnitOfWork
{
    public class MongoDbUnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public MongoDbUnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
