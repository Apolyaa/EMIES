using Server.Mapper;
using Server.Model;
using Server.Repositories;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDictionaryOfCharacteristicService, DictionaryOfCharacteristicService>();
builder.Services.AddScoped<ITypeOfDeviceService, TypeOfDeviceService>();
builder.Services.AddScoped<IUnitOfMeasurementService, UnitOfMeasurementService>();
builder.Services.AddScoped<ITypeCharacteristicService, TypeCharacteristicService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICharacteristicRepository, CharacteristicRepository>();
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddTransient<IDictionaryOfCharacteristicRepository, DictionaryOfCharacteristicRepository>();
builder.Services.AddTransient<IProducerRepository, ProducerRepository>();
builder.Services.AddTransient<IResultDeviceRepository, ResultDeviceRepository>();
builder.Services.AddTransient<IResultRepository, ResultRepository>();
builder.Services.AddTransient<ISourceRepository, SourceRepository>();
builder.Services.AddTransient<ITypeOfDeviceRepository, TypeOfDeviceRepository>();
builder.Services.AddTransient<IUnitOfMeasurementRepository, UnitOfMeasurementRepository>();
builder.Services.AddTransient<ITypeCharacteristicRepository, TypeCharacteristicRepository>();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(UserProfile));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.MapBlazorHub();

app.Run();
