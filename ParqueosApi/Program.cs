using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Azure Keyvault connection
var keyVaultEndpoint = new Uri(builder.Configuration["VaultKey"]);
var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
KeyVaultSecret kvs = secretClient.GetSecret("parqueoappsecret");
builder.Services.AddDbContext<UsuariosDbContext>(o => o.UseSqlServer(kvs.Value));
builder.Services.AddDbContext<ParqueosDbContext>(o => o.UseSqlServer(kvs.Value));
builder.Services.AddDbContext<EspaciosDbContext>(o => o.UseSqlServer(kvs.Value));
builder.Services.AddDbContext<VehiculosDbContext>(o => o.UseSqlServer(kvs.Value));
builder.Services.AddDbContext<Asig_VehiculosDbContext>(o => o.UseSqlServer(kvs.Value));




//var connection = String.Empty;
//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
//    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTION_STRING");
//}
//else
//{
//    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTION_STRING");
//}

//builder.Services.AddDbContext<UsuariosDbContext>(options =>
//    options.UseSqlServer(connection));

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

//***** Metodos para Usuarios *****
//Metodo para get Usuarios
app.MapGet("/Usuarios", async (UsuariosDbContext context) =>
{
    return await context.Usuarios.ToListAsync();
})
.WithName("GetUsuarios");

//Metodo para Post Usuarios
app.MapPost("/Usuarios", async (Usuarios usuario, UsuariosDbContext context) =>
{
    // Ensure user_id is not set to avoid primary key conflict
    usuario.user_id = 0;
    context.Add(usuario);
    await context.SaveChangesAsync();
    return Results.Created($"/Usuarios/{usuario.user_id}", usuario);
})
.WithName("CreateUsuario");

