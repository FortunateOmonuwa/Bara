using Microsoft.EntityFrameworkCore;
using ScriptModule.Models;
using Shared.Models;
using TransactionModule.Models;
using UserModule.Models;
using Document = UserModule.Models.Document;

namespace Infrastructure.DataContext
{
    public class BaraContext : DbContext
    {
        public BaraContext(DbContextOptions<BaraContext> options) : base(options)
        {
            Scripts = Set<Script>();
            Transactions = Set<Transaction>();
            Wallets = Set<Wallet>();
            Producers = Set<Producer>();
            Writers = Set<Writer>();
            AuthProfiles = Set<AuthProfile>();
            Services = Set<Service>();
            Documents = Set<Document>();
            Addresses = Set<Address>();
            //ScriptWritingPosts = Set<ScriptWritingPostByProducer>();
            EscrowOperations = Set<Escrow>();
            //Applicants = Set<ScriptWritingPostApplicant>();
        }

        public DbSet<Script> Scripts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<AuthProfile> AuthProfiles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<ScriptWritingPostByProducer> ScriptWritingPosts { get; set; }
        //public DbSet<ScriptWritingPostApplicant> Applicants { get; set; }
        public DbSet<Escrow> EscrowOperations { get; set; }
        public DbSet<BlackListedUser> BlackListedUsers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Script).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Transaction).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(User).Assembly);
        }
    }
}
