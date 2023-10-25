﻿using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IAssetGroupRepository : IGetAllAsync<AssetGroup>,
        IFindAsync<AssetGroup>,
        IAddAsync<AssetGroup>,
        IDelete<AssetGroup>
    {
        Task<AssetGroup?> FindByCodeAsync(string code);
        Task<AssetGroup> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<IEnumerable<AssetGroup>?> GetAllDeletedAssetGroup();
    }
}