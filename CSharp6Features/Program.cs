// See https://aka.ms/new-console-template for more information
using CSharp6Features;

Console.WriteLine("Hello, World!");
Employee employee = new ();
employee.FirstName = "Jalpesh";
employee.LastName = "Vadgama";
employee.Age = 40;

printMethod(employee);

void printMethod(Employee employee)
{
    ArgumentNullException.ThrowIfNull(employee, nameof(employee));
    Console.WriteLine("Printing Employee");
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine($"FirstName : {employee.FirstName}");
    Console.WriteLine($"LastName : {employee.LastName}");
    Console.WriteLine($"Age : { employee.Age}");
    Console.WriteLine("-------------------------------------------");
}
