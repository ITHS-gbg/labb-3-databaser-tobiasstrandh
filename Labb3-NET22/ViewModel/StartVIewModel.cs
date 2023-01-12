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
    public StartViewModel( QuizManger quizManger, NavigationManager navigationManager)
    {
        _quizManger = quizManger;
        _navigationManager = navigationManager;
       
        DefaultQuiz();

        


        NavigateCreateQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateQuizViewModel(_quizManger, _navigationManager));

        NavigatePlayQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new ChooseQuizViewModel(_quizManger, _navigationManager));

        NavigateEditQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditViewModel(_quizManger, _navigationManager));
    }

    

    private async Task DefaultQuiz()
    {
        _quizManger.CurrentQuiz = new QuizModel("tobbesquiz");

        _quizManger.CurrentQuiz.PopulateDefaultQuiz();

        await Task.Run((() => _quizManger.JsonDefaultQuizSave()));
        
    }

}