using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Kengic.Was.CrossCutting.Common.Validators;

namespace Kengic.Was.CrossCutting.EntityValidator
{
    /// <summary>
    ///     <see cref="Validator" /> based on Data Annotations. This validator use
    ///     <see cref="IValidatableObject" /> <see langword="interface" /> and
    ///     <see cref="ValidationAttribute" /> ( hierachy of this) for perform
    ///     validation
    /// </summary>
    public class DataAnnotationsEntityValidator
        : IEntityValidator
    {
        /// <summary>
        ///     <see cref="IEntityValidator" />
        /// </summary>
        /// <typeparam name="TEntity">
        ///     <see cref="IEntityValidator" />
        /// </typeparam>
        /// <param name="item">
        ///     <see cref="IEntityValidator" />
        /// </param>
        /// <returns>
        ///     <see cref="IEntityValidator" />
        /// </returns>
        public bool IsValid<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                return false;
            }

            var validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);

            return !validationErrors.Any();
        }

        /// <summary>
        ///     <see cref="IEntityValidator" />
        /// </summary>
        /// <typeparam name="TEntity">
        ///     <see cref="IEntityValidator" />
        /// </typeparam>
        /// <param name="item">
        ///     <see cref="IEntityValidator" />
        /// </param>
        /// <returns>
        ///     <see cref="IEntityValidator" />
        /// </returns>
        public IEnumerable<string> GetInvalidMessages<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                return null;
            }

            var validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);


            return validationErrors;
        }

        /// <summary>
        ///     Get erros if object implement <see cref="IValidatableObject" />
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The <see langword="typeof" /> entity
        /// </typeparam>
        /// <param name="item">The item to validate</param>
        /// <param name="errors">A collection of current errors</param>
        private static void SetValidatableObjectErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            if (!typeof (IValidatableObject).IsAssignableFrom(typeof (TEntity))) return;
            var validationContext = new ValidationContext(item, null, null);

            var validationResults = ((IValidatableObject) item).Validate(validationContext);

            errors.AddRange(validationResults.Select(vr => vr.ErrorMessage));
        }

        /// <summary>
        ///     Get <paramref name="errors" /> on <see cref="ValidationAttribute" />
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <param name="item">The entity to validate</param>
        /// <param name="errors">A collection of current errors</param>
        private static void SetValidationAttributeErrors<TEntity>(TEntity item, List<string> errors)
            where TEntity : class
        {
            var result = from property in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                from attribute in property.Attributes.OfType<ValidationAttribute>()
                where !attribute.IsValid(property.GetValue(item))
                select attribute.FormatErrorMessage(string.Empty);

            var enumerable = result as IList<string> ?? result.ToList();
            if (enumerable.Any())
            {
                errors.AddRange(enumerable);
            }
        }
    }
}