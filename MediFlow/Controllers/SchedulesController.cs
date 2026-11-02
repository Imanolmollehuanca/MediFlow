
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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetSchedulesQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var schedule = await _mediator.Send(new GetScheduleByIdQuery(id));
        return schedule is null ? NotFound() : Ok(schedule);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScheduleDto dto)
    {
        try
        {
            var id = await _mediator.Send(new CreateScheduleCommand(
                dto.DoctorId,
                dto.DayOfWeek,
                dto.StartTime,
                dto.EndTime,
                dto.SlotDurationMinutes));

            return CreatedAtAction(nameof(GetById),
                new { id },
                new { ScheduleId = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, 
        [FromBody] UpdateScheduleDto dto)
    {
        try
        {
            var updated = await _mediator.Send(new UpdateScheduleCommand(
                id,
                dto.DoctorId,
                dto.DayOfWeek,
                dto.StartTime,
                dto.EndTime,
                dto.SlotDurationMinutes ));

            return updated ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteScheduleCommand(id));
        return deleted ? NoContent() : NotFound();
    }
}
