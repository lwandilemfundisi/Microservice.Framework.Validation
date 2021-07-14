using Microservice.Framework.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Framework.Validation
{
    public abstract class BytesRangeRule<T> : DomainRule<T>, IRangeRule<T>
    {
        #region Constructors

        protected BytesRangeRule(T owner)
            : base(owner)
        {
            Construct();
        }

        #endregion

        #region Virtual Methods

        protected override string ValidationMessage => "{0} does not fall between the range of " + $"{OnGetMinimum()} and {OnGetMaximum()} bytes";

        protected override bool ValidationCondition()
        {
            var propertyValue = PropertyValue as byte[];

            if (propertyValue.IsNotNull())
            {
                var minimum = OnGetMinimum();
                var maximum = OnGetMaximum();

                if (propertyValue.Length < minimum || propertyValue.Length > maximum)
                {
                    return false;
                }
            }

            return true;
        }

        protected abstract int OnGetMaximum();

        protected virtual int OnGetMinimum()
        {
            return 1;
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
