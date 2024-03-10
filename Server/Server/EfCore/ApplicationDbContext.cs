using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;

namespace Server.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ResultEntity> Results { get; set; }
        public DbSet<ResultDeviceEntity> ResultsDevices { get; set; }
        public DbSet<ProducerEntity> Producers { get; set; }
        public DbSet<SourceEntity> Sources { get; set; }
        public DbSet<TypeOfDevicesEntity> TypesOfDevices { get; set; }
        public DbSet<SynonymEntity> Synonyms { get; set; }
        public DbSet<UnitOfMeasurementEntity> UnitOfMeasurements { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<CharacteristicEntity> Characteristics { get; set; }
        public DbSet<DictionaryOfCharacteristicEntity> DictionaryOfCharacteristics { get; set; }
        public DbSet<TypeCharacteristicEntity> TypesCharacteristics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=emies;Username=postgres;Password=1123");
        }
    }
}
