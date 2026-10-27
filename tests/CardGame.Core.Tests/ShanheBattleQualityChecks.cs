using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBattleQualityChecks
{
    private const string Forced = "fixture:battle-quality-100";
    private const string Guard = "fixture:shanhe-guard", Raid = "fixture:shanhe-raid";

    public static void RealBossAndTwentyPercentWinsPreserveExactBonusTerminalAndOwnershipFreeze()
    {
        var actual = Sources(2216).Single().Id;
        (int Seed, ShanheRewardCategory Category, string[] Offers)[] samples =
        [
            (71, ShanheRewardCategory.Skill, ["s4", "s5", "s1"]),
            (83, ShanheRewardCategory.Tactic, ["t4", "t2", "t5"])
        ];
        foreach (var sample in samples)
        {
            var session = Create(Definition(), sample.Seed, [actual]);
            Complete(session);
            Accept(session, new ChooseShanheRewardCategoryCommand(sample.Category, session.Revision));
            Need(Ids(session).SequenceEqual(sample.Offers),
                "The actual twenty-percent source has fixed successful and unsuccessful primary positions: eligible common t2 remains common at seed83 while t1 rises, rather than promoting every position or adding rarity weights.",
                new { sample.Seed, sample.Category, Actual = Ids(session), sample.Offers });
        }

        var boss = Create(BossDefinition(), 71, [Forced, "tactic-economy-2014"],
            [Forced, "tactic-economy-2014", actual, "red-tactic"]);
        Complete(boss);
        var summary = boss.Snapshot.BossRewardSummary ?? throw new InvalidOperationException("The actual boss victory did not publish its reward bundle.");
        var bonus = summary.Groups.Single(group => group.Id.StartsWith("$bonus:", StringComparison.Ordinal));
        Need(summary.Groups.Count == 3 && summary.FixedGold == 500 && summary.SkillSlots == 1 &&
            boss.Snapshot.Gold == 600 && boss.Snapshot.ExtraSkillSlots == 1 &&
            summary.Groups.Single(group => group.Id == "tactics").Offers.Select(reward => reward.Id).SequenceEqual(["red-tactic"]) &&
            summary.Groups.Single(group => group.Id == "skills").Offers.Select(reward => reward.Id).SequenceEqual(["red-guard"]) &&
            summary.Groups.Where(group => group.Id is "tactics" or "skills").SelectMany(group => group.Offers).All(reward => reward.Rarity == 4) &&
            bonus.Offers is [{ Id: "epic-raid", Rarity: 3 }],
            "At forced100 the real red boss main groups have no higher tier, and the earned exact-epic bonus remains rarity3 despite another legendary skill in the overall catalogue; fixed gold and capacity pay once.");
        Accept(boss, new ChooseShanheBossRewardCommand("skills", "red-guard", null, boss.Revision));
        RejectUnchanged(boss, new ChooseShanheBossRewardCommand(bonus.Id, "epic-raid", null, boss.Revision));
        Accept(boss, new ChooseShanheBossRewardCommand(bonus.Id, "epic-raid", Guard, boss.Revision));
        Accept(boss, new ChooseShanheBossRewardCommand("tactics", "red-tactic", null, boss.Revision));
        Need(boss.Snapshot.ExtraSkillIds.SequenceEqual([Raid]) && boss.Snapshot.BossRewardSummary!.CanConfirm &&
            boss.Snapshot.Gold == 600 && bonus.Offers.Single().Id == "epic-raid",
            "Research-locked earned red and epic skills still use the existing real slot replacement, and resolving groups never rerolls the protected old bonus quote or repeats fixed credits.");
        Accept(boss, new ConfirmShanheBossRewardCommand(boss.Revision));
        Need(boss.Snapshot.Gold == 600 && boss.Snapshot.ExtraSkillSlots == 1 && boss.Snapshot.CompletedNodeIds.SequenceEqual(["start"]),
            "Boss confirmation advances exactly once without paying another gold or skill-slot award.");

        var lowerDefinition = Definition();
        lowerDefinition = lowerDefinition with { Nodes = lowerDefinition.Nodes.Select(node => node.Id == "start" ? node with
        {
            Kind = ShanheNodeKind.BossBattle, ChapterNumber = 1, CategorizedRewards = false,
            BossRewards = new(0, 0, [new("quality", "新取得蒸蒸日上", [actual], ChoiceCount: 1),
                new("skills", "授权低档技能组", ["s1", "s2", "s3", "s4", "s5"], ChoiceCount: 3)])
        } : node).ToArray() };
        var lower = Create(lowerDefinition, 71, [Forced], [Forced]);
        Complete(lower);
        Need(lower.Snapshot.BossRewardSummary!.Groups.Single(group => group.Id == "skills").Offers.Select(reward => reward.Id)
                .SequenceEqual(["s4", "s2", "s5"]),
            "An authored lower-tier boss main group really promotes within its own authorized earned pool, even when its higher skill has not been researched.");
        Accept(lower, new ChooseShanheBossRewardCommand("skills", "s4", null, lower.Revision));
        Need(lower.Snapshot.ExtraSkillIds.SequenceEqual(["standard:wusheng"]),
            "A promoted earned boss-main skill retains its existing research-independent claim qualification.");

        var newlyOwned = Create(lowerDefinition, 71, []);
        Complete(newlyOwned);
        var frozen = newlyOwned.Snapshot.BossRewardSummary!.Groups.Single(group => group.Id == "skills");
        Need(frozen.Offers.Select(reward => reward.Id).SequenceEqual(["s1", "s2", "s5"]),
            "Without prior ownership the same actual boss victory freezes its ordinary unpromoted skill baseline.");
        Accept(newlyOwned, new ChooseShanheBossRewardCommand("quality", actual, null, newlyOwned.Revision));
        Need(newlyOwned.Snapshot.TacticIds.Contains(actual) && newlyOwned.Snapshot.BossRewardSummary!.Groups.Single(group => group.Id == "skills")
                .Offers.Select(reward => reward.Id).SequenceEqual(["s1", "s2", "s5"]) && frozen.Offers.Select(reward => reward.Id).SequenceEqual(["s1", "s2", "s5"]),
            "First earning actual2216 cannot retrospectively promote this victory's other frozen boss group; the fixed seed would otherwise have a successful twenty-percent second position.");

        var terminalDefinition = BossDefinition();
        terminalDefinition = terminalDefinition with { Nodes = [terminalDefinition.Nodes[0] with
            { NextNodeIds = [], ChapterNumber = 3, BossRewards = null, RewardIds = ["red-tactic"], AddSkillSlotsOnVictory = 0 }] };
        var terminal = Create(terminalDefinition, 71, [Forced, "tactic-economy-2014"]);
        Complete(terminal);
        Need(terminal.Snapshot.Phase == ShanheCampaignPhase.Won && terminal.Snapshot.RewardOffers.Count == 0 &&
            terminal.Snapshot.RewardCategories.Count == 0 && terminal.Snapshot.BossRewardSummary is null &&
            terminal.Snapshot.Gold == 107 && terminal.Snapshot.ExtraSkillSlots == 0 && terminal.Snapshot.Bonuses == new ShanheTacticBonuses(),
            "The actual terminal third-chapter victory settles before any ordinary, categorized, boss or exact-bonus growth generation, regardless of the forced modifier.");
    }

    private static ShanheCampaignDefinition BossDefinition()
    {
        var rewards = Sources(2216, 2014).Concat(new ShanheRewardDefinition[]
        {
            new(Forced, "强制胜利品质") { CampaignEconomy = new(BattlePrimaryPromotionPercent: 100) },
            new("red-tactic", "传说战法") { Rarity = 4, CampaignEconomy = new(ShopDiscountPercent: 1) },
            new("red-guard", "传说守御", SkillId: Guard) { Rarity = 4 },
            new("epic-raid", "史诗突袭", SkillId: Raid) { Rarity = 3 },
            new("epic-raid-alias", "史诗突袭别名", SkillId: Raid) { Rarity = 3 },
            new("other-legendary", "授权组外的传说技能", SkillId: "standard:yingzi") { Rarity = 4 }
        }).ToArray();
        return new("fixture:battle-quality-boss", "首领主组与精确追加品质", [ShanheCampaignChecks.Player], ["start"],
            [new("start", "首领", ShanheNodeKind.BossBattle, ["finish"],
                [new("guard", "首领守军", [new(ShanheCampaignChecks.Enemy)], VictoryGold: 7)])
                { ChapterNumber = 1, BossRewards = new(500, 1,
                    [new("tactics", "传说战法", ["red-tactic"], ChoiceCount: 1), new("skills", "传说技能", ["red-guard"], ChoiceCount: 1)]) },
             new("finish", "归程", ShanheNodeKind.Camp, [], Choices: [new("leave", "归程")])],
            rewards, StartingGold: 100, ExtraSkillSlots: 0);
    }

    private static ShanheCampaignDefinition Definition(bool categorized = true)
    {
        var tactics = Enumerable.Range(1, 6).Select(index => new ShanheRewardDefinition("t" + index, "成长" + index,
            Bonuses: new(MaxHp: 1)) { Rarity = new[] { 1, 1, 1, 3, 3, 4 }[index - 1] }).ToArray();
        var skillIds = new[] { Guard, Raid, "standard:yingzi", "standard:wusheng", "standard:paoxiao" };
        var skills = skillIds.Select((skill, index) => new ShanheRewardDefinition("s" + (index + 1), "技能" + (index + 1), SkillId: skill)
            { Rarity = index < 3 ? 1 : 3 }).ToArray();
        var controls = new ShanheRewardDefinition[]
        {
            new("c1", "随身闪", StartingCards: [new(CardKind.Dodge)]) { Rarity = 1 },
            new("c2", "随身连弩", StartingEquipment: [new(CardKind.Crossbow)]) { Rarity = 3 },
            new("u1", "铜钱", Gold: 5) { Rarity = 1 }, new("u2", "军医", Heal: 1) { Rarity = 4 }
        };
        var primary = tactics.Concat(skills).Concat(controls).ToArray();
        var extra = new ShanheRewardDefinition[]
        {
            new(Forced, "强制胜利品质") { CampaignEconomy = new(BattlePrimaryPromotionPercent: 100) },
            new("locked-low", "普通守御", SkillId: Guard) { Rarity = 1 },
            new("locked-high", "未研习龙胆", SkillId: "standard:longdan") { Rarity = 4 }
        };
        var sources = Sources(2216, 2014);
        return new("fixture:shanhe-battle-quality", "真实胜利主奖励品质", [ShanheCampaignChecks.Player], ["start"],
            [new("start", "首场", ShanheNodeKind.Battle, ["finish"],
                [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)], VictoryGold: 7)], primary.Select(reward => reward.Id).ToArray(),
                RewardChoiceCount: categorized ? 3 : 4) { CategorizedRewards = categorized },
             new("finish", "归程", ShanheNodeKind.Camp, [], Choices: [new("leave", "归程")])],
            primary.Concat(extra).Concat(sources).ToArray(), StartingGold: 100, ExtraSkillSlots: 3);
    }

    private static ShanheRewardDefinition[] Sources(params int[] ids) => StandardShanheCampaignCatalog.CurrentEconomyTactics
        .Where(source => ids.Contains(source.SourceId)).Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description }).ToArray();
    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, int seed, IReadOnlyList<string> tactics, IReadOnlyList<string>? unlocked = null) =>
        ShanheCampaignSession.Create(definition, new(seed, ShanheCampaignChecks.Player)
        {
            UseCodexRewardPool = true, EquippedTacticIds = tactics,
            UnlockedRewardIds = unlocked ?? definition.Rewards.Where(reward => reward.Id != "locked-high").Select(reward => reward.Id).ToArray()
        }, ShanheCampaignChecks.Registry);
    private static void Complete(ShanheCampaignSession session)
    {
        Accept(session, new MoveShanheNodeCommand("start", session.Revision));
        var battle = ShanheCampaignChecks.CreateBattle(session.PendingBattleSetup!);
        ShanheCampaignChecks.FinishBattle(battle, "defeat");
        var result = session.CompleteBattle(battle, session.Revision);
        Need(result.Accepted, result.Error ?? "The real finished native battle journal must create its authorized victory rewards.");
    }
    private static string[] Ids(ShanheCampaignSession session) => session.Snapshot.RewardOffers.Select(reward => reward.Id).ToArray();
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real battle-quality command."); }
    private static void RejectUnchanged(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var before = session.Snapshot;
        Need(!session.Submit(command).Accepted && ReferenceEquals(before, session.Snapshot),
            "A stale claim or forbidden category/reward cannot reroll or mutate finalized primary offers.");
    }
    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }
}
