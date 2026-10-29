using System.Windows;
using System.Windows.Controls;

namespace CardGame.Wpf.Controls;

/// <summary>Original map progress artwork projected onto the local six-step route.</summary>
public partial class ShanheChapterProgressStrip : UserControl
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int),
        typeof(ShanheChapterProgressStrip), new PropertyMetadata(0, ProgressChanged));

    public ShanheChapterProgressStrip() { InitializeComponent(); RefreshProgress(); }
    public int Value { get => (int)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

    private static void ProgressChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args) =>
        ((ShanheChapterProgressStrip)owner).RefreshProgress();

    private void RefreshProgress()
    {
        if (ProgressFill is null) return;
        var completed = Math.Clamp(Value, 0, 6);
        FillClip.Rect = new Rect(0, 0, completed * 86, 38);
        Frontier.Visibility = completed is > 0 and < 6 ? Visibility.Visible : Visibility.Collapsed;
        Canvas.SetLeft(Frontier, completed * 86 - 1);
    }
}
