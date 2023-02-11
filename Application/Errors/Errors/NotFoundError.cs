
using FluentResults;

namespace Application.Errors.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string name) : base(name)
        {

        }
    }
}
