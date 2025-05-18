using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_O.Classes
{
    internal static class GroupSettings
    {
        public static bool isMaster = true; 
        public static string[][] Lessons = new string[][]
            {
                new string[] {  },
                new string[] { "Высшая математика", "Программирование", "Философия" },
                new string[] { "Физика", "Иностранный язык", "История науки" },
                new string[] { "Алгоритмы", "Базы данных", "Веб-разработка" },
                new string[] { "Высшая математика", "Программирование", "Философия" },
                new string[] { "Физика", "Иностранный язык", "История науки" },
                new string[] { "Алгоритмы", "Базы данных", "Веб-разработка" }
            };
        public static string[] UniqueLessons = new string[] { };
    }
}
