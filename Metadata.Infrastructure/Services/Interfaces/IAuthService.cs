﻿using Metadata.Infrastructure.DTOs.AccountMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AccountMappping> GetAccountByIdAsync(string Id);
    }
}
