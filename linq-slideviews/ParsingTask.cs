using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        var arr = lines.Skip(1).Select(x => x.Split(';', StringSplitOptions.RemoveEmptyEntries))
            .Where(x => x.Count() == 3 && GetInt(x[0]).Item1 && GetSlideType(x[1]).Item1);
        //var dict = new Dictionary<int, SlideRecord>();
        // arr.GroupBy(x => GetInt(x[0]), x => new SlideRecord(GetInt(x[0]), GetSlideType(x[1]), x[2]))
        //   .ToDictionary( x => x.Key, x => x);
        return arr.ToDictionary(x => GetInt(x[0]).Item2, x => new SlideRecord(GetInt(x[0]).Item2, GetSlideType(x[1]).Item2, x[2]));
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        var arr = lines.Skip(1).Select(x => x.Split(';')).ToList();
        bool hasWronglines = arr.Any((x => x.Count() < 4
            || x.Contains("") || !GetInt(x[0]).Item1 || !GetInt(x[1]).Item1
            || !GetDateTime(x[2], x[3]).Item1 || !slides.ContainsKey(GetInt(x[1]).Item2)));
        var wronglineIndex = -1;
        if (hasWronglines)
            wronglineIndex = arr.IndexOf(arr.Select(x => x).Where(x => hasWronglines).Take(1).ToArray()[0]);
        if (wronglineIndex >=0) 
        {
            throw new FormatException($"Wrong line [{lines.ToArray()[wronglineIndex +1]}]");
        }
        
            
        return arr.Select(x => new VisitRecord(GetInt(x[0]).Item2, GetInt(x[1]).Item2, 
            GetDateTime(x[2], x[3]).Item2, slides[GetInt(x[1]).Item2].SlideType));
    }

    public static (bool, int) GetInt(string str)
    {
        int a;
        bool success = int.TryParse(str, out a);
        return (success, a);
    }

    public static (bool, SlideType) GetSlideType(string str)
    {
        var a = new SlideType();
        bool success = Enum.TryParse(str, true, out a);
        return (success, a);
    }

    public static (bool, DateTime) GetDateTime(string str1, string str2 )
    {
        DateOnly date;
        TimeOnly time;
        
        bool success = DateOnly.TryParse(str1, out date) && TimeOnly.TryParse(str2, out time);
        return (success, new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second));
    }
}