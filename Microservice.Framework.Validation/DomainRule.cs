using Microservice.Framework.Common;
using System;
using System.Collections;
using System.Reflection;

namespace Microservice.Framework.Validation
{
    public abstract class DomainRule<T> : IDomainRule<T>
    {
        #region Constructors

        protected DomainRule(T owner)
        {
            Owner = owner;

            Construct();
        }

        #endregion

        #region Private Members

        private static Type stringType = typeof(string);
        private bool? propertyHasValue;
        private PropertyInfo propertyType;
        private static Type enumerableType = typeof(IEnumerable);

        #endregion

        #region Properties

        public SystemCulture Culture { get; set; }

        #endregion

        #region Protected Members

        protected virtual string ValidationMessage { get; }

        protected Type PropertyValueType { get { return PropertyValue.GetType(); } }

        protected object PropertyValue { get { return Property.GetValue(Owner); } }

        protected PropertyInfo PropertyType
        {
            get
            {
                if (propertyType.IsNull())
                {
                    var instanceType = Owner.GetType();
                    propertyType = instanceType.GetProperty(Property.Name);
                    Invariant.IsFalse(propertyType.IsNull(), () => "No property {0} exists on {1}".FormatInvariantCulture(Property.Name, instanceType.FullName));
                }

                return propertyType;
            }
        }

        #endregion

        #region Virtual Methods

        protected virtual bool ValidationCondition()
        {
            return true;
        }

        protected virtual NotificationMessage OnValidate()
        {
            var message = default(NotificationMessage);

            if(Property != null)
            {
                if (!ValidationCondition())
                {
                    message = NotificationHelper.CreateNotificationMessage(string.Format(ValidationMessage, Property.Name.ToString()), Property, NotificationMessageType.Error);
                }
            }

            return message;
        }

        protected virtual bool OnMustValidate()
        {
            return true;
        }

        #endregion

        #region Private Methods

        private void Construct()
        {

        }

        protected bool PropertyHasValue()
        {
            if (!propertyHasValue.HasValue)
            {
                propertyHasValue = false;

                if (PropertyValue.IsNotNull())
                {
                    if (PropertyType.PropertyType.Equals(stringType))
                    {
                        propertyHasValue = PropertyValue.AsString().IsNotNullOrEmpty();
                    }
                    else if (enumerableType.IsAssignableFrom(PropertyType.PropertyType))
                    {
                        var list = PropertyValue as IList;
                        if (list.IsNotNull())
                        {
                            propertyHasValue = list.Count > 0;
                        }
                    }
                    else
                    {
                        propertyHasValue = true;
                    }
                }
            }

            return propertyHasValue.Value;
        }

        #endregion

        #region IDomainRule Members

        public T Owner { get; }

        public PropertyInfo Property { get; set; }

        public NotificationMessage Validate()
        {
            return OnValidate();
        }

        public bool MustValidate()
        {
            return OnMustValidate();
        }

        #endregion
    }
}
