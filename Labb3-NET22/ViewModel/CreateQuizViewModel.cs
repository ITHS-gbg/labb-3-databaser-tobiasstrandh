using System;
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
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;

    public ICommand NewQuizCommand { get; }
    public ICommand DeleteQuizCommand { get; }

    public ICommand RemoveSelectedQuestionCommand { get; }
    public ICommand ResetCategoryCommand { get; }
    public ICommand BackToStartCommand { get; }
    public ICommand SearchByWordCommand { get; }
    public CreateQuizViewModel(QuizManger quizManger, QuestionManager questionManager, CategoryManager categoryManager, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;
        _questionManager = questionManager;
        _categoryManager = categoryManager;

        GetAllQuestionsAndQuiz();

        NewQuizCommand = new RelayCommand(() => NewQuiz());

        DeleteQuizCommand = new RelayCommand(() => DeleteQuiz());
        
        RemoveSelectedQuestionCommand = new RelayCommand(() => RemoveQuestionFromQuiz());

        ResetCategoryCommand = new RelayCommand(() => ResetCategory());

        SearchByWordCommand = new RelayCommand(() => GetQuestionsByName());

        BackToStartCommand  = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _questionManager, _categoryManager, _navigationManager));
    }

    public void GetAllQuestionsAndQuiz()
    {
        AllQuestions = _questionManager.GetAllQuestionsFromMongoDb();

        AllQuiz = _quizManger.GetAllQuiz();

        AllCategories = _categoryManager.GetAllCategories();

    }

    public void NewQuiz()
    {
        if (QuizName != String.Empty)
        {
            var newQuiz = new QuizModel() { QuizTitle = QuizName};

            _quizManger.MongoDbSaveQuiz(newQuiz);

            GetAllQuestionsAndQuiz();

            QuizName = String.Empty;
        }
    }

    public void DeleteQuiz()
    {
        if (SelectedQuiz == null)
        {
            return;
        }

        _quizManger.DeleteQuiz(SelectedQuiz.Id);

        GetAllQuestionsAndQuiz();
    }

    public void AddQuestionToQuiz()
    {
        if (SelectedQuiz is null || QuestionToQuiz is null)
        {
            return;
        }

        _quizManger.AddQuestionToQuiz(SelectedQuiz, QuestionToQuiz);

        var quizTitle = SelectedQuiz.QuizTitle;

        AllQuiz = _quizManger.GetAllQuiz();

        SelectedQuiz = AllQuiz.First(q => q.QuizTitle == quizTitle);

        if (Category is not null)
        {
            AllQuestions = _questionManager.GetQuestionsByCategories(Category);
        }
    }

    public void RemoveQuestionFromQuiz()
    {
        if (SelectedQuiz is null || SelectedQuestionFromQuiz is null)
        {
            return;
        }

        _quizManger.RemoveSelectedQuestionFromQuiz(SelectedQuiz.Id, SelectedQuestionFromQuiz);

        var quizTitle = SelectedQuiz.QuizTitle;

        GetAllQuestionsAndQuiz();

        SelectedQuiz = AllQuiz.First(q => q.QuizTitle == quizTitle);

    }

    public void GetQuestionsByName()
    {
        AllQuestions = _questionManager.GetQuestionsByName(SearchName);

        SearchName = String.Empty;
    }

    private string _searchName = String.Empty;

    public string SearchName
    {
        get { return _searchName; }
        set { SetProperty(ref _searchName, value); }
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

    private IEnumerable<Category> _allCategories;

    public IEnumerable<Category> AllCategories
    {
        get { return _allCategories; }
        set { SetProperty(ref _allCategories, value); }
    }

    private Category _selectedCategory;

    public Category SelectedCategory
    {
        get { return _selectedCategory; }
        set
        {
            SetProperty(ref _selectedCategory, value);
            SerachForQuestionsByCategory();
            Category = SelectedCategory;
        }
    }

    public void SerachForQuestionsByCategory()
    {
        AllQuestions = _questionManager.GetQuestionsByCategories(SelectedCategory);
    }

    public void ResetCategory()
    {
        Category = null!;
        SelectedCategory = null!;

        GetAllQuestionsAndQuiz();
    }

    private Category _category;

    public Category Category
    {
        get { return _category; }
        set { SetProperty(ref _category, value); }
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
                SelectedQuiz = null!;
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

            if (QuestionToQuiz is not null && SelectedQuiz is not null)
            {

                if (!SelectedQuiz.Questions.Any())
                {
                    AddQuestionToQuiz();
                    return;
                }

                foreach (var question in SelectedQuiz.Questions!)
                {

                        if (question.Id == QuestionToQuiz.Id)
                        {
                            return;
                        }
                }

                AddQuestionToQuiz();
                
            }


            

        }
    }

    
}