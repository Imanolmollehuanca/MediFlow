using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Medications.Commands;
using MediFlow.Application.UseCases.Medications.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicationDto>>> GetAll()
    {
        var medications = await sender.Send(
            new GetAllMedicationsQuery());

        return Ok(medications);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MedicationDto>> GetById(int id)
    {
        var medication = await sender.Send(
            new GetMedicationByIdQuery(id));

        if (medication is null)
            return NotFound(new { message = "Medicamento no encontrado." });

        return Ok(medication);
    }

    [HttpPost]
    public async Task<ActionResult<MedicationDto>> Create(
        [FromBody] CreateMedicationDto dto)
    {
        var medication = await sender.Send(
            new CreateMedicationCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = medication.MedicationId },
            medication);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateMedicationDto dto)
    {
        var updated = await sender.Send(
            new UpdateMedicationCommand(id, dto));

        if (!updated)
            return NotFound(new { message = "Medicamento no encontrado." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await sender.Send(
            new DeleteMedicationCommand(id));

        if (!deleted)
            return NotFound(new { message = "Medicamento no encontrado." });

        return NoContent();
    }
}