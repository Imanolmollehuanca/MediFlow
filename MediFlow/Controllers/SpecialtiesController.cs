using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Specialties.Commands;
using MediFlow.Application.UseCases.Specialties.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly ISender _mediator;

    public SpecialtiesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var specialties = await _mediator.Send(new GetSpecialtiesQuery());
        return Ok(specialties);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var specialty = await _mediator.Send(new GetSpecialtyByIdQuery(id));

        if (specialty is null)
            return NotFound();

        return Ok(specialty);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto dto)
    {
        try
        {
            var specialtyId = await _mediator.Send(new CreateSpecialtyCommand(dto));

            return CreatedAtAction(
                nameof(GetById),
                new { id = specialtyId },
                new { SpecialtyId = specialtyId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyDto dto)
    {
        try
        {
            var updated = await _mediator.Send(new UpdateSpecialtyCommand(id, dto));

            if (!updated)
                return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteSpecialtyCommand(id));
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
