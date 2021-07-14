using Microservice.Framework.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Framework.Validation
{
    public abstract class IntRangeRule<T> : DomainRule<T>, IRangeRule<T> where T : class
    {
        #region Constructors

        protected IntRangeRule(T owner)
            : base(owner)
        {
            Construct();
        }

        #endregion

        #region Virtual Methods

        protected override string ValidationMessage => "{0} does not fall within the range of " + $"{OnGetMinimum()} and {OnGetMaximum()}";

        protected override bool ValidationCondition()
        {
            var propertyValue = PropertyValue as int?;

            if (propertyValue.IsNotNull())
            {
                var minimum = GetMinimum() as int?;
                var maximum = GetMaximum() as int?;

                if (propertyValue < minimum || propertyValue > maximum)
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual int OnGetMinimum()
        {
            return int.MinValue;
        }

        protected virtual int OnGetMaximum()
        {
            return int.MaxValue;
        }

        #endregion

        #region IRangeRule Members

        public object GetMaximum()
        {
            return OnGetMaximum();
        }

        public object GetMinimum()
        {
            return OnGetMinimum();
        }

        #endregion

        #region Private Methods

        private void Construct()
        {
        }

        #endregion
    }
}
