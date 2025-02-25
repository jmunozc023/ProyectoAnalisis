using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTION_STRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTION_STRING");
}

builder.Services.AddDbContext<UsuariosDbContext>(options =>
    options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/Usuarios", async (UsuariosDbContext context) =>
{
    return await context.Usuarios.ToListAsync();
})
.WithName("GetUsuarios");


app.MapPost("/Usuarios", async (Usuarios usuario, UsuariosDbContext context) =>
{
    context.Add(usuario);
    await context.SaveChangesAsync();
})
.WithName("CreateUsuario");

app.Run();

public class Usuarios
{
    public int user_id { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public string email { get; set; }
    public string contrasena { get; set; }
    public int Roles_id_rol { get; set; }

}



public class UsuariosDbContext : DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuarios> Usuarios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.user_id); // Asegúrate de que UserId sea la clave primaria
                                           // Otras configuraciones de la entidad
        });

        base.OnModelCreating(modelBuilder);
    }
}