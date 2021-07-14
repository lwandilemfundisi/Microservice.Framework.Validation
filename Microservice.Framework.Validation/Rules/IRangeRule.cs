namespace Microservice.Framework.Validation
{
    public interface IRangeRule<T> : IDomainRule<T>
    {
        object GetMinimum();

        object GetMaximum();
    }
}