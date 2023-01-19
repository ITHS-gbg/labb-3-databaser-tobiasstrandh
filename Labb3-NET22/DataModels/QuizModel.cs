using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3_NET22.DataModels;

public class QuizModel
{
    [BsonId] public ObjectId Id { get; set; }

    [BsonElement] public string QuizTitle { get; set; }


    [BsonElement] public IEnumerable<QuestionModel> Questions { get; set; }

    
    public QuizModel()
    {
        Questions = new List<QuestionModel>();

    }

    private List<int> NumberList { get; set; } = new List<int>();
    private QuestionModel Question { get; set; } = null!;

    public QuestionModel GetRandomQuestion()
    {
        var random = new Random();
        var randNext = random.Next(0, Questions.Count());
       

        if (!NumberList.Contains(randNext))
        {
            Question = Questions.ElementAt(randNext);
            NumberList.Add(randNext);
        }

        else
        {
            GetRandomQuestion();
        }

        return Question;
    }


}