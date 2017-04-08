using System;
using Farfetch.Data.MongoDb.Helpers;
using Farfetch.Domain.Entities.Corporate;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Farfetch.Services.Web.Api.Tests.Base
{
    public class CollectionFixture : IDisposable
    {
        #region Fields | Members
        private IConfigurationRoot configuration;

        private string databaseName;

        private IMongoDatabase database;

        private MongoClient client;
        #endregion

        #region Constructors | Destructors
        public CollectionFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.testing.json", optional: true);

            configuration = builder.Build();
            var connectionString = configuration.GetSection("Data:MongoDb:ConnectionString").Value;

            ClassMapHelper.RegisterConventionPacks();

            databaseName = configuration.GetSection("Data:MongoDb:Database").Value;
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);

            InitializeDatabase();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            client.DropDatabase(databaseName);
            client = null;
        }
        #endregion

        #region Private methods
        private void InitializeDatabase()
        {
            InitializeEmployees();
        }

        private void InitializeEmployees()
        {
            ClassMapHelper.SetupClassMap<Employee, Guid>();
            var collection = database.GetCollection<Employee>("employees");

            collection.InsertOneAsync(new Employee { Id = Guid.NewGuid(), FirstName = "John", LastName = "Smith Doe", Email = "john@domain.com" }, null);
            collection.InsertOneAsync(new Employee { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith Doe", Email = "jane@domain.com" }, null);
            collection.InsertOneAsync(new Employee { Id = Guid.NewGuid(), FirstName = "Josh", LastName = "Smith Doe", Email = "conflict@domain.com" }, null);
            collection.InsertOneAsync(new Employee { Id = Guid.NewGuid(), FirstName = "Alex", LastName = "Smith Doe", Email = "alex@domain.com" }, null);
            collection.InsertOneAsync(new Employee { Id = Guid.NewGuid(), FirstName = "Johnny", LastName = "Smith Doe", Email = "johnny@domain.com" }, null);
            collection.InsertOneAsync(new Employee { Id = Guid.NewGuid(), FirstName = "Jude", LastName = "Smith Doe", Email = "jude@domain.com" }, null);
        }
        #endregion
    }
}
