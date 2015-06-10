
namespace Recls.Exceptions
{
	using System;
	using System.Runtime.Serialization;
	using System.Security.Permissions;

    /// <summary>
    ///  The exception that is thrown when a path containing a mal-formed
    ///  UNC drive is found.
    /// </summary>
    [Serializable]
    public class IllformedUncPathException
        : ReclsException
    {
        #region Member Variables
        private readonly string m_path;
        #endregion

        #region Construction
        /// <summary>
        ///  Constructs an instance of the exception.
        /// </summary>
        public IllformedUncPathException()
        { }

        /// <summary>
        ///  Constructs an instance from the given
        ///  <paramref name="message"/>.
        /// </summary>
        /// <param name="message">
        ///  The error message that explains the reason for the exception.
        /// </param>
        public IllformedUncPathException(string message)
            : base(message)
        {
            m_path = "";
        }

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
        public IllformedUncPathException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_path = "";
        }

        /// <summary>
        ///  Constructs an instance of the exception from the given
        ///  <paramref name="message"/>
        ///  and
        ///  <paramref name="path"/>.
        /// </summary>
        /// <param name="message">
        ///  The error message that explains the reason for the exception.
        /// </param>
        /// <param name="path">
        ///  The illformed path.
        /// </param>
        public IllformedUncPathException(string message, string path)
            : base(message)
        {
            m_path = path;
        }

        /// <summary>
        ///  Constructs an instance of the exception from the given
        ///  <paramref name="message"/>
        ///  and inner exception.
        /// </summary>
        /// <param name="message">
        ///  The error message that explains the reason for the exception.
        /// </param>
        /// <param name="path">
        ///  The illformed path.
        /// </param>
        /// <param name="innerException">
        ///  The exception that is the cause of the current exception.
        /// </param>
        public IllformedUncPathException(string message, string path, Exception innerException)
            : base(message, innerException)
        {
            m_path = path;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Recls.Exceptions.IllformedUncPathException"/>
        ///  class with serialized data.
        /// </summary>
        /// <param name="info">
        ///  The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///  The contextual information about the source or destination.
        /// </param>
        protected IllformedUncPathException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
        #endregion

        #region Properties
        /// <summary>
        ///  The illformed path.
        /// </summary>
        public string Path
        {
            get { return m_path; }
        }
        #endregion

        #region ISerializable Members
        /// <summary>
        ///  Populates the serialisation info with the data needed to
        ///  serialize the target object.
        /// </summary>
        /// <param name="info">
        ///  The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///  The contextual information about the source or destination.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if(info == null)
            {
                throw new ArgumentNullException("info");
            }

            base.GetObjectData(info, context);

            info.AddValue("Path", m_path);
        }
        #endregion
    }
}
