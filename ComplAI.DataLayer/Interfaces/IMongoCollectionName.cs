namespace ComplAI.DataLayer.Interfaces
{
    public interface IMongoCollectionName<TEntity>
    {
        string GetCollectionName();
    }
}