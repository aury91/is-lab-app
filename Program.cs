using IsLabApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseHttpsRedirection();

// ----- Временное хранилище заметок -----
var notes = new List<Note>();

app.MapPost("/api/notes", (Note note) =>
{
    note.Id = notes.Count + 1;
    note.CreatedAt = DateTime.UtcNow;
    notes.Add(note);
    return Results.Created($"/api/notes/{note.Id}", note);
});

app.MapGet("/api/notes", () => notes);

app.MapGet("/api/notes/{id}", (int id) =>
{
    var note = notes.FirstOrDefault(n => n.Id == id);
    return note is not null ? Results.Ok(note) : Results.NotFound();
});

app.MapDelete("/api/notes/{id}", (int id) =>
{
    var note = notes.FirstOrDefault(n => n.Id == id);
    if (note is null) return Results.NotFound();
    notes.Remove(note);
    return Results.NoContent();
});

// ----- Диагностика -----
app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTime.UtcNow }));
app.MapGet("/version", () => Results.Ok(new { name = "IsLabApp", version = "0.1.0-lab4" }));
app.MapGet("/db/ping", () => Results.Ok(new { message = "DB connection not configured yet", status = "stub" }));

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
