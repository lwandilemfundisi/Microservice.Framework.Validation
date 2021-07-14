using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Framework.Validation
{
    public abstract class PercentageRangeRule<T> : DecimalRangeRule<T> where T : class
    {
        #region Constructors

        protected PercentageRangeRule(T owner)
            : base(owner)
        {

        }

        #endregion
    }
}
