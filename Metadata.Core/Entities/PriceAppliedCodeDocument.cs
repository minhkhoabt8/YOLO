using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Entities
{
    public class PriceAppliedCodeDocument
    {
        public string PriceAppliedCodeDocumentId { get; set; }

        public string PriceAppliedCodeId { get; set; } = null!;

        public string DocumentId { get; set; } = null!;


        public virtual PriceAppliedCode PriceAppliedCode { get; set; } = null!;

        public virtual Document Document { get; set; } = null!;

        public static PriceAppliedCodeDocument CreatePriceAppliedCodeDocument(string PriceAppliedCodeId, string DocumentId)
        {
            var projectDocument = new PriceAppliedCodeDocument
            {
                PriceAppliedCodeDocumentId = Guid.NewGuid().ToString(),
                PriceAppliedCodeId = PriceAppliedCodeId,
                DocumentId = DocumentId,
            };
            return projectDocument;
        }
    }
}
