using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DocumentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Document
{
    public class DocumentReadDTO
    {
        public string DocumentId { get; set; } = null!;

        public string DocumentTypeId { get; set; }

        public string Number { get; set; }

        public string Notation { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string Epitome { get; set; }

        public string SignInfo { get; set; }

        public string Note { get; set; }

        public string Pen { get; set; }

        public string ReferenceLink { get; set; }

        public bool IsPublic { get; set; }

        public bool IsDeleted { get; set; }

        /*public DocumentType DocumentType { get; set; }*/
        public DocumentTypeReadDTO DocumentType { get; set; }
    }
}
