using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void AutomaticRareEliteOffersAllowCardCompensationAndRequireTacticClaim(string output)
    {
        var tactics = Enumerable.Range(1, 3).Select(index => new ShanheRewardDefinition("rare-tactic-" + index,
            "精英军阵" + index, Bonuses: new(Draw: index)) { Rarity = 2 }).ToArray();
        var cards = new[] { CardKind.FiveGrains, CardKind.Duel, CardKind.Peach }.Select((kind, index) =>
            new ShanheRewardDefinition("rare-card-" + index, CardCatalog.Get(kind).DisplayName,
                StartingCards: [new(kind)]) { Rarity = 2 }).ToArray();
        ShanheRewardDefinition[] common =
        [
            new("common-tactic", "普通军阵", Bonuses: new(Draw: 1)) { Rarity = 1 },
            new("common-card", "普通随身杀", StartingCards: [new(CardKind.Slash)]) { Rarity = 1 }
        ];
        var rewards = tactics.Concat(cards).Concat(common).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:shanhe-rare-elite-rewards", "精英凯旋", [Player], ["elite"],
        [
            new("elite", "精英守军", ShanheNodeKind.EliteBattle, ["home"],
                [new("guard", "守军", [new(Enemy)], VictoryGold: 200)], rewards.Select(reward => reward.Id).ToArray())
            {
                CategorizedRewards = true, AutomaticRewardCategory = true, RewardChoiceCount = 3, RewardSkipGold = 100,
                PrimaryTacticRarity = 2, PrimarySkillRarity = 2, PrimaryCardRarity = 2
            },
            new("home", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])
        ], rewards, StartingGold: 10);
        using var vm = new MainViewModel(false, 270028, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var session = EliteRewardStage(ShanheRewardCategory.Card); Attach(vm, session); Layout(panel);
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            Program.Assert(session.Snapshot.Gold == 210 && session.Snapshot.RewardOffers.Count == 3 &&
                session.Snapshot.RewardCategories.Count == 0 && vm.ShanheRewardCards.All(card => card.RewardCategoryChoice is null),
                "The elite reward stage opens its predetermined rare card candidates directly, without a category-selection page.");
            var physical = vm.ShanheRewardCards.Where(card => !card.IsSkip).ToArray();
            var skip = vm.ShanheRewardCards.Single(card => card.IsSkip);
            var renderPath = Path.Combine(output, "shanhe-rare-elite-card-compensation.png");
            Program.Render(panel, 1440, 880, renderPath);
            Program.Assert(physical.Length == 3 && physical.All(card => card.Rarity == 2 && card.HasCardImage &&
                    card.HeadingText == "稀有 · 随身牌") && session.Snapshot.RewardOffers.All(reward => reward.Rarity == 2) &&
                physical.All(card => Program.Find<TextBlock>(panel).Any(text => text.Text == card.HeadingText &&
                    text.Visibility == Visibility.Visible && text.ActualHeight > 0)) &&
                skip.Name.Contains("稀有随身牌", StringComparison.Ordinal) && skip.BonusText.Contains("100", StringComparison.Ordinal) && File.Exists(renderPath),
                "The predetermined card reward shows three actual rare physical cards, rendered rare type labels and the separate hundred-coin decline.");
            var gold = session.Snapshot.Gold;
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, skip), panel);
            Click(confirm, panel);
            Program.Assert(session.Snapshot.Gold == gold + 100 && session.Snapshot.StartingCards.Count == 0 &&
                session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "home" &&
                session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Count() == 1,
                "The actual decline and confirmation buttons pay once without granting a card and open the next route.");

            session = EliteRewardStage(ShanheRewardCategory.Tactic); Attach(vm, session); Layout(panel);
            Program.Assert(vm.ShanheRewardCards.Count == 3 && vm.ShanheRewardCards.All(card => !card.IsSkip &&
                    card.RewardCategoryChoice is null && card.Rarity == 2 && card.HeadingText == "稀有 · 战法") &&
                session.Snapshot.RewardCategories.Count == 0 && !session.Snapshot.CanSkipReward &&
                vm.ShanheRewardNotice.Contains("必须领取", StringComparison.Ordinal),
                "A predetermined tactic reward directly shows three rare tactics and keeps tactic compensation unavailable.");
            var chosen = vm.ShanheRewardCards[0];
            gold = session.Snapshot.Gold;
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, chosen), panel);
            Click(confirm, panel);
            Program.Assert(session.Snapshot.Gold == gold && session.Snapshot.Bonuses.Draw > 0 &&
                session.Snapshot.Phase == ShanheCampaignPhase.Map && !session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Any(),
                "The mandatory tactic is granted through its real confirmation without also paying decline compensation.");

        }
        finally { panel.DataContext = null; }

        ShanheCampaignSession EliteRewardStage(ShanheRewardCategory category)
        {
            // Bounded map-only probes choose deterministic coverage without
            // constructing or driving a battle engine for seed exploration.
            var session = Enumerable.Range(1, 10).Select(seed => ShanheCampaignSession.Create(definition,
                new(seed, Player), _ => FixtureRegistry.Value)).First(candidate => candidate.PreviewRewardCategory("elite") == category);
            Accept(session, new MoveShanheNodeCommand("elite", session.Revision));
            // Seed only this UI fixture's precredited reward boundary. The real
            // Move supplied its locked node; the production reward planner and
            // every category, decline and claim command below remain unchanged.
            var type = typeof(ShanheCampaignSession);
            var fields = BindingFlags.Instance | BindingFlags.NonPublic;
            var stateField = type.GetField("_state", fields)!;
            var original = stateField.GetValue(session)!;
            var state = original.GetType().GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public)!
                .Invoke(original, null)!;
            state.GetType().GetProperty("BattleSetup")!.SetValue(state, null);
            state.GetType().GetProperty("Gold")!.SetValue(state, definition.StartingGold + 200);
            var node = session.Definition.Nodes.Single(node => node.Id == "elite");
            var next = type.GetMethod("BeginBattlePrimaryRewards", fields)!.Invoke(session, [state, node])!;
            var snapshot = type.GetMethod("CreateSnapshot", fields)!.Invoke(session, [next, session.Revision]);
            stateField.SetValue(session, next);
            type.GetField("_snapshot", fields)!.SetValue(session, snapshot);
            return session;
        }
    }
}
