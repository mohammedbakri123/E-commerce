using E_commerce_Endpoints.Data;
using E_commerce_Endpoints.Helper;
using E_commerce_Endpoints.Services.Interfaces;
using E_commerce_Endpoints.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using E_commerce_Endpoints.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

//-------------------------------------------------cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//-------------------------------------------------cors

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<appDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Extract strongly typed settings for configuring JWT
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
var key = Encoding.UTF8.GetBytes(jwtOptions.Secret);

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService , AuthService>();
builder.Services.AddScoped<IUserService , UserService>();
builder.Services.AddScoped<ICategoryService , CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IProductService , ProductService>();
builder.Services.AddScoped<IVariantService , VariantService>();
builder.Services.AddScoped<IOfferService , OfferService>();
builder.Services.AddScoped<IAdminService , AdminService>();
builder.Services.AddScoped<ISupplierService , SupplierService>();
builder.Services.AddScoped<IStockService , StockService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IOrderService , OrderService>();
builder.Services.AddScoped<ICartService , CartService>();
builder.Services.AddScoped<ICartItemService , CartItemService>();





var app = builder.Build();

// ✅ Use CORS before controllers
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
