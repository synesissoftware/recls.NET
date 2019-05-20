
/* /////////////////////////////////////////////////////////////////////////
 * File:        Exceptions/IllformedUncPathException.cs
 *
 * Created:     4th August 2009
 * Updated:     6th May 2019
 *
 * Home:        http://recls.net/
 *
 * Copyright (c) 2009-2019, Matthew Wilson and Synesis Software
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * - Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 * - Neither the name(s) of Matthew Wilson and Synesis Software nor the
 *   names of any contributors may be used to endorse or promote products
 *   derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
 * IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * ////////////////////////////////////////////////////////////////////// */


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
		#region fields
		private readonly string m_path;
		#endregion

		#region construction
		/// <summary>
		///  Constructs an instance of the exception.
		/// </summary>
		public IllformedUncPathException()
		{
		}

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
		{
		}
		#endregion

		#region properties
		/// <summary>
		///  The illformed path.
		/// </summary>
		public string Path
		{
			get { return m_path; }
		}
		#endregion

		#region ISerializable members
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

/* ///////////////////////////// end of file //////////////////////////// */

