using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3_NET22.DataModels;

public class Category
{
    [BsonId] public ObjectId Id { get; set; }

    [BsonElement] public string CategoryName { get; set; }
}