using MedReminder.API.Database;
using MedReminder.API.Enterprise;

namespace MedReminder.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add connections to db
            builder.Services.AddScoped<SymptomRepoSqliteContext>(_ => new SymptomRepoSqliteContext("Data Source=medreminder.db"));
            builder.Services.AddScoped<StageRepoSqliteContext>(_ => new StageRepoSqliteContext("Data Source=medreminder.db"));
            builder.Services.AddScoped<MedicationRepoSqliteContext>(_ => new MedicationRepoSqliteContext("Data Source=medreminder.db"));


            // Add connection to enterprise 
            builder.Services.AddScoped<SymptomEC>();
            builder.Services.AddScoped<MedicationEC>();


            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Initialize both database contexts to ensure all tables are created
            using (var scope = app.Services.CreateScope())
            {
                var symptomRepo = scope.ServiceProvider.GetRequiredService<SymptomRepoSqliteContext>();
                var stageRepo = scope.ServiceProvider.GetRequiredService<StageRepoSqliteContext>();
                var medRepo = scope.ServiceProvider.GetRequiredService<MedicationRepoSqliteContext>();
            }

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
        }
    }
}
