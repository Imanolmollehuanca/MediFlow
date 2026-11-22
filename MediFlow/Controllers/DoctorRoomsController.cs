using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.DoctorRooms.Commands;
using MediFlow.Application.UseCases.DoctorRooms.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorRoomsController : ControllerBase
{
    private readonly ISender _mediator;

    public DoctorRoomsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DoctorRoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<DoctorRoomDto>>> GetAll(
        [FromQuery] int? doctorId,
        [FromQuery] int? roomId,
        CancellationToken cancellationToken)
    {
        try
        {
            var assignments = await _mediator.Send(
                new GetDoctorRoomsQuery(doctorId, roomId), cancellationToken);

            return Ok(assignments);
        }
        catch (ArgumentException ex)
        {
            return CommandProblem(ex);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DoctorRoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorRoomDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var assignment = await _mediator.Send(new GetDoctorRoomByIdQuery(id), cancellationToken);

        if (assignment is null)
            return AssignmentNotFound();

        return Ok(assignment);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DoctorRoomDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DoctorRoomDto>> Create(
        [FromBody] CreateDoctorRoomDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var assignment = await _mediator.Send(new CreateDoctorRoomCommand(dto), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = assignment.DoctorRoomId }, assignment);
        }
        catch (Exception ex) when (
            ex is ArgumentException or KeyNotFoundException or InvalidOperationException)
        {
            return CommandProblem(ex);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateDoctorRoomDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _mediator.Send(new UpdateDoctorRoomCommand(id, dto), cancellationToken);

            if (!updated)
                return AssignmentNotFound();

            return NoContent();
        }
        catch (Exception ex) when (
            ex is ArgumentException or KeyNotFoundException or InvalidOperationException)
        {
            return CommandProblem(ex);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteDoctorRoomCommand(id), cancellationToken);

        if (!deleted)
            return AssignmentNotFound();

        return NoContent();
    }

    private ObjectResult AssignmentNotFound() => Problem(
        title: "Asignación de sala no encontrada.",
        statusCode: StatusCodes.Status404NotFound);

    private ObjectResult CommandProblem(Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status409Conflict
        };

        return Problem(title: exception.Message, statusCode: statusCode);
    }
}
