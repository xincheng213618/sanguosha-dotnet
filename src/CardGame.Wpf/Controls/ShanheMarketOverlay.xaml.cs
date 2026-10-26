using System.Windows.Controls;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf.Controls;

public partial class ShanheMarketOverlay : UserControl
{
    public ShanheMarketOverlay() => InitializeComponent();

    public void FocusMarket()
    {
        if (DataContext is MainViewModel { IsShanheSkillReplacementOpen: true }) ReplacementHost.FocusCampaign();
        else MarketHost.FocusMarket();
    }

    public void HandleEscape()
    {
        if (DataContext is not MainViewModel model) return;
        if (model.IsShanheSkillReplacementOpen) ReplacementHost.HandleEscape();
        else model.CloseShanheMarketCommand.Execute(null);
    }
}
