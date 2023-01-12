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
   

    public EditViewModel(QuizManger quizManger, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;
        
       
        LoadListView();

        
        RemoveCommand = new RelayCommand(() => RemoveQuestion());
        SaveEditCommand = new RelayCommand(() => SaveEdit());
        GoBackToStartCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _navigationManager));
        
    }

    public ICommand RemoveCommand { get; }

    public ICommand SaveEditCommand { get; }

    public ICommand GoBackToStartCommand { get; }


    public async Task SaveEdit()
    {
        Correct();

        var QuizAnswers = new string[] { QuestionAnswerOne, QuestionAnswerTwo, QuestionAnswerThree };

        _quizManger.CurrentQuiz.EditQuestion(QuestionIndex, QuestionStatment, QuestionCorrectAnswer, QuizAnswers);

        await _quizManger.JsonSave();

        SetList();
    }
    public async Task RemoveQuestion()
    {
        
        _quizManger.CurrentQuiz.RemoveQuestion(QuestionIndex);
        await _quizManger.JsonSave();

        QuestionStatment = string.Empty;

        QuestionAnswerOne = string.Empty;
        QuestionAnswerTwo = string.Empty;
        QuestionAnswerThree = string.Empty;

        CorrectAnswerOne = false;
        CorrectAnswerTwo = false;
        CorrectAnswerThree = false;

        SetList();
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

   
    public async Task LoadListView()
    {
        FileTitles = await _quizManger.JsonTitleList();
    }

    private List<string> _fileTitles;

    public List<string> FileTitles
    {
        get { return _fileTitles; }
        set
        {
            SetProperty(ref _fileTitles, value);
        }
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
        await _quizManger.DownloadJson(QuizTitle);
        QuestionList = _quizManger.CurrentQuiz.Questions;
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
        

        if (QuestionIndex >= 0)
        {
            QuestionStatment = QuestionList.ElementAt(QuestionIndex).Statement;

            QuestionAnswerOne = QuestionList.ElementAt(QuestionIndex).Answers[0];
            QuestionAnswerTwo = QuestionList.ElementAt(QuestionIndex).Answers[1];
            QuestionAnswerThree = QuestionList.ElementAt(QuestionIndex).Answers[2];

            QuestionCorrectAnswer = QuestionList.ElementAt(QuestionIndex).CorrectAnswer;

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

        else
        {
            QuestionStatment = String.Empty;

            QuestionAnswerOne = String.Empty;
            QuestionAnswerTwo = String.Empty;
            QuestionAnswerThree = String.Empty;

           
            CorrectAnswerOne = false;
            CorrectAnswerTwo = false;
            CorrectAnswerThree = false;
           
        }

        CheckButtons();
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