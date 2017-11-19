
/* /////////////////////////////////////////////////////////////////////////
 * File:        Internal/DirectoryEntry.cs
 *
 * Created:     5th June 2009
 * Updated:     24th September 2017
 *
 * Home:        http://recls.net/
 *
 * Copyright (c) 2009-2017, Matthew Wilson and Synesis Software
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
	using System;
	using System.Diagnostics;
	using System.IO;

	internal sealed class DirectoryEntry
		: Entry
	{
		#region construction
		internal DirectoryEntry(DirectoryInfo info, string searchRoot, SearchOptions options, object context)
			: base(info, searchRoot, options, context)
		{
			if(0 != (options & SearchOptions.MarkDirectories))
			{
				m_path = Util.EnsureDirEnd(RawPath);
			}
			else
			{
				m_path = RawPath;
			}
		}
		#endregion

		#region IEntry members
		public override string Path
		{
			get
			{
				return m_path;
			}
		}

		public override long Size
		{
			get { return 0; }
		}

		public override bool IsReadOnly
		{
			get { return false; }
		}

		public override bool IsDirectory
		{
			get { return true; }
		}
		#endregion

		#region fields

		private readonly string m_path;
		#endregion
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

