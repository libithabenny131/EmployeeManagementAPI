var builder = DistributedApplication.CreateBuilder(args);

var employeeApi = builder
    .AddProject<Projects.EmployeeManagementAPI>("employeeapi");

builder.AddProject<Projects.EmployeeManagementUI>("employeeui")
    .WithReference(employeeApi)
    .WaitFor(employeeApi);

builder.Build().Run();