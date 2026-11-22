using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.PrescriptionDetails.Commands;
using MediFlow.Application.UseCases.PrescriptionDetails.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionDetailsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrescriptionDetailDto>>> GetAll()
    {
        var details = await sender.Send(
            new GetAllPrescriptionDetailsQuery());

        return Ok(details);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PrescriptionDetailDto>> GetById(int id)
    {
        var detail = await sender.Send(
            new GetPrescriptionDetailByIdQuery(id));

        if (detail is null)
            return NotFound(new { message = "Detalle de receta no encontrado." });

        return Ok(detail);
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionDetailDto>> Create(
        [FromBody] CreatePrescriptionDetailDto dto)
    {
        var detail = await sender.Send(
            new CreatePrescriptionDetailCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = detail.PrescriptionDetailId },
            detail);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePrescriptionDetailDto dto)
    {
        var updated = await sender.Send(
            new UpdatePrescriptionDetailCommand(id, dto));

        if (!updated)
            return NotFound(new { message = "Detalle de receta no encontrado." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await sender.Send(
            new DeletePrescriptionDetailCommand(id));

        if (!deleted)
            return NotFound(new { message = "Detalle de receta no encontrado." });

        return NoContent();
    }
}