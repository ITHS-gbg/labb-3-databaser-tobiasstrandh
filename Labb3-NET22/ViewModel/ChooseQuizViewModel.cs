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

    public async Task GoToQuizView()
    {

       
        await _quizManger.DownloadJson(QuizTitle);

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