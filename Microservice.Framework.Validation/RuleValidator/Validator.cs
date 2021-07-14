using Microservice.Framework.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Framework.Validation
{
    public class Validator<T>
    {
        private Type TypeOfEntity = typeof(T);

        #region Constructors

        public Validator(T entity)
        {
            Entity = entity;

            Construct();
        }

        #endregion

        #region Private Members

        private static object syncer = new object();

        private static ConcurrentDictionary<Type, IList<Type>> entityRules = new ConcurrentDictionary<Type, IList<Type>>();

        #endregion

        #region Properties

        public T Entity { get; set; }

        #endregion

        #region Public Methods

        public ConcurrentDictionary<Type, Notification> ValidateEntity()
        {
            return ValidateRules(Entity);
        }

        #endregion

        #region Private Methods

        private void FetchRules()
        {
            if(!entityRules.ContainsKey(TypeOfEntity))
            {
                lock(syncer)
                {
                    if (!entityRules.ContainsKey(TypeOfEntity))
                    {
                        entityRules.SafeAddKey(TypeOfEntity, new List<Type>());

                        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                        foreach (var assembly in allAssemblies.Where(c => !c.FullName.Contains("NHibernate")).AsEnumerable())
                        {
                            var typeTRules = assembly
                                            .GetTypes()
                                            .AsEnumerable()
                                            .Where(t => typeof(IDomainRule<T>).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

                            foreach (var rule in typeTRules)
                            {
                                entityRules[TypeOfEntity].Add(rule);
                            }
                        }
                    }
                }
            }
        }

        private ConcurrentDictionary<Type, Notification> ValidateRules(T entity)
        {
            var noticationDictionary = new ConcurrentDictionary<Type, Notification>();

            Parallel.ForEach(entityRules[TypeOfEntity], (rule) => 
            {
                var notification = NotificationHelper.CreateNotification(entity.GetType(), rule);

                noticationDictionary.TryAdd(rule, notification);

                var actualRules = CreatePropertyRules(rule, entity);

                Parallel.ForEach(actualRules, (actualRule) => 
                {
                    if (actualRule.MustValidate())
                    {
                        var notificationMessage = actualRule.Validate();
                        if (notificationMessage.IsNotNull())
                        {
                            notification.Append(notificationMessage);
                        }
                    }
                });
            });

            return noticationDictionary;
        }

        public IList<IDomainRule<T>> CreatePropertyRules(Type ruleType, T entity)
        {
            var propertyRules = new List<IDomainRule<T>>();

            var customAttributes = ruleType.CustomAttributes;

            foreach(var customAttribute in customAttributes)
            {
                var propertyNameToValidate = customAttribute.ConstructorArguments.FirstOrDefault();

                if (propertyNameToValidate != null)
                {
                    var rule = CreateRule(ruleType, entity);
                    rule.Property = entity.GetType().GetProperty(propertyNameToValidate.Value.ToString());
                    propertyRules.Add(rule);
                }
            }

            return propertyRules;
        }

        public IDomainRule<T> CreateRule(Type ruleType, T entity)
        {
            return (IDomainRule<T>)Activator.CreateInstance(ruleType, new object[] { entity });
        }

        private void Construct()
        {
            FetchRules();
        }

        #endregion
    }
}
