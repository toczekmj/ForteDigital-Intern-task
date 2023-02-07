using System;
using System.Collections.Generic;
using System.IO;
using Csv;
using ForteDigital_BackEnd_Internship_Task.Models;

namespace ForteDigital_BackEnd_Internship_Task.Implementations;

public class CsvExtractor : InternsParseTemplate
{
    protected override IEnumerable<Person> ExtractData(string name)
    {
        var list = new List<Person>();
        try
        {
            var csv = string.Empty;
            if(!name.Contains(".csv"))
            {
                File.Move(name, $"{name}.csv");
                csv = File.ReadAllText($"{name}.csv");
                list = ProcessFile(csv);
                File.Delete($"{name}.csv");
            }
            else
            {
                csv = File.ReadAllText(name);
                list = ProcessFile(csv);
                Directory.Delete(Path.GetDirectoryName(name), true);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error: Cannot process the file.");
        }

        return list;
    }

    private static List<Person> ProcessFile(string csv)
    {
        var list = new List<Person>();
        foreach (var line in CsvReader.ReadFromText(csv))
        {
            var startDate = ConvertToRegularDateTime(line[4]);
            var endDate = ConvertToRegularDateTime(line[5]);

            list.Add(new Person()
            {
                id = Convert.ToInt32(line[0]),
                age = Convert.ToInt32(line[1]),
                name = line[2],
                email = line[3],
                internshipStart = startDate,
                internshipEnd = endDate,
            });
        }

        return list;
    }
}