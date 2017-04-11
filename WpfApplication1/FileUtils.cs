using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace WpfApplication1
{
    public class FileUtils
    {
        public static string ReadAll(string path)
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static CsvWriter CreateCsvWriter(string path = @"F:\Result.csv")
        {
            var fs = new FileStream(path, FileMode.Create);

            return new CsvWriter(new StreamWriter(fs, Encoding.Default));
        }

        public static List<CSVMapper> ReadDateList(string path, int colIndex = 0)
        {
            var fs = new FileStream(path, FileMode.Open);

            var csv = new CsvReader(new StreamReader(fs, Encoding.Default));
            var result = csv.GetRecords<CSVMapper>().ToList();
            csv.Dispose();
            return result;
            //List<String> result = new List<string>();
            //using (StreamReader reader = new StreamReader(path))
            //{
            //    string line = reader.ReadLine();
            //    var split = line.Split(',');

            //    if (split != null)
            //    {
            //        result.Add(split[colIndex]);
            //    }
            //}
            //return result;
        }
    }
}
