﻿using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class LandCompensation
{
    public string LandCompensationId { get; set; } = null!;

    public string? CompensationContent { get; set; }

    public string? CompensationRate { get; set; }

    public string? WithdrawArea { get; set; }

    public decimal? CompensationPrice { get; set; }

    public string? CompensationNote { get; set; }

    public string? UnitPriceLandId { get; set; }

    public string? MeasuredLandInfoId { get; set; }

    public string? LandPosition { get; set; }

    public string? OwnerId { get; set; }

    public virtual MeasuredLandInfo? MeasuredLandInfo { get; set; }

    public virtual UnitPriceLand? UnitPriceLand { get; set; }
}
