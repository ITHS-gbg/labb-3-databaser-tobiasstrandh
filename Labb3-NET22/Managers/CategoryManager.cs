using System.Collections.Generic;
using Labb3_NET22.DataModels;
using MongoDB.Driver;

namespace Labb3_NET22.Managers;

public class CategoryManager
{
    private readonly IMongoCollection<Category> _collectionQuestionCategory;
    public CategoryManager()
    {
        var hostname = "localhost";
        var databaseName = "TobiasQuiz";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _collectionQuestionCategory = database.GetCollection<Category>("questionCategory", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public void CreateNewCategory(Category questionCategory)
    {
        _collectionQuestionCategory.InsertOne(questionCategory);
    }

    public void RemoveCategory(Category questionCategory)
    {
        var filter = Builders<Category>.Filter.Eq("Id", questionCategory.Id);
        _collectionQuestionCategory.FindOneAndDelete(filter);
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _collectionQuestionCategory.Find(_ => true).ToEnumerable();
    }

}