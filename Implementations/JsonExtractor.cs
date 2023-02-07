using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ForteDigital_BackEnd_Internship_Task.Models;
using Newtonsoft.Json;

namespace ForteDigital_BackEnd_Internship_Task.Implementations;

public class JsonExtractor : InternsParseTemplate
{
    protected override IEnumerable<Person> ExtractData(string name)
    {
        var list = new List<Person>();
        try
        {
            var text = File.ReadAllText(name);
            var dataSet = JsonConvert.DeserializeObject<DataSet>(text);
            if (dataSet is null || dataSet.Tables.Count == 0 || dataSet.Tables["interns"].Rows is null)
                throw new Exception();
            var dataTable = dataSet.Tables["interns"];
            
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(new Person()
                {
                    id = Convert.ToInt32(row["id"]),
                    age = Convert.ToInt32(row["age"]),
                    email = (string)row["email"],
                    name = (string)row["name"],
                    internshipStart = ConvertToRegularDateTime((string)row["internshipStart"]),
                    internshipEnd = ConvertToRegularDateTime((string)row["internshipEnd"])
                });
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error: Cannot process the file.");
        }
        File.Delete(name);
        return list;
    }
}