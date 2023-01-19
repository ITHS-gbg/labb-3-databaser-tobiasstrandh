using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;
using Labb3_NET22.DataModels;
using MongoDB.Driver;

namespace Labb3_NET22.Managers;

public class QuizManger
{
    private QuizModel _quizModel;

    public QuizModel CurrentQuiz
    {
        get => _quizModel;
        set
        {
            _quizModel = value;
            CurrentQuizChanged?.Invoke();
        }
    }

    public event Action CurrentQuizChanged;

    private readonly IMongoCollection<QuizModel> _collectionQuiz;

    public QuizManger()
    {
        var hostname = "localhost";
        var databaseName = "TobiasQuiz";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _collectionQuiz = database.GetCollection<QuizModel>("quiz", new MongoCollectionSettings() { AssignIdOnInsert = true });

    }

    

    public IEnumerable<QuizModel> GetAllQuiz()
    {
        return _collectionQuiz.Find(_ => true).ToEnumerable();
    }

    public void MongoDbSaveQuiz(QuizModel quiz)
    {
        _collectionQuiz.InsertOne(quiz);
    }

    public void AddQuestionToQuiz(QuizModel quiz, QuestionModel questions)
    {
        var filter = Builders<QuizModel>.Filter.Eq("Id", quiz.Id);
        var update = Builders<QuizModel>.Update.AddToSet("Questions", questions);

        _collectionQuiz.UpdateOne(filter, update);
    }

    public void DeleteQuiz(object id)
    {
        var filter = Builders<QuizModel>.Filter.Eq("Id", id);
        _collectionQuiz.FindOneAndDelete(filter);
    }

    public void RemoveSelectedQuestionFromQuiz(object quizId, QuestionModel question)
    {
        var filter = Builders<QuizModel>.Filter.Eq("Id", quizId);
        var update = Builders<QuizModel>.Update.Pull("Questions", question);

        _collectionQuiz.UpdateOne(filter, update);
    }

    public IEnumerable<QuizModel> GetQuizByCategories(Category category)
    {

        var filter = Builders<QuizModel>.Filter.ElemMatch(q => q.Questions, c => c.Category.Contains(category));

        return _collectionQuiz.Find(filter).ToEnumerable();
    }

}