using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ShanheCampaignChecks
{
    public static void GroupedNonTacticDeclinesResolveOnlyTheirGroupAndReplayCold()
    {
        ShanheRewardDefinition[] rewards =
        [
            new("tactic", "刀阵", Bonuses: new(Slash: 1)) { Rarity = 3 },
            new("card", "随身杀", StartingCards: [new(CardKind.Slash)]) { Rarity = 3 },
            new("skill", "守御", SkillId: Guard) { Rarity = 1 }
        ];
        var definition = new ShanheCampaignDefinition("fixture:group-decline", "首领战利品", [Player], ["boss"],
        [
            new("boss", "首领", ShanheNodeKind.BossBattle, ["home"], [new("guard", "守军", [new(Enemy)], VictoryGold: 500)])
            {
                ChapterNumber = 1, RewardSkipGold = 100,
                BossRewards = new(500, 1,
                [new("tactics", "史诗战法", ["tactic"], 1), new("cards", "史诗手牌", ["card"], 1), new("skills", "普通技能", ["skill"], 1)])
            },
            new("home", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])
        ], rewards);
        var session = Create(definition);
        Accept(session, new MoveShanheNodeCommand("boss", session.Revision));
        var battle = CreateBattle(session.PendingBattleSetup!); FinishBattle(battle, "defeat");
        Require(session.CompleteBattle(battle, session.Revision).Accepted, "A real boss victory opens its grouped rewards.");
        var summary = session.Snapshot.BossRewardSummary!;
        Require(session.Snapshot.Gold == 500 && summary.Groups.Single(group => group.Id == "tactics").SkipGold == 0 &&
            summary.Groups.Single(group => group.Id == "cards").SkipGold == 100 &&
            summary.Groups.Single(group => group.Id == "skills").SkipGold == 50,
            "The boss pays its base credit and exposes compulsory tactics, authored card compensation and common-skill compensation.");
        var before = Snapshot(session);
        Require(!session.Submit(new SkipShanheRewardCommand(session.Revision)).Accepted &&
            !session.Submit(new SkipShanheRewardCommand(session.Revision) { GroupId = "tactics" }).Accepted &&
            !session.Submit(new SkipShanheRewardCommand(session.Revision) { GroupId = "unknown" }).Accepted &&
            Snapshot(session) == before, "Unscoped, tactic and unknown-group declines cannot mint coins or resolve a group.");
        Accept(session, new SkipShanheRewardCommand(session.Revision) { GroupId = "cards" });
        summary = session.Snapshot.BossRewardSummary!;
        Require(session.Snapshot.Gold == 600 && session.Snapshot.StartingCards.Count == 0 && !summary.CanConfirm &&
            summary.Groups.Single(group => group.Id == "cards") is { IsSkipped: true, IsResolved: true, CanSkip: false } &&
            summary.Groups.Where(group => group.Id != "cards").All(group => !group.IsResolved),
            "Declining the card group pays exactly once and leaves the tactic and skill groups pending.");
        Cold(session, definition);
        session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, Registry);
        before = Snapshot(session);
        Require(!session.Submit(new SkipShanheRewardCommand(session.Revision) { GroupId = "cards" }).Accepted &&
            !session.Submit(new ChooseShanheBossRewardCommand("cards", "card", null, session.Revision)).Accepted &&
            !session.Submit(new ConfirmShanheBossRewardCommand(session.Revision)).Accepted && Snapshot(session) == before,
            "Cold restoration preserves the consumed group and prevents duplicate pay, later claims or early continuation.");
        Accept(session, new ChooseShanheBossRewardCommand("tactics", "tactic", null, session.Revision));
        Accept(session, new SkipShanheRewardCommand(session.Revision) { GroupId = "skills" });
        Require(session.Snapshot.Gold == 650 && session.Snapshot.Bonuses.Slash == 1 &&
            session.Snapshot.ExtraSkillIds.Count == 0 && session.Snapshot.BossRewardSummary!.CanConfirm &&
            session.Snapshot.CompletedNodeIds.Count == 0,
            "Claiming the mandatory tactic and declining the common skill resolve all groups without prematurely leaving the boss.");
        Accept(session, new ConfirmShanheBossRewardCommand(session.Revision));
        Require(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "home" &&
            session.Snapshot.Gold == 650, "Only final confirmation opens the next route without paying income twice.");
        Cold(session, definition);
    }

    public static void SignedIncomeModifiersApplyToOrdinaryAndBossBasesOnceCold()
    {
        foreach (var modifier in new[] { 20, -25 })
        {
            var definition = new ShanheCampaignDefinition("fixture:signed-income", "战斗铜币", [Player], ["battle"],
            [
                new("battle", "驻军", ShanheNodeKind.Battle, ["boss"], [new("one", "守军", [new(Enemy)], VictoryGold: 200)]),
                new("boss", "首领", ShanheNodeKind.BossBattle, [], [new("two", "守军", [new(Enemy)], VictoryGold: 500)])
                    { ChapterNumber = 3 }
            ], [new("income", "战斗收入") { CampaignEconomy = new(BattleGoldPercent: modifier) }]);
            var session = ShanheCampaignSession.Create(definition, new(71, Player) { EquippedTacticIds = ["income"] }, Registry);
            var expectedGold = 0;
            foreach (var (id, baseGold) in new[] { ("battle", 200), ("boss", 500) })
            {
                Accept(session, new MoveShanheNodeCommand(id, session.Revision));
                var battle = CreateBattle(session.PendingBattleSetup!); FinishBattle(battle, "defeat");
                Require(session.CompleteBattle(battle, session.Revision).Accepted, "The native completed battle must settle its authored income.");
                expectedGold += baseGold + baseGold * modifier / 100;
                Require(session.Snapshot.Gold == expectedGold,
                    "The same owned modifier raises or lowers the two-hundred ordinary and five-hundred boss bases.");
                var before = Snapshot(session);
                Require(!session.CompleteBattle(battle, session.Revision).Accepted && Snapshot(session) == before,
                    "A repeated completion cannot apply income or its modifier again.");
                Cold(session, definition);
            }
            Require(session.Snapshot.Phase == ShanheCampaignPhase.Won, "Final victory retains direct expedition settlement.");
        }
    }
}
