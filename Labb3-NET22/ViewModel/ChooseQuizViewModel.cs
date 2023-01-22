using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;

public class ChooseQuizViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizManger _quizManger;
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;
    public ICommand ReturnToStartViewCommand { get; }
    public ICommand GoToQuizViewCommand { get; }
    public ICommand ResetCategoryCommand { get; }

    public ChooseQuizViewModel(QuizManger quizManger, QuestionManager questionManager, CategoryManager categoryManager ,NavigationManager navigationManager)
    {
        
        
        _navigationManager = navigationManager;
        _quizManger = quizManger;
        _questionManager = questionManager;
        _categoryManager = categoryManager;
        LoadListView();


        ReturnToStartViewCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _questionManager, _categoryManager, _navigationManager));

        GoToQuizViewCommand = new RelayCommand(() => GoToQuizView());

        ResetCategoryCommand = new RelayCommand(() => LoadListView());
    }

    public void GoToQuizView()
    {


        _quizManger.CurrentQuiz = SelectedQuiz;

        var amountQuestions = _quizManger.CurrentQuiz.Questions.ToList();
        if (amountQuestions.Count == 0)
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _questionManager, _categoryManager, _navigationManager);
        }

        else
        {
            _navigationManager.CurrentViewModel = new QuizViewModel(_quizManger, _questionManager, _categoryManager,_navigationManager);
        }

    }

    public void LoadListView()
    {
        AllCategories = _categoryManager.GetAllCategories();

        AllQuiz = _quizManger.GetAllQuiz();

    }



    private IEnumerable<QuizModel> _allQuiz = null!;

    public IEnumerable<QuizModel> AllQuiz
    {
        get { return _allQuiz; }
        set
        {
            SetProperty(ref _allQuiz, value);
        }
    }


    private QuizModel _selectedQuiz = null!;

    public QuizModel SelectedQuiz
    {
        get { return _selectedQuiz; }
        set
        {
            SetProperty(ref _selectedQuiz, value);
            CheckQuizStatus = true;
        }
    }

    private bool _checkQuizStatus = false;

    public bool CheckQuizStatus
    {
        get { return _checkQuizStatus; }
        set
        {
            SetProperty(ref _checkQuizStatus, value);
        }
    }

    private IEnumerable<Category> _allCategories = null!;

    public IEnumerable<Category> AllCategories
    {
        get { return _allCategories; }
        set { SetProperty(ref _allCategories, value); }
    }

    private Category _selectedCategory = null!;

    public Category SelectedCategory
    {
        get { return _selectedCategory; }
        set
        {
            SetProperty(ref _selectedCategory, value);
            GetQuizByCategory();
        }
    }

    public void GetQuizByCategory()
    {
        AllQuiz = _quizManger.GetQuizByCategories(SelectedCategory);


        //var allQuestions = _questionManager.GetAllQuestionsFromMongoDb().ToList();
        //var questions = allQuestions.ToList();
        //questions.Clear();


        //foreach (var question in allQuestions)
        //{
        //    foreach (var cat in question.Category)
        //    {
        //        if (cat.Id == SelectedCategory.Id)
        //        {
        //            if (questions.Contains(question))
        //            {
        //                return;
        //            }

        //            questions.Add(question);
        //        }
        //    }
        //}



        //ObservableCollection<QuizModel> QuizWithSelectedCategory = new ObservableCollection<QuizModel>();

        //AllQuiz = _quizManger.GetAllQuiz();

        //foreach (var quiz in AllQuiz)
        //{
        //    foreach (var question in quiz.Questions)
        //    {
        //        foreach (var q in questions)
        //        {
        //            if (q.Id == question.Id )
        //            {
        //                if (!QuizWithSelectedCategory.Contains(quiz))
        //                {
        //                    QuizWithSelectedCategory!.Add(quiz);
        //                }

        //            }
        //        }
        //    }
        //}

    }

}