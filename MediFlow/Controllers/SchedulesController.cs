using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Schedules.Commands;
using MediFlow.Application.UseCases.Schedules.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly ISender _mediator;

    public SchedulesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAll(
        [FromQuery] int? doctorId,
        CancellationToken cancellationToken)
    {
        try
        {
            var schedules = await _mediator.Send(
                new GetSchedulesQuery(doctorId), cancellationToken);

            return Ok(schedules);
        }
        catch (ArgumentException ex)
        {
            return CommandProblem(ex);
        }
    }

    [HttpGet("availability")]
    [EndpointSummary("Consultar turnos disponibles de un médico")]
    [EndpointDescription(
        "date usa el formato yyyy-MM-dd en UTC. Devuelve turnos completos futuros, " +
        "descontando citas no canceladas y conflictos de salas. " +
        "RoomIds contiene las salas activas asignadas; " +
        "puede estar vacío si el médico no tiene sala. La consulta no reserva el turno.")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleAvailabilityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<ScheduleAvailabilityDto>>> GetAvailability(
        [FromQuery] int doctorId,
        [FromQuery] DateOnly date,
        CancellationToken cancellationToken)
    {
        try
        {
            var slots = await _mediator.Send(
                new GetDoctorAvailabilityQuery(doctorId, date),
                cancellationToken);

            return Ok(slots);
        }
        catch (Exception ex) when (
            ex is ArgumentException or KeyNotFoundException or InvalidOperationException)
        {
            return CommandProblem(ex);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScheduleDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var schedule = await _mediator.Send(
            new GetScheduleByIdQuery(id), cancellationToken);

        if (schedule is null)
            return ScheduleNotFound();

        return Ok(schedule);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ScheduleDto>> Create(
        [FromBody] CreateScheduleDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var schedule = await _mediator.Send(
                new CreateScheduleCommand(dto), cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = schedule.ScheduleId },
                schedule);
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
        [FromBody] UpdateScheduleDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _mediator.Send(
                new UpdateScheduleCommand(id, dto), cancellationToken);

            if (!updated)
                return ScheduleNotFound();

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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404BadRequest)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(
            new DeleteScheduleCommand(id), cancellationToken);

        if (!deleted)
            return ScheduleNotFound();

        return NoContent();
    }

    private ObjectResult ScheduleNotFound() => Problem(
        title: "Horario no encontrado.",
        statusCode: StatusCodes.Status404NotFound);

    private ObjectResult CommandProblem(Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status409Conflict
        };

        return Problem(
            title: exception.Message,
            statusCode: statusCode);
    }
}