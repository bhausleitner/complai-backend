using System;

namespace ComplAI.DataLayer.Entity
{
    public class RegulationEntity
    {
        public RegulationEntity(string description, string title)
        {
            Description = description;
            Title = title;
            Id = Guid.NewGuid();
        }

        public RegulationEntity(Guid id, string description, string title)
        {
            Id = id;
            Description = description;
            Title = title;
        }

        public string Title { get; }
        public string Description { get; }
        public Guid Id { get; set; }

        
    }
}
