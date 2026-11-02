using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases;
using MediFlow.Application.UseCases.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(AppointmentUseCase useCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await useCase.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var appointment = await useCase.GetByIdAsync(id, cancellationToken);
        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create(AppointmentRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var appointment = await useCase.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = appointment.AppointmentId }, appointment);
        }
        catch (BusinessRuleException exception) { return BadRequest(new { message = exception.Message }); }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> Update(int id, AppointmentRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var appointment = await useCase.UpdateAsync(id, request, cancellationToken);
            return appointment is null ? NotFound() : Ok(appointment);
        }
        catch (BusinessRuleException exception) { return BadRequest(new { message = exception.Message }); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
        await useCase.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
