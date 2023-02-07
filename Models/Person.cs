using System;
using Newtonsoft.Json;

namespace ForteDigital_BackEnd_Internship_Task.Models;

public class Person
{
    public int id { get; set; }
    public int age { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public DateTime internshipEnd { get; set; } 
    public DateTime internshipStart { get; set; }
}
