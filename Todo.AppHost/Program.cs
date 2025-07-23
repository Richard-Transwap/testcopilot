using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Add the Todo API project
var todoApi = builder.AddProject("todoapi", "../Todo.Api/Todo.Api.csproj");

builder.Build().Run();

public partial class Program { }
