
// Created: 18th November 2017
// Updated: 18th November 2017


namespace Test.Unit.recls.Core
{
    using global::Recls.Internal;

#if NUNIT
	using Assert = global::NUnit.Framework.Assert;
	using TestClass = global::NUnit.Framework.TestFixtureAttribute;
	using TestInitialize = global::NUnit.Framework.SetUpAttribute;
	using TestMethod = global::NUnit.Framework.TestAttribute;
	using ExpectedException = global::NUnit.Framework.ExpectedExceptionAttribute;
#else
	using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    using global::System;

    [TestClass]
    public class ExceptionUtil_tester
    {
        private class HResultException
            : Exception
        {
            public HResultException(int hr)
            {
                base.HResult = hr;
            }
        }

        [TestMethod]
        public void test_type_exists()
        {
            Assert.IsNotNull(typeof(ExceptionUtil));
        }

        [TestMethod]
        public void test_HResultFromException_with_default_options()
        {
            {
                Exception x = new HResultException(unchecked((int)0x80070005));

                Assert.AreEqual(unchecked((int)0x80070005), ExceptionUtil.HResultFromException(x));
            }

            {
                Exception x = new HResultException(1234);

                Assert.AreEqual(1234, ExceptionUtil.HResultFromException(x));
            }

            {
                Exception x = new System.IO.DirectoryNotFoundException();

                Assert.AreEqual(unchecked((int)0x80070003), ExceptionUtil.HResultFromException(x));
            }
        }

        [TestMethod]
        public void test_TryGetHResultFromException_1()
        {
            {
                Exception x = new HResultException(unchecked((int)0x80070005));

                int         hresult;
                Exception   x2;

                Assert.IsTrue(ExceptionUtil.TryGetHResultFromException(x, HResultFromExceptionOptions.Default, out hresult, out x2));
                Assert.AreEqual(unchecked((int)0x80070005), hresult);
            }

            {
                Exception x = new HResultException(unchecked((int)0x80070005));

                int         hresult;
                Exception   x2;

                Assert.IsTrue(ExceptionUtil.TryGetHResultFromException(x, HResultFromExceptionOptions.None, out hresult, out x2));
                Assert.AreEqual(unchecked((int)0x80070005), hresult);
            }
        }
    }
}
