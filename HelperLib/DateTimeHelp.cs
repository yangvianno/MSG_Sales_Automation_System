using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HelperLib
{
    public class DateTimeHelp
    {
        public static List<TuanVM> Tuans = new List<TuanVM>();

        public static TuanVM LayTuan(int Tuan, int Nam)
        {
            var found = Tuans.Find(x => x.Nam == Nam && x.Tuan == Tuan);
            if (found == null)
            {
                TaoTuanTrongNam(Nam);
            }
            found = Tuans.Find(x => x.Nam == Nam && x.Tuan == Tuan);
            return found;
        }

        public static TuanVM LayTuan(DateTime ngay)
        {
            CultureInfo myCI = new CultureInfo("vi-VN");
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            Calendar myCal = myCI.Calendar;            
            int Tuan = myCal.GetWeekOfYear(ngay, myCWR, myFirstDOW);
            int Nam = ngay.Date.Year;

            var found = Tuans.Find(x => x.Nam == Nam && x.Tuan == Tuan);
            if (found == null)
            {
                TaoTuanTrongNam(Nam);
            }
            found = Tuans.Find(x => x.Nam == Nam && x.Tuan == Tuan);
            return found;
        }

        private static void TaoTuanTrongNam(int nam)
        {
            CultureInfo myCI = new CultureInfo("vi-VN");
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            Calendar myCal = myCI.Calendar;
            DateTime NgayDauNam = new DateTime(nam, 1, 1);
            int TuanDau= myCal.GetWeekOfYear(NgayDauNam, myCWR, myFirstDOW);            
            int SoTuan = myCal.GetWeekOfYear(NgayDauNam.AddYears(1).AddDays(-1), myCWR, myFirstDOW);
            for (int i=0; i<=SoTuan; i++)
            {
                //if (NgayDauNam.DayOfWeek == DayOfWeek.Sunday)
                //{
                //    TuanVM tuan = new TuanVM
                //    {
                //        Nam = nam,
                //        Tuan = TuanDau + i,
                //        TuNgay = NgayDauNam,
                //        DenNgay = NgayDauNam,
                //    };
                //    Tuans.Add(tuan);
                //}
                //else
                {
                    TuanVM tuan = new TuanVM
                    {
                        Nam = nam,
                        Tuan = TuanDau + i,
                        TuNgay = NgayDauNam.AddDays(0 - 7 + 1),
                        DenNgay = NgayDauNam.AddDays(6 - 7 + 1),
                    };
                    Tuans.Add(tuan);
                }                
                NgayDauNam = NgayDauNam.AddDays(7);
            }
        }

        public static List<TuanVM> LayTuanTrongNam(int nam)
        {
            var found = Tuans.Find(x => x.Nam == nam);
            if (found==null)
            {
                TaoTuanTrongNam(nam);
            }
            var result = Tuans.FindAll(x => x.Nam == nam);
            return result;
        }
    }
}
