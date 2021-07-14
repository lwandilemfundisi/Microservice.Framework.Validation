using Microservice.Framework.Common;

namespace Microservice.Framework.Validation
{
    public abstract class RequiredRule<T> : DomainRule<T>, IRequiredRule<T> where T : class
    {
        #region Constructors

        protected RequiredRule(T owner)
            : base(owner)
        {
            Construct();
        }

        #endregion

        #region IRequiredRule

        public bool IsRequired()
        {
            return OnIsRequired();
        }

        #endregion

        #region Virtual Methods

        protected override string ValidationMessage => "'{0}' is a required property";

        protected virtual bool OnIsRequired()
        {
            return true;
        }

        protected override bool ValidationCondition()
        {
            if(IsRequired())
            {
                var value = Property.GetValue(Owner);

                if (value.IsNull())
                {
                    return false;
                }

                return RequiredPropertyHasValue();
            }

            return true;
        }

        protected virtual bool RequiredPropertyHasValue()
        {
            if (PropertyValueType.Equals(typeof(string)))
            {
                if (PropertyValue.AsString().IsNullOrEmpty())
                {
                    return false;
                }
            }
            else if (PropertyValueType.Equals(typeof(byte[])) && ((byte[])PropertyValue).Length == 0)
            {
                return false;
            }
            else if (PropertyValueType.IsSubclassOf(typeof(XmlValueObject)) && ((XmlValueObject)PropertyValue).Code.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Private Methods

        private void Construct()
        {
        }

        #endregion
    }
}
