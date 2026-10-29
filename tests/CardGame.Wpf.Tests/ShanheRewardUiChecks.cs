using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    private const string Player = "fixture:shanhe-ui-player";
    private const string Enemy = "fixture:shanhe-ui-enemy";
    private const string Force = "fixture:shanhe-ui-victory";
    private const string Guard = "fixture:shanhe-ui-guard";
    private const string Learned = "fixture:shanhe-ui-learned";
    private static readonly Lazy<ContentRegistry> FixtureRegistry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardTeamModePackage(), new RewardBattlePackage()));

    public static void RealVictoryRewardCardsConfirmSkipSkillAndReplacement(string output)
    {
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var session = WinRewardSession();
            Attach(vm, session);
            Layout(panel);
            var overlay = (FrameworkElement)panel.FindName("ShanheRewardOverlay");
            var rows = (ItemsControl)panel.FindName("ShanheRewardCardRows");
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            var objective = (TextBlock)panel.FindName("ShanheChapterObjective");
            var ordinaryRevision = Program.Engine(vm).Revision;
            Program.Assert(vm.IsShanheRewardOpen && overlay.Visibility == Visibility.Visible &&
                vm.ShanheRewardCards.Count == 4 && vm.ShanheRewardCards.Count(card => card.IsSkip) == 1 &&
                rows.Items.Count == 4 && !confirm.IsEnabled && !vm.ConfirmShanheRewardCommand.CanExecute(null),
                "A real completed victory must show three reward cards and a separate decline card, with confirmation initially disabled.");
            var gold = session.Snapshot.Gold;
            Program.Assert(session.Snapshot.CanSkipReward && session.Snapshot.RewardSkipGold == 100 &&
                    objective.Text == vm.ShanheChapterObjectiveText &&
                    objective.Text.Contains(session.Snapshot.RewardSkipGold.ToString(), StringComparison.Ordinal) &&
                    objective.Text.Contains("放弃", StringComparison.Ordinal) && objective.Text.Contains("铜币", StringComparison.Ordinal) &&
                    BindingOperations.GetBinding(objective, TextBlock.TextProperty)?.Path.Path == nameof(MainViewModel.ShanheChapterObjectiveText),
                "The actual bound campaign header derives the normal reward's available decline action and exact compensation from its current snapshot.");
            var mapGoldValue = (TextBlock)panel.FindName("ShanheMapGoldValue");
            Program.Assert(gold == 17 && mapGoldValue.Text == vm.ShanheGoldText && mapGoldValue.Text == "铜币   17",
                "The fixed map wallet immediately displays the actual seventeen coins from the validated native victory while its reward is still pending.");
            var beforeSelection = Snapshot(session);
            var revision = session.Revision;
            var skill = vm.ShanheRewardCards.Single(card => card.Id == "learned");
            var staleSkip = vm.ShanheRewardCards.Single(card => card.IsSkip);
            var staleSkipButton = Bound(panel, vm.SelectShanheRewardCardCommand, staleSkip);
            AssertNativeRewardTooltip(vm, session, vm.SelectShanheRewardCardCommand, skill, skill.RuleTooltip,
                output, "shanhe-reward-rule-tooltip");
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, skill), panel);
            Program.Assert(skill.IsSelected && ReferenceEquals(vm.SelectedShanheRewardCard, skill) &&
                confirm.IsEnabled && Snapshot(session) == beforeSelection && mapGoldValue.Text == "铜币   17",
                "Clicking a skill card must only preview its selection; the real campaign journal and resources stay unchanged.");
            Click(staleSkipButton, panel);
            Program.Assert(staleSkip.IsSelected && !skill.IsSelected && vm.ShanheRewardCards.Count(card => card.IsSelected) == 1 &&
                staleSkip.BonusText.Contains("100", StringComparison.Ordinal) && Snapshot(session) == beforeSelection &&
                mapGoldValue.Text == "铜币   17" &&
                panel.HandleEscape() && vm.IsShanheRewardOpen && session.Revision == revision,
                "The decline card must show its 100-coin compensation, select exclusively, and retain the unresolved reward when Escape is pressed.");
            Program.Assert(objective.Text == vm.ShanheChapterObjectiveText &&
                    objective.Text.Contains(session.Snapshot.RewardSkipGold.ToString(), StringComparison.Ordinal) &&
                    objective.Text.Contains("放弃", StringComparison.Ordinal),
                "Selecting the decline card leaves the actual compensation objective pending until its separate confirmation.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-reward-choice.png"));
            Click(confirm, panel);
            Program.Assert(!vm.IsShanheRewardOpen && overlay.Visibility == Visibility.Collapsed &&
                session.Revision == revision + 1 && session.Snapshot.Gold == gold + 100 &&
                session.Snapshot.ExtraSkillIds.Count == 0 && session.Snapshot.TacticIds.Count == 0 &&
                session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Count() == 1 &&
                mapGoldValue.Text == vm.ShanheGoldText && mapGoldValue.Text == "铜币   117" &&
                Program.Engine(vm).Revision == ordinaryRevision,
                "Only confirmation may grant exactly 100 extra coins, without learning a skill or touching the hidden ordinary game.");
            Program.Assert(objective.Text == vm.ShanheChapterObjectiveText &&
                    !objective.Text.Contains("放弃", StringComparison.Ordinal) && !objective.Text.Contains("100", StringComparison.Ordinal),
                "The same header immediately retires the paid decline offer after its real confirmation rather than leaving stale reward instructions on the map.");
            var afterSkip = Snapshot(session);
            RawClick(confirm);
            RawClick(staleSkipButton);
            vm.SelectShanheRewardCardCommand.Execute(staleSkip);
            vm.ConfirmShanheRewardCommand.Execute(null);
            Layout(panel);
            Program.Assert(Snapshot(session) == afterSkip && !vm.SelectShanheRewardCardCommand.CanExecute(staleSkip) &&
                mapGoldValue.Text == "铜币   117",
                "Repeated confirmation and retired card buttons cannot grant the decline payment again.");

            session = WinRewardSession();
            Attach(vm, session);
            Layout(panel);
            Program.Assert(session.Revision == revision && !vm.SelectShanheRewardCardCommand.CanExecute(staleSkip) &&
                mapGoldValue.Text == vm.ShanheGoldText && mapGoldValue.Text == "铜币   17",
                "A previous run's same-id, same-revision card cannot select or claim the new run's reward.");
            var freshBefore = Snapshot(session);
            RawClick(staleSkipButton);
            vm.SelectShanheRewardCardCommand.Execute(staleSkip);
            Program.Assert(Snapshot(session) == freshBefore && vm.SelectedShanheRewardCard is null && !confirm.IsEnabled,
                "A stale bound card leaves the new offer unselected and uncommitted.");
            skill = vm.ShanheRewardCards.Single(card => card.Id == "learned");
            gold = session.Snapshot.Gold;
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, skill), panel);
            Program.Assert(session.Snapshot.ExtraSkillIds.Count == 0 && session.Snapshot.Gold == gold,
                "A new skill is not granted by the selection click alone.");
            Click(confirm, panel);
            Program.Assert(session.Snapshot.ExtraSkillIds.SequenceEqual([Learned]) && session.Snapshot.Gold == gold &&
                !session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Any() && !vm.IsShanheRewardOpen,
                "Confirming a skill must grant only that skill, without also receiving decline compensation.");

            session = WinRewardSession(fillSkillSlot: true);
            Attach(vm, session);
            Layout(panel);
            gold = session.Snapshot.Gold;
            revision = session.Revision;
            skill = vm.ShanheRewardCards.Single(card => card.Id == "learned");
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, skill), panel);
            var beforeReplacement = Snapshot(session);
            Program.Assert(vm.ShanheRewardNeedsReplacement && confirm.IsEnabled && !vm.IsShanheSkillReplacementOpen &&
                vm.SelectedShanheRewardReplacements.Single().ReplaceSkillId == Guard,
                "A full-slot reward must enable its confirmation button so that the separate replacement dialog can be requested.");
            Click(confirm, panel);
            var replacementOverlay = (FrameworkElement)panel.FindName("ShanheSkillReplacementOverlay");
            var replacementConfirm = (Button)panel.FindName("ShanheConfirmSkillReplacementButton");
            var replacementCancel = (Button)panel.FindName("ShanheCancelSkillReplacementButton");
            var locked = vm.ShanheSkillReplacementOptions.Single(option => option.SkillId == Force);
            var replaceable = vm.ShanheSkillReplacementOptions.Single(option => option.SkillId == Guard);
            var lockedButton = Bound(panel, vm.SelectShanheSkillReplacementCommand, locked);
            var staleReplacementButton = Bound(panel, vm.SelectShanheSkillReplacementCommand, replaceable);
            Program.Assert(vm.IsShanheSkillReplacementOpen && replacementOverlay.Visibility == Visibility.Visible &&
                vm.ShanheSkillReplacementOptions.Count == 2 && locked.IsLocked && !locked.IsEnabled &&
                !lockedButton.IsEnabled && !vm.SelectShanheSkillReplacementCommand.CanExecute(locked) &&
                replaceable.IsEnabled && !replaceable.IsLocked && !replacementConfirm.IsEnabled &&
                !overlay.IsEnabled && !confirm.IsEnabled && Snapshot(session) == beforeReplacement,
                "The real replacement modal must lock printed skills, permit the acquired skill, disable the reward underlay, and leave the journal and resources unchanged.");
            AssertNativeRewardTooltip(vm, session, vm.SelectShanheSkillReplacementCommand, replaceable, replaceable.Description,
                output, "shanhe-replacement-rule-tooltip");
            RawClick(lockedButton);
            vm.SelectShanheSkillReplacementCommand.Execute(locked);
            RawClick(confirm);
            vm.ConfirmShanheSkillReplacementCommand.Execute(null);
            Layout(panel);
            Program.Assert(!replacementConfirm.IsEnabled && vm.ShanheSkillReplacementOptions.All(option => !option.IsSelected) &&
                Snapshot(session) == beforeReplacement,
                "Locked printed skills and covered reward confirmation cannot select a replacement or prematurely commit the reward.");
            Click(staleReplacementButton, panel);
            Program.Assert(replaceable.IsSelected && replacementConfirm.IsEnabled && Snapshot(session) == beforeReplacement,
                "Selecting an acquired skill in the actual modal must only preview the replacement until the modal confirmation.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-skill-replacement.png"));
            Click(replacementCancel, panel);
            Program.Assert(!vm.IsShanheSkillReplacementOpen && vm.IsShanheRewardOpen && confirm.IsEnabled &&
                Snapshot(session) == beforeReplacement && !vm.SelectShanheSkillReplacementCommand.CanExecute(replaceable),
                "Canceling the replacement must return to the selected reward without learning, removing a skill, or changing coins.");
            Click(confirm, panel);
            RawClick(staleReplacementButton);
            vm.SelectShanheSkillReplacementCommand.Execute(replaceable);
            vm.ConfirmShanheSkillReplacementCommand.Execute(null);
            Layout(panel);
            Program.Assert(vm.IsShanheSkillReplacementOpen && !replacementConfirm.IsEnabled &&
                vm.ShanheSkillReplacementOptions.All(option => !option.IsSelected) && Snapshot(session) == beforeReplacement &&
                panel.HandleEscape() && !vm.IsShanheSkillReplacementOpen && vm.IsShanheRewardOpen && Snapshot(session) == beforeReplacement,
                "A retired option from a previous modal cannot select the reopened dialog; Escape must cancel it and preserve the pending reward.");
            Click(confirm, panel);
            var freshReplacement = vm.ShanheSkillReplacementOptions.Single(option => option.SkillId == Guard);
            Click(Bound(panel, vm.SelectShanheSkillReplacementCommand, freshReplacement), panel);
            Click(replacementConfirm, panel);
            Program.Assert(session.Snapshot.ExtraSkillIds.SequenceEqual([Learned]) && session.Snapshot.ExtraSkillSlots == 1 &&
                session.Snapshot.Gold == gold && session.Revision == revision + 1 && !vm.IsShanheRewardOpen &&
                !vm.IsShanheSkillReplacementOpen && Program.Engine(vm).Revision == ordinaryRevision,
                "Only the modal confirmation may exchange exactly one acquired skill without changing printed skills, slot count, coins, or the ordinary table.");
            var afterReplacement = Snapshot(session);
            RawClick(replacementConfirm);
            vm.SelectShanheSkillReplacementCommand.Execute(freshReplacement);
            vm.ConfirmShanheSkillReplacementCommand.Execute(null);
            Layout(panel);
            Program.Assert(Snapshot(session) == afterReplacement, "Retired modal controls cannot repeat a confirmed skill replacement.");
        }
        finally { panel.DataContext = null; }
    }

    public static void PersistentMarketSkillsReplaceOnlyOnConfirmation(string output)
    {
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheMarketOverlay { DataContext = vm };
        var replacement = (ShanheCampaignPanel)panel.FindName("ReplacementHost");
        try
        {
            var session = CreateSkillMarketSession(fillSkillSlot: true);
            Attach(vm, session);
            vm.OpenShanheMarketCommand.Execute(null);
            vm.SelectedShanheMarketCategory = "技能";
            Layout(panel);
            var ordinaryRevision = Program.Engine(vm).Revision;
            var item = SkillMarketProduct(vm, session, "learned");
            var buy = Bound(panel, vm.RequestShanheShopPurchaseCommand, item);
            var beforePurchase = Snapshot(session);
            var gold = session.Snapshot.Gold;
            var revision = session.Revision;
            var alreadyOwned = SkillMarketProduct(vm, session, "guard");
            var alreadyOwnedButton = Bound(panel, vm.RequestShanheShopPurchaseCommand, alreadyOwned);
            Program.Assert(vm.IsShanheMarketOpen && vm.IsShanheShopActive && vm.ShanheShopItems.Count == 2 &&
                vm.ShanheMarketGoods.Count(product => ReferenceEquals(product, item)) == 1 &&
                Program.Find<Button>(panel).Count(button => ReferenceEquals(button.CommandParameter, item)) == 1 &&
                item.CanBuy && item.Price == 25 && buy.IsEnabled &&
                alreadyOwned is { State: ShanheShopItemState.AlreadyOwned, Status: "已拥有", CanBuy: false } &&
                !alreadyOwnedButton.IsEnabled && Program.Find<TextBlock>(alreadyOwnedButton).Any(text => text.Text == "已拥有"),
                "A full-slot skill product must appear as one bound purchasable card rather than one card per replacement target.");
            AssertMarketStateCard(item);
            AssertMarketStateCard(alreadyOwned);
            Click(buy, panel);
            var replacementConfirm = (Button)replacement.FindName("ShanheConfirmSkillReplacementButton");
            var replacementCancel = (Button)replacement.FindName("ShanheCancelSkillReplacementButton");
            var locked = vm.ShanheSkillReplacementOptions.Single(option => option.SkillId == Force);
            var oldOption = vm.ShanheSkillReplacementOptions.Single(option => option.SkillId == Guard);
            var oldOptionButton = Bound(panel, vm.SelectShanheSkillReplacementCommand, oldOption);
            Program.Assert(vm.IsShanheSkillReplacementOpen && replacement.Visibility == Visibility.Visible &&
                !vm.IsShanheRewardOpen && !buy.IsEnabled &&
                locked.IsLocked && !vm.SelectShanheSkillReplacementCommand.CanExecute(locked) &&
                !replacementConfirm.IsEnabled && Snapshot(session) == beforePurchase,
                "Requesting a full-slot shop skill must open the shared modal and preserve its price and resources until confirmation.");
            Click(oldOptionButton, panel);
            Click(replacementCancel, panel);
            Program.Assert(!vm.IsShanheSkillReplacementOpen && vm.IsShanheMarketOpen && buy.IsEnabled && Snapshot(session) == beforePurchase,
                "Canceling a selected shop replacement must spend no coins, consume no stock, and remove no skill.");
            AssertMarketStateCard(SkillMarketProduct(vm, session, "learned"));
            AssertMarketStateCard(SkillMarketProduct(vm, session, "guard"));
            Click(buy, panel);
            RawClick(oldOptionButton);
            vm.SelectShanheSkillReplacementCommand.Execute(oldOption);
            vm.ConfirmShanheSkillReplacementCommand.Execute(null);
            Layout(panel);
            Program.Assert(vm.IsShanheSkillReplacementOpen && !replacementConfirm.IsEnabled &&
                vm.ShanheSkillReplacementOptions.All(option => !option.IsSelected) && Snapshot(session) == beforePurchase,
                "A canceled modal's retired option cannot select or confirm the next market dialog.");
            panel.HandleEscape(); Layout(panel);
            Program.Assert(!vm.IsShanheSkillReplacementOpen && vm.IsShanheMarketOpen && Snapshot(session) == beforePurchase,
                "Escape cancels only the replacement and returns to the same market without charging its original price.");

            var previousItem = item;
            session = CreateSkillMarketSession(fillSkillSlot: true);
            Attach(vm, session);
            vm.OpenShanheMarketCommand.Execute(null);
            Layout(panel);
            Program.Assert(session.Revision == revision && !vm.RequestShanheShopPurchaseCommand.CanExecute(previousItem),
                "A previous expedition's same-id, same-revision product must not act on the new shop.");
            beforePurchase = Snapshot(session);
            RawClick(buy);
            vm.RequestShanheShopPurchaseCommand.Execute(previousItem);
            vm.SelectShanheSkillReplacementCommand.Execute(oldOption);
            vm.ConfirmShanheSkillReplacementCommand.Execute(null);
            Program.Assert(!vm.IsShanheSkillReplacementOpen && Snapshot(session) == beforePurchase,
                "Retired shop and replacement objects cannot spend money or open a modal in a replacement expedition.");
            item = SkillMarketProduct(vm, session, "learned");
            buy = Bound(panel, vm.RequestShanheShopPurchaseCommand, item);
            Click(buy, panel);
            var option = vm.ShanheSkillReplacementOptions.Single(entry => entry.SkillId == Guard);
            Click(Bound(panel, vm.SelectShanheSkillReplacementCommand, option), panel);
            Program.Assert(Snapshot(session) == beforePurchase && replacementConfirm.IsEnabled,
                "The chosen shop replacement remains uncommitted until the modal's own confirmation.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-shop-skill-replacement.png"));
            Click(replacementConfirm, panel);
            var purchased = SkillMarketProduct(vm, session, "learned");
            var purchasedButton = Bound(panel, vm.RequestShanheShopPurchaseCommand, purchased);
            Program.Assert(!vm.IsShanheSkillReplacementOpen && session.Revision == revision + 1 &&
                session.Snapshot.Gold == gold - 25 && session.Snapshot.ExtraSkillIds.SequenceEqual([Learned]) &&
                session.Snapshot.ExtraSkillSlots == 1 && session.Snapshot.Phase == ShanheCampaignPhase.Map &&
                purchased is { State: ShanheShopItemState.Purchased, Status: "已购", CanBuy: false } && !purchasedButton.IsEnabled &&
                Program.Find<TextBlock>(purchasedButton).Any(text => text.Text == "已购") &&
                Program.Engine(vm).Revision == ordinaryRevision,
                "Confirming the shared shop modal must debit the unchanged price once, replace one acquired skill, and mark only that product purchased.");
            AssertMarketStateCard(purchased);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-shop-skill-purchased-source-frame.png"));
            var afterPurchase = Snapshot(session);
            RawClick(buy);
            RawClick(replacementConfirm);
            vm.SelectShanheSkillReplacementCommand.Execute(option);
            vm.RequestShanheShopPurchaseCommand.Execute(item);
            vm.ConfirmShanheSkillReplacementCommand.Execute(null);
            Layout(panel);
            Program.Assert(Snapshot(session) == afterPurchase,
                "Repeated shop or modal confirmations must not charge twice or repeat a completed replacement.");

            session = CreateSkillMarketSession(fillSkillSlot: false);
            Attach(vm, session);
            vm.OpenShanheMarketCommand.Execute(null);
            Layout(panel);
            item = SkillMarketProduct(vm, session, "learned");
            gold = session.Snapshot.Gold;
            revision = session.Revision;
            Click(Bound(panel, vm.RequestShanheShopPurchaseCommand, item), panel);
            Program.Assert(!vm.IsShanheSkillReplacementOpen && session.Snapshot.Gold == gold - item.Price &&
                session.Snapshot.ExtraSkillIds.SequenceEqual([Learned]) && session.Revision == revision + 1,
                "With an unused skill slot, the same purchase command must learn the skill and charge its price directly without a replacement dialog.");
            AssertMarketStateCard(SkillMarketProduct(vm, session, "learned"));

            void AssertMarketStateCard(ShanheShopItem product)
            {
                var actual = session.Snapshot.ShopItems.Single(actual => actual.ChoiceId == product.Id);
                var body = Program.Find<Border>(panel).Single(border => ReferenceEquals(border.DataContext, product) && border.ToolTip is ToolTip);
                var shared = Program.Find<ShanheRewardChoiceCard>(body).Single();
                var stamp = Program.Find<Image>(body).Single(image => image.Name == "MarketPurchasedStamp");
                var purchase = Bound(panel, vm.RequestShanheShopPurchaseCommand, product);
                AssertSharedRewardChoiceFrame(shared, product.Rarity);
                Program.Assert(product.State == actual.State && product.Price == actual.GoldCost && product.HpCost == actual.HpCost &&
                        product.Name == actual.Reward!.Name && product.CanBuy == actual.CanPurchase && purchase.IsEnabled == product.CanBuy &&
                        shared.Heading == product.HeadingText && shared.Title == product.DisplayName && shared.Footer == product.CardStatusText &&
                        shared.TypeIconIndex == 3 && ReferenceEquals(shared.TypeArtwork, WushuangArt.Get("rogue_award_type_icon_3.png")) &&
                        body.IsEnabled && body.Focusable && body.ToolTip is ToolTip && shared.Opacity == 1 && body.Opacity == 1 &&
                        ReferenceEquals(stamp.Source, product.PurchasedStampArtwork) &&
                        ReferenceEquals(stamp.Source, WushuangArt.Get("rogue1v1_had_buy.png")) &&
                        product.ShowPurchasedStamp == (actual.State == ShanheShopItemState.Purchased) &&
                        stamp.Visibility == (actual.State == ShanheShopItemState.Purchased ? Visibility.Visible : Visibility.Collapsed),
                    "The real shop skill retains its authoritative product identity, price and purchase permission while reusing the exact quality/type frame, readable status and a separate focusable rule body; the original purchased stamp follows only a completed purchase.");
                if (actual.State == ShanheShopItemState.Purchased)
                    Program.Assert(!purchase.IsEnabled && !vm.RequestShanheShopPurchaseCommand.CanExecute(product) &&
                            stamp.ActualWidth > 0 && stamp.ActualHeight > 0 && stamp.Source is BitmapSource { PixelWidth: 156, PixelHeight: 72 },
                        "Only an actual confirmed purchase displays the original 156-by-72 purchased stamp; its disabled buy control does not disable or dim the reading card.");
                if (actual.State == ShanheShopItemState.AlreadyOwned)
                    Program.Assert(!purchase.IsEnabled && !product.ShowPurchasedStamp && stamp.Visibility == Visibility.Collapsed &&
                            shared.Footer == "已拥有" && !vm.RequestShanheShopPurchaseCommand.CanExecute(product),
                        "The naturally pre-owned guard skill remains readable with its own already-owned status and never borrows the purchased stamp before spending coins.");
            }
        }
        finally { panel.DataContext = null; }
    }

    private static ShanheShopItem SkillMarketProduct(MainViewModel vm, ShanheCampaignSession session, string rewardId)
    {
        var id = session.Snapshot.ShopItems.Single(product => product.Reward?.Id == rewardId).ChoiceId;
        return vm.ShanheShopItems.Single(product => product.Id == id);
    }

    private static ShanheCampaignSession CreateSkillMarketSession(bool fillSkillSlot)
    {
        var definition = new ShanheCampaignDefinition("fixture:shanhe-ui-skill-shop", "渡口集市", [Player], ["prepare"],
        [
            new("prepare", "营前整备", ShanheNodeKind.Event, ["shop"],
                Choices: [new("ready", "整备出发", RewardId: fillSkillSlot ? "guard" : null)]),
            new("shop", "渡口集市", ShanheNodeKind.Shop, ["battle"],
                Choices: [new("learn", "传习·振奋", GoldCost: 25, RewardId: "learned", LeavesNode: false),
                    new("known", "既有守御", GoldCost: 25, RewardId: "guard", LeavesNode: false)]),
            new("battle", "渡口守军", ShanheNodeKind.Battle, [], [new("gate", "守军", [new(Enemy)])])
        ],
        [
            new("guard", "既有守御", SkillId: Guard) { Description = "已有额外技能，占用一个槽。" },
            new("learned", "传习·振奋", SkillId: Learned) { Description = "习得振奋，占用一个槽。" }
        ], StartingGold: 80, ExtraSkillSlots: 1);
        var session = ShanheCampaignSession.Create(definition, new(71, Player)
            { UseCodexRewardPool = true, PersistentMarket = true }, _ => FixtureRegistry.Value);
        Accept(session, new MoveShanheNodeCommand("prepare", session.Revision));
        Accept(session, new ChooseShanheNodeOptionCommand("ready", null, session.Revision));
        Program.Assert(session.HasPersistentMarket && session.Snapshot.Phase == ShanheCampaignPhase.Map &&
            session.Snapshot.ShopItems.Count == 2, "The fixture's current permanent shelf retains both original skill positions after the map preparation event.");
        return session;
    }

    public static void BossBundlesRequireEachGroupThenAdvanceThreeChapters(string output)
    {
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var session = CreateThreeBossSession();
            var objective = (TextBlock)panel.FindName("ShanheChapterObjective");
            var ordinaryRevision = Program.Engine(vm).Revision;
            Button? lastContinue = null;
            for (var chapter = 1; chapter <= 2; chapter++)
            {
                var beforeGold = session.Snapshot.Gold;
                var beforeSlots = session.Snapshot.ExtraSkillSlots;
                Accept(session, new MoveShanheNodeCommand("boss" + chapter, session.Revision));
                WinPendingBattle(session);
                Attach(vm, session);
                Layout(panel);
                var summary = (FrameworkElement)panel.FindName("ShanheBossRewardOverlay");
                var cardOverlay = (FrameworkElement)panel.FindName("ShanheRewardOverlay");
                var finish = (Button)panel.FindName("ShanheConfirmBossRewardButton");
                lastContinue = finish;
                var confirmCard = (Button)panel.FindName("ShanheConfirmRewardButton");
                var rewardBack = (Button)panel.FindName("ShanheRewardBackButton");
                Program.Assert(vm.IsShanheBossRewardOpen && !vm.IsShanheRewardOpen &&
                    summary.Visibility == Visibility.Visible && cardOverlay.Visibility == Visibility.Collapsed &&
                    ReferenceEquals(finish.Command, vm.ConfirmShanheBossRewardCommand) && !finish.IsEnabled &&
                    session.Snapshot.Gold == beforeGold + 500 && session.Snapshot.ExtraSkillSlots == beforeSlots + 1 &&
                    vm.ShanheBossFixedGoldText.Contains("500", StringComparison.Ordinal) &&
                    vm.ShanheBossSkillSlotsText.Contains("1", StringComparison.Ordinal) &&
                    session.Snapshot.AvailableNodes.Count == 0 && vm.ShanheBossRewardGroups.Count == 2 &&
                    vm.ShanheChapterTitle == "第" + chapter + "章 · 渡口",
                    "The first two real chapter boss victories show the bound overview, credit 500 coins and one slot once, and keep the next chapter closed.");
                AssertBossObjective(2);
                var initial = Snapshot(session);
                RawClick(finish);
                vm.ConfirmShanheBossRewardCommand.Execute(null);
                Layout(panel);
                Program.Assert(Snapshot(session) == initial && !vm.ConfirmShanheBossRewardCommand.CanExecute(null),
                    "Final continuation must reject direct and button attempts while either boss reward group is unresolved.");
                if (chapter == 1) Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-boss-reward-overview.png"));
                var staleGroup = vm.ShanheBossRewardGroups.Single(group => group.Id == "tactics");
                var staleGroupButton = Bound(panel, vm.OpenShanheBossRewardGroupCommand, staleGroup);
                Click(staleGroupButton, panel);
                Program.Assert(vm.HasShanheBossRewardGroup && vm.IsShanheRewardOpen && !vm.IsShanheBossRewardOpen &&
                    cardOverlay.Visibility == Visibility.Visible && summary.Visibility == Visibility.Collapsed &&
                    ReferenceEquals(rewardBack.Command, vm.CloseShanheBossRewardGroupCommand) && rewardBack.IsEnabled &&
                    vm.ShanheRewardCards.Count == 3 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
                    Snapshot(session) == initial,
                    "Opening a boss reward group must show its three actual candidates and a return button without committing or offering normal decline coins.");
                Click(rewardBack, panel);
                Program.Assert(vm.IsShanheBossRewardOpen && Snapshot(session) == initial &&
                    vm.ShanheBossRewardGroups.All(group => !group.IsResolved),
                    "Returning from an unselected group must preserve both frozen boss groups without granting anything.");
                AssertBossObjective(2);

                foreach (var groupId in new[] { "tactics", "skills" })
                {
                    var group = vm.ShanheBossRewardGroups.Single(item => item.Id == groupId);
                    Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand, group), panel);
                    var card = vm.ShanheRewardCards.First(item => item.IsEnabled);
                    var offered = session.Snapshot.BossRewardSummary!.Groups.Single(item => item.Id == groupId).Offers
                        .Single(reward => reward.Id == card.Id);
                    Program.Assert(vm.ShanheRewardCards.Count == 3 && vm.ShanheRewardCards.All(item => !item.IsSkip &&
                        item.Category == (groupId == "skills" ? "额外技能" : "战法") &&
                        StandardShanheCampaignCatalog.ShopMetadataByRewardId[item.Id].Rarity == 4),
                        "The actual chapter selector presents three highest-quality tactics or native skills, with no physical-card or decline substitute.");
                    var beforeSelection = Snapshot(session);
                    var revision = session.Revision;
                    Click(Bound(panel, vm.SelectShanheRewardCardCommand, card), panel);
                    Program.Assert(card.IsSelected && confirmCard.IsEnabled && Snapshot(session) == beforeSelection,
                        "Selecting a boss candidate must only update the card highlight and confirmation binding.");
                    Click(confirmCard, panel);
                    var resolved = vm.ShanheBossRewardGroups.Single(item => item.Id == groupId);
                    var boundSummary = Bound(panel, vm.OpenShanheBossRewardGroupCommand, resolved);
                    Program.Assert(vm.IsShanheBossRewardOpen && !vm.IsShanheRewardOpen && resolved.IsResolved &&
                        resolved.SelectedName == offered.Name && !boundSummary.IsEnabled &&
                        Program.Find<TextBlock>(boundSummary).Any(text => text.Text == resolved.DisplaySelectedName) &&
                        session.Revision == revision + 1 && session.Snapshot.Phase == ShanheCampaignPhase.Reward &&
                        session.Snapshot.AvailableNodes.Count == 0 && session.Snapshot.Gold == beforeGold + 500 &&
                        session.Snapshot.ExtraSkillSlots == beforeSlots + 1,
                        "Each group confirmation must grant exactly that reward and show its selected name on the actual overview, without advancing the chapter or repeating fixed credits.");
                    AssertBossObjective(groupId == "tactics" ? 1 : 0);
                    var claimed = Snapshot(session);
                    RawClick(confirmCard);
                    RawClick(staleGroupButton);
                    vm.SelectShanheRewardCardCommand.Execute(card);
                    vm.OpenShanheBossRewardGroupCommand.Execute(staleGroup);
                    Layout(panel);
                    Program.Assert(Snapshot(session) == claimed && vm.IsShanheBossRewardOpen,
                        "Retired group and reward buttons cannot re-open or claim an already resolved group.");
                    Program.Assert(groupId != "skills" || session.Snapshot.ExtraSkillIds.Contains(offered.SkillId!),
                        "Confirming the skill selector grants its actual program to the expedition rather than issuing a physical card.");
                    Program.Assert(finish.IsEnabled == (groupId == "skills"),
                        "Final continuation becomes enabled only when both separate reward choices have been claimed.");
                }
                if (chapter == 1) Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-boss-reward-claimed.png"));
                var completedRevision = session.Revision;
                Click(finish, panel);
                Program.Assert(session.Revision == completedRevision + 1 && !vm.IsShanheBossRewardOpen &&
                    session.Snapshot.BossRewardSummary is null && session.Snapshot.CompletedNodeIds.Count == chapter &&
                    session.Snapshot.TacticIds.Count == chapter && session.Snapshot.ExtraSkillIds.Count == chapter &&
                    session.Snapshot.StartingCards.Count == 0 &&
                    Program.Engine(vm).Revision == ordinaryRevision,
                    "The overview confirmation completes one of the first two chapters after its tactic and native skill have been received, leaving the ordinary table untouched.");
                Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map &&
                        session.Snapshot.AvailableNodes.Single().Id == "boss" + (chapter + 1) &&
                        vm.ShanheMapNodes.Select(node => node.Node.Id).SequenceEqual(["boss" + (chapter + 1)]) &&
                        vm.ShanheChapterTitle == "第" + (chapter + 1) + "章 · 渡口" &&
                        vm.ShanheMapNodes.Count(node => node.IsPlayerLocation) == 1 &&
                        vm.ShanheMapNodes.Single(node => node.IsPlayerLocation).IsAvailable &&
                        vm.ShanheMapNodes.Single(node => node.IsPlayerLocation).Node.Id == "boss" + (chapter + 1),
                        "Only the completed overview may reveal the next numbered chapter and move the single player pin to its newly available destination, even while the engine retains the previous boss as its current node.");
                var finished = Snapshot(session);
                RawClick(finish);
                vm.ConfirmShanheBossRewardCommand.Execute(null);
                Layout(panel);
                Program.Assert(Snapshot(session) == finished, "Repeated chapter continuation cannot credit or advance a second time.");
            }
            var finalGold = session.Snapshot.Gold;
            var finalSlots = session.Snapshot.ExtraSkillSlots;
            Accept(session, new MoveShanheNodeCommand("boss3", session.Revision));
            Program.Assert(session.PendingBattleSetup!.ExtraSkillIds.SequenceEqual(session.Snapshot.ExtraSkillIds),
                "The two selected chapter skills enter the final actual battle's native setup.");
            WinPendingBattle(session);
            Attach(vm, session); Layout(panel);
            Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Won && vm.IsShanhePreparation &&
                !vm.IsShanheRunInProgress && vm.CanPrepareShanheExpedition && vm.IsShanheSettlementOpen &&
                ((FrameworkElement)panel.FindName("ShanheSettlementHost")).Visibility == Visibility.Visible &&
                !vm.IsShanheBossRewardOpen && !vm.IsShanheRewardOpen &&
                ((FrameworkElement)panel.FindName("ShanheBossRewardOverlay")).Visibility == Visibility.Collapsed &&
                ((FrameworkElement)panel.FindName("ShanheRewardOverlay")).Visibility == Visibility.Collapsed &&
                session.Snapshot.BossRewardSummary is null && session.Snapshot.RewardOffers.Count == 0 &&
                session.Snapshot.Gold == finalGold + 500 && session.Snapshot.ExtraSkillSlots == finalSlots &&
                session.Snapshot.CompletedNodeIds.Count == 3 && session.Snapshot.TacticIds.Count == 2 &&
                session.Snapshot.ExtraSkillIds.Count == 2 && session.Snapshot.StartingCards.Count == 0 &&
                session.AcceptedCommands.OfType<ChooseShanheBossRewardCommand>().Count() == 4 &&
                session.AcceptedCommands.OfType<ConfirmShanheBossRewardCommand>().Count() == 2 &&
                Program.Engine(vm).Revision == ordinaryRevision,
                "The third actual boss victory directly opens final settlement, pays copper once and grants no further tactic, skill, card or slot.");
            Program.Assert(objective.Text == vm.ShanheChapterObjectiveText &&
                    objective.Text.Contains("已完成", StringComparison.Ordinal) && !objective.Text.Contains("下一章", StringComparison.Ordinal) &&
                    ((Border)panel.FindName("ShanheChapterObjectiveBanner")).Visibility == Visibility.Collapsed,
                "The actual final boss enters its existing direct settlement path: the retained objective says the expedition is complete, and no next-chapter banner remains on the finished preparation screen.");
            var finalSnapshot = Snapshot(session);
            RawClick(lastContinue!); vm.ConfirmShanheBossRewardCommand.Execute(null); Layout(panel);
            Program.Assert(Snapshot(session) == finalSnapshot,
                "A stale chapter continuation button cannot reopen or re-credit the final settlement.");
            vm.SelectedShanheScenario = vm.ShanheScenarios.Single(s => s.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            Program.Assert(vm.ShanheScenarioDetails == StandardShanheCampaignCatalog.CreateCodexDefinition(
                vm.SelectedShanheDifficulty.Value, StandardShanheCampaignCatalog.Returning2026EditionId).IntroText,
                "Completed preparation details follow the newly selected scenario rather than the retained finished session.");

            void AssertBossObjective(int expectedUnresolved)
            {
                var checkpointBeforeReading = Snapshot(session);
                var summary = session.Snapshot.BossRewardSummary!;
                var unresolved = summary.Groups.Count(group => !group.IsResolved);
                var target = summary.IsBonusOnly ? "继续行军" : summary.ChapterNumber >= 3 ? "完成征程" : "进入下一章";
                var expected = unresolved > 0 ? $"还有{unresolved}组奖励待处理，完成后{target}" : $"本次奖励已处理，确认后{target}";
                var finish = (Button)panel.FindName("ShanheConfirmBossRewardButton");
                Program.Assert(vm.IsShanheBossRewardOpen && unresolved == expectedUnresolved &&
                        objective.Text == expected && objective.Text == vm.ShanheChapterObjectiveText &&
                        BindingOperations.GetBinding(objective, TextBlock.TextProperty)?.Path.Path == nameof(MainViewModel.ShanheChapterObjectiveText) &&
                        finish.Content?.ToString()?.Replace(" ", "", StringComparison.Ordinal) == target &&
                        finish.IsEnabled == (unresolved == 0) && Snapshot(session) == checkpointBeforeReading,
                    "The actual boss overview header counts only unresolved snapshot groups and states its real confirmation destination without changing reward state.");
            }
        }
        finally { panel.DataContext = null; }
    }

    private static ShanheCampaignSession CreateThreeBossSession(bool includeBonusSkill = false)
    {
        string[] tacticRewardIds = [StandardShanheCampaignCatalog.CurrentDangtou2RewardId,
            StandardShanheCampaignCatalog.CurrentLeihuo2RewardId, StandardShanheCampaignCatalog.CurrentLianji2RewardId,
            StandardShanheCampaignCatalog.CurrentDuoduoYishan3RewardId, StandardShanheCampaignCatalog.CurrentYaoli4RewardId,
            StandardShanheCampaignCatalog.CurrentYaoli6RewardId];
        string[] skillRewardIds = ["chapter-skill-3017", "chapter-skill-3038", "chapter-skill-3091",
            "chapter-skill-3020", "chapter-skill-3070", "chapter-skill-3261"];
        var rewards = new List<ShanheRewardDefinition>();
        var nodes = new List<ShanheNodeDefinition>();
        for (var chapter = 1; chapter <= 3; chapter++)
        {
            var tacticIds = new List<string>();
            var skillIds = new List<string>();
            for (var choice = 1; chapter < 3 && choice <= 3; choice++)
            {
                var ordinal = (chapter - 1) * 3 + choice;
                var tacticId = tacticRewardIds[ordinal - 1];
                var skillId = skillRewardIds[ordinal - 1];
                tacticIds.Add(tacticId); skillIds.Add(skillId);
                rewards.Add(new(tacticId, "战法·军阵" + ordinal)
                    { TacticSkillId = "fixture:shanhe-ui-tactic" + ordinal, Description = "本次征程每战攻击范围加一。" });
                rewards.Add(new(skillId, "传习·军阵" + ordinal, SkillId: "fixture:shanhe-ui-reward-skill" + ordinal)
                    { Description = "实际获得一个额外技能，后续战斗攻击范围加一，占一个技能槽。" });
            }
            nodes.Add(new("boss" + chapter, "渡口首领" + chapter, ShanheNodeKind.BossBattle,
                chapter == 3 ? [] : ["boss" + (chapter + 1)],
                [new("encounter" + chapter, "守军", [new(Enemy)], VictoryGold: chapter == 3 ? 500 : 7)],
                RewardIds: chapter == 3 ? skillRewardIds : null)
            {
                ChapterNumber = chapter, ChapterTitle = "第" + chapter + "章 · 渡口",
                BossRewards = chapter < 3 ? new(500, 1, [new("tactics", "传说战法", tacticIds), new("skills", "传说技能", skillIds)]) : null,
                StoryText = "守军已退，整理本章收获。"
            });
        }
        var definition = new ShanheCampaignDefinition("fixture:shanhe-ui-boss-bundles", "三章渡口", [Player], ["boss1"],
            nodes, rewards, StartingGold: 10, ExtraSkillSlots: 1);
        var equipped = Array.Empty<string>();
        if (includeBonusSkill)
        {
            var source = StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(source => source.SourceId == 2014);
            definition = definition with { Rewards = definition.Rewards.Concat(new ShanheRewardDefinition[]
            {
                new(source.RewardId, source.Name) { CampaignEconomy = source.Effect, Rarity = source.Rarity },
                new("bonus-epic-skill", "追加·振奋", SkillId: Learned) { Rarity = 3 }
            }).ToArray() };
            equipped = [source.RewardId];
        }
        return ShanheCampaignSession.Create(definition, new(71, Player) { EquippedTacticIds = equipped }, _ => FixtureRegistry.Value);
    }

    private static ShanheCampaignSession WinRewardSession(bool fillSkillSlot = false)
    {
        var definition = new ShanheCampaignDefinition("fixture:shanhe-ui-rewards", "渡口归来", [Player], ["prepare"],
        [
            new("prepare", "营前整备", ShanheNodeKind.Event, ["battle"],
                Choices: [new("ready", "整备出发", RewardId: fillSkillSlot ? "guard" : null)]),
            new("battle", "渡口守军", ShanheNodeKind.Battle, ["home"],
                [new("gate", "守军", [new(Enemy)], VictoryGold: 7)], ["learned", "draw", "medicine"])
                { RewardChoiceCount = 3, RewardSkipGold = 100, StoryText = "守军已退，整理此战收获。" },
            new("home", "河畔营火", ShanheNodeKind.Camp, [], Choices: [new("finish", "继续前行")])
        ],
        [
            new("guard", "既有守御", SkillId: Guard) { Description = "已有的额外技能，占用一个技能槽。" },
            new("learned", "传习·振奋", SkillId: Learned) { Description = "获得振奋技能，攻击范围加一；占用一个额外技能槽。" },
            new("draw", "战备连弩", StartingEquipment: [new(CardKind.Crossbow)]) { Description = "此后每场战斗携带诸葛连弩。" },
            new("medicine", "随身药囊", StartingCards: [new(CardKind.Peach, Suit.Heart, 3)])
                { Description = "此后每场战斗开局额外获得一张桃。" }
        ], StartingGold: 10, ExtraSkillSlots: 1);
        var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => FixtureRegistry.Value);
        Accept(session, new MoveShanheNodeCommand("prepare", session.Revision));
        Accept(session, new ChooseShanheNodeOptionCommand("ready", null, session.Revision));
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        WinPendingBattle(session);
        Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Reward && session.Snapshot.RewardOffers.Count == 3 &&
            session.Snapshot.Gold == 17,
            "Reward UI setup must come from an actual fully resolved native victory and its validated battle journal.");
        return session;
    }

    private static void WinPendingBattle(ShanheCampaignSession session, ContentRegistry? registry = null)
    {
        var setup = session.PendingBattleSetup!;
        var battle = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false,
            AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 8
        }, registry ?? FixtureRegistry.Value);
        Accept(battle, new StartGameCommand());
        for (var step = 0; step < 40 && battle.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            Accept(battle, new AdvanceOneStepCommand(battle.Revision));
        Program.Assert(battle.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 },
            "The small native fixture must reach its real human play prompt without a seed search.");
        Accept(battle, new UseProgramSkillCommand(0, Force, "victory", [], [1], battle.Revision, battle.PendingDecision!.PromptId));
        for (var step = 0; step < 40 && battle.State.Status != EngineStatus.Completed; step++)
        {
            // A supplied Peach in any fixture recipe can open the real rescue prompt.
            // Decline its published rescue instead of waiting at a human prompt.
            if (battle.PendingDecision is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 } rescue)
                Accept(battle, new AnswerPromptCommand(rescue.PlayerSeat, rescue.PromptId,
                    rescue.Choices.Single(choice => choice.Parameters.GetValueOrDefault("response") == "let-die").Id,
                    battle.Revision));
            else Accept(battle, new AdvanceOneStepCommand(battle.Revision));
        }
        Program.Assert(battle.State.Status == EngineStatus.Completed,
            $"The native UI battle fixture at {setup.NodeId} must finish; actual status {battle.State.Status}.");
        var completion = session.CompleteBattle(battle, session.Revision);
        Program.Assert(completion.Accepted,
            $"Reward UI native victory at {setup.NodeId} failed validation: {completion.Error}");
    }

    private static void AssertNativeRewardTooltip(MainViewModel vm, ShanheCampaignSession session, ICommand command,
        object parameter, string expectedText, string output, string label)
    {
        var before = Snapshot(session);
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var ordinary = Program.Engine(vm);
        var ordinaryCheckpoint = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
        using (var host = new NativeResponseStatusChecks.NativeHost(vm))
        {
            // The supplied live offer is already open. Reopening the campaign here
            // would refresh its card identities and invalidate the caller's draft.
            host.Root.Width = 1120; host.Root.Height = 780;
            host.Window.SizeToContent = SizeToContent.WidthAndHeight;
            Flush();
            var panel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
            var button = Bound(panel, command, parameter);
            Program.Assert(button.IsVisible && button.IsEnabled && button.ToolTip is ToolTip,
                "The actual enabled reward or replacement option must expose its own native complete-rule tooltip.");
            if (parameter is ShanheRewardCard)
            {
                Program.Assert(vm.ShanheRewardCards.Count == 4 && session.Snapshot.RewardOffers.Count == 3 &&
                        session.Snapshot.RewardOffers.Single(reward => reward.Id == "learned").SkillId == Learned &&
                        session.Snapshot.RewardOffers.Single(reward => reward.Id == "draw").StartingEquipment?.Single().Kind == CardKind.Crossbow &&
                        session.Snapshot.RewardOffers.Single(reward => reward.Id == "medicine").StartingCards?.Single().Kind == CardKind.Peach,
                    "The existing native reward fixture retains its actual skill, Crossbow, Peach and separate decline offers while their category icons are inspected.");
                foreach (var candidate in vm.ShanheRewardCards)
                {
                    var expectedIconIndex = candidate.Id switch
                    {
                        "learned" => 3,
                        "draw" or "medicine" => 4,
                        "reward:skip" => 0,
                        _ => throw new InvalidOperationException("Unexpected reward in the existing native tooltip fixture: " + candidate.Id)
                    };
                    var expectedDisplayName = candidate.Id switch
                    {
                        "learned" => "振奋",
                        "draw" => "战备连弩",
                        "medicine" => "随身药囊",
                        _ => candidate.Name
                    };
                    var owner = Bound(panel, vm.SelectShanheRewardCardCommand, candidate);
                    var shared = Program.Find<ShanheRewardChoiceCard>(owner).Single();
                    var originalIcon = WushuangArt.Get($"rogue_award_type_icon_{expectedIconIndex}.png");
                    Program.Assert(originalIcon is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 },
                        "The actual native reward category must resolve its original type-icon PNG.");
                    var icon = Program.Find<Image>(shared).Single(image => ReferenceEquals(image.Source, originalIcon));
                    var retained = candidate.IsSkip ? null : session.Snapshot.RewardOffers.Single(reward => reward.Id == candidate.Id);
                    Program.Assert(candidate.TypeIconIndex == expectedIconIndex && shared.TypeIconIndex == expectedIconIndex &&
                            ReferenceEquals(shared.TypeArtwork, originalIcon) && icon.IsVisible &&
                            icon.ActualWidth > 0 && icon.ActualHeight > 0 && PresentationSource.FromVisual(icon) is HwndSource &&
                            candidate.DisplayName == expectedDisplayName && shared.Title == expectedDisplayName &&
                            shared.FallbackText == expectedDisplayName &&
                            (candidate.IsSkip || candidate.Name == retained!.Name && candidate.Id == retained.Id) &&
                            ReferenceEquals(owner.Command, vm.SelectShanheRewardCardCommand) && ReferenceEquals(owner.CommandParameter, candidate),
                        "The native skill, equipment, hand-card and generic-decline faces render their exact original 3/4/0 type icons and display-only names while retaining the actual reward identity and bound selection command: " +
                        JsonSerializer.Serialize(new { candidate.Id, candidate.Name, candidate.DisplayName, candidate.Category,
                            candidate.TypeIconIndex, ExpectedIconIndex = expectedIconIndex, shared.Title, shared.FallbackText,
                            RetainedName = retained?.Name, icon.IsVisible, icon.ActualWidth, icon.ActualHeight }));
                }
            }
            var tooltip = (ToolTip)button.ToolTip;
            tooltip.PlacementTarget = button;
            tooltip.Placement = PlacementMode.AbsolutePoint; tooltip.HorizontalOffset = -30000; tooltip.VerticalOffset = -30000;
            tooltip.Opened += Conceal;
            try
            {
                tooltip.IsOpen = true; Flush();
                var visibleText = Program.Find<TextBlock>(tooltip).Where(text =>
                    text.IsVisible && text.ActualWidth > 0 && text.ActualHeight > 0).ToArray();
                Program.Assert(tooltip.IsOpen && tooltip.ContentTemplate is null &&
                    PresentationSource.FromVisual(tooltip) is HwndSource &&
                    visibleText.Any(text => text.Text == expectedText) &&
                    visibleText.All(text => !text.Text.StartsWith("System.Windows.Controls.", StringComparison.Ordinal)),
                    "The opened reward and replacement tooltips must visually render the full actual rule instead of their ScrollViewer or TextBlock type names.");
                if (parameter is ShanheRewardCard)
                    Program.Assert(tooltip.Content is ScrollViewer { MaxHeight: 480, Content: TextBlock rewardText } &&
                        rewardText.Text == expectedText && rewardText.TextWrapping == TextWrapping.Wrap,
                        "The actual reward tooltip retains its bounded scroll surface and complete wrapped RuleTooltip.");
                else
                    Program.Assert(tooltip.Content is TextBlock { MaxWidth: 360, TextWrapping: TextWrapping.Wrap } replacementText &&
                        replacementText.Text == expectedText,
                        "The explicit replacement tooltip retains the option's wrapped description and existing maximum text width.");
                Program.Render(tooltip, (int)Math.Ceiling(tooltip.ActualWidth), (int)Math.Ceiling(tooltip.ActualHeight),
                    Path.Combine(output, label + ".png"));
            }
            finally { tooltip.IsOpen = false; tooltip.Opened -= Conceal; }

            void Flush() { host.Root.UpdateLayout(); host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); host.Root.UpdateLayout(); }
            void Conceal(object sender, RoutedEventArgs args)
            { if (PresentationSource.FromVisual((ToolTip)sender) is HwndSource source) ShowRewardTooltipWindow(source.Handle, 0); }
        }
        Program.Assert(Snapshot(session) == before && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
            ReferenceEquals(Program.Engine(vm), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryCheckpoint,
            "Opening and closing the real rule tooltips must not issue a reward or replacement command or alter either live game's state.");
    }

    [DllImport("user32.dll", EntryPoint = "ShowWindow")]
    private static extern bool ShowRewardTooltipWindow(nint handle, int command);

    private static void Attach(MainViewModel vm, ShanheCampaignSession session)
    {
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
        vm.OpenShanheCommand.Execute(null);
    }
    private static Button Bound(FrameworkElement root, ICommand command, object parameter) =>
        Program.Find<Button>(root).Single(button => ReferenceEquals(button.Command, command) && ReferenceEquals(button.CommandParameter, parameter));
    private static string Snapshot(ShanheCampaignSession session) => JsonSerializer.Serialize(session.Snapshot);
    private static void Click(Button button, FrameworkElement root)
    {
        Program.Assert(button.IsEnabled && button.Command?.CanExecute(button.CommandParameter) == true,
            "The actual reward button must be enabled and correctly bound before its click.");
        RawClick(button);
        Layout(root);
    }
    private static void RawClick(Button button) =>
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    private static void Layout(FrameworkElement root)
    {
        root.Measure(new Size(1440, 880)); root.Arrange(new Rect(0, 0, 1440, 880)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var result = session.Submit(command);
        Program.Assert(result.Accepted, result.Error ?? "Campaign fixture command rejected.");
    }
    private static void Accept(GameEngine battle, GameCommand command)
    {
        var result = battle.Submit(command);
        Program.Assert(result.Accepted, result.Error?.ToString() ?? "Native battle fixture command rejected.");
    }

    private sealed class RewardBattlePackage : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-reward-ui", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rangeSkillIds = Enumerable.Range(1, 9).Select(ordinal => "fixture:shanhe-ui-tactic" + ordinal)
                .Concat(Enumerable.Range(1, 6).Select(ordinal => "fixture:shanhe-ui-reward-skill" + ordinal)).ToArray();
            var programs = new object[]
            {
                new { id = Force, revision = 1, activations = new[]
                {
                    new { id = "victory", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                        targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "loseHp", target = "selectedTarget", amount = 20 } } }
                } },
                new { id = Guard, revision = 1, modifiers = new[] { new { id = "guard", query = "attackRange", operation = "add", value = 1, priority = 0 } } },
                new { id = Learned, revision = 1, modifiers = new[] { new { id = "learned", query = "attackRange", operation = "add", value = 1, priority = 0 } } }
            }.Concat(rangeSkillIds.Select(id => (object)new
            {
                id, revision = 1,
                modifiers = new[] { new { id = "range", query = "attackRange", operation = "add", value = 1, priority = 0 } }
            })).ToArray();
            var catalog = SkillProgramCatalog.Load(
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = programs }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Force, Guard, Learned }.Concat(rangeSkillIds)
                        .ToDictionary(id => id, id => new { name = id, description = "Small native reward interaction fixture" }) }));
            foreach (var entry in catalog.Programs)
                builder.AddSkill(new(entry.Key, entry.Key == Learned ? "振奋" : entry.Key == Guard ? "守御" : "破阵", "攻击范围加一。") { Program = entry.Value });
            builder.AddGeneral(new(Player, "义军先锋", "", Force, "shu"));
            builder.AddGeneral(new(Enemy, "渡口守军", "", Guard, "qun"));
            builder.AddDeck(new("fixture:shanhe-ui-dodge", "闪牌堆", 4, 2, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河奖励交互", 2, 2, new Dictionary<string, int>(),
                "fixture:shanhe-ui-dodge", GeneralPoolIds: [Player, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
