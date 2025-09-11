using MovieRental.Customer;
using MovieRental.Data;
using MovieRental.Movie;
using MovieRental.PaymentProviders;
using MovieRental.Rental;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<MovieRentalDbContext>();


/* FIX builder.Services.AddSingleton<IRentalFeatures, RentalFeatures>();
 * 
 * Dependency lifetime mismatch
 * 
 * Singleton(Scoped) is a lifetime mismatch as singleton live forever and scoped (in WEB) is aligned with the request context.
 * Specially with DBContext which is registered as Scoped by default, memmory leaks and other issues may occur.  
 * 
 * Change the IRentalFeatures to Scoped, or if it MUST be a Singleton, use IDbContextFactory.
*/
builder.Services.AddScoped<IRentalFeatures, RentalFeatures>();
builder.Services.AddScoped<IMovieFeatures, MovieFeatures>();
builder.Services.AddScoped<ICustomerFeatures, CustomerFeatures>();
builder.Services.AddScoped<PaymentProcessor>(sp => {
    var processor = new PaymentProcessor();
    processor
        .AddPaymentProvider("PayPal", new PayPalProvider())
        .AddPaymentProvider("MBWay", new MbWayProvider());
    return processor;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var client = new MovieRentalDbContext())
{
    client.Database.EnsureCreated();
}

app.Run();
