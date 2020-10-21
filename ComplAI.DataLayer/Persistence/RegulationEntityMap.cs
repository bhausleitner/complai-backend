using ComplAI.DataLayer.Entity;
using MongoDB.Bson.Serialization;

namespace ComplAI.DataLayer.Persistence
{
    
    public class RegulationEntityMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<RegulationEntity>(map =>
            {
                map.AutoMap();
                map.MapCreator(p => new RegulationEntity(p.Description, p.Title));
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Description).SetIsRequired(true);
                map.MapMember(x => x.Title).SetIsRequired(true); ;

            });
        }
    }
}
