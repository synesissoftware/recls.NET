
/* /////////////////////////////////////////////////////////////////////////
 * File:        Internal/ExceptionUtil.cs
 *
 * Created:     5th June 2009
 * Updated:     20th May 2019
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

namespace Recls.Internal
{
	using global::System;

	/// <summary>
	///  Flags that control behaviour of
	///  <see cref="Recls.Internal.ExceptionUtil.HResultFromException(System.Exception, HResultFromExceptionOptions, int)"/>.
	/// </summary>
	[Flags]
	public enum HResultFromExceptionOptions : long
	{
		/// <summary>
		///  Nothing.
		/// </summary>
		None                                        =   0,

		/// <summary>
		///  Because
		///  <see cref="System.Runtime.InteropServices.Marshal.GetHRForException"/>
		///  has serious side effects, it is not used by
		///  <see cref="Recls.Internal.ExceptionUtil.HResultFromException(System.Exception, HResultFromExceptionOptions, int)"/>
		///  and
		///  <see cref="Recls.Internal.ExceptionUtil.TryGetHResultFromException(System.Exception, HResultFromExceptionOptions, out System.Int32, out System.Exception)"/>
		///  by default, unless this flag is specified, in which case it is
		///  the last mechanism attempted.
		/// </summary>
		UseMarshalGetHRForException                 =   0x0000000000000001,


		/// <summary>
		///  <see cref="Recls.Internal.ExceptionUtil.HResultFromException(System.Exception, HResultFromExceptionOptions, int)"/>
		///  and
		///  <see cref="Recls.Internal.ExceptionUtil.TryGetHResultFromException(System.Exception, HResultFromExceptionOptions, out System.Int32, out System.Exception)"/>
		///  will propagate an exception (other than
		///  <see cref="System.OutOfMemoryException"/>) emitted when trying
		///  to elicit the <c>HResult</c> property via reflection.
		/// </summary>
		PropagateReflectionAccessException          =   0x0000000000000010,

		/// <summary>
		///  <see cref="Recls.Internal.ExceptionUtil.HResultFromException(System.Exception, HResultFromExceptionOptions, int)"/>
		///  and
		///  <see cref="Recls.Internal.ExceptionUtil.HResultFromException(System.Exception, HResultFromExceptionOptions, int)"/>
		///  will propagate an exception (other than
		///  <see cref="System.OutOfMemoryException"/>) emitted when trying
		///  to elicit the <c>HResult</c> property via serialisation.
		/// </summary>
		PropagateSerialisationAccessException       =   0x0000000000000020,


		/// <summary>
		///  <see cref="Recls.Internal.ExceptionUtil.TryGetHResultFromException(System.Exception, HResultFromExceptionOptions, out System.Int32, out System.Exception)"/>
		///  will return (<b>false</b>) an exception (other than
		///  <see cref="System.OutOfMemoryException"/>) emitted when trying
		///  to elicit the <c>HResult</c> property via reflection.
		/// </summary>
		ReturnOnReflectionAccessException          =   0x0000000000100000,

		/// <summary>
		///  <see cref="Recls.Internal.ExceptionUtil.TryGetHResultFromException(System.Exception, HResultFromExceptionOptions, out System.Int32, out System.Exception)"/>
		///  will return (<b>false</b>) an exception (other than
		///  <see cref="System.OutOfMemoryException"/>) emitted when trying
		///  to elicit the <c>HResult</c> property via serialisation.
		/// </summary>
		ReturnOnSerialisationAccessException       =   0x0000000000200000,


		/// <summary>
		///  The default flags
		/// </summary>
		Default                                     =   None
	};
}

namespace Recls.Internal
{
	using global::System;
	using global::System.Diagnostics;
	using global::System.Runtime.InteropServices;
	using global::System.Reflection;
	using global::System.Runtime.Serialization;

	/// <summary>
	///  Exception utility functions
	/// </summary>
	public static class ExceptionUtil
	{
		#region fields

		static readonly bool            sm_is_4_5_or_later;
		static readonly PropertyInfo    sm_Exception_HResult;
		#endregion

		#region construction

		static ExceptionUtil()
		{
			Version fwkVersion = System.Environment.Version;

			sm_is_4_5_or_later = fwkVersion.Major > 4 || (fwkVersion.Major == 4 && fwkVersion.Minor >= 5);

			Type type = typeof(Exception);

			if(!sm_is_4_5_or_later)
			{
				sm_Exception_HResult = type.GetProperty("HResult", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			else
			{
				sm_Exception_HResult = type.GetProperty("HResult", BindingFlags.Instance);
			}

			Debug.Assert(null != sm_Exception_HResult);
			Debug.Assert(null != sm_Exception_HResult.GetGetMethod(!sm_is_4_5_or_later));
		}
		#endregion

		#region operations

		/// <summary>
		///  Attempts to obtain the value of the
		///  <see cref="System.Exception.HResult"/>
		///  property from the exception instance, according to given
		///  <paramref name="options"/>.
		/// </summary>
		/// <param name="x">
		///  The exception instance. May not be <b>null</b>.
		/// </param>
		/// <param name="options">
		///  Combination of
		///  <see cref="HResultFromExceptionOptions"/>
		///  that control the lookup mechanism(s) employed. <b>NOTE</b> this
		///  is ignored if the framework version is 4.5 or later, as the
		///  elicitation cannot fail.
		/// </param>
		/// <param name="result">
		///  The HResult code associated with the exception; <b>0</b> if it
		///  could not be obtained.
		/// </param>
		/// <param name="failureException">
		///  An exception thrown by the underlying reflection/serialisation
		///  mechanisms that indicated the result could not be be obtained;
		///  <b>null</b> otherwise.
		/// </param>
		/// <returns>
		///  <b>true</b> if the result was obtained; <b>false</b> otherwise.
		/// </returns>
		public static bool TryGetHResultFromException(Exception x, HResultFromExceptionOptions options, out int result, out Exception failureException)
		{
#if DOTNET_4_5_OR_LATER

			result              =   x.HResult;
			failureException    =   null;

			return return;
#else

			if(sm_is_4_5_or_later)
			{
				result              =   (int)sm_Exception_HResult.GetValue(x, null);
				failureException    =   null;

				return true;
			}
			else
			{
				// 1. Reflection

				try
				{
					result              =   (int)sm_Exception_HResult.GetValue(x, null);
					failureException    =   null;

					return true;
				}
				catch(OutOfMemoryException)
				{
					throw;
				}
				catch(Exception x2)
				{
					if(0 != (HResultFromExceptionOptions.PropagateReflectionAccessException & options))
					{
						throw;
					}

					if(0 != (HResultFromExceptionOptions.ReturnOnReflectionAccessException & options))
					{
						result              =   default(int);
						failureException    =   x2;

						return false;
					}
				}

				// 2. Serialisation

				try
				{
					// fall back to serialisation

					var info = new SerializationInfo(x.GetType(), new FormatterConverter());

					x.GetObjectData(info, new StreamingContext());

					int hr = info.GetInt32("HResult");

					result              =   hr;
					failureException    =   null;

					return true;
				}
				catch(OutOfMemoryException)
				{
					throw;
				}
				catch(Exception x2)
				{
					if(0 != (HResultFromExceptionOptions.PropagateSerialisationAccessException & options))
					{
						throw;
					}

					if(0 != (HResultFromExceptionOptions.ReturnOnReflectionAccessException & options))
					{
						result              =   default(int);
						failureException    =   x2;

						return false;
					}
				}

				// 3. Marshal

				if(0 != (HResultFromExceptionOptions.UseMarshalGetHRForException & options))
				{
					result              =   Marshal.GetHRForException(x);
					failureException    =   null;

					return true;
				}

				result              =   default(int);
				failureException    =   null;

				return false;
			}
#endif
		}


		/// <summary>
		///  Attempts to obtain the value of the
		///  <see cref="System.Exception.HResult"/>
		///  property from the exception instance, according to given
		///  <paramref name="options"/>
		///  and
		///  <paramref name="defaultValue"/>.
		/// </summary>
		/// <param name="x">
		///  The exception instance. May not be <b>null</b>.
		/// </param>
		/// <param name="options">
		///  Combination of
		///  <see cref="HResultFromExceptionOptions"/>
		///  that control the lookup mechanism(s) employed. <b>NOTE</b> this
		///  is ignored if the framework version is 4.5 or later, as the
		///  elicitation cannot fail.
		/// </param>
		/// <param name="defaultValue">
		///  Default value to be used (as a sentinel) in the case that the
		///  property cannot be elicited
		/// </param>
		/// <returns>
		///  The value of the
		///  <see cref="System.Exception.HResult"/>
		///  property held by <paramref name="x"/>, or the
		///  <paramref name="defaultValue"/> if the value cannot be elicited
		///  (which can only occur in frameworks earlier than 4.5).
		/// </returns>
		public static int HResultFromException(Exception x, HResultFromExceptionOptions options, int defaultValue)
		{
#if DOTNET_4_5_OR_LATER

			return x.HResult;
#else

			if(sm_is_4_5_or_later)
			{
				return (int)sm_Exception_HResult.GetValue(x, null);
			}
			else
			{
				// 1. Reflection

				try
				{
					return (int)sm_Exception_HResult.GetValue(x, null);
				}
				catch(OutOfMemoryException)
				{
					throw;
				}
				catch(MethodAccessException)
				{
					if(0 != (HResultFromExceptionOptions.PropagateReflectionAccessException & options))
					{
						throw;
					}
				}

				// 2. Serialisation

				try
				{
					// fall back to serialisation

					var info = new SerializationInfo(x.GetType(), new FormatterConverter());

					x.GetObjectData(info, new StreamingContext());

					int hr = info.GetInt32("HResult");

					return hr;
				}
				catch(OutOfMemoryException)
				{
					throw;
				}
				catch(Exception)
				{
					if(0 != (HResultFromExceptionOptions.PropagateSerialisationAccessException & options))
					{
						throw;
					}
				}

				// 3. Marshal

				if(0 != (HResultFromExceptionOptions.UseMarshalGetHRForException & options))
				{
					return Marshal.GetHRForException(x);
				}


				// 4. Default

				return defaultValue;
			}
#endif
		}

		/// <summary>
		///  Obtain the value of the
		///  <see cref="System.Exception.HResult"/>
		///  property from the exception instance
		/// </summary>
		/// <param name="x">
		///  The exception instance. May not be <b>null</b>.
		/// </param>
		/// <returns>
		///  The value of the
		///  <see cref="System.Exception.HResult"/>
		///  property held by <paramref name="x"/>, or the
		///  <c>0</c> if the value cannot be elicited
		///  (which can only occur in frameworks earlier than 4.5).
		/// </returns>
		public static int HResultFromException(Exception x)
		{
			return HResultFromException(x, HResultFromExceptionOptions.Default, 0);
		}
		#endregion
	}
}
