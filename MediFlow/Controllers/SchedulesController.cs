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
            var schedules = await _mediator.Send(new GetSchedulesQuery(doctorId), cancellationToken);

            return Ok(schedules);
        }
        catch (ArgumentException ex)
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
        var schedule = await _mediator.Send(new GetScheduleByIdQuery(id), cancellationToken);

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
            var schedule = await _mediator.Send(new CreateScheduleCommand(dto), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = schedule.ScheduleId }, schedule);
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
            var updated = await _mediator.Send(new UpdateScheduleCommand(id, dto), cancellationToken);

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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteScheduleCommand(id), cancellationToken);

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

        return Problem(title: exception.Message, statusCode: statusCode);
    }
}
