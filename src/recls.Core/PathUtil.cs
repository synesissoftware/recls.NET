
namespace Recls
{
	using Recls.Internal;

	using System;

	/// <summary>
	///  Provides methods for manipulating paths.
	/// </summary>
	/// <remarks>
	///  Except where documented, paths are treated as independent from any
	///  corresponding file-system entries.
	/// </remarks>
	public static class PathUtil
	{
		/// <summary>
		///  Determines the relative path between two paths.
		/// </summary>
		/// <param name="origin">
		///  The path against which the relativity of
		///  <paramref name="target"/> will be evaluated. If <b>null</b> or
		///  the empty string, then the resultant path is equivalent to
		///  <paramref name="target"/>
		/// </param>
		/// <param name="target">
		///  The path whose relatively (against <paramref name="origin"/>)
		///  will be evaluated. If this is on a different drive than
		///  <paramref name="origin"/>, <paramref name="target"/> is
		///  returned
		/// </param>
		/// <returns>
		///  The equivalent of the <paramref name="target"/> relative to the
		///  <paramref name="origin"/>
		/// </returns>
        /// <exception cref="Recls.Exceptions.IllformedUncPathException">
		///  If either parameter is a malformed UNC path. A correctly
		///  formed UNC path must contain at least server and share, e.g.
		///  <c>\\myserver\myshare</c>. Any path beginning with <c>\\</c>
		///  and containing less than the required server+share is deemed
		///  invalid
		/// </exception>
		public static string DeriveRelativePath(string origin, string target)
		{
			return Util.DeriveRelativePath(origin, target);
		}

		/// <summary>
		///  Canonicalizes a path.
		/// </summary>
		/// <param name="path">
		///  The path to canonicalize.
		/// </param>
		/// <returns>
		///  The canonicalized path. There is no error return: all problems
		///  are indicated by exceptions.
		/// </returns>
        /// <exception cref="Recls.Exceptions.IllformedUncPathException">
		///  If <paramref name="path"/> is a malformed UNC path. A correctly
		///  formed UNC path must contain at least server and share, e.g.
		///  <c>\\myserver\myshare</c>. Any path beginning with <c>\\</c>
		///  and containing less than the required server+share is deemed
		///  invalid.
		/// </exception>
		public static string CanonicalizePath(string path)
		{
			return Util.CanonicalisePath(path);
		}

		/// <summary>
		///  Evaluates the absolute path of the given path.
		/// </summary>
		/// <param name="path">
		///  The path whose absolute equivalent, relative to the current
		///  working directory, is to be evaluated.
		/// </param>
		/// <returns>
		///  The absolute path equivalent of <paramref name="path"/>.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		///  If <paramref name="path"/> is empty, contains invalid
		///  characters, or contains only whitespace, or the system could
		///  not retrieve the absolute path.
		/// </exception>
		/// <exception cref="System.Security.SecurityException">
		///  If the caller does not have the required permissions.
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		///  If <paramref name="path"/> is <b>null</b>.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		///  If <paramref name="path"/> contains a colon (<c>':'</c>) that
		///  is not part of a volume identifier.
		/// </exception>
		/// <exception cref="System.IO.PathTooLongException">
		///  If the specified path, file name, or both exceed the
		///  system-defined maximum length.
		/// </exception>
		/// <remarks>
		///  The CLR's facilities for determing absolute paths is defective
		///  in a few important ways, for which this function provides
		///  corrective behaviour.
		/// </remarks>
		public static string GetAbsolutePath(string path)
		{
			return Util.GetFullPath(path);
		}

		/// <summary>
		///  Evaluates the "<em>directory path</em>" of the given path.
		/// </summary>
		/// <param name="path">
		///  The path whose "<em>directory path</em>" is to be evaluated.
		/// </param>
		/// <returns>
		///  The evaluated "<em>directory path</em>".
		/// </returns>
		/// <remarks>
		///  The returned value will always be a directory, and will always
		///  have a trailing path name separator, ready to be combined with
		///  a relative path.
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		///  If <paramref name="path"/> is <b>null</b>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		///  If <paramref name="path"/> is empty, contains invalid
		///  characters, or contains only whitespace.
		/// </exception>
		/// <exception cref="Recls.Exceptions.IllformedUncPathException">
		///  If <paramref name="path"/> is a malformed UNC path. A correctly
		///  formed UNC path must contain at least server and share, e.g.
		///  <c>\\myserver\myshare</c>. Any path beginning with <c>\\</c>
		///  and containing less than the required server+share is deemed
		///  invalid.
		/// </exception>
		/// <exception cref="System.IO.PathTooLongException">
		///  If the path parameter is longer than the system-defined maximum
		///  length.
		/// </exception>
		public static string GetDirectoryPath(string path)
		{
			return Util.GetDirectoryPath(path);
		}

		/// <summary>
		///  Evaluates the file part of the given path.
		/// </summary>
		/// <param name="path">
		///  The path whose file is to be evaluated.
		/// </param>
		/// <returns>
		///  The evaluated file part of the path.
		/// </returns>
		/// <remarks>
		///  The returned value will never contain a leading path name
		///  separator, and will always be ready to be combined with a
		///  directory.
		/// </remarks>
        /// <exception cref="Recls.Exceptions.IllformedUncPathException">
		///  If <paramref name="path"/> is a malformed UNC path. A correctly
		///  formed UNC path must contain at least server and share, e.g.
		///  <c>\\myserver\myshare</c>. Any path beginning with <c>\\</c>
		///  and containing less than the required server+share is deemed
		///  invalid.
		/// </exception>
		/// <exception cref="System.IO.PathTooLongException">
		///  If the path parameter is longer than the system-defined maximum
		///  length.
		/// </exception>
		public static string GetFile(string path)
		{
			return Util.GetFile(path);
		}

		/// <summary>
		///  Evaluates the drive part of the given path.
		/// </summary>
		/// <param name="path">
		///  The path whose drive is to be evaluated.
		/// </param>
		/// <returns>
		///  The evaluated drive part of the path.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///  If <paramref name="path"/> is <b>null</b>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		///  If <paramref name="path"/> is empty, is not absolute, contains
		///  invalid characters, or contains only whitespace.
		/// </exception>
		/// <exception cref="Recls.Exceptions.IllformedUncPathException">
		///  If <paramref name="path"/> is a malformed UNC path. A correctly
		///  formed UNC path must contain at least server and share, e.g.
		///  <c>\\myserver\myshare</c>. Any path beginning with <c>\\</c>
		///  and containing less than the required server+share is deemed
		///  invalid.
		/// </exception>
		public static string GetDrive(string path)
		{
			return Util.GetDrive(path);
		}
	}
}
