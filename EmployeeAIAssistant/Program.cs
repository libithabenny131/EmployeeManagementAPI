using EmployeeAIAssistant.Plugins;
using Microsoft.SemanticKernel;

var builder = Kernel.CreateBuilder();

var kernel = builder.Build();

var plugin =
    kernel.ImportPluginFromObject(
        new EmployeePlugin(),
        "EmployeePlugin");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Employee Copilot");
    Console.WriteLine("1 - Employee Count");
    Console.WriteLine("2 - Highest Salary Employee");
    Console.WriteLine("3 - Get Employee By Id");
    Console.WriteLine("4 - Employees Above Salary");
    Console.WriteLine("0 - Exit");

    var input = Console.ReadLine();

    switch (input)
    {
        case "1":

            var count =
                await plugin["GetEmployeeCount"]
                    .InvokeAsync(kernel);

            Console.WriteLine(count);

            break;

        case "2":

            var highestSalary =
                await plugin["GetHighestSalaryEmployee"]
                    .InvokeAsync(kernel);

            Console.WriteLine(highestSalary);

            break;

        case "3":

            Console.Write("Enter Employee Id: ");

            var employeeId =
                int.Parse(Console.ReadLine()!);

            var employee =
                await plugin["GetEmployeeById"]
                    .InvokeAsync(
                        kernel,
                        new()
                        {
                            ["id"] = employeeId
                        });

            Console.WriteLine(employee);

            break;

        case "4":

            Console.Write("Enter Salary: ");

            var salary =
                decimal.Parse(Console.ReadLine()!);

            var employees =
                await plugin["GetEmployeesAboveSalary"]
                    .InvokeAsync(
                        kernel,
                        new()
                        {
                            ["salary"] = salary
                        });

            Console.WriteLine(employees);

            break;

        case "0":
            return;
    }
}