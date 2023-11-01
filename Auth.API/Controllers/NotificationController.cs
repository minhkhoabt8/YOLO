using Auth.Infrastructure.DTOs.Notification;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Auth.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("auth/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Query Notification
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<NotificationReadDTO>))]
        public async Task<IActionResult> QueryNotifications([FromQuery] NotificationQuery query)
        {
            var notifications = await _notificationService.QueryNotification(query);

            return ResponseFactory.Ok(notifications);
        }

        /// <summary>
        /// Create New Notification
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<NotificationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateNotification(NotificationWriteDTO input)
        {
            var notifications = await _notificationService.CreateNotificationAsync(input);

            return ResponseFactory.Created(notifications);
        }

        /// <summary>
        /// Mark Notification As Read
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<NotificationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            var notification = await _notificationService.MarkAsReadAsync(id);

            return ResponseFactory.Ok(notification);
        }


        /// <summary>
        /// Delete Notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAccount(string id)
        {

            await _notificationService.DeleteNotificationAsync(id);

            return ResponseFactory.NoContent();
        }
    }
}
