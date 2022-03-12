using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EmployeeContext>(opt => opt.UseInMemoryDatabase("Employees"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

//API to get all the employees
app.MapGet("/employees", async (EmployeeContext employeeContext) =>
    await employeeContext.Employees.ToListAsync());


//API to get specific employees
app.MapGet("/employees/{id}", async (int id, EmployeeContext employeeContext) =>
    await employeeContext.Employees.FindAsync(id)
        is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

//API to Create a new Employees
app.MapPost("/employees", async (Employee employee, EmployeeContext employeeContext) =>
{
    employeeContext.Employees.Add(employee);
    await employeeContext.SaveChangesAsync();

    return Results.Created($"/todoitems/{employee.EmployeeId}", employee);
});


app.MapPut("/employees/{id}", async (int id, Employee employeeToUpdate, EmployeeContext employeeContext) =>
{
    var employee = await employeeContext.Employees.FindAsync(id);

    if (employee is null) return Results.NotFound();

    employee.FirstName = employeeToUpdate.FirstName;
    employee.LastName = employeeToUpdate.LastName;
    employee.Designation = employeeToUpdate.Designation;
    await employeeContext.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/employees/{id}", async (int id, EmployeeContext employeeContext) =>
{
    if (await employeeContext.Employees.FindAsync(id) is Employee employee)
    {
        employeeContext.Employees.Remove(employee);
        await employeeContext.SaveChangesAsync();
        return Results.Ok(employee);
    }
    return Results.NotFound();
});
app.Run();


class Employee
{
    public int EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Designation { get; set; }
}

class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options)
        : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
}
