namespace Microservice.Framework.Validation
{
    public interface IRequiredRule<T> : IDomainRule<T>
    {
        bool IsRequired();
    }
}