using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.DataModels;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;

public class CreateQuizViewModel : ObservableObject
{
    public ICommand SaveQuestionCommand { get; }

    public ICommand NewQuestionCommand { get; }
    
    public ICommand ReturnToStartViewCommand { get; }

    private readonly QuizManger _quizManger;

   

    private readonly NavigationManager _navigationManager;
    public CreateQuizViewModel(QuizManger quizManger, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        
        _navigationManager = navigationManager;

        SaveQuestionCommand = new RelayCommand(() => AddQuestion());


       ReturnToStartViewCommand = new RelayCommand(() => ReturnToStartView());

       NewQuestionCommand = new RelayCommand(() => NewQuestion() );

    }

    

    public void NewQuestion()
    {
        CanSaveQuiz = false;
        CanCloseCreateQuiz = false;
        CanFillQuestionBoxes = true;

    }


    public async Task ReturnToStartView()
    {
        await _quizManger.JsonSave();
        _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _navigationManager);
    }

  

    public async Task AddQuestion() 
    {
        if (SaveButtonName == "Save Title")
        {
            _quizManger.CurrentQuiz = new QuizModel(QuizTitle);
            await _quizManger.JsonSave();
            
            SaveButtonName = "Save Question";
            CanSaveQuiz = false;
            CantChangeTitle = false;
            CanFillQuestionBoxes = true;
        }


        else
        {
            var QuizAnswers = new string[] { QuestionAnswerOne, QuestionAnswerTwo, QuestionAnswerThree };

            Correct();

            _quizManger.CurrentQuiz.AddQuestion(QuestionStatment, QuestionCorrectAnswer, QuizAnswers);

            

            #region stringEmpty
            QuestionStatment = string.Empty;

            QuestionAnswerOne = string.Empty;
            QuestionAnswerTwo = string.Empty;
            QuestionAnswerThree = string.Empty;

            CorrectAnswerOne = false;
            CorrectAnswerTwo = false;
            CorrectAnswerThree = false;

            #endregion
            CanSaveQuiz = false;
            CanNewQuestion = true;
            CanCloseCreateQuiz = true;
            CantChangeTitle = false;
            CanFillQuestionBoxes = false;
        }
    }

   
    public int QuestionCorrectAnswer { get; set; }
    void Correct()
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


    private string _quizTitle;

    public string QuizTitle
    {
        get { return _quizTitle; }
        set
        {
            SetProperty(ref _quizTitle, value);
            CheckTitleBox();
        }
    }

    public void CheckTitleBox()
    {
        if (QuizTitle != string.Empty)
        {
            CanSaveQuiz = true;
        }
    }

    private string _saveButtonName = "Save Title";

    public string SaveButtonName
    {
        get { return _saveButtonName; }
        set { SetProperty(ref _saveButtonName, value); }
    }

    private string _questionStatment = String.Empty;

    public string QuestionStatment
    {
        get { return _questionStatment; }
        set
        {
            SetProperty(ref _questionStatment, value);
            CheckAllBoxes();
        }
    }

    private string _questionAnswerOne = String.Empty;

    public string QuestionAnswerOne
    {
        get { return _questionAnswerOne; } 
        set
        {
            SetProperty(ref _questionAnswerOne, value);
            CheckAllBoxes();
        }
    }

    private string _questionAnswerTwo = String.Empty;

    public string QuestionAnswerTwo
    {
        get { return _questionAnswerTwo; }
        set
        {
            SetProperty(ref _questionAnswerTwo, value);
            CheckAllBoxes();
        }
    }

    private string _questionAnswerThree = String.Empty;

    public string QuestionAnswerThree
    {
        get { return _questionAnswerThree; }
        set
        {
            SetProperty(ref _questionAnswerThree, value);
            CheckAllBoxes();
        }
    }



    public void CheckAllBoxes()
    {
        if (QuestionAnswerOne != String.Empty)
        {
            if (QuestionAnswerTwo != String.Empty )
            {
                if (QuestionAnswerThree != String.Empty)
                {
                    if (QuestionStatment != String.Empty)
                    {
                        if (QuizTitle != String.Empty)
                        {
                            if (CorrectAnswerOne == true || CorrectAnswerTwo == true || CorrectAnswerThree == true)
                            {
                                CanSaveQuiz = true;
                                CanNewQuestion = false;
                            }
                        }
                    }
                }
            }
        }
    }


    private bool _correctAnswerOne;

    public bool CorrectAnswerOne
    {
        get { return _correctAnswerOne; }
        set
        {
            SetProperty(ref _correctAnswerOne, value);
            CheckAllBoxes();
        }
    }

    private bool _correctAnswerTwo;

    public bool CorrectAnswerTwo
    {
        get { return _correctAnswerTwo; }
        set
        {
            SetProperty(ref _correctAnswerTwo, value);
            CheckAllBoxes();
        }
    }

    private bool _correctAnswerThree;

    public bool CorrectAnswerThree
    {
        get { return _correctAnswerThree; }
        set
        {
            SetProperty(ref _correctAnswerThree, value);
            CheckAllBoxes();
        }
    }

    private bool _canCloseCreateQuiz = false;

    public bool CanCloseCreateQuiz
    {
        get { return _canCloseCreateQuiz; }
        set { SetProperty(ref _canCloseCreateQuiz, value); }
    }

    private bool _canFillQuestionBoxes = false;

    public bool CanFillQuestionBoxes
    {
        get { return _canFillQuestionBoxes; }
        set { SetProperty(ref _canFillQuestionBoxes, value); }
    }

    private bool _canSaveQuiz = false;

    public bool CanSaveQuiz
    {
        get { return _canSaveQuiz; }
        set { SetProperty(ref _canSaveQuiz, value); }
    }

    private bool _canNewQuestion = false;

    public bool CanNewQuestion
    {
        get { return _canNewQuestion; }
        set { SetProperty(ref _canNewQuestion, value);  }
    }

    private bool _cantChangeTitle = true;

    public bool CantChangeTitle
    {
        get { return _cantChangeTitle; }
        set { SetProperty(ref _cantChangeTitle, value); }
    }




    

}
