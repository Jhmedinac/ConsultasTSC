
using ConsultasTSC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var databaseType = builder.Configuration["DatabaseType"];
if (databaseType == "SqlServer")
{
    var catalog1ConnectionString = builder.Configuration.GetConnectionString("Catalog1");
    var catalog2ConnectionString = builder.Configuration.GetConnectionString("Catalog2");

    builder.Services.AddDbContext<UserContext>(options =>
                   options.UseSqlServer(catalog1ConnectionString));

    builder.Services.AddDbContext<CervezaContext>(options =>
                   options.UseSqlServer(catalog1ConnectionString));
}

//    builder.Services.AddDbContext<UserContext>(option =>
//option.UseSqlServer(builder.Configuration.GetConnectionString("catalog1ConnectionString")));


// Register DbContexts for multiple databases


//builder.Services.AddDbContext<AnotherDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("AnotherConnection")));

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

app.Run();
//using ConsultasTSC.Data;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//        // Add services to the container.
//        builder.Services.AddControllers();

//        // Configure the database connection based on the DatabaseType parameter
//        var databaseType = builder.Configuration["DatabaseType"];
//        if (databaseType == "SqlServer")
//        {
//            var catalog1ConnectionString = builder.Configuration.GetConnectionString("Catalog1");
//            //var catalog2ConnectionString = builder.Configuration.GetConnectionString("Catalog2");

//            builder.Services.AddDbContext<UserContext>(options =>
//                options.UseSqlServer(catalog1ConnectionString));

//            //builder.Services.AddDbContext<CervezaContext>(options =>
//            //    options.UseSqlServer(catalog2ConnectionString));
//        }


//        var app = builder.Build();

//        // Configure the HTTP request pipeline.
//        if (app.Environment.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//            app.UseSwagger();
//            app.UseSwaggerUI();
//        }

//        app.UseHttpsRedirection();
//        app.UseRouting();
//        app.UseAuthorization();

//        app.MapControllers();

//        app.Run();
