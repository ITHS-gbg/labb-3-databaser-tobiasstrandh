using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    public ICommand DeleteQuizCommand { get; }

    public ICommand RemoveSelectedQuestionCommand { get; }
    public CreateQuizViewModel(QuizManger quizManger, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;

        GetAllQuestionsAndQuiz();

        NewQuizCommand = new RelayCommand(() => NewQuiz());

        DeleteQuizCommand = new RelayCommand(() => DeleteQuiz());
        
        RemoveSelectedQuestionCommand = new RelayCommand(() => RemoveQuestionFromQuiz());
    }

    public void GetAllQuestionsAndQuiz()
    {
        AllQuestions = _quizManger.GetAllQuestionsFromMongoDb();

        AllQuiz = _quizManger.GetAllQuiz();
    }

    public void NewQuiz()
    {
        var newQuiz = new QuizModel(){QuizTitle = QuizName};

        _quizManger.MongoDbSaveQuiz(newQuiz);

        GetAllQuestionsAndQuiz();
    }

    public void DeleteQuiz()
    {
        _quizManger.DeleteQuiz(SelectedQuiz.Id);

        GetAllQuestionsAndQuiz();
    }

    public void AddQuestionToQuiz()
    {
        _quizManger.AddQuestionToQuiz(SelectedQuiz, QuestionToQuiz);

        var quizTitle = SelectedQuiz.QuizTitle;

        GetAllQuestionsAndQuiz();

        SelectedQuiz = AllQuiz.First(q => q.QuizTitle == quizTitle);
    }

    public void RemoveQuestionFromQuiz()
    {
        _quizManger.RemoveSelectedQuestionFromQuiz(SelectedQuiz.Id, SelectedQuestionFromQuiz);

        var quizTitle = SelectedQuiz.QuizTitle;

        GetAllQuestionsAndQuiz();

        SelectedQuiz = AllQuiz.First(q => q.QuizTitle == quizTitle);

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

    private IEnumerable<QuizModel> _allQuiz;

    public IEnumerable<QuizModel> AllQuiz
    {
        get { return _allQuiz; }
        set { SetProperty(ref _allQuiz, value); }
    }

    private QuizModel _selectedQuiz;

    public QuizModel SelectedQuiz
    {
        get { return _selectedQuiz; }
        set
        {
            SetProperty(ref _selectedQuiz, value);

        }
    }

    private QuestionModel _selectedQuestionFromQuiz;

    public QuestionModel SelectedQuestionFromQuiz
    {
        get { return _selectedQuestionFromQuiz; }
        set
        {
            SetProperty(ref _selectedQuestionFromQuiz, value);

        }
    }

    private string _quizName;

    public string QuizName
    {
        get { return _quizName; }
        set
        {
            SetProperty(ref _quizName, value);
            if (QuizName != string.Empty)
            {
                SelectedQuiz = null;
            }
        }
    }

    private QuestionModel? _questionToQuiz;

    public QuestionModel? QuestionToQuiz
    {
        get { return _questionToQuiz; }
        set
        {
            SetProperty(ref _questionToQuiz, value);

            if (QuestionToQuiz != null && SelectedQuiz != null)
            {
                AddQuestionToQuiz();
            }


            //if (QuestionToQuiz != null)
            //{
            //    foreach (var question in QuizQuestions!)
            //    {
            //        if (question.Id == QuestionToQuiz.Id)
            //        {
            //            return;
            //        }
            //    }

            //    QuizQuestions.Add(QuestionToQuiz);
            //}

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