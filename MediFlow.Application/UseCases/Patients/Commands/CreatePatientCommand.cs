using System.Text.RegularExpressions;
using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record CreatePatientCommand(CreatePatientDto Dto) : IRequest<int>;

internal sealed class CreatePatientCommandHandler
    : IRequestHandler<CreatePatientCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePatientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        ValidatePatientData(dto.DocumentType, dto.DocumentNumber, dto.FirstName, dto.LastName, dto.BirthDate, dto.Gender, dto.Phone, dto.Email);

        var exists = await _unitOfWork.Patients.ExistsByDocumentAsync(dto.DocumentType, dto.DocumentNumber);
        if (exists)
            throw new InvalidOperationException($"Ya existe un paciente registrado con {dto.DocumentType.Trim().ToUpper()} {dto.DocumentNumber.Trim()}.");

        var patient = new Patient
        {
            DocumentType = dto.DocumentType.Trim().ToUpper(),
            DocumentNumber = dto.DocumentNumber.Trim(),
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            BirthDate = dto.BirthDate,
            Gender = dto.Gender.Trim(),
            Phone = dto.Phone.Trim(),
            Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
            Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return patient.PatientId;
    }

    internal static void ValidatePatientData(
        string documentType,
        string documentNumber,
        string firstName,
        string lastName,
        DateOnly birthDate,
        string gender,
        string phone,
        string? email)
    {
        if (string.IsNullOrWhiteSpace(documentType))
            throw new ArgumentException("El tipo de documento es obligatorio.");

        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("El número de documento es obligatorio.");

        var normalizedDocType = documentType.Trim().ToUpper();
        var normalizedDocNumber = documentNumber.Trim();

        if (normalizedDocType == "DNI" && !Regex.IsMatch(normalizedDocNumber, @"^\d{8}$"))
            throw new ArgumentException("El DNI debe tener exactamente 8 dígitos numéricos.");

        if (normalizedDocType != "DNI" && (normalizedDocNumber.Length < 6 || normalizedDocNumber.Length > 20))
            throw new ArgumentException("El número de documento debe tener entre 6 y 20 caracteres.");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre del paciente es obligatorio.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido del paciente es obligatorio.");

        if (birthDate == default || birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("La fecha de nacimiento es obligatoria y no puede ser futura.");

        if (string.IsNullOrWhiteSpace(gender))
            throw new ArgumentException("El género del paciente es obligatorio.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("El teléfono es obligatorio.");

        if (!Regex.IsMatch(phone.Trim(), @"^\+?\d{7,15}$"))
            throw new ArgumentException("El teléfono debe contener entre 7 y 15 dígitos numéricos.");

        if (!string.IsNullOrWhiteSpace(email) &&
            !Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("El formato del correo electrónico no es válido.");
    }
}