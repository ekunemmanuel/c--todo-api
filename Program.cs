using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using todo_api.Data;
using todo_api.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. ALL SERVICES MUST BE ADDED BEFORE builder.Build()
builder.Services.AddOpenApi();
// Get connection string and register PostgreSQL Context
var connectionString = builder.Configuration["ConnectionStrings:DatabaseConnection"];
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddValidation();
builder.Services.AddProblemDetails();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Todo, TodoDTO>().ReverseMap();
    cfg.CreateMap<TodoCreateDTO, Todo>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false))
    .ForMember(dest => dest.CompletedAt, opt => opt.Ignore());
    cfg.CreateMap<TodoUpdateDTO, Todo>()
     .ForMember(dest => dest.Id, opt => opt.Ignore())
     .AfterMap((src, dest) =>
     {
         if (src.IsCompleted)
         {
             dest.CompletedAt = DateTime.UtcNow;
         }
     })
     .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
         srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
});

// 2. BUILD THE APP
var app = builder.Build();

// 3. CONFIGURE THE HTTP PIPELINE AFTER builder.Build()
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
} 

app.UseHttpsRedirection();

app.MapGet("/", async (ApplicationDbContext db) =>{
    var todos = await db.Todos.ToArrayAsync();
    return Results.Ok(todos);
}).WithName("Get All Todos");

app.MapPost("/", async (IMapper mapper, ApplicationDbContext db, TodoCreateDTO todoDto) =>
{
    var newTodo = mapper.Map<Todo>(todoDto);
    await db.Todos.AddAsync(newTodo);
    await db.SaveChangesAsync();
    var todoDTO = mapper.Map<TodoDTO>(newTodo);
    return Results.CreatedAtRoute("Get a Todo", new { id = todoDTO.Id }, todoDTO);
})
    .WithParameterValidation()
    .WithName("Create a Todo").Accepts<TodoCreateDTO>("application/json")
.Produces<TodoDTO>(StatusCodes.Status201Created)
.ProducesProblem(StatusCodes.Status400BadRequest);


app.MapGet("/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
})
.WithName("Get a Todo")
.Produces<TodoDTO>(StatusCodes.Status201Created)
.ProducesProblem(StatusCodes.Status400BadRequest)
.ProducesProblem(StatusCodes.Status404NotFound);

app.MapPatch("/{id:int}", async (IMapper mapper, ApplicationDbContext db, int id, TodoUpdateDTO todoDto) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    mapper.Map(todoDto, todo);
    await db.SaveChangesAsync();
    var todoDTO = mapper.Map<TodoDTO>(todo);
    return Results.CreatedAtRoute("Get a Todo", new { id = todoDTO.Id }, todoDTO);
})
    .WithParameterValidation()
    .WithName("Update a Todo").Accepts<TodoUpdateDTO>("application/json")
.Produces<TodoDTO>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status404NotFound)
.ProducesProblem(StatusCodes.Status400BadRequest);

app.MapDelete("/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithName("Delete a Todo")
.ProducesProblem(StatusCodes.Status404NotFound);

app.MapDelete("/", async (ApplicationDbContext db) =>
{
    await db.Todos.ExecuteDeleteAsync();
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithName("Delete all todos");


app.Run();