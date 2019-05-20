
namespace Recls.Extensions
{
	using IEntry1 = global::Recls.IEntry;
	using IEntry2_1 = global::Recls.IEntry2_1;
	using IEntry2_2 = global::Recls.IEntry2_2;

	using global::System.Collections.Generic;

	/// <summary>
	///  Extension methods to the <see cref="IEntry"/> interface
	/// </summary>
	public static class X_CDE3308B_6EE5_4b61_A9DD_00C58092D9A4
	{
		/// <summary>
		///  Indicates whether the entry is a file.
		/// </summary>
		/// <param name="entry">
		///  The entry
		/// </param>
		/// <returns>
		///  <b>true</b> if the entry is a file; <b>false</b>
		///  otherwise.
		/// </returns>
		public static bool IsFile(this IEntry1 entry)
		{
			IEntry2_1 ie2_1 = entry as IEntry2_1;

			if(null != ie2_1)
			{
				return ie2_1.IsFile;
			}

			return !entry.IsDirectory;
		}

		/// <summary>
		///  Indicates whether the represented file-system entry existed at
		///  the time the object was created
		/// </summary>
		/// <param name="entry">
		///  The entry
		/// </param>
		/// <returns>
		///  <b>true</b> if the entry existed at the time the object was
		///  created; <b>false</b> otherwise
		/// </returns>
		public static bool Existed(this IEntry1 entry)
		{
			IEntry2_1 ie2_1 = entry as IEntry2_1;

			if(null != ie2_1)
			{
				return ie2_1.Existed;
			}

			return -1 != (int)entry.Attributes;
		}

		/// <summary>
		///  A mutable dictionary of application-defined elements.
		/// </summary>
		/// <param name="entry">
		///  The entry
		/// </param>
		/// <returns>
		///  An <b>extras</b> dictionary if available; <c>null</c> otherwise
		/// </returns>
		public static IDictionary<string, object> GetExtras(this IEntry1 entry)
		{
			IEntry2_2 ie2_2 = entry as IEntry2_2;

			if(null != ie2_2)
			{
				return ie2_2.Extras;
			}

			return null;
		}
	}
}
