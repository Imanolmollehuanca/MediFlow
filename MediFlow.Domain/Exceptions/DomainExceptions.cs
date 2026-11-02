namespace MediFlow.Domain.Exceptions;


public class NotFoundException(string message) : Exception(message);

public class ConflictException(string message) : Exception(message);

public class BusinessValidationException(string message) : Exception(message);