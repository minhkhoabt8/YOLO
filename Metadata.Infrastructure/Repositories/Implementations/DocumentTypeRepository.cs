﻿using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DocumentType;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;
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
        public async Task<DocumentType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.DocumentTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }
        public async Task<IEnumerable<DocumentType>?> GetAllActivedDocumentTypes()
        {
            return await _context.DocumentTypes.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<DocumentType>> QueryAsync (DocumentTypeQuery query , bool trackChanges = false)
        {
            IQueryable<DocumentType> documentTypes = _context.DocumentTypes.Where(c => c.IsDeleted == false);

            if (!trackChanges)
            {
                documentTypes = documentTypes.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                documentTypes = documentTypes.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                documentTypes = documentTypes.Where(c => c.Name.Contains(query.SearchText)); ;
            }
            //search by code
            if (!string.IsNullOrWhiteSpace(query.SearchByNames))
            {
                documentTypes = documentTypes.Where(c => c.Code.Contains(query.SearchByNames)); ;
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                documentTypes = documentTypes.OrderByDynamic(query.OrderBy);
            }

            IEnumerable<DocumentType> enumeratedDocumentTypes = documentTypes.AsEnumerable();
            return await Task.FromResult(enumeratedDocumentTypes);
        }

        public async Task<DocumentType?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted)
        {
            var check = await _context.DocumentTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.DocumentTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }

        public async Task<DocumentType?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted)
        {
            var check = await _context.DocumentTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.DocumentTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }
    }
    
}
