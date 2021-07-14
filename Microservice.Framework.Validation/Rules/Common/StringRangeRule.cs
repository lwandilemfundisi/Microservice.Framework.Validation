using Microservice.Framework.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Framework.Validation.Rules.Common
{
    public abstract class StringRangeRule<T> : DomainRule<T>, IRangeRule<T> where T : class
    {
        #region Constructors

        protected StringRangeRule(T owner)
            : base(owner)
        {
            Construct();
        }

        #endregion

        #region Virtual Methods

        protected override string ValidationMessage => "{0} does not fall between the range of " + $"{OnGetMinimum()} and {OnGetMaximum()}";

        protected override bool ValidationCondition()
        {
            var propertyValue = PropertyValue.AsString();

            if (propertyValue.IsNotNullOrEmpty())
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

        protected virtual int OnGetMaximum()
        {
            return 255;
        }

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
