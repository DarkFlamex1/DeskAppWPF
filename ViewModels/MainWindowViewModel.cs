using CommunityToolkit.Mvvm.ComponentModel;
using DeskAppWPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.ViewModels
{
    /// <summary>
    /// This is the main view model for the application. It is basically the shell that contains other screens that are composed with view models.
    /// This is responsible only for the navigation and startup logic of the app.
    /// </summary>
    internal partial class MainWindowViewModel : ObservableObject
    {
        // required services expected from app.xaml.cs
        private readonly IDeskService _deskService;

        [ObservableProperty]
        private ObservableObject _currentPage;

        public MainWindowViewModel(IDeskService deskService)
        {
            _deskService = deskService;

            _currentPage = new DeskControlViewModel(_deskService);
        }
    }
}
