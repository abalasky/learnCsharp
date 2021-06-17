using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Settings;

namespace Play.Common.MongoDB
{
    /*
     *Class to simplify mongo setup
     */
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


            //Construct mongodb client
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();


                // var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

                // var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

                var ConnectionString = "mongodb://localhost:27017";

                var mongoClient = new MongoClient(ConnectionString);

                var ServiceName = "Catalog";

                return mongoClient.GetDatabase(ServiceName);
            });

            return services;

        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {

            services.AddSingleton<IRepository<T>>(
        serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(database, collectionName);
        });

            return services;

        }
    }


}
