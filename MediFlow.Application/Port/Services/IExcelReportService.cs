namespace MediFlow.Application.Port.Services;

public interface IExcelReportService
{
    Task<byte[]> GeneratePatientsReportAsync();
}