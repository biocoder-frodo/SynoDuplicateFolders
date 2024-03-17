using NUnit.Framework;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.Core;
using System;
using static NUnit.Framework.Legacy.ClassicAssert;

namespace SynoDuplicateFolders.Test
{
    [TestFixture]
    public class UnitTest1
    {

        private static SynoCSVReportPair<SynoReportSharesValues, SynoReportVolumeUsageValues> NewReportPair(ISynoCSVReport first, ISynoCSVReport second)
        {
            return new SynoCSVReportPair<SynoReportSharesValues, SynoReportVolumeUsageValues>(first, second);
        }

        [Test]
        public void TestReportPairs()
        {            
            AssertException.ThrowsParamName<ArgumentNullException>(() =>
            {
                NewReportPair(null, null);
            }, "first");

            AssertException.ThrowsParamName<ArgumentNullException>(() =>
            {
                NewReportPair(null, new SynoReportSharesValues());
            }, "first");

            AssertException.ThrowsParamName<ArgumentNullException>(() =>
            {
                NewReportPair(new SynoReportSharesValues(), null);
            }, "second");

            NotNull(NewReportPair(new SynoReportVolumeUsageValues(), new SynoReportSharesValues()));
            NotNull(NewReportPair(new SynoReportSharesValues(), new SynoReportVolumeUsageValues()));

            AssertException.ThrowsParamName<ArgumentException>
                (() =>
                {
                    NewReportPair(new SynoReportOwners(), new SynoReportSharesValues());
                }, "first");

            AssertException.ThrowsParamName<ArgumentException>
                (() =>
                {
                    NewReportPair(new SynoReportOwners(), new SynoReportVolumeUsageValues());
                }, "first");

            AssertException.ThrowsParamName<ArgumentException>
                (() =>
                {
                    NewReportPair(new SynoReportSharesValues(), new SynoReportOwners());
                }, "second");

            AssertException.ThrowsParamName<ArgumentException>
                (() =>
                {
                    NewReportPair(new SynoReportVolumeUsageValues(), new SynoReportOwners());
                }, "second");
        }
    }

    public static class AssertException
    {

        public static void ThrowsParamName<T>(TestDelegate code, string param) where T : Exception
        {
            T ex = Assert.Throws<T>(code, "The {0} exception was not raised.", typeof(T).Name);
            var prop = ex.GetType().GetProperty("ParamName");
            NotNull(prop, "The {0} exception does not implement a ParamName property.", typeof(T).Name);
            Assert.That(prop.GetMethod.Invoke(ex, null), Is.EqualTo(param), "An unexpected parameter name was returned.");
        }

    }
}
