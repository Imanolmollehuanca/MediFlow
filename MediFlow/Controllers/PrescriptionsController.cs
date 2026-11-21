using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Prescriptions.Commands;
using MediFlow.Application.UseCases.Prescriptions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAll()
    {
        var prescriptions = await sender.Send(
            new GetAllPrescriptionsQuery());

        return Ok(prescriptions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PrescriptionDto>> GetById(int id)
    {
        var prescription = await sender.Send(
            new GetPrescriptionByIdQuery(id));

        if (prescription is null)
            return NotFound(new { message = "Receta no encontrada." });

        return Ok(prescription);
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionDto>> Create(
        [FromBody] CreatePrescriptionDto dto)
    {
        var prescription = await sender.Send(
            new CreatePrescriptionCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = prescription.PrescriptionId },
            prescription);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePrescriptionDto dto)
    {
        var updated = await sender.Send(
            new UpdatePrescriptionCommand(id, dto));

        if (!updated)
            return NotFound(new { message = "Receta no encontrada." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await sender.Send(
            new DeletePrescriptionCommand(id));

        if (!deleted)
            return NotFound(new { message = "Receta no encontrada." });

        return NoContent();
    }
}