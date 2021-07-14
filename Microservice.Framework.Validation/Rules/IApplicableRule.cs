using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservice.Framework.Validation
{
    public interface IApplicableRule<T> : IDomainRule<T>
    {
        bool IsApplicable();
    }
}
