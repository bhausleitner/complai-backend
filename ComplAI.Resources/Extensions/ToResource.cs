using System.Collections.Generic;
using System.Linq;
using ComplAI.DataLayer.Entity;

namespace ComplAI.Resources.Extensions
{
    public static class ToResourceExtensionClass
    {
        public static RegulationResource ToResource(this RegulationEntity regulation)
        {
            return new RegulationResource
            {
                Title = regulation.Title,
                Description = regulation.Description
            };
        }

        public static IEnumerable<RegulationResource> ToResource(this IEnumerable<RegulationEntity> regulations)
        {
            return regulations.Select(regulationEntity => new RegulationResource
            {
                Title = regulationEntity.Title,
                Description = regulationEntity.Description
            }).ToList();
        }

        public static EuDocumentResource ToResource(this EuDocumentEntity regulationEntity)
        {
            return new EuDocumentResource
            {
                PdfUrl = regulationEntity.PdfUrl,
                SearchTerm = regulationEntity.SearchTerm,
                Title = regulationEntity.Title,
                TypeOfAct = regulationEntity.TypeOfAct
            };
        }

        public static IEnumerable<EuDocumentResource> ToResource(this IEnumerable<EuDocumentEntity> regulations)
        {
            return regulations.Select(regulationEntity => new EuDocumentResource
            {
                PdfUrl = regulationEntity.PdfUrl,
                SearchTerm = regulationEntity.SearchTerm,
                Title = regulationEntity.Title,
                TypeOfAct = regulationEntity.TypeOfAct,
                Id = regulationEntity.Id
            }).ToList();
        }
    }
}
