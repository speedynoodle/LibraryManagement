// See https://aka.ms/new-console-template for more information

using LibraryManagement.Console;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Data.Context;
using LibraryManagement.Data.Repositories;
using LibraryManagement.Services;
using LibraryManagement.Services.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddDbContext<LibraryDbContext>(options => 
        options.UseInMemoryDatabase("LibraryDatabase"))
    .AddScoped<IBookRepository, BookRepository>()
    .AddScoped<IValidator<Book>, BookValidator>()
    .AddScoped<IBookService, BookService>()
    .BuildServiceProvider();

var bookService = serviceProvider.GetRequiredService<IBookService>();

var libraryUI = new LibraryConsoleUI(bookService);
await libraryUI.Run();