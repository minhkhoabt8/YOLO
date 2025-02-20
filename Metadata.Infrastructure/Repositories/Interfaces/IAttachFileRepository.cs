﻿using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IAttachFileRepository :
        IAddAsync<AttachFile>,
        IFindAsync<AttachFile>,
        IGetAllAsync<AttachFile>,
        IUpdate<AttachFile>,
        IDelete<AttachFile>
    {
        Task<IEnumerable<AttachFile?>> GetAllAttachFilesOfOwnerAsync(string ownerId);
    }
}
