using System;
using System.Collections.Generic;
using MediFlow.Infrastructure.ScaffoldingTemp.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.ScaffoldingTemp;

public partial class MediFlowScaffoldContext : DbContext
{
    public MediFlowScaffoldContext(DbContextOptions<MediFlowScaffoldContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorRoom> DoctorRooms { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasIndex(e => e.DoctorId, "IX_Appointments_DoctorId");

            entity.HasIndex(e => e.PatientId, "IX_Appointments_PatientId");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments).HasForeignKey(d => d.DoctorId);

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasIndex(e => e.AppointmentId, "IX_Diagnoses_AppointmentId");

            entity.HasIndex(e => e.DoctorId, "IX_Diagnoses_DoctorId");

            entity.HasIndex(e => e.MedicalRecordId, "IX_Diagnoses_MedicalRecordId");

            entity.Property(e => e.Icdcode).HasColumnName("ICDCode");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Doctor).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.MedicalRecord).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.MedicalRecordId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasIndex(e => e.SpecialtyId, "IX_Doctors_SpecialtyId");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Doctors).HasForeignKey(d => d.SpecialtyId);
        });

        modelBuilder.Entity<DoctorRoom>(entity =>
        {
            entity.HasIndex(e => e.DoctorId, "IX_DoctorRooms_DoctorId");

            entity.HasIndex(e => e.RoomId, "IX_DoctorRooms_RoomId");

            entity.HasOne(d => d.Doctor).WithMany(p => p.DoctorRooms)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Room).WithMany(p => p.DoctorRooms)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasIndex(e => e.InvoiceNumber, "IX_Invoices_InvoiceNumber").IsUnique();

            entity.HasIndex(e => e.PaymentId, "IX_Invoices_PaymentId").IsUnique();

            entity.Property(e => e.TotalAmount).HasPrecision(12, 2);

            entity.HasOne(d => d.Payment).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasIndex(e => e.PatientId, "IX_MedicalRecords_PatientId");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasIndex(e => e.AppointmentId, "IX_Notifications_AppointmentId");

            entity.HasIndex(e => e.PatientId, "IX_Notifications_PatientId");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Patient).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasIndex(e => e.AppointmentId, "IX_Payments_AppointmentId");

            entity.Property(e => e.Amount).HasPrecision(12, 2);

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasIndex(e => e.DiagnosisId, "IX_Prescriptions_DiagnosisId");

            entity.HasOne(d => d.Diagnosis).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.DiagnosisId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PrescriptionDetail>(entity =>
        {
            entity.HasIndex(e => e.MedicationId, "IX_PrescriptionDetails_MedicationId");

            entity.HasIndex(e => e.PrescriptionId, "IX_PrescriptionDetails_PrescriptionId");

            entity.HasOne(d => d.Medication).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.MedicationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.PrescriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasIndex(e => e.DoctorId, "IX_Schedules_DoctorId");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Schedules).HasForeignKey(d => d.DoctorId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("UserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_UserRoles_RoleId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
