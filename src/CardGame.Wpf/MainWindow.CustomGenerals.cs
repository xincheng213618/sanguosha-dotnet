using System.Windows;
using CardGame.Wpf.Controls;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf;

public partial class MainWindow
{
    private CustomGeneralEditorWindow? _customGeneralEditor;

    private void OpenCustomGenerals(object sender, RoutedEventArgs e) => OpenCustomGeneralEditor();

    public void OpenCustomGeneralEditor()
    {
        if (_customGeneralEditor is { } existing)
        {
            if (existing.WindowState == WindowState.Minimized) existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }
        if (DataContext is not MainViewModel model) return;
        var editor = new CustomGeneralEditorWindow(model.CreateCustomGeneralEditor()) { Owner = this };
        _customGeneralEditor = editor;
        editor.Closed += (_, _) => _customGeneralEditor = null;
        editor.Show();
    }
}
