﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerLogic.Classes
{
    internal class TMDateFormatter
    {
        
        public static string ToString (DateTime date)
        {
            return date.ToString("dd.MM.yyyy_hh.mm.ss");
        }
        public static string ToStringWTime(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }
        public static DateTime ToDate(string date)
        {
            string dateFormat = "dd.MM.yyyy_hh.mm.ss";
            return DateTime.ParseExact(date,
            dateFormat,
            CultureInfo.InvariantCulture);
        }
        
        public static DateTime ToDateWTime(string date)
        {
            string dateFormat = "dd.MM.yyyy";
            return DateTime.ParseExact(date,
            dateFormat,
            CultureInfo.InvariantCulture);
        }
    }
}
