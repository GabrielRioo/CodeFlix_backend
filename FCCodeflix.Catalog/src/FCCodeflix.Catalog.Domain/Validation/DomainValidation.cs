using FCCodeflix.Catalog.Domain.Exceptions;

namespace FCCodeflix.Catalog.Domain.Validation;
public class DomainValidation
{
	public static void NotNull(object? target, string fieldName)
	{
		if (target is null)
			throw new EntityValidationException($"{fieldName} should not be null");
	}

	public static void NotNullOrEmpty(string? target, string fieldName)
	{
		if (String.IsNullOrWhiteSpace(target))
			throw new EntityValidationException($"{fieldName} should not be null or empty");
	}

	public static void MinLength(string target, int minLength, string fieldName)
	{
		if (target.Length < minLength)
			throw new EntityValidationException($"{fieldName} should not be less than {minLength} characters long");
	}
}
