using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerLogic.Classes
{
    internal class CSVreader
    {
        // Возвращает все строки файла по указанному пути в виде листа (0-индекс заголовки столбцов)
        public static List<string> Read(string FilePath)
        {
            var Lines = File.ReadAllLines(FilePath).ToList();
            return Lines;
        }
        // Возвращает строку файла по указанному пути с указанным номером (0-индекс заголовки столбцов)
        public static string ReadStringByNumber(string FilePath, int n)
        {
            var Lines = File.ReadAllLines(FilePath).ToList();
            
            return Lines[n];
        }
        // Возвращает строку файла по указанному пути с указанными значениями определённых столбцов 
        public static string ReadStringByColumns(string FilePath, string[] columnNames, string[] rowValues)
        {
            var Lines = File.ReadAllLines(FilePath).ToList();
            var columns = Lines[0].Split(';');
            int[] columnsToCheck = new int[columnNames.Length];
            for (int i = 0; i < columns.Length; i++) { 
                if (columnNames.Contains(columns[i])) columnsToCheck[i] = i;
            }
            for (int i = 0; i < Lines.Count(); i++)
            {
                var Line = Lines[i].Split(";");
                bool isEqual = true;
                for (int j = 0; j < columnsToCheck.Length; j++)
                {
                    if (!(Line[columnsToCheck[j]] == rowValues[j]))
                    {
                        isEqual = false;
                    }
                }
                if (isEqual) return Lines[i];
            }
            return "";
        }

        // Перезаписывает всё содержимое файла, не для использования из вне
        private static void Write(string FilePath, List<string> Lines)
        {
            var csv = new StringBuilder();
            foreach (var Line in Lines) { 
                csv.AppendLine(string.Join("\n", Line));
            }
            File.WriteAllText(FilePath, csv.ToString());
           
        }

        // Записывает строку в конец файла
        public static void AddNewRecord(string FilePath, string NewLine)
        {
            var Lines = File.ReadAllLines(FilePath).ToList();
            Lines.Add(NewLine);
            Write(FilePath, Lines);
        }

        // Перезаписывает строку файла по указанному пути с указанными значениями определённых столбцов, зачем - не знаю
        public static int WriteStringByColumns(string FilePath, string[] columnNames, string[] rowValues, string newValue)
        {
            var Lines = File.ReadAllLines(FilePath).ToList();
            var columns = Lines[0].Split(';');
            int[] columnsToCheck = new int[columnNames.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                if (columnNames.Contains(columns[i])) columnsToCheck[i] = i;
            }
            for (int i = 0; i < Lines.Count(); i++)
            {
                var Line = Lines[i].Split(";");
                bool isEqual = true;
                for (int j = 0; j < columnsToCheck.Length; j++)
                {
                    if (!(Line[columnsToCheck[j]] == rowValues[j]))
                    {
                        isEqual = false;
                    }
                }
                if (isEqual) {
                    Lines[i] = newValue;
                    Write(FilePath, Lines);
                    return 0;
                };
            }
            return 1;
        }
        // Возвращет название файла по указанной маске в указанной директории
        public static string GetFileNameByMask(string FilePath, string FileMask)
        {
            foreach (string s in Directory.GetFiles(FilePath, FileMask).Select(Path.GetFileName))
                return s;
            return "";
        }
        
    }
}
