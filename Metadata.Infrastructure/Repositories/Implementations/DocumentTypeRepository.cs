﻿using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class DocumentTypeRepository : GenericRepository<DocumentType , YoloMetadataContext> , IDocumentTypeRepository
    {
        public DocumentTypeRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<DocumentType?> FindByCodeAsync(string code)
        {
            return await _context.DocumentTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());

        }
        public async Task<DocumentType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.DocumentTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<DocumentType>?> GetAllDeletedDocumentTypes()
        {
            return await _context.DocumentTypes.Where(x => x.IsDeleted == true).ToListAsync();
        }

    }
    
}