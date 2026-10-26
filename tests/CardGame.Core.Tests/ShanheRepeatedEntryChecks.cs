using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheRepeatedEntryChecks
{
    private const string Owner = "fixture:repeated-entry-owner", Ally = "fixture:repeated-entry-ally";
    private const string Enemy = "fixture:repeated-entry-enemy", Quiet = "fixture:repeated-entry-quiet";
    private const string Deck = "fixture:repeated-entry-deck";
    private const string ArmorReward = "tactic-duijia-2026", DrawReward = "tactic-duipai-2026";
    private const string ArmorSkill = "shanhe:duijia-2026", DrawSkill = "shanhe:duipai-2026";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(43, 1, "fixture-production-repeatable-entry", "classic:guan-yu", 4, 0, 0, 0, 0,
            [], [], [], [new("standard:cao-cao")], [], new string('e', 64))));
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheSchoolTeamPackage(), new Fixture()));

    public static void EarnedRepeatableCopiesKeepArmorAndDrawAtBattleEntry()
    {
        var production = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var rewards = new[] { ArmorReward, DrawReward }.Select(id => production.Rewards.Single(r => r.Id == id)).ToArray();
        var acquisitionOrder = new[] { ArmorReward, ArmorReward, DrawReward, DrawReward };
        var nodes = acquisitionOrder.Select((reward, index) => new ShanheNodeDefinition("grant-" + index,
            "真实领取第" + (index + 1) + "份战法", ShanheNodeKind.Event,
            [index == acquisitionOrder.Length - 1 ? "battle" : "grant-" + (index + 1)],
            Choices: [new("take", "领取战法", RewardId: reward)])).Append(
            new("battle", "重复战法入场", ShanheNodeKind.Battle, [],
                [new("guard", "两名盟友与守军", [new(Enemy) { StartingHand = [] }],
                    [new(Ally, DrawBonus: 1) { StartingHand = [] }, new(Ally, DrawBonus: 1) { StartingHand = [] }])])).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:repeated-tactic-entry", "真实取得重复战法", [Owner], ["grant-0"],
            nodes, rewards, ExtraSkillSlots: 0, StartingBonuses: new(Draw: 1))
            { Schools = [production.Schools!.Single(s => s.Id == 6)] };
        var campaign = ShanheCampaignSession.Create(definition, new(71, Owner) { UseCodexRewardPool = true }, _ => Registry.Value);

        for (var index = 0; index < acquisitionOrder.Length; index++)
        {
            Accept(campaign, new MoveShanheNodeCommand("grant-" + index, campaign.Revision));
            Accept(campaign, new ChooseShanheNodeOptionCommand("take", null, campaign.Revision));
            Need(campaign.Snapshot.TacticIds.SequenceEqual(acquisitionOrder.Take(index + 1)),
                "Each accepted event reward must preserve its own actual repeated acquisition in campaign order.");
        }
        Need(rewards.Single(r => r.Id == ArmorReward).SchoolMembership is { SourceId: 2200, Rarity: 2 } &&
             rewards.Single(r => r.Id == DrawReward).SchoolMembership is null &&
             campaign.GetSchoolProgress() is [{ IsComplete: false } progress] && progress.OwnedCounts.SequenceEqual([0, 1, 0, 0]),
            "Both real repeatable copies stack in battle; the repeated armor source counts once. This 2026 draw reward has no school membership, unlike the separately registered current source 2201.");

        Accept(campaign, new MoveShanheNodeCommand("battle", campaign.Revision));
        var setup = campaign.PendingBattleSetup!;
        Need(setup.TacticSkillIds.SequenceEqual([ArmorSkill, ArmorSkill, DrawSkill, DrawSkill]),
            "A fresh encounter must carry every actually earned repeatable copy, without collapsing the identical skill IDs.");
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, Registry.Value);
        Accept(game, new StartGameCommand()); ReachOwnPlay(game);

        var players = game.CreateSnapshot(0).Players;
        var armor = game.Events.Select(e => e.Payload).OfType<ArmorGrantedEvent>().Where(e => e.Source.SkillId == ArmorSkill).ToArray();
        Need(players[0].Armor == 0 && players[3].Armor == 0 && new[] { 1, 2 }.All(seat =>
                players[seat].Armor == 6 && armor.Count(e => e.PlayerSeat == seat && e.Amount == 3) == 2 &&
                armor.Where(e => e.PlayerSeat == seat).Select(e => e.Source.SkillInstanceId).Distinct(StringComparer.Ordinal).Count() == 2),
            "Both independently granted opening instances must each give every other public ally three armor; owner and enemy are excluded.");

        var movementCount = game.CardMovements.Count;
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, game.PendingDecision!.PromptId)); ReachOwnPlay(game);
        var draws = game.CardMovements.Skip(movementCount).Where(m => m.From == CardLocation.DrawPile && m.Reason == CardMoveReasons.Draw).ToArray();
        Need(new[] { 1, 2 }.All(seat => draws.Count(m => m.To == CardLocation.Hand(seat)) == 7) &&
             draws.Count(m => m.To == CardLocation.Hand(0)) == 3 && draws.Count(m => m.To == CardLocation.Hand(3)) == 2,
            "On actual turns, each ally draws base two plus roster one plus two independent +2 copies; the owner's campaign +1 and enemy's base draw remain separate.");
        Need(game.Events.Select(e => e.Payload).OfType<ArmorGrantedEvent>().Count(e => e.Source.SkillId == ArmorSkill) == 4,
            "The two opening copies trigger only at this battle's real start, rather than being reapplied on later turns.");
    }

    private static void Accept(ShanheCampaignSession campaign, ShanheCampaignCommand command)
    { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "The actual reward command was rejected."); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The actual battle command was rejected."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void ReachOwnPlay(GameEngine game)
    {
        for (var step = 0; step < 400; step++)
        {
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            Need(game.State.Winner == Winner.None, "The repeatable-entry fixture ended before its intended Play boundary.");
            Need(game.PendingDecision is not { PlayerSeat: 0 }, "Unexpected human decision in the passive repeated-entry fixture.");
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The passive repeated-entry fixture did not reach the bounded Play boundary.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-repeated-entry", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var id in new[] { ArmorSkill, DrawSkill }) builder.AddSkill(Production.Value.GetSkill(id));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Quiet}}","revision":1,
                  "triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Quiet] = new { name = "静默观测者", description = "摸牌后跳过出牌阶段。" } } }));
            builder.AddSkill(new(Quiet, "静默观测者", "摸牌后跳过出牌阶段。")
                { Program = catalog.Programs[Quiet], ProgramPresentation = catalog.Presentations[Quiet] });
            builder.AddGeneral(new(Owner, "战法持有者", "supporter", "standard:none", "shu", 6));
            builder.AddGeneral(new(Ally, "实际盟友", "supporter", Quiet, "shu", 6));
            builder.AddGeneral(new(Enemy, "实际守军", "supporter", Quiet, "wei", 6));
            builder.AddDeck(new(Deck, "静默闪牌堆", 0, 2, [])
                { PhysicalCards = Enumerable.Range(0, 80).Select(n => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, n % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "重复战法公开团队", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 3, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
