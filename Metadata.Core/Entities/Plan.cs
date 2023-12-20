using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

    public partial class Plan : ITextSearchableEntity
    {
        [Key]
        public string PlanId { get; set; } = Guid.NewGuid().ToString();

        public string ProjectId { get; set; } = null!;

        public string PlanReportInfo { get; set; } = null!;

        public string PlanCode { get; set; } = null!;

        public string? PlanDescription { get; set; }

        public string? PlanCreateBase { get; set; }

        public string PlanApprovedBy { get; set; } = null!;

        public string? PlanReportSignal { get; set; }

        public DateTime? PlanReportDate { get; set; }

        public DateTime PlanCreatedTime { get; set; } = DateTime.Now;

    public DateTime PlanEndedTime { get; set; }

        public string PlanCreatedBy { get; set; } = null!;

        public string PlanStatus { get; set; } = null!;

        public string? RejectReason { get; set; } = "";

        public int TotalOwnerSupportCompensation { get; set; } = 0;

        public decimal TotalPriceCompensation { get; set; } = 0;

        public decimal TotalPriceLandSupportCompensation { get; set; } = 0;

        public decimal TotalPriceHouseSupportCompensation { get; set; } = 0;

        public decimal TotalPriceArchitectureSupportCompensation { get; set; } = 0;

        public decimal TotalPricePlantSupportCompensation { get; set; } = 0;

        public decimal TotalDeduction { get; set; } = 0;

        public decimal TotalLandRecoveryArea { get; set; } = 0;
    
        public decimal TotalOwnerSupportPrice { get; set; } = 0;

        public decimal TotalGpmbServiceCost { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<AttachFile> AttachFiles { get; set; } = new List<AttachFile>();

        public virtual ICollection<Owner> Owners { get; set; } = new List<Owner>();

        public virtual Project Project { get; set; } = null!;

        public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
        {
            {() => nameof(PlanCode), .5},
            {() => nameof(PlanCode), .2},
        };
    }
