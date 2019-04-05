
namespace Test.Unit.recls.Core.Api
{
	using global::Recls;

	using global::Microsoft.VisualStudio.TestTools.UnitTesting;

	using global::System;

	[TestClass]
	public class constants_tester
	{
		[TestMethod]
		public void Test_Recls_Api_WildcardsAll()
		{
			string expected;

			switch(Api.DeducedOperatingSystem.Platform)
			{
			case PlatformID.Unix:
			case PlatformID.MacOSX:
			default:
				expected = "*";
				break;
			case PlatformID.Win32NT:
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.WinCE:
			case PlatformID.Xbox:
				expected = "*.*";
				break;
			}

			Assert.AreEqual(expected, Recls.Api.WildcardsAll);
		}

		[TestMethod]
		public void Test_Recls_Api_PathNameSeparatorCharacters()
		{
			char[] expected;

			switch (Api.DeducedOperatingSystem.Platform)
			{
				case PlatformID.Unix:
				case PlatformID.MacOSX:
				default:
					expected = new char[] { '/' };
					break;
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
				case PlatformID.Xbox:
					expected = new char[] { '\\', '/' };
					break;
			}

			CollectionAssert.AllItemsAreNotNull(Recls.Api.PathNameSeparatorCharacters);
			CollectionAssert.AllItemsAreInstancesOfType(Recls.Api.PathNameSeparatorCharacters, typeof(char));
			CollectionAssert.AllItemsAreUnique(Recls.Api.PathNameSeparatorCharacters);
			CollectionAssert.IsSubsetOf(expected, Recls.Api.PathNameSeparatorCharacters);
			CollectionAssert.IsSubsetOf(Recls.Api.PathNameSeparatorCharacters, expected);
		}

		[TestMethod]
		public void Test_Recls_Api_PathSeparatorCharacters()
		{
			char[] expected;

			switch (Api.DeducedOperatingSystem.Platform)
			{
				case PlatformID.Unix:
				case PlatformID.MacOSX:
				default:
					expected = new char[] { ':', '|' };
					break;
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
				case PlatformID.Xbox:
					expected = new char[] { ';', '|' };
					break;
			}

			CollectionAssert.AllItemsAreNotNull(Recls.Api.PathSeparatorCharacters);
			CollectionAssert.AllItemsAreInstancesOfType(Recls.Api.PathSeparatorCharacters, typeof(char));
			CollectionAssert.AllItemsAreUnique(Recls.Api.PathSeparatorCharacters);
			CollectionAssert.IsSubsetOf(expected, Recls.Api.PathSeparatorCharacters);
			CollectionAssert.IsSubsetOf(Recls.Api.PathSeparatorCharacters, expected);
		}
	}
}
