namespace LibraryManagement.API.Domain.Exceptions;

public class DomainException(string message) : Exception(message);

public class NotFoundException(string entity, int id): Exception($"{entity}	with	id	{id}	was	not	found.");
