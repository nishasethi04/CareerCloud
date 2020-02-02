using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using CareerCloud.Pocos;

class CareerCloudContext : DbContext
{
    public DbSet<ApplicantEducationPoco> ApplicantEducations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        config.AddJsonFile(path, false);
        var root = config.Build();
       string  _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        optionsBuilder.UseSqlServer("_connStr");
        base.OnConfiguring(optionsBuilder);
    }



}