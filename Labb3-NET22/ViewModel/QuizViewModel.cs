using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;

public class QuizViewModel : ObservableObject
{
    
    private readonly NavigationManager _navigationManager;
    private readonly QuizManger _quizManger;

    public ICommand NextQuestionCommand { get; }

    public QuizViewModel (QuizManger quizManger, NavigationManager navigationManager)
    {

        _quizManger = quizManger;

        _navigationManager = navigationManager;

        RandomQuestion();

        NextQuestionCommand = new RelayCommand(() => NextQuestion());
    }

    public void NextQuestion()
    {
        
        
            AmountAnswersTotal++;


        if (PlayersQuestionAnswer == CorrrectAnswer)
        {
            AmountCorrectAnswers++;
        }

        var amountQuestions = _quizManger.CurrentQuiz.Questions.ToList();

        if (AmountAnswersTotal > amountQuestions.Count)
        {

            _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _navigationManager);
            
        }

        if (AmountAnswersTotal == amountQuestions.Count)
        {
            ButtonName = "Done with Quiz";
            CanNextQuestion = true;

            QuestionStatment = "Press button to go back to start";

            QuestionAnswerOne = string.Empty;
            QuestionAnswerTwo = string.Empty;
            QuestionAnswerThree = string.Empty;

            CorrectAnswerOne = false;
            CorrectAnswerTwo = false;
            CorrectAnswerThree = false;



           
        }


        else if (AmountAnswersTotal < amountQuestions.Count)
        {
            #region stringEmpty
            QuestionStatment = string.Empty;

            QuestionAnswerOne = string.Empty;
            QuestionAnswerTwo = string.Empty;
            QuestionAnswerThree = string.Empty;

            CorrectAnswerOne = false;
            CorrectAnswerTwo = false;
            CorrectAnswerThree = false;

            
            CorrectOrWrongQuestionAnswer = string.Empty;

            #endregion

            
            CanFillBoxes = true;

            CanNextQuestion = false;


            RandomQuestion();
        }
    }


    public void RandomQuestion()
    {
       

        var randomQuestion = _quizManger.CurrentQuiz.GetRandomQuestion();
           
        QuestionStatment = randomQuestion.Statement;

        QuestionAnswerOne = randomQuestion.Answers[0];
        QuestionAnswerTwo = randomQuestion.Answers[1];
        QuestionAnswerThree = randomQuestion.Answers[2];

        CorrrectAnswer = randomQuestion.CorrectAnswer;
        
    }

    public int CorrrectAnswer { get; set; }

    private string _questionStatment;

    public string QuestionStatment
    {
        get
        {

            return _questionStatment;
        }
        set
        {
            SetProperty(ref _questionStatment, value);
        }
    }

    private string _questionAnswerOne;

    public string QuestionAnswerOne
    {
        get { return _questionAnswerOne; }
        set
        {
            SetProperty(ref _questionAnswerOne, value);
        }
    }

    private string _questionAnswerTwo;

    public string QuestionAnswerTwo
    {
        get { return _questionAnswerTwo;  }
        set
        {
            SetProperty(ref _questionAnswerTwo, value);
        }
    }

    private string _questionAnswerThree;

    public string QuestionAnswerThree
    {
        get { return _questionAnswerThree;  }
        set
        {
            SetProperty(ref _questionAnswerThree, value);
        }
    }

    public int PlayersQuestionAnswer { get; set; }

    private string _correctOrWrongQuestionAnswer = String.Empty;

    public string CorrectOrWrongQuestionAnswer
    {
        get { return _correctOrWrongQuestionAnswer; }
        set
        {
            SetProperty(ref _correctOrWrongQuestionAnswer, value);
        }
    }
    public void IsItCorrect()
    {
        

        if (CorrectAnswerOne == true)
        {
            PlayersQuestionAnswer = 0;
            if (PlayersQuestionAnswer == CorrrectAnswer)
            {
                CorrectOrWrongQuestionAnswer = "Correct";
                CanNextQuestion = true;
                CanFillBoxes = false;
            }
            else
            {
                CorrectOrWrongQuestionAnswer = "Wrong";
                CanNextQuestion = true;
                CanFillBoxes = false;
            }
        }

        else if (CorrectAnswerTwo == true)
        {
            PlayersQuestionAnswer = 1;
            if (PlayersQuestionAnswer == CorrrectAnswer)
            {
                CorrectOrWrongQuestionAnswer = "Correct";
                CanNextQuestion = true;
                CanFillBoxes = false;
            }
            else
            {
                CorrectOrWrongQuestionAnswer = "Wrong";
                CanNextQuestion = true;
                CanFillBoxes = false;
            }
        }

        else if (CorrectAnswerThree == true)
        {
            PlayersQuestionAnswer = 2;
            if (PlayersQuestionAnswer == CorrrectAnswer)
            {
                CorrectOrWrongQuestionAnswer = "Correct";
                CanNextQuestion = true;
                CanFillBoxes = false;
            }
            else
            {
                CorrectOrWrongQuestionAnswer = "Wrong";
                CanNextQuestion = true;
                CanFillBoxes = false;
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
            IsItCorrect();
        }
    }

    private bool _correctAnswerTwo;

    public bool CorrectAnswerTwo
    {
        get { return _correctAnswerTwo; }
        set
        {
            SetProperty(ref _correctAnswerTwo, value);
            IsItCorrect();
        }
    }

    private bool _correctAnswerThree;

    public bool CorrectAnswerThree
    {
        get { return _correctAnswerThree; }
        set
        {
            SetProperty(ref _correctAnswerThree, value);
            IsItCorrect();
        }
    }


    private int _amountCorrectAnswers = 0;

    public int AmountCorrectAnswers
    {
        get { return _amountCorrectAnswers; }
        set
        {
            SetProperty(ref _amountCorrectAnswers, value);
        }
    }



    

    private int _amountAnswersTotal;

    public int AmountAnswersTotal
    {
        get { return _amountAnswersTotal; }
        set { SetProperty(ref _amountAnswersTotal, value);  }
    }

    private bool _canFillBoxes = true;

    public bool CanFillBoxes
    {
        get { return _canFillBoxes; }
        set { SetProperty(ref _canFillBoxes, value); }
    }

    private bool _canNextQuestion = false;

    public bool CanNextQuestion
    {
        get { return _canNextQuestion; }
        set { SetProperty(ref _canNextQuestion, value); }
    }

    private string _buttonName = "Next Question";

    public string ButtonName
    {
        get { return _buttonName; }
        set { SetProperty(ref _buttonName, value);  }
    }

}