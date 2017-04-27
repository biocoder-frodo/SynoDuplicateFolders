using System;
using NUnit.Framework;
using SynoDuplicateFolders.Data;

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

            Assert.NotNull(NewReportPair(new SynoReportVolumeUsageValues(), new SynoReportSharesValues()));
            Assert.NotNull(NewReportPair(new SynoReportSharesValues(), new SynoReportVolumeUsageValues()));

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

        public static void ThrowsParamName<T>(TestDelegate code, string param) where T : Exception, new()
        {
            T ex = Assert.Throws<T>(code, "The {0} exception was not raised.", new T().GetType().Name);
            var prop = ex.GetType().GetProperty("ParamName");
            Assert.NotNull(prop, "The {0} exception does not implement a ParamName property.", new T().GetType().Name);
            Assert.That(prop.GetMethod.Invoke(ex, null), Is.EqualTo(param), "An unexpected parameter name was returned.");
        }

    }
}
