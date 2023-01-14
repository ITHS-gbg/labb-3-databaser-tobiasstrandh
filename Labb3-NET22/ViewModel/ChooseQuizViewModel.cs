using System.Collections.Generic;
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

    public ICommand ReturnToStartViewCommand { get; }
    public ICommand GoToQuizViewCommand { get; }

    public ChooseQuizViewModel(QuizManger quizManger, NavigationManager navigationManager)
    {
        
        
        _navigationManager = navigationManager;
        _quizManger = quizManger;
        LoadListView();


        ReturnToStartViewCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _navigationManager));

        GoToQuizViewCommand = new RelayCommand(() => GoToQuizView());

    }

    public void GoToQuizView()
    {


        _quizManger.CurrentQuiz = SelectedQuiz;

        var amountQuestions = _quizManger.CurrentQuiz.Questions.ToList();
        if (amountQuestions.Count == 0)
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _navigationManager);
        }

        else
        {
            _navigationManager.CurrentViewModel = new QuizViewModel(_quizManger, _navigationManager);
        }

    }

    public void LoadListView()
    {
        AllQuiz = _quizManger.GetAllQuiz();

    }


    private IEnumerable<QuizModel> _allQuiz;

    public IEnumerable<QuizModel> AllQuiz
    {
        get { return _allQuiz; }
        set
        {
            SetProperty(ref _allQuiz, value);
        }
    }


    private QuizModel _selectedQuiz;

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

}