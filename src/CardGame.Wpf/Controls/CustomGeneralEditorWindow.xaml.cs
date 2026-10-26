using System.Windows;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf.Controls;

public partial class CustomGeneralEditorWindow : Window
{
    public CustomGeneralEditorWindow(CustomGeneralEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void CloseEditor(object sender, RoutedEventArgs e) => Close();
}
