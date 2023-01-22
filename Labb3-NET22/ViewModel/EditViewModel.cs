using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    private readonly CategoryManager _categoryManager;


    public EditViewModel(QuizManger quizManger, QuestionManager questionManager, CategoryManager categoryManager, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;
        _questionManager = questionManager;
        _categoryManager = categoryManager;

        LoadListView();

        
        RemoveCommand = new RelayCommand(() => RemoveQuestion());
        SaveEditCommand = new RelayCommand(() => SaveEdit());
        GoBackToStartCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _questionManager, _categoryManager,_navigationManager));
        ClearFieldsCommand = new RelayCommand(() => ClearFields());
        NewQuestionCommand = new RelayCommand(() => NewQuestion());
        SaveNewCategoryCommand = new RelayCommand(() => CreateNewCategory());
        RemoveCategoryCommand = new RelayCommand(() => RemoveCategoryFromDatabase());
        RemoveCategoryFromQuestionCommand = new  RelayCommand(() => RemoveCategoryFromQuestion());

    }

    public ICommand RemoveCommand { get; }

    public ICommand SaveEditCommand { get; }

    public ICommand GoBackToStartCommand { get; }
    public ICommand ClearFieldsCommand { get; }
    public ICommand NewQuestionCommand { get; }
    public ICommand SaveNewCategoryCommand { get; }
    public ICommand RemoveCategoryCommand { get; }
    public ICommand RemoveCategoryFromQuestionCommand { get; }


    public void NewQuestion()
    {
        
        

        Correct();

        var quizAnswers = new string[] { QuestionAnswerOne, QuestionAnswerTwo, QuestionAnswerThree };

        var newQuestion = new QuestionModel(QuestionStatment, quizAnswers, QuestionCorrectAnswer){Category = CategoriesForAQuestion};

        _questionManager.MongoDbSaveQuestion(newQuestion);

        ClearFields();

        LoadListView();
    }

    public void SaveEdit()
    {
        Correct();

        var QuizAnswers = new string[] { QuestionAnswerOne, QuestionAnswerTwo, QuestionAnswerThree };

        var editQuestion = new QuestionModel(QuestionStatment, QuizAnswers, QuestionCorrectAnswer){Category = CategoriesForAQuestion};

        _questionManager.EditQuestion(SelectedQuestion.Id, editQuestion);

        ClearFields();

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

        CanSaveOrRemove = false;

        LoadListView();

        ClearFields();
    }


    public void CreateNewCategory()
    {
        var newCategory = new Category() { CategoryName = CategoryName };

        _categoryManager.CreateNewCategory(newCategory);

        LoadListView();

        CategoryName = string.Empty;

        CanSaveNewCategory = false;
    }


    public void RemoveCategoryFromDatabase()
    {
        if (SelectedCategory is not null)
        {
             _categoryManager.RemoveCategory(SelectedCategory);
             ClearFields();
        }
    }

    public void RemoveCategoryFromQuestion()
    {
        if (SelectedCategoryForAQuestion is not null)
        {
            CategoriesForAQuestion.Remove(SelectedCategoryForAQuestion);
            SaveEdit();
        }
    }

    public void CheckButtons()
    {
        if (SelectedQuestion is null)
        {
            CanSaveOrRemove = false;
        }

        else
        {
            CanSaveOrRemove = true;
            CanSaveNewQuestion = false;
        }
    }

   
    public void LoadListView()
    {
        AllQuestions = _questionManager.GetAllQuestionsFromMongoDb();

        AllCategories = new ObservableCollection<Category>(_categoryManager.GetAllCategories());

        CategoriesForAQuestion = new ObservableCollection<Category>();
    }

    private IEnumerable<QuestionModel> _allQuestions = null!;

    public IEnumerable<QuestionModel> AllQuestions
    {
        get { return _allQuestions; }
        set
        {
            SetProperty(ref _allQuestions, value);
        }
    }

    private QuestionModel _selectedQuestion = null!;

    public QuestionModel SelectedQuestion
    {
        get { return _selectedQuestion; }
        set
        {
            SetProperty(ref _selectedQuestion, value);
            FillInBoxes();

        }
    }

    private Category _selectedCategory = null!;

    public Category SelectedCategory
    {
        get { return _selectedCategory; }
        set
        {
            SetProperty(ref _selectedCategory, value);
            if (SelectedCategory is not null)
            {
                if (CategoriesForAQuestion is not null)
                {
                    foreach (var question in CategoriesForAQuestion!)
                    {
                        if (question.Id == SelectedCategory.Id)
                        {
                            return;
                        }
                    }
                }

                CategoriesForAQuestion!.Add(SelectedCategory);
            }
        }
    }

    private ObservableCollection<Category> _categoriesForAQuestion = new ObservableCollection<Category>();

    public ObservableCollection<Category> CategoriesForAQuestion
    {
        get { return _categoriesForAQuestion; }
        set { SetProperty(ref _categoriesForAQuestion, value); }
    }

    private Category _selectedCategoryForAQuestion = null!;

    public Category SelectedCategoryForAQuestion
    {
        get { return _selectedCategoryForAQuestion; }
        set
        {
            SetProperty(ref _selectedCategoryForAQuestion, value);
        }
    }

    private string _categoryName = null!;

    public string CategoryName
    {
        get { return _categoryName; }
        set
        {
            SetProperty(ref _categoryName, value);
            CheckCategoryTextBox();
        }
    }

    private ObservableCollection<Category> _allCategories = null!;

    public ObservableCollection<Category> AllCategories
    {
        get { return _allCategories; }
        set { SetProperty(ref _allCategories, value); }
    }


    private string _removeCategoryButton = String.Empty;

    public string RemoveCategoryButton
    {
        get { return _removeCategoryButton; }
        set { SetProperty(ref _removeCategoryButton, value); }
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


    public void FillInBoxes()
    {


        if (SelectedQuestion is not null)
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



        CheckButtons();
    }


    public void CheckFields()
    {
        if (SelectedQuestion is null)
        {
            CanSaveOrRemove = false;

            if (QuestionStatment != String.Empty)
            {
                if (QuestionAnswerOne != String.Empty)
                {
                    if (QuestionAnswerTwo != String.Empty)
                    {
                        if (QuestionAnswerThree != String.Empty)
                        {
                            if (CorrectAnswerOne != false || CorrectAnswerTwo != false || CorrectAnswerThree != false)
                            {
                                CanSaveNewQuestion = true;
                            }
                        }
                    }
                }
            }
        }

    }

    public void CheckCategoryTextBox()
    {
        if (CategoryName == String.Empty)
        {
            CanSaveNewCategory = false;
            return;
        }

        CanSaveNewCategory = true;
    }

    public void ClearFields()
    {
        QuestionStatment = String.Empty;

        QuestionAnswerOne = String.Empty;
        QuestionAnswerTwo = String.Empty;
        QuestionAnswerThree = String.Empty;

        CategoriesForAQuestion = new ObservableCollection<Category>();

        CanSaveOrRemove = false;

        CanSaveNewQuestion = false;

        CorrectAnswerOne = false;
        CorrectAnswerTwo = false;
        CorrectAnswerThree = false;

        LoadListView();
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
            CheckFields();
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
            CheckFields();
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
            CheckFields();

        }
    }

    private string _questionStatment = null!;

    public string QuestionStatment
    {
        get { return _questionStatment; }
        set
        {
            SetProperty(ref _questionStatment, value);
            CheckFields();
        }

    }

    private string _questionAnswerOne = String.Empty;

    public string QuestionAnswerOne
    {
        get { return _questionAnswerOne; }
        set
        {
            SetProperty(ref _questionAnswerOne, value);
            CheckFields();
        }
    }

    private string _questionAnswerTwo = String.Empty;

    public string QuestionAnswerTwo
    {
        get { return _questionAnswerTwo; }
        set
        {
            SetProperty(ref _questionAnswerTwo, value);
            CheckFields();
        }
    }

    private string _questionAnswerThree = String.Empty;

    public string QuestionAnswerThree
    {
        get { return _questionAnswerThree; }
        set
        {
            SetProperty(ref _questionAnswerThree, value);
            CheckFields();
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

    private bool _canSaveNewQuestion = false;

    public bool CanSaveNewQuestion
    {
        get { return _canSaveNewQuestion; }
        set { SetProperty(ref _canSaveNewQuestion, value); }
    }


    private bool _canSaveNewCategory = false;

    public bool CanSaveNewCategory
    {
        get { return _canSaveNewCategory; }
        set { SetProperty(ref _canSaveNewCategory, value); }
    }



}