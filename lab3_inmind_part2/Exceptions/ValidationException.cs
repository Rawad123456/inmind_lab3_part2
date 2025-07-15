namespace lab3_inmind_part2.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}