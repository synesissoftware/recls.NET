
namespace Recls.Exceptions
{
	using System;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Security.Permissions;

    /// <summary>
    ///  Root exception for <see cref="Recls"/> namespace.
    /// </summary>
    [Serializable]
    public class ReclsException
        : IOException
    {
        #region Construction
        /// <summary>
        ///  Constructs an instance.
        /// </summary>
        public ReclsException()
        { }

        /// <summary>
        ///  Constructs an instance from the given
        ///  <paramref name="message"/>.
        /// </summary>
        /// <param name="message">
        ///  The error message that explains the reason for the exception.
        /// </param>
        public ReclsException(string message)
            : base(message)
        { }

        /// <summary>
        ///  Constructs an instance of the exception from the given
        ///  <paramref name="message"/>
        ///  and inner exception.
        /// </summary>
        /// <param name="message">
        ///  The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        ///  The exception that is the cause of the current exception.
        /// </param>
        public ReclsException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        ///  Initializes a new instance of the exception with serialized
        ///  data.
        /// </summary>
        /// <param name="info">
        ///  The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///  The contextual information about the source or destination.
        /// </param>
        protected ReclsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
        #endregion
    }
}
