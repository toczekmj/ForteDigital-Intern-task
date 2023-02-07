using ForteDigital_BackEnd_Internship_Task.Implementations;

namespace ForteDigital_BackEnd_Internship_Task;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            if (string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[0]) || args.Length == 3)
                throw new Exception();

            var ext = DetectExtension(args[1]);

            switch (ext)
            {
                case (int)ExtensionsEnum.Json:
                    var jsonParser = new JsonExtractor();
                    jsonParser.ParseInterns(args);
                    break;
                case (int)ExtensionsEnum.Csv:
                    var csvParser = new CsvExtractor();
                    csvParser.ParseInterns(args);
                    break;
                case (int)ExtensionsEnum.Zip:
                    var zipParser = new CsvExtractor();
                    zipParser.ParseInterns(args, true);
                    break;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error: Invalid command.");
        }
    }

    private static int DetectExtension(string url)
    {
        if (url[^1] == '/')
            url = url.Remove(url.Length - 1, 1);
        var fileExtension = url.Split('.').Last();
        return fileExtension switch
        {
            "json" => 0,
            "csv" => 1,
            "zip" => 2,
            _ => -1
        };
    }
}