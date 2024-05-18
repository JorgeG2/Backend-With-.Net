using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var todos = new List<Todo>();

app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));

//Below are the routes for the API such as GET, POST, DELETE
app.MapGet("/todos", () => todos);

//This route is for getting a specific todo item by its id
app.MapGet("/todos/{id}",Results<Ok<Todo>, NotFound> (int id) => 
{
    var targetTodo = todos.SingleOrDefault(t => id == t.Id);
    return targetTodo is null 
      ? TypedResults.NotFound()
      : TypedResults.Ok(targetTodo);
});

//This route is for creating a new todo item
app.MapPost("/todos", (Todo task)  =>
{
    todos.Add(task);
    return TypedResults.Created("/todos/{task.Id}", task);
});

//This route is for deleting a todo item
app.MapDelete("/todos/{id}", (int id) =>
{
    todos.RemoveAll(t => id == t.Id);
    return TypedResults.NoContent();
});
;

app.Run();
//This is a class that represents a todo item with parameters such as Id, Name, DueDate, and IsCompleted
public record Todo(int id, string Name, DateTime DueDate, bool IsCompleted);