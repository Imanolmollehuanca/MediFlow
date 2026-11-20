using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Notifications.Commands;
using MediFlow.Application.UseCases.Notifications.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize(Roles = "Admin")]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await mediator.Send(new GetAllNotificationsQuery()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var notification = await mediator.Send(
            new GetNotificationByIdQuery(id));

        return notification is null
            ? NotFound()
            : Ok(notification);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationDto dto)
    {
        var notification = await mediator.Send(
            new CreateNotificationCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = notification.NotificationId },
            notification);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateNotificationDto dto)
    {
        var notification = await mediator.Send(
            new UpdateNotificationCommand(id, dto));

        return notification is null
            ? NotFound()
            : Ok(notification);
    }

    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notification = await mediator.Send(
            new MarkNotificationAsReadCommand(id));

        return notification is null
            ? NotFound()
            : Ok(notification);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await mediator.Send(
            new DeleteNotificationCommand(id));

        return deleted
            ? NoContent()
            : NotFound();
    }
}
