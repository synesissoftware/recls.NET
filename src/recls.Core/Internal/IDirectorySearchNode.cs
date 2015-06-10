
namespace Recls.Internal
{
	using System;

	internal interface IDirectorySearchNode
    {
        /// <summary>
        ///  Returns the next directory search node under this node.
        /// </summary>
        /// <returns>
        ///  A reference to the next directory search node, or null if
        ///  there are no more search nodes under this node.
        /// </returns>
        IDirectorySearchNode GetNextNode();

        /// <summary>
        ///  Returns the next directory entry under this node.
        /// </summary>
        /// <returns>
        ///  A reference to the next directory entry, or null if
        ///  there are no more entries under this node.
        /// </returns>
        IEntry GetNextEntry();
    }
}
