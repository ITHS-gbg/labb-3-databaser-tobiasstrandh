using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;
 

public class MainViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizManger _quizManager;
    private readonly QuestionManager _questionManager;

    public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;


    public MainViewModel(NavigationManager navigationManager, QuizManger quizManager, QuestionManager questionManager)
    {
        _navigationManager = navigationManager;
        _quizManager = quizManager;
        _questionManager = questionManager;

        _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
    }

    private void CurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}