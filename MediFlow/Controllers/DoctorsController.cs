
using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Doctors.Commands;
using MediFlow.Application.UseCases.Doctors.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ISender _mediator;

    public DoctorsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _mediator.Send(new GetDoctorsQuery());
        return Ok(doctors);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _mediator.Send(new GetDoctorByIdQuery(id));

        if (doctor is null)
            return NotFound();

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
    {
        if (!IsValid(dto.FirstName, dto.LastName,
                     dto.LicenseNumber, dto.Phone, dto.Email))
        {
            return BadRequest("Todos los datos del médico son obligatorios.");
        }

        var command = new CreateDoctorCommand(dto.SpecialtyId, dto.FirstName, dto.LastName, dto.LicenseNumber, dto.Phone, dto.Email);

        try
        {
            var doctorId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById),
                new { id = doctorId },
                new { DoctorId = doctorId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateDoctorDto dto)
    {
        if (!IsValid(dto.FirstName, dto.LastName, dto.LicenseNumber, dto.Phone, dto.Email))
        {
            return BadRequest("Todos los datos del médico son obligatorios.");
        }

        var command = new UpdateDoctorCommand(id, dto.SpecialtyId, dto.FirstName, dto.LastName, dto.LicenseNumber, dto.Phone, dto.Email);

        try
        {
            var updated = await _mediator.Send(command);

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
        var deleted = await _mediator.Send(new DeleteDoctorCommand(id));

        if (!deleted)
            return NotFound();
        return NoContent();
    }
    private static bool IsValid(string firstName, string lastName, string licenseNumber, string phone, string email)
    {
        return !string.IsNullOrWhiteSpace(firstName)
            && !string.IsNullOrWhiteSpace(lastName)
            && !string.IsNullOrWhiteSpace(licenseNumber)
            && !string.IsNullOrWhiteSpace(phone)
            && !string.IsNullOrWhiteSpace(email);
    }
}
