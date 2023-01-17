using System.Collections.Generic;
using System.Threading.Tasks;
using Labb3_NET22.DataModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3_NET22.Managers;

public class QuestionManager
{
    private readonly IMongoCollection<QuestionModel> _collectionQuestion;

    public QuestionManager()
    {
        var hostname = "localhost";
        var databaseName = "TobiasQuiz";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _collectionQuestion = database.GetCollection<QuestionModel>("question", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public IEnumerable<QuestionModel> GetAllQuestionsFromMongoDb()
    {
        return _collectionQuestion.Find(_ => true).ToEnumerable();
    }

    public void MongoDbSaveQuestion(QuestionModel question)
    {

        _collectionQuestion.InsertOne(question);

    }

    public void DeleteQuestion(object id)
    {
        var filter = Builders<QuestionModel>.Filter.Eq("Id", id);
        _collectionQuestion.FindOneAndDelete(filter);
    }

    public void EditQuestion(object id, QuestionModel question)
    {
        var filter = Builders<QuestionModel>.Filter.Eq("Id", id);
        var update = Builders<QuestionModel>.Update
            .Set("Statement", question.Statement)
            .Set("Answers", question.Answers)
            .Set("CorrectAnswer", question.CorrectAnswer)
            .Set("Category", question.Category);

        _collectionQuestion.UpdateOne(filter, update);
    }

    public IEnumerable<QuestionModel> GetQuestionsByCategories(object id)
    {
        var filter = Builders<QuestionModel>.Filter.Eq("Category", id);

        return _collectionQuestion.Find(filter).ToEnumerable();
    }

    public IEnumerable<QuestionModel> GetQuestionsByName(string name)
    {
        return _collectionQuestion.Find(x => x.Statement.Contains(name)).ToEnumerable();
    }

}