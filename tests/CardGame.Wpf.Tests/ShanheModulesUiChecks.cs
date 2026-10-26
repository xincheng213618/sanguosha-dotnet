using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheModulesUiChecks
{
    public static void PreRunCatalogAndCollectionNavigation(string output)
    {
        using var vm = new MainViewModel(false, 270017, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        var host = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            Layout(host);
            var codex = (ShanheCodexPanel)host.FindName("ShanheCodexHost");
            var tabs = (TabControl)host.FindName("ShanheJourneyTabs");
            tabs.SelectedIndex = 1; Layout(host);
            Program.Assert(vm.ShanheCodexCount > 0 && !vm.IsShanheShopActive && vm.ShanheShopItems.All(item => !item.CanBuy),
                "Before a run the catalog must be explorable and merchant previews must never permit purchase.");
            Program.Assert(vm.ShanheCodexCards.All(card => card.Id is not "supplies" and not "medicine") &&
                vm.ShanheShopItems.Any(item => item.Category == "随身牌") && vm.ShanheShopItems.Any(item => item.Category == "随身装备"),
                "One-off money and healing supplies must not be presented as persistent tactics.");
            var toggle = (CheckBox)codex.FindName("CollectionToggle");
            toggle.IsChecked = true; Layout(host);
            Program.Assert(vm.ShanheCodexShowCollection && vm.ShanheCodexCount == 0,
                "The actual collection toggle must display the honest empty collection before an expedition.");
            toggle.IsChecked = false; Layout(host);
            Program.Assert(!vm.ShanheCodexShowCollection && vm.ShanheCodexCount > 0,
                "Switching back returns to the full tactic catalog.");
            Program.Render(host, 1200, 750, System.IO.Path.Combine(output, "shanhe-codex-page.png"));
            Program.Assert(host.HandleEscape() && tabs.SelectedIndex == 0 && !vm.HasShanheSession,
                "Escape returns from the codex to preparation without beginning an expedition.");
        }
        finally { host.DataContext = null; }
    }
    private static void Layout(FrameworkElement root)
    {
        root.Measure(new Size(1000, 530)); root.Arrange(new Rect(0, 0, 1000, 530)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
    }
}



