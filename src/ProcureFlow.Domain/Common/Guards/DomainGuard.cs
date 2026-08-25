using ProcureFlow.Domain.Common.Exceptions;

namespace ProcureFlow.Domain.Common.Guards
{
    public static class DomainGuard
    {
        public static string Required(string? value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException(message);
            }
            return value.Trim();
        }

    }
}
