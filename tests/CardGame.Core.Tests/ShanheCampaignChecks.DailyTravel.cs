using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ShanheCampaignChecks
{
    public static void MarketTravelUpgradeExhaustsFrozenRewardsWithoutBlockingTheRouteCold()
    {
        var first = StandardShanheCampaignCatalog.CurrentDailyTravelRewardId;
        var second = StandardShanheCampaignCatalog.CurrentDailyTravel2RewardId;
        var travel = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => source.Effect.ExtraChapterBattles > 0)
            .Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
                { CampaignEconomy = source.Effect, Rarity = source.Rarity }).ToArray();
        foreach (var mode in new[] { "ordinary", "category", "grouped" })
        {
            var grouped = mode == "grouped";
            var definition = new ShanheCampaignDefinition("fixture:market-travel-offer-" + mode, "战利品期间升级", [Player], ["shop"],
            [
                new("shop", "沿途集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("upgrade", "日行千里Ⅱ", 700, RewardId: second, LeavesNode: false)]) { ChapterNumber = 1 },
                new("battle", "待领战利品", grouped ? ShanheNodeKind.BossBattle : ShanheNodeKind.Battle, ["next"],
                    [new("garrison", "驻军", [new(Enemy)], VictoryGold: 200)], grouped ? [] : [first])
                {
                    ChapterNumber = 1, CategorizedRewards = mode == "category",
                    BossRewards = grouped ? new(500, 0, [new("travel", "日行千里", [first], 1)]) : null
                },
                new("next", "继续行军", ShanheNodeKind.Camp, [], Choices: [new("go", "继续")]) { ChapterNumber = 1 }
            ], travel, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(71, Player)
                { UseCodexRewardPool = true, PersistentMarket = true }, Registry);
            Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
            var battle = CreateBattle(session.PendingBattleSetup!); FinishBattle(battle, "defeat");
            Require(session.CompleteBattle(battle, session.Revision).Accepted, "Only a genuine native victory freezes the travel reward.");
            if (mode == "category") Accept(session, new ChooseShanheRewardCategoryCommand(ShanheRewardCategory.Tactic, session.Revision));
            Require(session.Snapshot.Phase == ShanheCampaignPhase.Reward && !session.Snapshot.CanSkipReward &&
                (grouped ? session.Snapshot.BossRewardSummary!.Groups.Single().Offers.Single().Id : session.Snapshot.RewardOffers.Single().Id) == first,
                "The actual frozen tier-I tactic is compulsory before the player purchases its higher tier from the still-open market.");
            Cold(session, definition);
            var quote = session.Snapshot.ShopItems.Single(item => item.Reward.Id == second);
            Accept(session, new CardGame.Core.Campaigns.BuyShanheMarketItemCommand(quote.ChoiceId, null, session.Revision));
            Require(session.ChapterExtraBattleCount == 2 && session.Snapshot.TacticIds.SequenceEqual([second]) &&
                session.Snapshot.Gold == (grouped ? 800 : 500),
                "The purchase spends seven hundred once and cannot mint compensation or grant the now-obsolete lower tier.");
            if (grouped)
            {
                Require(session.Snapshot.BossRewardSummary is { CanConfirm: true } summary &&
                    summary.Groups.Single().Offers.Count == 0 && !session.Snapshot.CompletedNodeIds.Contains("battle"),
                    "An exhausted grouped offer remains explicitly confirmable without prematurely completing its owning battle.");
                Cold(session, definition);
                Accept(session, new ConfirmShanheBossRewardCommand(session.Revision));
            }
            Require(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "next" &&
                session.Snapshot.CompletedNodeIds.Count(id => id == "battle") == 1,
                "Exhausting the frozen offer resumes the original route exactly once and does not reroll another reward.");
            Cold(session, definition);
            session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, Registry);
            var before = Snapshot(session);
            Require(!session.Submit(new CardGame.Core.Campaigns.BuyShanheMarketItemCommand(quote.ChoiceId, null, session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCommand(first, null, session.Revision)).Accepted && Snapshot(session) == before,
                "Cold recovery preserves the one payment and consumed reward boundary; neither can be replayed to duplicate or downgrade the tactic.");
        }
    }

    public static void SixMainlineStepsGateMarketAndLeaveOptionalBattlesSkippableCold()
    {
        const string route = "fixture:chapter-six";
        ShanheNodeDefinition Meta(ShanheNodeDefinition node, ShanheChapterNodeRole role, int step = 0, int required = 0) =>
            node with { ChapterNumber = 1, ChapterRouteId = route, ChapterRole = role, ChapterProgressStep = step, RequiredChapterProgress = required };
        ShanheNodeDefinition Battle(string id, ShanheChapterNodeRole role, int step, int required, string[] next) =>
            Meta(new(id, id, role == ShanheChapterNodeRole.Boss ? ShanheNodeKind.BossBattle : ShanheNodeKind.Battle,
                next, [new(id + ":enemy", "驻军", [new(Enemy)], VictoryGold: 200)]), role, step, required);
        ShanheNodeDefinition Story(string id, int step, string[] next) => Meta(new(id, id, ShanheNodeKind.Event, next,
            Choices: [new("go", "继续")]), ShanheChapterNodeRole.Event, step, step - 1);
        var definition = new ShanheCampaignDefinition("fixture:six-mainline", "六格行军", [Player], ["opening1", "opening2"],
        [
            Battle("opening1", ShanheChapterNodeRole.OpeningBattle, 1, 0, ["story1"]),
            Battle("opening2", ShanheChapterNodeRole.OpeningBattle, 1, 0, ["story1"]),
            Story("story1", 2, ["story2"]), Story("story2", 3, ["shop", "arena", "story3"]),
            Story("story3", 4, ["main"]), Battle("main", ShanheChapterNodeRole.MainBattle, 5, 4, ["rest"]),
            Meta(new("rest", "休息", ShanheNodeKind.Camp, ["optional", "boss"], Choices:
                [new("draw", "摸牌+1", RewardId: "rest-draw"), new("maxhp", "体力上限+1", RewardId: "rest-hp")]), ShanheChapterNodeRole.Rest, 6, 5),
            Battle("optional", ShanheChapterNodeRole.OptionalBattle, 0, 6, ["boss"]),
            Battle("arena", ShanheChapterNodeRole.Arena, 0, 3, ["story3"]),
            Battle("boss", ShanheChapterNodeRole.Boss, 0, 6, []),
            Meta(new("shop", "集市", ShanheNodeKind.Shop, ["story3"], Choices:
                [new("buy", "补给", 100, RewardId: "supply", LeavesNode: false)]), ShanheChapterNodeRole.Market, 0, 3)
        ], [new("rest-draw", "摸牌成长", Bonuses: new(Draw: 1)), new("rest-hp", "体力上限成长", Bonuses: new(MaxHp: 1)),
            new("supply", "军资", StartingCards: [new(CardKind.Peach)])], StartingGold: 1000)
        { RandomStartingNode = true };
        var untyped = Meta(new("untyped", "未声明地点", ShanheNodeKind.Event, ["story3"],
            Choices: [new("go", "继续")]), ShanheChapterNodeRole.None);
        foreach (var malformed in new[]
        {
            definition with { Nodes = definition.Nodes.Append(untyped).ToArray(),
                StartingNodeIds = definition.StartingNodeIds.Append(untyped.Id).ToArray() },
            definition with { Nodes = definition.Nodes.Select(node => node.Id == "story1" ? node with
                { Kind = ShanheNodeKind.Battle, Choices = [], Encounters = [new("wrong-kind", "驻军", [new(Enemy)])] } : node).ToArray() }
        })
        {
            var rejected = false;
            try { _ = ShanheCampaignContent.FreezeAndValidate(malformed); }
            catch (ArgumentException) { rejected = true; }
            Require(rejected, "The loader rejects an untyped route member or a story role that impersonates an actual battle.");
        }
        var session = ShanheCampaignSession.Create(definition, new(71, Player)
            { UseCodexRewardPool = true, PersistentMarket = true }, Registry);
        Require(session.ChapterProgress == 0 && session.Snapshot.AvailableNodes.Count == 2 && !session.IsChapterMarketUnlocked &&
            session.Snapshot.ShopItems.Count == 0 && session.ValidateMarketRefresh() is not null,
            "A chapter opens with exactly two alternative battlefields and its persistent market is truly locked.");
        var reversed = ShanheCampaignSession.Create(definition, new(71, Player)
            { UseCodexRewardPool = true, PersistentMarket = true }, Registry);
        Accept(reversed, new MoveShanheNodeCommand("opening2", reversed.Revision));
        var reversedBattle = CreateBattle(reversed.PendingBattleSetup!); FinishBattle(reversedBattle, "defeat");
        Require(reversed.CompleteBattle(reversedBattle, reversed.Revision).Accepted && reversed.ChapterProgress == 1 &&
            !reversed.IsChapterNodeMainline("opening1") && reversed.Snapshot.AvailableNodes.Select(node => node.Id).ToHashSet()
                .SetEquals(["opening1", "story1"]),
            "Choosing the second opening battlefield first advances the same single step and leaves the first battlefield optional.");
        Cold(reversed, definition);
        Win("opening1", expectedMarket: false);
        Require(session.ChapterProgress == 1 && !session.IsChapterNodeMainline("opening2") &&
            session.Snapshot.AvailableNodes.Any(node => node.Id == "opening2"), "The other opening battlefield becomes optional without spending another mainline step.");
        Win("opening2", expectedMarket: false);
        Require(session.ChapterProgress == 1, "Completing both opening fights still advances only one of the six steps.");
        Choose("story1", "go");
        Require(session.ChapterProgress == 2 && !session.IsChapterMarketUnlocked &&
            !session.Submit(new MoveShanheNodeCommand("story3", session.Revision)).Accepted,
            "The second and third story steps cannot be bypassed to unlock the market early.");
        Choose("story2", "go");
        Require(session.ChapterProgress == 3 && session.IsChapterMarketUnlocked && session.Snapshot.ShopItems.Count > 0 &&
            session.Snapshot.AvailableNodes.Any(node => node.Id == "arena"), "Step three really unlocks both persistent trade and optional arena access.");
        Win("arena", expectedMarket: true);
        Require(session.ChapterProgress == 3, "The arena grants its battle income without consuming mainline progress.");
        Choose("story3", "go"); Win("main", expectedMarket: true);
        var alternateRest = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, Registry);
        var beforeRest = alternateRest.Snapshot;
        Accept(alternateRest, new MoveShanheNodeCommand("rest", alternateRest.Revision));
        Accept(alternateRest, new ChooseShanheNodeOptionCommand("maxhp", null, alternateRest.Revision));
        Require(alternateRest.ChapterProgress == 6 && alternateRest.Snapshot.MaxHp == beforeRest.MaxHp + 1 &&
            alternateRest.Snapshot.Hp == beforeRest.Hp && alternateRest.Snapshot.Bonuses.Draw == beforeRest.Bonuses.Draw,
            "The alternate rest choice really grants only persistent maximum health, keeps current health and draw unchanged, and completes step six.");
        Cold(alternateRest, definition);
        Choose("rest", "draw");
        Require(session.ChapterProgress == 6 && session.Snapshot.Bonuses.Draw == 1 &&
            session.Snapshot.AvailableNodes.Select(node => node.Id).ToHashSet().SetEquals(["optional", "boss"]),
            "Rest grants the chosen growth once, completes six steps and offers both the optional late fight and the boss immediately.");
        Cold(session, definition);
        Win("boss", expectedMarket: true);
        Require(session.Snapshot.Phase == ShanheCampaignPhase.Won && !session.Snapshot.CompletedNodeIds.Contains("optional"),
            "The boss can finish the chapter while its late optional fight remains unplayed.");
        Cold(session, definition);

        void Choose(string id, string choice)
        {
            Accept(session, new MoveShanheNodeCommand(id, session.Revision));
            Accept(session, new ChooseShanheNodeOptionCommand(choice, null, session.Revision));
            Cold(session, definition);
        }
        void Win(string id, bool expectedMarket)
        {
            Accept(session, new MoveShanheNodeCommand(id, session.Revision));
            Require((session.PendingBattleSetup!.MarketSetup is not null) == expectedMarket,
                "Only battles entered after market unlock carry a detached native market; opening battles cannot smuggle early trade.");
            var battle = CreateBattle(session.PendingBattleSetup!); FinishBattle(battle, "defeat");
            Require(session.CompleteBattle(battle, session.Revision).Accepted, "Progress and income require a verified native win.");
            Cold(session, definition);
        }
    }
}
