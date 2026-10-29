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

    public static void DailyTravelExtraBattlesPayOnceUpgradeAndRecoverAcrossChapters()
    {
        var travel = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => source.Effect.ExtraChapterBattles > 0)
            .Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity }).ToArray();
        var first = StandardShanheCampaignCatalog.CurrentDailyTravelRewardId;
        var second = StandardShanheCampaignCatalog.CurrentDailyTravel2RewardId;
        var nodes = new List<ShanheNodeDefinition>
        {
            new("shop", "沿途集市", ShanheNodeKind.Shop, ["gate1"], Choices:
                [new("upgrade", "日行千里Ⅱ", 700, RewardId: second, LeavesNode: false)]) { ChapterNumber = 1 }
        };
        foreach (var chapter in new[] { 1, 2 })
        {
            nodes.Add(new("gate" + chapter, "营前路口", ShanheNodeKind.Camp, ["extra" + chapter + "-1", "boss" + chapter],
                Choices: [new("go", "继续")]) { ChapterNumber = chapter });
            for (var extra = 1; extra <= 2; extra++)
                nodes.Add(new("extra" + chapter + "-" + extra, "追击", ShanheNodeKind.Battle,
                    extra == 1 ? ["extra" + chapter + "-2", "boss" + chapter] : ["boss" + chapter],
                    [new("garrison", "驻军", [new(Enemy)], VictoryGold: 200)], ["loot"])
                { ChapterNumber = chapter, ExtraChapterBattleIndex = extra });
            nodes.Add(new("boss" + chapter, "关底", ShanheNodeKind.BossBattle, chapter == 1 ? ["gate2"] : [],
                [new("boss", "首领", [new(Enemy)])]) { ChapterNumber = chapter });
        }
        var definition = new ShanheCampaignDefinition("fixture:daily-travel", "额外行军", [Player], ["shop"], nodes,
            travel.Append(new("loot", "战法成长", Gold: 5, Bonuses: new(Draw: 1))).ToArray(), StartingGold: 1000);
        foreach (var count in new[] { 0, 1, 2 })
        {
            var equipped = count == 0 ? Array.Empty<string>() : new[] { count == 1 ? first : second };
            var session = ShanheCampaignSession.Create(definition, new(71, Player)
                { UseCodexRewardPool = true, PersistentMarket = true, EquippedTacticIds = equipped }, Registry);
            Require(session.ChapterExtraBattleCount == count &&
                session.VisibleMapNodes.Count(node => node.ExtraChapterBattleIndex > 0) == count * 2,
                "Only the held travel tier exposes one or two real additional battles in each chapter.");
            for (var chapter = 1; chapter <= 2; chapter++)
            {
                Accept(session, new MoveShanheNodeCommand("gate" + chapter, session.Revision));
                Accept(session, new ChooseShanheNodeOptionCommand("go", null, session.Revision));
                Require(session.Snapshot.AvailableNodes.Any(node => node.Id == "boss" + chapter),
                    "Extra pursuits remain optional and never block the chapter boss.");
                if (count == 0)
                {
                    var before = Snapshot(session);
                    Require(!session.Submit(new MoveShanheNodeCommand("extra" + chapter + "-1", session.Revision)).Accepted && Snapshot(session) == before,
                        "An unowned extra action cannot be forged into a map move.");
                }
                for (var extra = 1; extra <= count; extra++)
                {
                    var id = "extra" + chapter + "-" + extra;
                    Win(id);
                    Accept(session, new ChooseShanheRewardCommand("loot", null, session.Revision));
                    Require(session.Snapshot.Gold == 1000 + ((chapter - 1) * count + extra) * 205 &&
                        session.Snapshot.Bonuses.Draw == (chapter - 1) * count + extra,
                        "Every actual extra victory grants its ordinary copper credit and real persistent build growth.");
                    Cold(session, definition);
                    session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, Registry);
                    Require(!session.Submit(new MoveShanheNodeCommand(id, session.Revision)).Accepted,
                        "Cold recovery cannot reopen a completed pursuit for another reward.");
                }
                if (chapter == 1) Win("boss1");
            }

            void Win(string id)
            {
                Accept(session, new MoveShanheNodeCommand(id, session.Revision));
                var battle = CreateBattle(session.PendingBattleSetup!); FinishBattle(battle, "defeat");
                Require(session.CompleteBattle(battle, session.Revision).Accepted, "Extra action rewards require an actual verified victory.");
            }
        }

        var upgraded = ShanheCampaignSession.Create(definition, new(71, Player)
        { UseCodexRewardPool = true, PersistentMarket = true, EquippedTacticIds = [first] }, Registry);
        Accept(upgraded, new MoveShanheNodeCommand("gate1", upgraded.Revision));
        Accept(upgraded, new ChooseShanheNodeOptionCommand("go", null, upgraded.Revision));
        Accept(upgraded, new MoveShanheNodeCommand("extra1-1", upgraded.Revision));
        var completed = CreateBattle(upgraded.PendingBattleSetup!); FinishBattle(completed, "defeat");
        Require(upgraded.CompleteBattle(completed, upgraded.Revision).Accepted, "The lower-tier pursuit is a genuine paid-out native victory.");
        Accept(upgraded, new ChooseShanheRewardCommand("loot", null, upgraded.Revision));
        Cold(upgraded, definition);
        var product = upgraded.Snapshot.ShopItems.Single(item => item.Reward.Id == second);
        Accept(upgraded, new CardGame.Core.Campaigns.BuyShanheMarketItemCommand(product.ChoiceId, null, upgraded.Revision));
        Require(upgraded.ChapterExtraBattleCount == 2 && upgraded.Snapshot.TacticIds.Contains(second) &&
            !upgraded.Snapshot.TacticIds.Contains(first) && upgraded.Snapshot.Gold == 505 &&
            upgraded.Snapshot.AvailableNodes.Any(node => node.Id == "extra1-2") &&
            !upgraded.Snapshot.AvailableNodes.Any(node => node.Id == "extra1-1"),
            "The real seven-hundred-copper purchase replaces tier I and yields two total extra battles, never three.");
        Cold(upgraded, definition);
    }

    public static void SourceChaptersPlaceTenStopsAndKeepTheirNativeEnemyCities()
    {
        var definition = StandardShanheCampaignCatalog.CreateSourceDefinition(25);
        foreach (var region in StandardShanheSourceRouteCatalog.Season5.Regions)
        {
            var route = definition.Nodes.Where(node => node.ChapterRouteId == "source-region-" + region.SourceUniqueId).ToArray();
            var baseline = route.Where(node => node.ChapterRole is not (ShanheChapterNodeRole.Profile or ShanheChapterNodeRole.Market or ShanheChapterNodeRole.ExtraBattle)).ToArray();
            Require(baseline.Length == 10 && baseline.Count(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle) == 2 &&
                baseline.Count(node => node.ChapterRole == ShanheChapterNodeRole.Event) == 3 &&
                baseline.Count(node => node.ChapterRole == ShanheChapterNodeRole.OptionalBattle) == 1 &&
                baseline.Count(node => node.ChapterRole == ShanheChapterNodeRole.Arena) == 1 &&
                baseline.Where(node => node.ChapterProgressStep > 0).Select(node => node.ChapterProgressStep).Distinct().Order().SequenceEqual(Enumerable.Range(1, 6)),
                "Six distinct mainline steps plus two optional fights, the arena and the boss form the maximum ten-action chapter.");
            var boss = route.Single(node => node.ChapterRole == ShanheChapterNodeRole.Boss);
            Require(boss.NextNodeIds.Select(id => definition.Nodes.Single(node => node.Id == id).ChapterRouteId).Distinct().ToHashSet()
                    .SetEquals(region.NextRegionIds.Select(id => "source-region-" + id)) &&
                route.Where(node => node.Kind == ShanheNodeKind.Battle && node.ExtraChapterBattleIndex == 0)
                    .All(node => node.Encounters!.All(encounter => encounter.SourceCityId is { } cityId && region.CityIds.Contains(cityId))),
                "Chapter boss exits keep the original region graph and every ordinary source body stays at its actual city.");
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
