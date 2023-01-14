﻿using System;
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

    private readonly IMongoCollection<QuestionModel> _collectionQuestion;

    public QuizManger()
    {
        var hostname = "localhost";
        var databaseName = "TobiasQuiz";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _collectionQuiz = database.GetCollection<QuizModel>("quiz", new MongoCollectionSettings() { AssignIdOnInsert = true });

        _collectionQuestion = database.GetCollection<QuestionModel>("question", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public async Task MongoDbSaveQuestion(QuestionModel question)
    {

       _collectionQuestion.InsertOne(question);

       // CurrentQuizChanged?.Invoke();

    }

    public IEnumerable<QuestionModel> GetAllQuestionsFromMongoDb()
    {
       return _collectionQuestion.Find(_ => true).ToEnumerable();
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

    public void DeleteQuestion(object id)
    {
        var filter = Builders<QuestionModel>.Filter.Eq("Id", id);
        _collectionQuestion.FindOneAndDelete(filter);
    }

    public void EditQuestion(object id, QuestionModel question)
    {
        var filter = Builders<QuestionModel>.Filter.Eq("Id", id);
        var update = Builders<QuestionModel>.Update.Set("Statement", question.Statement)
                                                   .Set("Answers", question.Answers)
                                                   .Set("CorrectAnswer", question.CorrectAnswer);

        _collectionQuestion.UpdateOne(filter, update);
    }

    public IEnumerable<QuizModel> PlayQuiz(object id)
    {
        var filter = Builders<QuizModel>.Filter.Eq("Id", id);

        return _collectionQuiz.Find(filter).ToEnumerable();
    }



    public async Task JsonDefaultQuizSave()
    {
        var folderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TobiasQuizApp");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var pathDefaultQuiz = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"TobiasQuizApp", $"tobbesquiz.json");

        if (!File.Exists(pathDefaultQuiz))
        {

            var json = JsonSerializer.Serialize(CurrentQuiz, new JsonSerializerOptions() { WriteIndented = true });


            await using StreamWriter sw = new StreamWriter(pathDefaultQuiz);


            sw.WriteLine(json);

        }


    }

    public async Task<List<string>> JsonTitleList()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"TobiasQuizApp"); 
        string[] file = Directory.GetFiles(path, "*.json");

        await Task.Delay(100);

        for (int i = 0; i < file.Length; i++)
        {
            file[i] = Path.GetFileNameWithoutExtension(file[i]);
        }

        return new List<string>(file);

    }

    public async Task DownloadJson(string title)
    {

        await Task.Run(() =>
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TobiasQuizApp", $"{title}.json");


            if (File.Exists(path))
            {
                var text = string.Empty;
                string? line = string.Empty;


                using StreamReader sr = new StreamReader(path);

                while ((line = sr.ReadLine()) != null)
                {
                    text += line;
                }

                CurrentQuiz = JsonSerializer.Deserialize<QuizModel>(text);



            }
        });


    }


    

}