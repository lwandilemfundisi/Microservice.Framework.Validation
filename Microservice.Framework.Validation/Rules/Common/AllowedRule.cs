using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microservice.Framework.Common;

namespace Microservice.Framework.Validation
{
    public abstract class AllowedRule<T> : DomainRule<T>, IAllowedRule<T> where T : class
    {
        #region Constructors

        public AllowedRule(T owner)
            : base(owner)
        {
            Construct();
        }

        #endregion

        #region Virtual Methods

        protected override string ValidationMessage => "'{0}' is not allowed";

        protected override bool ValidationCondition()
        {
            if (PropertyHasValue())
            {
                if (OnContainsPropertyValue(GetAllowedValues(), PropertyValue))
                {
                    return true;
                }
            }

            return false;
        }

        protected abstract IEnumerable OnGetAllowedValues();

        protected virtual bool OnContainsPropertyValue(IEnumerable allowedValues, object propertyValue)
        {
            return allowedValues.OfType<object>().Contains(v => v.Equals(propertyValue));
        }

        #endregion

        #region IAllowedRule

        public IEnumerable GetAllowedValues()
        {
            return OnGetAllowedValues();
        }

        #endregion

        #region Private Methods

        private void Construct()
        {
        }

        #endregion
    }
}
