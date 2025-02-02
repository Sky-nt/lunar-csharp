using System;
using Lunar.Util;
// ReSharper disable MemberCanBePrivate.Global

namespace Lunar
{
    /// <summary>
    /// 农历月
    /// </summary>
    public class LunarMonth
    {
        /// <summary>
        /// 农历年
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// 农历月：1-12，闰月为负数，如闰2月为-2
        /// </summary>
        public int Month { get; }

        /// <summary>
        /// 天数，大月30天，小月29天
        /// </summary>
        public int DayCount { get; }

        /// <summary>
        /// 初一的儒略日
        /// </summary>
        public double FirstJulianDay { get; }
        
        public int Index { get; }
        
        public int ZhiIndex { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lunarYear">农历年</param>
        /// <param name="lunarMonth">农历月：1-12，闰月为负数，如闰2月为-2</param>
        /// <param name="dayCount">天数</param>
        /// <param name="firstJulianDay">初一的儒略日</param>
        /// <param name="index">序号</param>
        public LunarMonth(int lunarYear, int lunarMonth, int dayCount, double firstJulianDay, int index)
        {
            Year = lunarYear;
            Month = lunarMonth;
            DayCount = dayCount;
            FirstJulianDay = firstJulianDay;
            Index = index;
            ZhiIndex = (index - 1 + LunarUtil.BASE_MONTH_ZHI_INDEX) % 12;
        }

        /// <summary>
        /// 通过农历年月初始化
        /// </summary>
        /// <param name="lunarYear">农历年</param>
        /// <param name="lunarMonth">农历月：1-12，闰月为负数，如闰2月为-2</param>
        /// <returns>农历月</returns>
        public static LunarMonth FromYm(int lunarYear, int lunarMonth)
        {
            return LunarYear.FromYear(lunarYear).GetMonth(lunarMonth);
        }

        /// <summary>
        /// 是否闰月
        /// </summary>
        public bool Leap => Month < 0;

        public int GanIndex
        {
            get
            {
                var offset = (LunarYear.FromYear(Year).GanIndex + 1) % 5 * 2;
                return (Index - 1 + offset) % 10;
            }
        }

        public string Gan => LunarUtil.GAN[GanIndex + 1];
        
        public string Zhi => LunarUtil.ZHI[ZhiIndex + 1];
        
        public string GanZhi => Gan + Zhi;
        
        public string PositionXi => LunarUtil.POSITION_XI[GanIndex + 1];

        public string PositionXiDesc => LunarUtil.POSITION_DESC[PositionXi];

        public string PositionYangGui => LunarUtil.POSITION_YANG_GUI[GanIndex + 1];

        public string PositionYangGuiDesc => LunarUtil.POSITION_DESC[PositionYangGui];

        public string PositionYinGui => LunarUtil.POSITION_YIN_GUI[GanIndex + 1];

        public string PositionYinGuiDesc => LunarUtil.POSITION_DESC[PositionYinGui];

        public string PositionFu => GetPositionFu();
        
        public string GetPositionFu(int sect = 2) {
            return (1 == sect ? LunarUtil.POSITION_FU : LunarUtil.POSITION_FU_2)[GanIndex + 1];
        }

        public string PositionFuDesc => GetPositionFuDesc();
        
        public string GetPositionFuDesc(int sect = 2) {
            return LunarUtil.POSITION_DESC[GetPositionFu(sect)];
        }

        public string PositionCai => LunarUtil.POSITION_CAI[GanIndex + 1];

        public string PositionCaiDesc => LunarUtil.POSITION_DESC[PositionCai];

        /// <summary>
        /// 太岁方位，如艮
        /// </summary>
        public string PositionTaiSui
        {
            get
            {
                string p;
                var m = Math.Abs(Month);
                switch (m)
                {
                    case 1:
                    case 5:
                    case 9:
                        p = "艮";
                        break;
                    case 3:
                    case 7:
                    case 11:
                        p = "坤";
                        break;
                    case 4:
                    case 8:
                    case 12:
                        p = "巽";
                        break;
                    default:
                        p = LunarUtil.POSITION_GAN[Solar.FromJulianDay(FirstJulianDay).Lunar.MonthGanIndex];
                        break;
                }
                return p;
            }
        }

        /// <summary>
        /// 太岁方位描述，如东北
        /// </summary>
        public string PositionTaiSuiDesc => LunarUtil.POSITION_DESC[PositionTaiSui];

        /// <summary>
        /// 九星
        /// </summary>
        public NineStar NineStar
        {
            get
            {
                var index = LunarYear.FromYear(Year).ZhiIndex % 3;
                var m = Math.Abs(Month);
                var monthZhiIndex = (13 + m) % 12;
                var n = 27 - (index * 3);
                if (monthZhiIndex < LunarUtil.BASE_MONTH_ZHI_INDEX)
                {
                    n -= 3;
                }
                var offset = (n - monthZhiIndex) % 9;
                return NineStar.FromIndex(offset);
            }
        }

        public override string ToString()
        {
            return Year + "年" + (Leap ? "闰" : "") + LunarUtil.MONTH[Math.Abs(Month)] + "月(" + DayCount + "天)";
        }

        public LunarMonth Next(int n)
        {
            if (0 == n)
            {
                return FromYm(Year, Month);
            }
            if (n > 0)
            {
                var rest = n;
                var ny = Year;
                var iy = ny;
                var im = Month;
                var index = 0;
                var months = LunarYear.FromYear(ny).Months;
                while (true)
                {
                    var size = months.Count;
                    for (var i = 0; i < size; i++)
                    {
                        var m = months[i];
                        if (m.Year != iy || m.Month != im) continue;
                        index = i;
                        break;
                    }

                    var more = size - index - 1;
                    if (rest < more)
                    {
                        break;
                    }

                    rest -= more;
                    var lastMonth = months[size - 1];
                    iy = lastMonth.Year;
                    im = lastMonth.Month;
                    ny++;
                    months = LunarYear.FromYear(ny).Months;
                }

                return months[index + rest];
            }
            else
            {
                var rest = -n;
                var ny = Year;
                var iy = ny;
                var im = Month;
                var index = 0;
                var months = LunarYear.FromYear(ny).Months;
                while (true)
                {
                    var size = months.Count;
                    for (var i = 0; i < size; i++)
                    {
                        var m = months[i];
                        if (m.Year != iy || m.Month != im) continue;
                        index = i;
                        break;
                    }

                    if (rest <= index)
                    {
                        break;
                    }

                    rest -= index;
                    var firstMonth = months[0];
                    iy = firstMonth.Year;
                    im = firstMonth.Month;
                    ny--;
                    months = LunarYear.FromYear(ny).Months;
                }

                return months[index - rest];
            }
        }
    }
}