using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MitiConsulting.UI.Fiches.ViewModels;
using System.Threading.Tasks;

namespace MitiConsulting.UI.Views
{
    // Le nom de la classe doit correspondre à celui défini dans x:Class du fichier .axaml, soit ListeFiche
    public partial class ListeFiche : UserControl
    {
        public ListeFiche()
        {
            InitializeComponent();
            
            // Attachement de l'événement pour déclencher le chargement initial des données 
            // une fois que la vue est prête.
            this.AttachedToVisualTree += OnAttachedToVisualTree;
        }

        private void InitializeComponent()
        {
            // Charge le contenu XAML du fichier ListeFiche.axaml
            AvaloniaXamlLoader.Load(this);
        }
        
        /// <summary>
        /// Gère l'événement déclenché lorsque le contrôle est attaché à l'arbre visuel.
        /// Déclenche la commande de chargement initial.
        /// </summary>
        private void OnAttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
        {
            // Détacher l'événement pour ne pas le déclencher plusieurs fois après le premier attachement
            this.AttachedToVisualTree -= OnAttachedToVisualTree;

            // Le DataContext devrait être votre RapportViewModel
            if (DataContext is RapportsListViewModel viewModel)
            {
                // Exécute la commande ChargerRapport pour peupler la liste initialement vide
                if (viewModel.ChargerRapport?.CanExecute(null) == true)
                {
                    Task.Run(() => viewModel.ChargerRapport.ExecuteAsync(null));
                }
            }
        }
    }
}