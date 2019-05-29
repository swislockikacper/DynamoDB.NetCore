using System;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDB.NetCore
{
    [DynamoDBTable("Client")]
    class Client : IEquatable<Client>
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string FullName { get; set; }
        public short Age { get; set; }

        public bool Equals(Client other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id && string.Equals(FullName, other.FullName) && string.Equals(FullName, other.FullName);
        }
    }
}