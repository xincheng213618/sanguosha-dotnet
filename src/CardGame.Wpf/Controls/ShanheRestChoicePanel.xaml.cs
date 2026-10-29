using System.Windows.Controls;

namespace CardGame.Wpf.Controls;

public partial class ShanheRestChoicePanel : UserControl
{
    public ShanheRestChoicePanel() => InitializeComponent();
    public void FocusChoices() => RestChoiceList.Focus();
}
