using MediFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Context;

public class MediFlowContext : DbContext
{
    public MediFlowContext(DbContextOptions<MediFlowContext> options)
        : base(options)
    {
    }

    // ENTIDADES
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<PrescriptionDetail> PrescriptionDetails => Set<PrescriptionDetail>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<DoctorRoom> DoctorRooms => Set<DoctorRoom>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================================
        // RELACIONES EXISTENTES: USUARIOS Y ROLES
        // ==========================================

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // ==========================================
        // HISTORIA CLÍNICA
        // ==========================================

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Patient)
            .WithMany()
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // ==========================================
        // DIAGNÓSTICOS
        // ==========================================

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.MedicalRecord)
            .WithMany(m => m.Diagnoses)
            .HasForeignKey(d => d.MedicalRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.Appointment)
            .WithMany()
            .HasForeignKey(d => d.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.Doctor)
            .WithMany()
            .HasForeignKey(d => d.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // ==========================================
        // RECETAS
        // ==========================================

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Diagnosis)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.DiagnosisId)
            .OnDelete(DeleteBehavior.Restrict);

        // ==========================================
        // DETALLES DE RECETA
        // ==========================================

        modelBuilder.Entity<PrescriptionDetail>()
            .HasOne(pd => pd.Prescription)
            .WithMany(p => p.Details)
            .HasForeignKey(pd => pd.PrescriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PrescriptionDetail>()
            .HasOne(pd => pd.Medication)
            .WithMany(m => m.PrescriptionDetails)
            .HasForeignKey(pd => pd.MedicationId)
            .OnDelete(DeleteBehavior.Restrict);

        // ==========================================
        // PAGOS
        // ==========================================

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Appointment)
            .WithMany()
            .HasForeignKey(p => p.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(12, 2);

        // ==========================================
        // COMPROBANTES (1 A 1)
        // ==========================================

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Payment)
            .WithOne(p => p.Invoice)
            .HasForeignKey<Invoice>(i => i.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalAmount)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        // ==========================================
        // MÉDICOS Y CONSULTORIOS
        // ==========================================

        modelBuilder.Entity<DoctorRoom>()
            .HasOne(dr => dr.Doctor)
            .WithMany()
            .HasForeignKey(dr => dr.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DoctorRoom>()
            .HasOne(dr => dr.Room)
            .WithMany(r => r.DoctorRooms)
            .HasForeignKey(dr => dr.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // ==========================================
        // NOTIFICACIONES
        // ==========================================

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Patient)
            .WithMany()
            .HasForeignKey(n => n.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Appointment)
            .WithMany()
            .HasForeignKey(n => n.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
