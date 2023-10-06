﻿using Metadata.Infrastructure.DTOs.Document;
using Microsoft.AspNetCore.Http;
using SharedLib.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Project
{
    public class ProjectWriteDTO
    {
        [Required]
        public string ProjectCode { get; set; }
        [Required]
        public string ProjectName { get; set; }

        public string ProjectLocation { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        [InputType(typeof(decimal))]
        public decimal ProjectExpense { get; set; }

        [InputType(typeof(DateTime))]
        public DateTime ProjectApprovalDate { get; set; }

        public string ImplementationYear { get; set; }

        public string RegulatedUnitPrice { get; set; }

        public string ProjectBriefNumber { get; set; }

        public string ProjectNote { get; set; }

        public string PriceAppliedCodeId { get; set; }

        public string CheckCode { get; set; }

        public string ReportSignal { get; set; }

        public string ReportNumber { get; set; }

        public string PriceBasis { get; set; }

        public string LandCompensationBasis { get; set; }

        public string AssetCompensationBasis { get; set; }

        public bool ProjectStatus { get; set; } 

        public IEnumerable<DocumentWriteDTO>? Documents { get; set; }
    }

    public class DocumentInProjectWriteDTO
    {
        public string DocumentTypeId { get; set; }
        [Required]
        [MaxLength(10)]
        public string Number { get; set; }
        [Required]
        [MaxLength(10)]
        public string Notation { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        [Required]
        public string Epitome { get; set; }

        public string SignInfo { get; set; }

        public string Note { get; set; }

        public string Pen { get; set; }

        public bool? IsPublic { get; set; } = false;

        [Required]
        public IFormFile FileAttach { get; set; }
    }

}
