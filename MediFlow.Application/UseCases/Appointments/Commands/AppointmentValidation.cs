
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Commands;

internal static class AppointmentValidation
{
    public static async Task ValidateAsync(IGenericRepository<Appointment> appointments, IGenericRepository<Patient> patients, IGenericRepository<Doctor> doctors,
        IGenericRepository<Schedule> schedules, int patientId, int doctorId,
        DateTime start, DateTime end, int? excludeAppointmentId = null)
    {
        if (start >= end)
            throw new ArgumentException("La fecha y hora inicial debe ser menor que la final.");

        if (start.Date != end.Date)
            throw new ArgumentException("La cita debe comenzar y terminar el mismo día.");

        var patient = await patients.GetByIdAsync(patientId);

        if (patient is null || !patient.IsActive)
            throw new ArgumentException("El paciente no existe o está inactivo.");

        var doctor = await doctors.GetByIdAsync(doctorId);

        if (doctor is null || !doctor.IsActive)
            throw new ArgumentException("El médico no existe o está inactivo.");

        var dayOfWeek = (short)start.DayOfWeek;
        var startTime = TimeOnly.FromDateTime(start);
        var endTime = TimeOnly.FromDateTime(end);

        var allSchedules = await schedules.GetAllAsync();

        var validSchedule = allSchedules.Any(s =>
            s.DoctorId == doctorId &&
            s.IsActive &&
            s.DayOfWeek == dayOfWeek &&
            startTime >= s.StartTime &&
            endTime <= s.EndTime &&
            s.SlotDurationMinutes > 0 &&
            (end - start).TotalMinutes == s.SlotDurationMinutes &&
            (startTime - s.StartTime).TotalMinutes %
                s.SlotDurationMinutes == 0);

        if (!validSchedule)
            throw new ArgumentException("La cita está fuera del horario del médico " + "o no coincide con la duración de sus turnos.");

        var allAppointments = await appointments.GetAllAsync();

        var overlaps = allAppointments.Any(a =>
            a.DoctorId == doctorId &&
            a.AppointmentId != excludeAppointmentId &&
            a.Status != "Cancelled" &&
            start < a.EndDateTime &&
            end > a.StartDateTime);

        if (overlaps)
            throw new ArgumentException("El médico ya tiene una cita en ese horario.");
    }
}
