using System.Reflection;

namespace Microservice.Framework.Validation
{
    public interface IDomainRule<T>
    {
        #region IDomainRule Members

        T Owner { get; }

        PropertyInfo Property { get; set; }

        NotificationMessage Validate();

        bool MustValidate();

        #endregion
    }
}
