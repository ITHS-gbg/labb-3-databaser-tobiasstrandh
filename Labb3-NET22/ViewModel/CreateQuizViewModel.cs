using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;

public class CreateQuizViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizManger _quizManger;

    public ICommand NewQuizCommand { get; }
    public CreateQuizViewModel(QuizManger quizManger, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;

        GetAllQuestions();

        NewQuizCommand = new RelayCommand(() => NewQuiz());
    }

    public void GetAllQuestions()
    {
        AllQuestions = _quizManger.GetAllQuestionsFromMongoDb();
    }

    public void NewQuiz()
    {
        var newQuiz = new QuizModel(){Questions = QuizQuestions, QuizTitle = QuizName};

        _quizManger.MongoDbSaveQuiz(newQuiz);
    }

    private IEnumerable<QuestionModel> _allQuestions;

    public IEnumerable<QuestionModel> AllQuestions
    {
        get { return _allQuestions; }
        set
        {
            SetProperty(ref _allQuestions, value);
        }
    }

    private string _quizName;

    public string QuizName
    {
        get { return _quizName; }
        set { SetProperty(ref _quizName, value); }
    }

    private QuestionModel? _questionToNewQuiz;

    public QuestionModel? QuestionToNewQuiz
    {
        get { return _questionToNewQuiz; }
        set
        {
            SetProperty(ref _questionToNewQuiz, value);


            if (QuestionToNewQuiz != null)
            {
                foreach (var author in QuizQuestions!)
                {
                    if (author.Id == QuestionToNewQuiz.Id)
                    {
                        return;
                    }
                }

                QuizQuestions.Add(QuestionToNewQuiz);
            }

        }
    }

    private ObservableCollection<QuestionModel>? _quizQuestions = new ObservableCollection<QuestionModel>();

    public ObservableCollection<QuestionModel>? QuizQuestions
    {
        get { return _quizQuestions; }
        set
        {
            SetProperty(ref _quizQuestions, value);
        }
    }
}