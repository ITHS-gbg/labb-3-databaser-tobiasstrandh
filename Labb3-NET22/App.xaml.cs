using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.ViewModel;

namespace Labb3_NET22
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly QuizManger _quizManger;
        private readonly QuestionManager _questionManager;
        private readonly CategoryManager _categoryManager;
        public App()
        {
            _navigationManager = new NavigationManager();
            _quizManger = new QuizManger();
            _questionManager = new QuestionManager();
            _categoryManager = new CategoryManager();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
            _navigationManager.CurrentViewModel = new StartViewModel(_quizManger, _questionManager,_categoryManager, _navigationManager);

            var mainWindow = new MainWindow() { DataContext = new MainViewModel(_navigationManager, _quizManger, _questionManager, _categoryManager) };

            mainWindow.Show();
        }
    }
}
