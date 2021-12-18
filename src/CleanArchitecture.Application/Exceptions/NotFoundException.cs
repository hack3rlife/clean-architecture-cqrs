using System;

namespace CleanArchitecture.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"{name} with guid ({key}) was not found")
        {
        }
    }
}
