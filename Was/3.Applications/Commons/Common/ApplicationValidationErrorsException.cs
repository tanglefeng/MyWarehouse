using System;
using System.Collections.Generic;

namespace Kengic.Was.Application.WasModel.Common
{
    /// <summary>
    ///     The custom exception for validation errors
    /// </summary>
    [Serializable]
    public class ApplicationValidationErrorsException
        : Exception
    {
        /// <summary>
        ///     Create new instance of Application validation errors exception
        /// </summary>
        /// <param name="validationErrors">
        ///     The collection of validation errors
        /// </param>
        public ApplicationValidationErrorsException(IEnumerable<string> validationErrors)
            : base("exception_ApplicationValidationExceptionDefaultMessage")
        {
            ValidationErrors = validationErrors;
        }

        /// <summary>
        ///     Get or set the validation errors messages
        /// </summary>
        public IEnumerable<string> ValidationErrors { get; private set; }
    }
}