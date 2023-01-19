using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;

public class StartViewModel : ObservableObject
{
    public ICommand NavigateCreateQuizCommand { get; }
    public ICommand NavigatePlayQuizCommand { get; }
    public ICommand NavigateEditQuizCommand { get; }

    private readonly NavigationManager _navigationManager;
    private readonly QuizManger _quizManger;
    private readonly QuestionManager _questionManager;
    private readonly CategoryManager _categoryManager;
    public StartViewModel(QuizManger quizManger, QuestionManager questionManager, CategoryManager categoryManager, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;
        _questionManager = questionManager;
        _categoryManager = categoryManager;



        NavigateCreateQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateQuizViewModel(_quizManger, _questionManager, _categoryManager, _navigationManager));

        NavigatePlayQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new ChooseQuizViewModel(_quizManger, _questionManager, _categoryManager, _navigationManager));

        NavigateEditQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditViewModel(_quizManger, _questionManager,_categoryManager,_navigationManager));
    }


}