//Metodo para Put Usuarios
app.MapPut("/Usuarios/{id}", async (int id, Usuarios usuario, UsuariosDbContext context) =>
{
    var usuarioToUpdate = await context.Usuarios.FindAsync(id);
    if (usuarioToUpdate == null)
    {
        return Results.NotFound();
    }
    usuarioToUpdate.nombre = usuario.nombre;
    usuarioToUpdate.apellido = usuario.apellido;
    usuarioToUpdate.email = usuario.email;
    usuarioToUpdate.contrasena = usuario.contrasena;
    usuarioToUpdate.Roles_id_rol = usuario.Roles_id_rol;
    await context.SaveChangesAsync();
    return Results.NoContent();
});
//Metodo para Delete Usuarios
app.MapDelete("/Usuarios/{id}", async (int id, UsuariosDbContext context) =>
{
    var usuario = await context.Usuarios.FindAsync(id);
    if (usuario == null)
    {
        return Results.NotFound();
    }
    context.Usuarios.Remove(usuario);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
//***** Metodo para Parqueos *****
//Metodo para get Parqueos
app.MapGet("/Parqueos", async (ParqueosDbContext context) =>
{
    return await context.Parqueos.ToListAsync();
});

////Metodo para Post Parqueos
app.MapPost("/Parqueos", async (Parqueos parqueo, ParqueosDbContext context) =>
{
    // Ensure id_parqueo is not set to avoid primary key conflict
    parqueo.id_parqueo = 0;
    context.Add(parqueo);
    await context.SaveChangesAsync();
    return Results.Created($"/Parqueos/{parqueo.id_parqueo}", parqueo);
});
////Metodo para Put Parqueos
app.MapPut("/Parqueos/{id}", async (int id, Parqueos parqueo, ParqueosDbContext context) =>
{
    var parqueoToUpdate = await context.Parqueos.FindAsync(id);
    if (parqueoToUpdate == null)
    {
        return Results.NotFound();
    }
    parqueoToUpdate.nombre_parqueo = parqueo.nombre_parqueo;
    parqueoToUpdate.ubicacion = parqueo.ubicacion;
    await context.SaveChangesAsync();
    return Results.NoContent();
});
////Metodo para Delete Parqueos
app.MapDelete("/Parqueos/{id}", async (int id, ParqueosDbContext context) =>
{
    var parqueo = await context.Parqueos.FindAsync(id);
    if (parqueo == null)
    {
        return Results.NotFound();
    }
    context.Parqueos.Remove(parqueo);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

////**** Metodo para espacios ****

////Metodo para get espacios
app.MapGet("/Espacios", async (EspaciosDbContext context) =>
{
    return await context.Espacios.ToListAsync();
});

////Metodo para Post espacios
app.MapPost("/Espacios", async (Espacios espacio, EspaciosDbContext context) =>
{
    // Ensure id_espacio is not set to avoid primary key conflict
    espacio.id_espacio = 0;
    context.Add(espacio);
    await context.SaveChangesAsync();
    return Results.Created($"/Espacios/{espacio.id_espacio}", espacio);
});
////Metodo para Put espacios
app.MapPut("/Espacios/{id}", async (int id, Espacios espacio, EspaciosDbContext context) =>
{
    var espacioToUpdate = await context.Espacios.FindAsync(id);
    if (espacioToUpdate == null)
    {
        return Results.NotFound();
    }
    espacioToUpdate.tipo_espacio = espacio.tipo_espacio;
    espacioToUpdate.disponibilidad = espacio.disponibilidad;
    espacioToUpdate.Parqueos_id_parqueo = espacio.Parqueos_id_parqueo;
    await context.SaveChangesAsync();
    return Results.NoContent();
});
////Metodo para Delete espacios
app.MapDelete("/Espacios/{id}", async (int id, EspaciosDbContext context) =>
{
    var espacio = await context.Espacios.FindAsync(id);
    if (espacio == null)
    {
        return Results.NotFound();
    }
    context.Espacios.Remove(espacio);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

////**** Metodo para vehiculos ****

////Metodo para get vehiculos
app.MapGet("/Vehiculos", async (VehiculosDbContext context) =>
{
    return await context.Vehiculos.ToListAsync();
});

////Metodo para Post vehiculos
app.MapPost("/Vehiculos", async (Vehiculos vehiculo, VehiculosDbContext context) =>
{
    // Ensure id_vehiculo is not set to avoid primary key conflict
    vehiculo.id_vehiculo = 0;
    context.Add(vehiculo);
    await context.SaveChangesAsync();
    return Results.Created($"/Vehiculos/{vehiculo.id_vehiculo}", vehiculo);
});
////Metodo para Put vehiculos
app.MapPut("/Vehiculos/{id}", async (int id, Vehiculos vehiculo, VehiculosDbContext context) =>
{
    var vehiculoToUpdate = await context.Vehiculos.FindAsync(id);
    if (vehiculoToUpdate == null)
    {
        return Results.NotFound();
    }
    vehiculoToUpdate.placa = vehiculo.placa;
    vehiculoToUpdate.tipo_vehiculo = vehiculo.tipo_vehiculo;
    vehiculoToUpdate.Usuarios_user_id = vehiculo.Usuarios_user_id;
    await context.SaveChangesAsync();
    return Results.NoContent();
});

////Metodo para Delete vehiculos
app.MapDelete("/Vehiculos/{id}", async (int id, VehiculosDbContext context) =>
{
    var vehiculo = await context.Vehiculos.FindAsync(id);
    if (vehiculo == null)
    {
        return Results.NotFound();
    }
    context.Vehiculos.Remove(vehiculo);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

//**** Metodo para asignacion de vehiculos ****

//Metodo para get asignacion de vehiculos
app.MapGet("/Asig_Vehiculos", async (Asig_VehiculosDbContext context) =>
{
    return await context.Asig_Vehiculos.ToListAsync();
});

//Metodo para Post asignacion de vehiculos
app.MapPost("/Asig_Vehiculos", async (Asig_Vehiculos asig_vehiculo, Asig_VehiculosDbContext context) =>
{
    // Ensure id_asig_vehiculo is not set to avoid primary key conflict
    asig_vehiculo.id_asig_vehiculo = 0;
    context.Add(asig_vehiculo);
    await context.SaveChangesAsync();
    return Results.Created($"/Asig_Vehiculos/{asig_vehiculo.id_asig_vehiculo}", asig_vehiculo);
});


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
public class Parqueos
{
    public int id_parqueo { get; set; }
    public string nombre_parqueo { get; set; }
    public string ubicacion { get; set; }
}
public class Espacios
{
    public int id_espacio { get; set; }
    public string tipo_espacio { get; set; }
    public bool disponibilidad { get; set; }
    public int Parqueos_id_parqueo { get; set; }
}
public class Vehiculos
{
    public int id_vehiculo { get; set; }
    public string placa { get; set; }
    public string tipo_vehiculo { get; set; }
    public int Usuarios_user_id { get; set; }
}
public class Asig_Vehiculos
{
    public int id_asig_vehiculo { get; set; }
    public DateTime fecha_hora_entrada { get; set; }
    public DateTime fecha_hora_salida { get; set; }
    public int Usuarios_user_id { get; set; }
    public int Espacios_id_espacios { get; set; }
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
            entity.HasKey(e => e.user_id); 
        });

        base.OnModelCreating(modelBuilder);
    }
}
public class ParqueosDbContext : DbContext
{
    public ParqueosDbContext(DbContextOptions<ParqueosDbContext> options)
        : base(options)
    {
    }
    public DbSet<Parqueos> Parqueos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parqueos>(entity =>
        {
            entity.HasKey(e => e.id_parqueo); 
        });
        base.OnModelCreating(modelBuilder);
    }
}
public class EspaciosDbContext : DbContext
{
    public EspaciosDbContext(DbContextOptions<EspaciosDbContext> options)
        : base(options)
    {
    }
    public DbSet<Espacios> Espacios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Espacios>(entity =>
        {
            entity.HasKey(e => e.id_espacio);
        });
        base.OnModelCreating(modelBuilder);
    }
}
public class VehiculosDbContext : DbContext
{
    public VehiculosDbContext(DbContextOptions<VehiculosDbContext> options)
        : base(options)
    {
    }
    public DbSet<Vehiculos> Vehiculos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehiculos>(entity =>
        {
            entity.HasKey(e => e.id_vehiculo);
        });
        base.OnModelCreating(modelBuilder);
    }
}
public class Asig_VehiculosDbContext : DbContext
{
    public Asig_VehiculosDbContext(DbContextOptions<Asig_VehiculosDbContext> options)
        : base(options)
    {
    }
    public DbSet<Asig_Vehiculos> Asig_Vehiculos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asig_Vehiculos>(entity =>
        {
            entity.HasKey(e => e.id_asig_vehiculo);
        });
        base.OnModelCreating(modelBuilder);
    }
}
