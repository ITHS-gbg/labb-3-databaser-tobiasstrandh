using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.Managers;

namespace Labb3_NET22.ViewModel;
 

public class MainViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizManger _quizManager;

    public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;


    public MainViewModel(NavigationManager navigationManager, QuizManger quizManager)
    {
        _navigationManager = navigationManager;
        _quizManager = quizManager;

        _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
    }

    private void CurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}