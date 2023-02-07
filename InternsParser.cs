using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ForteDigital_BackEnd_Internship_Task.Models;

namespace ForteDigital_BackEnd_Internship_Task;

public abstract class InternsParseTemplate
{
    private static readonly HttpClient _httpClient = new();

    public void ParseInterns(string[] args, bool isZipped = false)
    {
        var name = DownloadFileFromUrl(args[1]);
        
        if (isZipped)
        {
            Unzip(name);
            name = $"{name}dir/interns.csv";
        }
        
        var data = ExtractData(name);
        
        switch (args[0])
        {
            case "max-age":
                Console.WriteLine(data.MaxBy(person => person.age).age);
                break;
            case "count" when args.Length > 2:
            {
                var defAge = Convert.ToInt32(args[3]);

                Console.WriteLine(args[2] == "--age-gt"
                    ? data.Count(person => person.age > defAge)
                    : data.Count(person => person.age < defAge));
                break;
            }
            case "count":
                Console.WriteLine(data.Count());
                break;
            default:
                Console.WriteLine("Error: Invalid command");
                break;
        }
    }
    
    private static void Unzip(string name)
    {
        ZipFile.ExtractToDirectory(name, $"{name}dir");
        File.Delete(name);
    }
    private static string DownloadFileFromUrl(string URL)
    {
        var filename = Guid.NewGuid();
        Task<Stream>? stream = null;
        try
        {
             stream = _httpClient.GetStreamAsync(URL);
        }
        catch (Exception)
        {
            Console.WriteLine("Error: Cannot get file.");
        }
        
        using var fileStream = new FileStream($"{filename}", FileMode.Create);
        stream.Result.CopyTo(fileStream);
        return filename.ToString();
    }
    protected static DateTime ConvertToRegularDateTime(string dateTimeString)
    {

        DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd'T'hh:mm+ss'Z'", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var endDate);
        return endDate;
    }
    protected abstract IEnumerable<Person> ExtractData(string name);
}