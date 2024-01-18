using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL.Model;
using DAL.Connections; // Update with your actual models namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ConfigurationProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("/", () => "Hello World!");

var ob = app.Services.GetRequiredService<ConfigurationProvider>();
string cs = ob.GetConnectionString();

var entities = new List<Person>();
using (var connection = new SqlConnection(cs))
{
    connection.Open();

    using (var command = new SqlCommand($"SELECT * FROM Person", connection))
    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            var entity = Maper<Person>.MapDataReaderToEntity(reader);
            entities.Add(entity);
        }
    }
}

foreach (var item in entities)
{
    Console.WriteLine(item.name); // Assuming the property is named "Name"
}

app.Run();
