using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;

public class EditViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizManger _quizManger;
    private readonly QuestionManager _questionManager;
   

    public EditViewModel(QuizManger quizManger, QuestionManager questionManager, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;
        _questionManager = questionManager;


        LoadListView();

        
        RemoveCommand = new RelayCommand(() => RemoveQuestion());
        SaveEditCommand = new RelayCommand(() => SaveEdit());
        GoBackToStartCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _questionManager, _navigationManager));
        ClearFieldsCommand = new RelayCommand(() => ClearFields());
        NewQuestionCommand = new RelayCommand(() => NewQuestion());
        SaveNewCategoryCommand = new RelayCommand(() => CreateNewCategory());


    }

    public ICommand RemoveCommand { get; }

    public ICommand SaveEditCommand { get; }

    public ICommand GoBackToStartCommand { get; }
    public ICommand ClearFieldsCommand { get; }
    public ICommand NewQuestionCommand { get; }
    public ICommand SaveNewCategoryCommand { get; }


    public void NewQuestion()
    {
        Correct();

        var QuizAnswers = new string[] { QuestionAnswerOne, QuestionAnswerTwo, QuestionAnswerThree };

        var newQuestion = new QuestionModel(QuestionStatment, QuizAnswers, QuestionCorrectAnswer){Category = CategoriesForAQuestion};

        _questionManager.MongoDbSaveQuestion(newQuestion);

        LoadListView();
    }

    public void SaveEdit()
    {
        Correct();

        var QuizAnswers = new string[] { QuestionAnswerOne, QuestionAnswerTwo, QuestionAnswerThree };

        var editQuestion = new QuestionModel(QuestionStatment, QuizAnswers, QuestionCorrectAnswer){Category = CategoriesForAQuestion};

        _questionManager.EditQuestion(SelectedQuestion.Id, editQuestion);


        LoadListView();
    }
    public void RemoveQuestion()
    {

        _questionManager.DeleteQuestion(SelectedQuestion.Id);

        QuestionStatment = string.Empty;

        QuestionAnswerOne = string.Empty;
        QuestionAnswerTwo = string.Empty;
        QuestionAnswerThree = string.Empty;

        CorrectAnswerOne = false;
        CorrectAnswerTwo = false;
        CorrectAnswerThree = false;

        LoadListView();
    }


    public void CreateNewCategory()
    {
        var newCategory = new Category() { CategoryName = CategoryName };

        _quizManger.CreateNewCategory(newCategory);

        LoadListView();

        CategoryName = string.Empty;
    }


    public void CheckButtons()
    {
        if (QuestionIndex == null || QuestionIndex < 0)
        {
            CanSaveOrRemove = false;
        }

        else
        {
            CanSaveOrRemove = true;
        }
    }

   
    public void LoadListView()
    {
        AllQuestions = _questionManager.GetAllQuestionsFromMongoDb();

        AllCategories = _quizManger.GetAllCategories();
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

    private QuestionModel _selectedQuestion;

    public QuestionModel SelectedQuestion
    {
        get { return _selectedQuestion; }
        set
        {
            SetProperty(ref _selectedQuestion, value);
            FillInBoxes();

        }
    }

    private Category _selectedCategory;

    public Category SelectedCategory
    {
        get { return _selectedCategory; }
        set
        {
            SetProperty(ref _selectedCategory, value);
            if (SelectedCategory != null)
            {
                if (CategoriesForAQuestion != null)
                {
                    foreach (var question in CategoriesForAQuestion!)
                    {
                        if (question.Id == SelectedCategory.Id)
                        {
                            return;
                        }
                    }
                }

                CategoriesForAQuestion.Add(SelectedCategory);
            }
        }
    }

    private ObservableCollection<Category> _categoriesForAQuestion = new ObservableCollection<Category>();

    public ObservableCollection<Category> CategoriesForAQuestion
    {
        get { return _categoriesForAQuestion; }
        set { SetProperty(ref _categoriesForAQuestion, value); }
    }

    private string _categoryName;

    public string CategoryName
    {
        get { return _categoryName; }
        set { SetProperty(ref _categoryName, value); }
    }

    private IEnumerable<Category> _allCategories;

    public IEnumerable<Category> AllCategories
    {
        get { return _allCategories; }
        set { SetProperty(ref _allCategories, value); }
    }




    private string _quizTitle;

    public string QuizTitle
    {
        get { return _quizTitle; }
        set
        {
            SetProperty(ref _quizTitle, value);
            SetList();
            
            

        }
    }

    public async Task SetList()
    {
        //await _quizManger.DownloadJson(QuizTitle);
       // QuestionList = _quizManger.CurrentQuiz.Questions;
    }


    public void Correct()
    { 
       if (CorrectAnswerOne == true)
       {
           QuestionCorrectAnswer = 0;
       }

       else if (CorrectAnswerTwo == true)
       {
           QuestionCorrectAnswer = 1;
       }

       else if (CorrectAnswerThree == true)
       {
           QuestionCorrectAnswer = 2;
       }
    }


    private IEnumerable<QuestionModel> _questionList;

    public IEnumerable<QuestionModel> QuestionList
    {
        get { return _questionList; }
        set
        {
            SetProperty(ref _questionList, value);
            FillInBoxes();
        }
    }

    public void FillInBoxes()
    {


        if (SelectedQuestion != null)
        {
            QuestionStatment = SelectedQuestion.Statement;

            QuestionAnswerOne = SelectedQuestion.Answers[0];
            QuestionAnswerTwo = SelectedQuestion.Answers[1];
            QuestionAnswerThree = SelectedQuestion.Answers[2];

            QuestionCorrectAnswer = SelectedQuestion.CorrectAnswer;

            CategoriesForAQuestion = new ObservableCollection<Category>(SelectedQuestion.Category);

            if (QuestionCorrectAnswer == 0)
            {
                CorrectAnswerOne = true;
            }

            else if (QuestionCorrectAnswer == 1)
            {
                CorrectAnswerTwo = true;
            }

            else if (QuestionCorrectAnswer == 2)
            {
                CorrectAnswerThree = true;
            }
        }

        


       // CheckButtons();
    }

    public void ClearFields()
    {
        QuestionStatment = String.Empty;

        QuestionAnswerOne = String.Empty;
        QuestionAnswerTwo = String.Empty;
        QuestionAnswerThree = String.Empty;

        CategoriesForAQuestion = new ObservableCollection<Category>();


        CorrectAnswerOne = false;
        CorrectAnswerTwo = false;
        CorrectAnswerThree = false;
    }

    private bool _correctAnswerOne;

    public bool CorrectAnswerOne
    {
        get { return _correctAnswerOne; }
        set
        {
            SetProperty(ref _correctAnswerOne, value);
            if (CorrectAnswerOne.Equals(true))
            {
                CorrectAnswerTwo = false;
                CorrectAnswerThree = false;
            }
        }
    }

    private bool _correctAnswerTwo;

    public bool CorrectAnswerTwo
    {
        get { return _correctAnswerTwo; }
        set
        {
            SetProperty(ref _correctAnswerTwo, value);
            if (CorrectAnswerTwo.Equals(true))
            {
                CorrectAnswerThree = false;
                CorrectAnswerOne = false;
            }
        }
    }

    private bool _correctAnswerThree;

    public bool CorrectAnswerThree
    {
        get { return _correctAnswerThree; }
        set
        {
            SetProperty(ref _correctAnswerThree, value);
            if (CorrectAnswerThree.Equals(true))
            {
                CorrectAnswerTwo = false;
                CorrectAnswerOne = false;
                
            }

        }
    }

    private string _questionStatment;

    public string QuestionStatment
    {
        get { return _questionStatment; }
        set
        {
            SetProperty(ref _questionStatment, value);
        }
    }

    private string _questionAnswerOne = String.Empty;

    public string QuestionAnswerOne
    {
        get { return _questionAnswerOne; }
        set
        {
            SetProperty(ref _questionAnswerOne, value);
            
        }
    }

    private string _questionAnswerTwo = String.Empty;

    public string QuestionAnswerTwo
    {
        get { return _questionAnswerTwo; }
        set
        {
            SetProperty(ref _questionAnswerTwo, value);
            
        }
    }

    private string _questionAnswerThree = String.Empty;

    public string QuestionAnswerThree
    {
        get { return _questionAnswerThree; }
        set
        {
            SetProperty(ref _questionAnswerThree, value);
        }
    }

   

    private int _questionCorrectAnswer;

    public int QuestionCorrectAnswer
    {
        get { return _questionCorrectAnswer; }
        set
        {
            SetProperty(ref _questionCorrectAnswer, value);
        }
    }

    

    private bool _canSaveOrRemove = false;

    public bool CanSaveOrRemove
    {
        get { return _canSaveOrRemove; }
        set { SetProperty(ref _canSaveOrRemove, value); }
    }

  

    private int _questionIndex;

    public int QuestionIndex
    {
        get { return _questionIndex; }
        set
        {
            SetProperty(ref _questionIndex, value);
            FillInBoxes();
        }
    }

    
}