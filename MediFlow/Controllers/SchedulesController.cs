using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases;
using MediFlow.Application.UseCases.Schedules;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController(ScheduleUseCase useCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ScheduleDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await useCase.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScheduleDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var schedule = await useCase.GetByIdAsync(id, cancellationToken);
        return schedule is null ? NotFound() : Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<ScheduleDto>> Create(ScheduleRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var schedule = await useCase.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = schedule.ScheduleId }, schedule);
        }
        catch (BusinessRuleException exception) { return BadRequest(new { message = exception.Message }); }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScheduleDto>> Update(int id, ScheduleRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var schedule = await useCase.UpdateAsync(id, request, cancellationToken);
            return schedule is null ? NotFound() : Ok(schedule);
        }
        catch (BusinessRuleException exception) { return BadRequest(new { message = exception.Message }); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
        await useCase.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
