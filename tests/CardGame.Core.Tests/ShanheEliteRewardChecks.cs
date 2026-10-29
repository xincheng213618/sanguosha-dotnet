using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheEliteRewardChecks
{
    public static void PublishedEliteDraftsKeepRareCategoriesCompensationAndPromotionsForward()
    {
        foreach (var difficulty in new[] { 1, 2, 3 })
        foreach (var codex in new[] { false, true })
        {
            var definition = codex
                ? StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var elite = definition.Nodes.Single(node => node.Id == "hulao");
            Need(elite.Kind == ShanheNodeKind.EliteBattle && elite.CategorizedRewards && elite.AutomaticRewardCategory && elite.RewardSkipGold > 0 &&
                elite.PrimaryTacticRarity == 2 && elite.PrimarySkillRarity == 2 && elite.PrimaryCardRarity == 2 &&
                elite.RewardIds!.Any(id => definition.Rewards.Single(reward => reward.Id == id).Rarity > 2),
                "The published Lu Bu/Zhang Liao elite keeps rare categorized baseline loot, a decline quote and its higher promotion pool in both entry paths.");
            var ordinary = definition.Nodes.Single(node => node.Id == "guangcheng");
            Need(ordinary.PrimaryTacticRarity == 1 && ordinary.PrimarySkillRarity == 1 && ordinary.PrimaryCardRarity == 1 &&
                definition.StartingNodeIds.All(id => definition.Nodes.Single(node => node.Id == id).PrimarySkillRarity == 2),
                "Ordinary route battles and the existing rare opening-skill rewards keep their own authored qualities.");
            Need(elite.RewardIds!.Any(id => definition.Rewards.Single(reward => reward.Id == id) is
                    { Rarity: 2, StartingCards.Count: > 0 }),
                "Both published entry paths include actual rare cards for an elite card reward.");
            var third = definition.Nodes.Single(node => node.Id == StandardShanheCampaignCatalog.ThirdChapterDongwuEliteId);
            Need(third.PrimaryTacticRarity == 3 && third.PrimarySkillRarity == 3 && third.PrimaryCardRarity == 3,
                "The third-chapter elite upgrades the optional rare patrol baseline to epic rewards.");
            {
                var defaults = StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(definition);
                var session = ShanheCampaignSession.Create(definition with { StartingNodeIds = [.. definition.StartingNodeIds, elite.Id] },
                    new(71, "classic:guan-yu") { UseCodexRewardPool = codex, UnlockedRewardIds = codex ? defaults : [] },
                    StandardShanheCampaignCatalog.CreateBattleRegistry);
                EnterRewardStage(session, elite.Id);
                Need(session.Snapshot.RewardCategories.Count == 0 && session.Snapshot.RewardOffers.Count > 0 &&
                    session.Snapshot.RewardOffers.All(reward => reward.Rarity == 2),
                    "Each entry path receives direct rare offers of the location's fixed type, without a category-selection step or common fallback.");
            }
        }

        foreach (var category in new[] { ShanheRewardCategory.Tactic, ShanheRewardCategory.Skill, ShanheRewardCategory.Card })
        {
            var session = Draft(category);
            var quote = session.Snapshot;
            Need(quote.RewardCategories.Count == 0 && quote.RewardOffers.Count > 0 && quote.RewardOffers.All(reward => reward.Rarity == 2) &&
                session.PreviewRewardCategory("elite") == category &&
                !session.Submit(new ChooseShanheRewardCategoryCommand(category, session.Revision)).Accepted &&
                ReferenceEquals(quote, session.Snapshot),
                "An appearing elite locks its reward type and opens only rare same-type offers; there is no category-selection command or mixed draft.");
            var beforeGold = quote.Gold;
            if (category == ShanheRewardCategory.Tactic)
            {
                Need(!quote.CanSkipReward && !session.Submit(new SkipShanheRewardCommand(session.Revision)).Accepted &&
                    ReferenceEquals(quote, session.Snapshot), "Tactic drafts reject compensation without changing the frozen offer.");
                Accept(session, new ChooseShanheRewardCommand(quote.RewardOffers[0].Id, null, session.Revision));
                Need(session.Snapshot.Bonuses.Draw + session.Snapshot.Bonuses.Slash + session.Snapshot.Bonuses.InitialHand > 0 &&
                    session.Snapshot.ExtraSkillIds.Count == 0 && session.Snapshot.ExtraSkillSlots == 1,
                    "The selected tactic is acquired without consuming an extra general skill slot.");
            }
            else
            {
                Need(quote.CanSkipReward && quote.RewardSkipGold == 100, "Rare skill/card categories expose the authored hundred-coin decline.");
                var revision = session.Revision;
                Accept(session, new SkipShanheRewardCommand(revision));
                Need(session.Snapshot.Gold == beforeGold + 100 && session.Snapshot.StartingCards.Count == 0 &&
                    session.Snapshot.StartingEquipment.Count == 0 && session.Snapshot.ExtraSkillIds.Count == 0 &&
                    session.Snapshot.Phase == ShanheCampaignPhase.Map &&
                    !session.Submit(new SkipShanheRewardCommand(revision)).Accepted,
                    "A real decline pays exactly once, grants no item or skill and opens the next route.");
            }
        }
        var promoted = Draft(ShanheRewardCategory.Tactic, promote: true);
        Need(promoted.Snapshot.RewardOffers.Any(reward => reward.Rarity == 3) &&
            promoted.Snapshot.RewardOffers.All(reward => reward.Rarity >= 2),
            "An owned promotion tactic can still use the full epic pool after the elite baseline is filtered to rare.");
        var expanded = Draft(ShanheRewardCategory.Tactic, expand: true);
        Need(expanded.Snapshot.RewardOffers.Count == 4,
            "An automatic reward type preserves the owned tactic's four-candidate bonus.");
        var unavailable = Draft(ShanheRewardCategory.Card).Definition;
        unavailable = unavailable with { Nodes = unavailable.Nodes.Select(node => node.Id == "elite"
            ? node with { PrimaryCardRarity = 4 } : node).ToArray() };
        var withoutCard = ShanheCampaignSession.Create(unavailable, new(4, ShanheCampaignChecks.Player), ShanheCampaignChecks.Registry);
        Need(withoutCard.PreviewRewardCategory("elite") != ShanheRewardCategory.Card,
            "The verified card seed cannot consume a reward on a type without an eligible base-quality candidate.");
        var route = Draft(ShanheRewardCategory.Tactic).Definition;
        var eliteProfile = route.Nodes.Single(node => node.Id == "elite");
        var wolf = new ShanheRewardDefinition("wolf", "狼战") { CampaignEconomy = new(UpgradeOrdinaryBattles: true) };
        route = route with
        {
            StartingNodeIds = ["normal", "elite"], Rewards = route.Rewards.Append(wolf).ToArray(),
            Nodes = route.Nodes.Select(node => node.Id == "elite" ? node with
                { RewardIds = node.RewardIds!.Where(id => route.Rewards.Single(reward => reward.Id == id).SkillId is null).ToArray() } : node)
                .Append(eliteProfile with { Id = "normal", Kind = ShanheNodeKind.Battle, EliteUpgradeNodeId = "elite",
                    PrimaryTacticRarity = 1, PrimarySkillRarity = 1, PrimaryCardRarity = 1 }).ToArray()
        };
        var normal = ShanheCampaignSession.Create(route, new(1, ShanheCampaignChecks.Player), ShanheCampaignChecks.Registry);
        var upgraded = ShanheCampaignSession.Create(route, new(1, ShanheCampaignChecks.Player)
            { EquippedTacticIds = [wolf.Id] }, ShanheCampaignChecks.Registry);
        Need(normal.PreviewRewardCategory("normal") == upgraded.PreviewRewardCategory("normal") &&
            normal.PreviewRewardCategory("normal") is not (null or ShanheRewardCategory.Skill),
            "Wolf Battles upgrades quality without rerolling the appearing route's type, even when the elite profile lacks a base type.");
    }

    private static ShanheCampaignSession Draft(ShanheRewardCategory category, bool promote = false, bool expand = false)
    {
        ShanheRewardDefinition[] rewards =
        [
            new("t1", "普通战法", Bonuses: new(MaxHp: 2)) { Rarity = 1 },
            new("t2", "稀有战法一", Bonuses: new(Draw: 1)) { Rarity = 2 },
            new("t3", "稀有战法二", Bonuses: new(Slash: 1)) { Rarity = 2 },
            new("t4", "稀有战法三", Bonuses: new(InitialHand: 1)) { Rarity = 2 },
            new("t6", "稀有战法四", Bonuses: new(Draw: 2)) { Rarity = 2 },
            new("t5", "史诗战法", Bonuses: new(MaxHp: 1)) { Rarity = 3 },
            new("s1", "普通技能", SkillId: "fixture:shanhe-raid") { Rarity = 1 },
            new("s2", "稀有技能", SkillId: "fixture:shanhe-guard") { Rarity = 2 },
            new("c1", "普通五谷", StartingCards: [new(CardKind.FiveGrains)]) { Rarity = 1 },
            new("c2", "稀有闪", StartingCards: [new(CardKind.Dodge)]) { Rarity = 2 },
            new("c3", "稀有桃", StartingCards: [new(CardKind.Peach)]) { Rarity = 2 },
            new("c4", "稀有装备", StartingEquipment: [new(CardKind.BaguaFormation)]) { Rarity = 2 },
            new("c5", "史诗决斗", StartingCards: [new(CardKind.Duel)]) { Rarity = 3 },
            new("promotion", "品质晋升") { CampaignEconomy = new(BattlePrimaryPromotionPercent: 100) },
            new("expansion", "战法四选一") { CampaignEconomy = new(TacticRewardChoiceCount: 4) }
        ];
        var definition = new ShanheCampaignDefinition("fixture:elite-reward-forward", "精英奖励", [ShanheCampaignChecks.Player], ["elite"],
            [new("elite", "精英", ShanheNodeKind.EliteBattle, ["camp"],
                [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)])], rewards.Where(reward => reward.Id is not ("promotion" or "expansion")).Select(reward => reward.Id).ToArray())
                { CategorizedRewards = true, AutomaticRewardCategory = true, RewardSkipGold = 100,
                    PrimaryTacticRarity = 2, PrimarySkillRarity = 2, PrimaryCardRarity = 2 },
             new("camp", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])], rewards, StartingGold: 10, ExtraSkillSlots: 1);
        var seed = category switch { ShanheRewardCategory.Tactic => 1, ShanheRewardCategory.Card => 4, _ => 7 };
        var session = ShanheCampaignSession.Create(definition, new(seed, ShanheCampaignChecks.Player)
            { EquippedTacticIds = promote ? ["promotion"] : expand ? ["expansion"] : [] }, ShanheCampaignChecks.Registry);
        Need(session.PreviewRewardCategory("elite") == category, "The fixed seed still selects the intended reward category.");
        EnterRewardStage(session, "elite");
        return session;
    }

    private static void EnterRewardStage(ShanheCampaignSession session, string nodeId)
    {
        Accept(session, new MoveShanheNodeCommand(nodeId, session.Revision));
        // Exercise the production reward planner on a cloned in-memory fixture.
        // Battle verification, persisted journals and restore APIs are not invoked.
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var stateField = typeof(ShanheCampaignSession).GetField("_state", flags)!;
        var state = stateField.GetValue(session)!;
        state = state.GetType().GetMethod("<Clone>$")!.Invoke(state, null)!;
        state.GetType().GetProperty("BattleSetup")!.SetValue(state, null);
        var next = typeof(ShanheCampaignSession).GetMethod("BeginBattlePrimaryRewards", flags)!
            .Invoke(session, [state, session.Definition.Nodes.Single(node => node.Id == nodeId)])!;
        stateField.SetValue(session, next);
        var snapshot = typeof(ShanheCampaignSession).GetMethod("CreateSnapshot", flags)!.Invoke(session, [next, session.Revision]);
        typeof(ShanheCampaignSession).GetField("_snapshot", flags)!.SetValue(session, snapshot);
    }

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Reward command rejected."); }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
