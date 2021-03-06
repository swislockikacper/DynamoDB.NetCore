using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon;
using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Amazon.DynamoDBv2.DocumentModel;

namespace DynamoDB.NetCore
{
    class DynamoDBService
    {
        private const string accessKey = "PLACEHOLDER";
        private const string secretKey = "PLACEHOLDER";
        private readonly AmazonDynamoDBClient client;
        private readonly DynamoDBContext context;

        public DynamoDBService()
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
            context = new DynamoDBContext(client);
        }

        public async Task PrintClients()
        {
            var clients = await context.ScanAsync<Client>(new List<ScanCondition>()).GetRemainingAsync();

            Console.WriteLine("ALL CLIENTS:");

            foreach(var client in clients)
            {
                Console.WriteLine($"Id: {client.Id}, FullName: {client.FullName}, Age: {client.Age}");
            }
        }

        public async Task PrintClientById(string id)
        {
            var client = await context.LoadAsync<Client>(id);

            Console.WriteLine($"CLIENT WITH ID = {id}:");
            Console.WriteLine($"FullName: {client.FullName}, Age: {client.Age}");
        }

        public async Task PrintClientsByFullName(string fullName)
        {
            var query = context.ScanAsync<Client>
            (
                new[]
                {
                    new ScanCondition
                    (
                        nameof(Client.FullName),
                        ScanOperator.Contains,
                        fullName
                    )
                }
            );

            var result = await query.GetRemainingAsync();
            var clients = result.ToArray();

            Console.WriteLine($"CLIENTS WHICH FULLNAME CONTAINS {fullName}:");

            foreach(var client in clients)
            {
                Console.WriteLine($"Id: {client.Id}, FullName: {client.FullName}, Age: {client.Age}");
            }
        }

        public async Task PrintUsersWhichAgeIsBiggerThan(short age)
        {
             var query = context.ScanAsync<Client>
            (
                new[]
                {
                    new ScanCondition
                    (
                        nameof(Client.Age),
                        ScanOperator.GreaterThan,
                        age
                    )
                }
            );

             var result = await query.GetRemainingAsync();
            var clients = result.ToArray();

            Console.WriteLine($"CLIENTS WHICH AGE IS BIGGER THAN {age}:");

            foreach(var client in clients)
            {
                Console.WriteLine($"Id: {client.Id}, FullName: {client.FullName}, Age: {client.Age}");
            }
        }

        public async Task CreateOrUpdateClient(Client client) => await context.SaveAsync(client);
    }
}