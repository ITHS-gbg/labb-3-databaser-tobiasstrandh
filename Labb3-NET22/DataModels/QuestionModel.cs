using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3_NET22.DataModels;

public class QuestionModel
{
    [BsonId] public ObjectId Id { get; set; }

    [BsonElement]
    public string Statement { get; set; }

    [BsonElement]
    public string[] Answers { get; set; }

    [BsonElement]
    public int CorrectAnswer { get; set; }

    [BsonElement]
    public string Category { get; set; } = String.Empty;


    public QuestionModel(string statement, string[] answers, int correctAnswer)
    {
        Statement = statement;
        Answers = answers;
        CorrectAnswer = correctAnswer;

    }


}

