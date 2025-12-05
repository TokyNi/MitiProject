using Avalonia.Controls;
using MitiConsulting.UI.Main;
using AutoMapper;
using MitiConsulting.ApplicationCore.Interfaces;
using MitiConsulting.ApplicationCore.Services;
using Microsoft.Extensions.DependencyInjection;


namespace MitiConsulting.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // IRapportService rapportService = App.Services.GetService<IRapportService>();
            // IMapper mapper = App.Services.GetService<IMapper>();

            // DataContext = new MainViewModel(rapportService, mapper);
        }
    }
}
