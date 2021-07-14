using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Microservice.Framework.Validation
{
    public interface IAllowedRule<T> : IDomainRule<T>
    {
        IEnumerable GetAllowedValues();
    }
}
