using HotChocolate.AspNetCore;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();


//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL("/graphql")
    .WithOptions(
    new GraphQLServerOptions
    {
        EnableGetRequests = true,
        AllowedGetOperations = AllowedGetOperations.All,
        EnableSchemaRequests = true, // Enables schema download at /graphql/schema, Schema exposure might reveal internal implementation details
        Tool =
        {
            Enable = true, // activate Banana Cake Pop
            ServeMode = GraphQLToolServeMode.Embedded // Serves from own server, always available with own API
        }
    });

app.Run();

