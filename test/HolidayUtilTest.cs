using Lunar.Util;
using NUnit.Framework;

namespace test
{
    /// <summary>
    /// 节假日
    /// </summary>
    public class HolidayUtilTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetHolidayTest()
        {
            Assert.AreEqual("2020-05-01 劳动节 2020-05-01", HolidayUtil.GetHoliday(2020, 5, 1).ToString());
        }

        /// <summary>
        ///GetHoliday (string) 的测试
        ///</summary>
        [Test]
        public void GetHolidayTest1()
        {
            Assert.AreEqual("2011-05-02 劳动节 2011-05-01", HolidayUtil.GetHoliday("2011-05-02").ToString());
        }

        /// <summary>
        ///GetHolidays (int) 的测试
        ///</summary>
        [Test]
        public void GetHolidaysTest()
        {
            Assert.AreEqual(35, HolidayUtil.GetHolidays(2012).Count);
        }

        /// <summary>
        ///GetHolidays (int, int) 的测试
        ///</summary>
        [Test]
        public void GetHolidaysTest1()
        {
            Assert.AreEqual(1, HolidayUtil.GetHolidays(2013, 5).Count);
        }

        /// <summary>
        ///GetHolidays (string) 的测试
        ///</summary>
        [Test]
        public void GetHolidaysTest2()
        {
            Assert.AreEqual(35, HolidayUtil.GetHolidays("2012").Count);
        }

        /// <summary>
        ///GetHolidaysByTarget (int, int, int) 的测试
        ///</summary>
        [Test]
        public void GetHolidaysByTargetTest()
        {
            Assert.AreEqual(4, HolidayUtil.GetHolidaysByTarget(2018, 5, 1).Count);
        }

        /// <summary>
        ///GetHolidaysByTarget (string) 的测试
        ///</summary>
        [Test]
        public void GetHolidaysByTargetTest1()
        {
            Assert.AreEqual(4, HolidayUtil.GetHolidaysByTarget("2018-05-01").Count);
        }

        /// <summary>
        ///fix (string[], string) 的测试
        ///</summary>
        [Test]
        public void TestFix()
        {
            Assert.AreEqual("2020-01-01 元旦节 2020-01-01", HolidayUtil.GetHoliday("2020-01-01") + "");

            // 将2020-01-01修改为春节
            HolidayUtil.Fix("202001011120200101");
            Assert.AreEqual("2020-01-01 春节 2020-01-01", HolidayUtil.GetHoliday("2020-01-01") + "");

            // 追加2099-01-01为元旦节
            HolidayUtil.Fix("209901010120990101");
            Assert.AreEqual("2099-01-01 元旦节 2099-01-01", HolidayUtil.GetHoliday("2099-01-01") + "");

            // 将2020-01-01修改为春节，并追加2099-01-01为元旦节
            HolidayUtil.Fix("202001011120200101209901010120990101");
            Assert.AreEqual("2020-01-01 春节 2020-01-01", HolidayUtil.GetHoliday("2020-01-01") + "");
            Assert.AreEqual("2099-01-01 元旦节 2099-01-01", HolidayUtil.GetHoliday("2099-01-01") + "");

            // 自定义节假日名称
            var names = new string[HolidayUtil.NAMES.Length];
            for (int i = 0, j = HolidayUtil.NAMES.Length; i < j; i++) {
                names[i] = HolidayUtil.NAMES[i];
            }
            names[0] = "元旦";
            names[1] = "大年初一";

            HolidayUtil.Fix(names, null);
            Assert.AreEqual("2020-01-01 大年初一 2020-01-01", HolidayUtil.GetHoliday("2020-01-01") + "");
            Assert.AreEqual("2099-01-01 元旦 2099-01-01", HolidayUtil.GetHoliday("2099-01-01") + "");

            // 追加节假日名称和数据
            names = new string[12];
            for (int i = 0, j = HolidayUtil.NAMES.Length; i < j; i++)
            {
                names[i] = HolidayUtil.NAMES[i];
            }
            names[9] = "我的生日";
            names[10] = "结婚纪念日";
            names[11] = "她的生日";

            HolidayUtil.Fix(names, "20210529912021052920211111:12021111120211201;120211201");
            Assert.AreEqual("2021-05-29 我的生日 2021-05-29", HolidayUtil.GetHoliday("2021-05-29") + "");
            Assert.AreEqual("2021-11-11 结婚纪念日 2021-11-11", HolidayUtil.GetHoliday("2021-11-11") + "");
            Assert.AreEqual("2021-12-01 她的生日 2021-12-01", HolidayUtil.GetHoliday("2021-12-01") + "");

        }

        [Test]
        public void Test5()
        {
            Assert.AreEqual("2016-10-01", HolidayUtil.GetHoliday("2016-10-04").Target);
        }

        [Test]
        public void Test6()
        {
            // 设置默认的节假日名称
            HolidayUtil.Fix(HolidayUtil.NAMES, null);
            Assert.AreEqual("元旦节", HolidayUtil.GetHoliday("2010-01-01").Name);

            HolidayUtil.Fix("20100101~000000000000000000000000000");
            Assert.IsNull(HolidayUtil.GetHoliday("2010-01-01"));
        }
    }
}