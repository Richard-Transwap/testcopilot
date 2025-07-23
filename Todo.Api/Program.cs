var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add in-memory todo storage
builder.Services.AddSingleton<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Todo API endpoints
app.MapGet("/todos", (ITodoService todoService) =>
{
    return todoService.GetAllTodos();
})
.WithName("GetTodos")
.WithOpenApi();

app.MapGet("/todos/{id}", (int id, ITodoService todoService) =>
{
    var todo = todoService.GetTodoById(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
})
.WithName("GetTodoById")
.WithOpenApi();

app.MapPost("/todos", (CreateTodoRequest request, ITodoService todoService) =>
{
    var todo = todoService.CreateTodo(request.Title, request.Description);
    return Results.Created($"/todos/{todo.Id}", todo);
})
.WithName("CreateTodo")
.WithOpenApi();

app.MapPut("/todos/{id}", (int id, UpdateTodoRequest request, ITodoService todoService) =>
{
    var todo = todoService.UpdateTodo(id, request.Title, request.Description, request.IsCompleted);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
})
.WithName("UpdateTodo")
.WithOpenApi();

app.MapDelete("/todos/{id}", (int id, ITodoService todoService) =>
{
    var deleted = todoService.DeleteTodo(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteTodo")
.WithOpenApi();

app.Run();

// Todo models
public record TodoItem(int Id, string Title, string? Description, bool IsCompleted, DateTime CreatedAt);

public record CreateTodoRequest(string Title, string? Description);

public record UpdateTodoRequest(string Title, string? Description, bool IsCompleted);

// Todo service interface and implementation
public interface ITodoService
{
    IEnumerable<TodoItem> GetAllTodos();
    TodoItem? GetTodoById(int id);
    TodoItem CreateTodo(string title, string? description);
    TodoItem? UpdateTodo(int id, string title, string? description, bool isCompleted);
    bool DeleteTodo(int id);
}

public class TodoService : ITodoService
{
    private readonly List<TodoItem> _todos = new();
    private int _nextId = 1;

    public IEnumerable<TodoItem> GetAllTodos()
    {
        return _todos.ToList();
    }

    public TodoItem? GetTodoById(int id)
    {
        return _todos.FirstOrDefault(t => t.Id == id);
    }

    public TodoItem CreateTodo(string title, string? description)
    {
        var todo = new TodoItem(_nextId++, title, description, false, DateTime.UtcNow);
        _todos.Add(todo);
        return todo;
    }

    public TodoItem? UpdateTodo(int id, string title, string? description, bool isCompleted)
    {
        var existingTodo = _todos.FirstOrDefault(t => t.Id == id);
        if (existingTodo is null)
            return null;

        var updatedTodo = existingTodo with 
        { 
            Title = title, 
            Description = description, 
            IsCompleted = isCompleted 
        };
        
        var index = _todos.IndexOf(existingTodo);
        _todos[index] = updatedTodo;
        return updatedTodo;
    }

    public bool DeleteTodo(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo is null)
            return false;

        _todos.Remove(todo);
        return true;
    }
}

public partial class Program { }
