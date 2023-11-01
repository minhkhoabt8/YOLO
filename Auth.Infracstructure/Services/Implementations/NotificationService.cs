using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Notification;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;

namespace Auth.Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationReadDTO> CreateNotificationAsync(NotificationWriteDTO dto)
        {
            var notification =  _mapper.Map<Notification>(dto);

            await _unitOfWork.NotificationRepository.AddAsync(notification);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<NotificationReadDTO>(notification);
        }

        public async Task<PaginatedResponse<NotificationReadDTO>> QueryNotification(NotificationQuery query)
        {
            var notifications = await _unitOfWork.NotificationRepository.QueryAsync(query);

            return PaginatedResponse<NotificationReadDTO>.FromEnumerableWithMapping(notifications, query, _mapper);
        }

        public async Task<NotificationReadDTO> MarkAsReadAsync(string notiId)
        {
            var notification = await _unitOfWork.NotificationRepository.FindAsync(notiId)
                ?? throw new EntityWithIDNotFoundException<Notification>(notiId);
            
            notification.IsRead = true;
            
            await _unitOfWork.CommitAsync();

            return _mapper.Map<NotificationReadDTO>(notification);
        }

        public async Task DeleteNotificationAsync(string notiId)
        {
            var notification = await _unitOfWork.NotificationRepository.FindAsync(notiId)
                ?? throw new EntityWithIDNotFoundException<Notification>(notiId);

            notification.IsDeleted = true;

            await _unitOfWork.CommitAsync();

        }

    }
}
