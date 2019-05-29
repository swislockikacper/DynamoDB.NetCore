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
        private const string accessKey = "AKIA3B2NJPV3EL3H4N53";
        private const string secretKey = "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";
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

            Console.WriteLine("ALL STUDENTS:");

            foreach(var client in clients)
            {
                Console.WriteLine($"Id: {client.Id}, FullName: {client.FullName}, Age: {client.Age}");
            }
        }

        public async Task PrintClientById(string id)
        {
            var client = await context.LoadAsync<Client>(id);

            Console.WriteLine($"STUDENT WITH ID = {id}:");
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

            Console.WriteLine($"USERS WHICH FULLNAMEW CONTAINS {fullName}:");

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

            Console.WriteLine($"USERS WHICH AGE IS BIGGER THAN {age}:");

            foreach(var client in clients)
            {
                Console.WriteLine($"Id: {client.Id}, FullName: {client.FullName}, Age: {client.Age}");
            }
        }

        public async Task CreateOrUpdateClient(Client client) => await context.SaveAsync(client);
    }
}