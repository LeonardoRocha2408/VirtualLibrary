using LibraryApi;
using VirtualLibrary.Web.DTOs;
using LibraryApi.Methods;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", 
    policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

LibraryAccount? User = null;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowFrontEnd");
app.UseAuthorization();
app.MapControllers();

app.MapStaticAssets();

app.MapPost("/users", ([FromBody] CreateAccountRequest request) =>
{
    User = new LibraryAccount(request.Username, request.Password);
    bool accountExist = Services.CreateAccount(User);
    if (accountExist == true)
    {
        return Results.Conflict(new
        {
            message = "Conta já existe"
        });
    }
    else
    {
        return Results.Created("", new
        {
            message = "Conta criada com sucesso"
        });
    }
});

//LOGIN
app.MapPost("/login", ([FromBody] LoginRequest request) =>
{
    User = new LibraryAccount(request.Username, request.Password);
    var userLogin = Services.Login(User);
    if (userLogin == null)
    {
        return Results.Unauthorized();
    }
    return Results.Ok("Logado com sucesso");
});

app.MapPatch("/users/changed-password", ([FromBody] ChangePasswordRequest request) =>
{
    bool TrueOrFalse = Services.ChangePassword(request.Username, request.Password, request.NewPassword);
    if (TrueOrFalse == false)
    {
        return Results.BadRequest("Dados incorretos");
    }
    else
    {
        User!.ChangePasswordInMemory(request.NewPassword);
        return Results.Ok("Alterado com sucesso");
    }
});

//ADICIONA GÊNERO FAVORITO
app.MapPut("/users/favorite-categories", ([FromBody] AddFavoriteGender request) =>
{
    var account = Services.Login(User);

    if (account == null)
    {
        return Results.BadRequest("Erro");
    }
    Console.WriteLine("Entrou na rota");
    Services.AddFavoriteGender(account, request.FavoriteCategories);

    return Results.Accepted("", new
    {
        message = "Categorias adicionadas"
    });
});
//DELETA CONTA
app.MapPost("/users/delete-account", (DeleteRequest request) =>
{
    Services.DeleteAccount(
        request.Username, 
        request.Password
        );
    return Results.Accepted("Conta deletada");
});

//PESQUISA LIVROS
app.MapGet("home/search-books", (string bookName) =>
{
    Console.Write("Entrou na rota");
    var books = BookServices.SearchBook(bookName);
    if (books?.Count == 0 || books == null)
    {
        return Results.Ok(new List<string>());
    }
    return Results.Ok(books);
});
app.Run();
