using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheThirdChapterEliteChecks
{
    private const string Owner = "fixture:third-elite-owner", Peer = "fixture:third-elite-peer", Driver = "fixture:third-elite-driver";

    public static void CompleteEliteRosterEarnsLootThenRejoinsFinalRoutesCold()
    {
        const string id = StandardShanheCampaignCatalog.ThirdChapterDongwuEliteId;
        var published = StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        var elite = published.Nodes.Single(node => node.Id == id);
        var onward = elite.NextNodeIds.Where(id => published.Nodes.Single(node => node.Id == id).Kind == ShanheNodeKind.BossBattle)
            .ToHashSet(StringComparer.Ordinal);
        Need(elite is { Kind: ShanheNodeKind.EliteBattle, ChapterNumber: 3, BossRewards: null,
                PrimaryTacticRarity: 3, PrimarySkillRarity: 3, PrimaryCardRarity: 3 } &&
            published.Nodes.Single(node => node.Id == "finalcamp").NextNodeIds.Contains(id) && onward.Count == 3 &&
            elite.NextNodeIds.All(next => onward.Contains(next) || published.Nodes.Single(node => node.Id == next).ExtraChapterBattleIndex == 1) &&
            elite.Encounters is [{ Enemies.Count: 3, BossTransition: null }] &&
            !StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.LocalEditionId).Nodes.Any(node => node.Id == id),
            "The optional third-chapter elite keeps three enemies, has no boss transition or ending bundle, and rejoins the three original final routes only in the current edition.");
        var shortened = published with { StartingNodeIds = [id], RandomStartingNode = false,
            Nodes = published.Nodes.Where(node => node.Id == id || onward.Contains(node.Id)).Select(node => node with
                { NextNodeIds = node.NextNodeIds.Where(onward.Contains).ToArray() }).ToArray() };
        var campaign = ShanheCampaignSession.Create(shortened, new(91, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        Need(campaign.Submit(new MoveShanheNodeCommand(id, campaign.Revision)).Accepted, "The real elite map command accepts the encounter.");
        var setup = campaign.PendingBattleSetup!;
        var native = StandardShanheCampaignCatalog.CreateBattle(setup);
        Need(native.CreateSnapshot(0).Players.Skip(1).Select(player => player.GeneralId).SequenceEqual(
                [StandardShanheEliteDongwuPackage.HuangGaiId, StandardShanheEliteHandangPackage.HandangId, StandardShanheEliteDongwuPackage.ChengPuId]) &&
            native.CreateSnapshot(0).Players.Skip(1).All(player => player is { Hp: 5, MaxHp: 5, HandCount: 4 }) &&
            native.Submit(new StartGameCommand()).Accepted && native.CreateSnapshot(0).CurrentSeat == 0,
            "The actual conditional battle registry starts the full three-general 5/5 roster with ordinary hands and player initiative: " +
                JsonSerializer.Serialize(native.CreateSnapshot(0).Players.Select(player => new { player.GeneralId, player.Hp, player.MaxHp, player.HandCount })));

        var loot = new ShanheRewardDefinition("elite-loot", "史诗整军", Bonuses: new(Draw: 1)) { Rarity = 3 };
        var fixture = new ShanheCampaignDefinition("fixture:third-elite", "第三章精英领奖", [Owner], [id],
            new[] { elite with { RewardIds = [loot.Id], NextNodeIds = onward.ToArray() } }.Concat(onward.Select(next => new ShanheNodeDefinition(next,
                "最终路线", ShanheNodeKind.BossBattle, [], [new(next, "最终战", [new(Peer)])]) { ChapterNumber = 3 })).ToArray(), [loot]);
        var session = ShanheCampaignSession.Create(fixture, new(91, Owner), Registry);
        Need(session.Submit(new MoveShanheNodeCommand(id, session.Revision)).Accepted, "The bounded reward fixture enters the original elite recipe.");
        setup = session.PendingBattleSetup!;
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, Registry(setup));
        Accept(game, new StartGameCommand());
        foreach (var seat in new[] { 1, 2, 3 })
        {
            Reach(game, () => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Accept(game, new UseProgramSkillCommand(0, Driver, "defeat", [], [seat], game.Revision, game.PendingDecision!.PromptId));
            Reach(game, () => !game.State.Players[seat].IsAlive);
        }
        Reach(game, () => game.State.Status == EngineStatus.Completed);
        Need(session.CompleteBattle(game, session.Revision).Accepted && session.Snapshot is
            { Phase: ShanheCampaignPhase.Reward, Gold: 200, BossRewardSummary: null },
            "Authentic victory pays the local elite credit and opens ordinary classified loot instead of ending the expedition.");
        var saved = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(saved), fixture, Registry);
        Need(session.Snapshot.RewardCategories.Count == 0 &&
            session.Snapshot.RewardOffers.Single().Rarity == 3 &&
            session.Submit(new ChooseShanheRewardCommand(loot.Id, null, session.Revision)).Accepted &&
            session.Snapshot.Phase == ShanheCampaignPhase.Map &&
            session.Snapshot.AvailableNodes.Select(node => node.Id).ToHashSet(StringComparer.Ordinal).SetEquals(onward),
            "Cold recovery retains the elite reward, and claiming it resumes every original final-boss route.");
    }

    private static ContentRegistry Registry(ShanheBattleSetup _) => ContentRegistry.Build(new StandardContentPackage(),
        new StandardActiveSkillExpansionPackage(true), new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
        new StandardShanheEliteDongwuPackage(), new StandardShanheEliteHandangPackage(), new Fixture());
    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Elite fixture command rejected.");
    }
    private static void Reach(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 100 && !ready(); step++)
        {
            if (game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.RescueDying } prompt)
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(choice => choice.Cards.Count == 0).Id, game.Revision));
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        Need(ready(), "The elite fixture did not reach its native boundary.");
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:third-elite-battle", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var program = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Driver, revision = 1, activations = new[] { new { id = "defeat", minCards = 0, maxCards = 0,
                    minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "damage", target = "selectedTarget", amount = 20 } } } } } } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实战斗驱动", description = "结束精英战斗。" } } }));
            builder.AddSkill(new(Driver, "真实战斗驱动", "fixture") { Program = program.Programs[Driver] });
            builder.AddGeneral(new(Owner, "行者", "supporter", Driver, "shu", 30));
            builder.AddGeneral(new(Peer, "后续守军", "supporter", null, "qun", 4));
            builder.AddDeck(new("fixture:third-elite-deck", "固定闪牌堆", 4, 2, [new("standard:dodge", 100)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "精英原生短局", 2, 8, new Dictionary<string, int>(), "fixture:third-elite-deck",
                GeneralPoolIds: [Owner, Peer, StandardShanheEliteDongwuPackage.HuangGaiId, StandardShanheEliteHandangPackage.HandangId,
                    StandardShanheEliteDongwuPackage.ChengPuId], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
