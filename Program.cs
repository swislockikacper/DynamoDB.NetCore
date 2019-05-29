using System;

namespace DynamoDB.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new DynamoDBService();
            service.PrintClients().Wait();
            service.PrintClientById("PLACEHOLDER").Wait();
            service.PrintClientsByFullName("PLACEHOLDER").Wait();
            service.PrintUsersWhichAgeIsBiggerThan(10).Wait();

            service.CreateOrUpdateClient(new Client
            {
                Id = "100",
                FullName = "Test User",
                Age = 8
            })
            .Wait();
        }
    }
}
