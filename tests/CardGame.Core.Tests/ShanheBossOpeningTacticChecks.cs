using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBossOpeningTacticChecks
{
    private const string Owner = "fixture:boss-opening-owner", Ally = "fixture:boss-opening-ally";
    private const string First = "fixture:boss-opening-first", Second = "fixture:boss-opening-second";
    private const string Driver = "fixture:boss-opening-driver", Quiet = "fixture:boss-opening-quiet";
    private const string Deck = "fixture:boss-opening-deck";
    private static readonly (string Skill, int Bonus)[] Tiers =
    [
        (StandardShanheBossOpeningTacticsPackage.WeiyuChoumouSkillId, 2),
        (StandardShanheBossOpeningTacticsPackage.WeiyuChoumou2SkillId, 3),
        (StandardShanheBossOpeningTacticsPackage.WeiyuChoumou3SkillId, 4)
    ];
    private static readonly Lazy<ContentRegistry> BattleRegistry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheBossOpeningTacticsPackage(), new Fixture()));

    public static void OwnedSourcesUseBattleKindAndNativePhysicalDeal()
    {
        var rewards = SourceRewards();
        foreach (var (skill, bonus) in Tiers)
        {
            var reward = rewards.Single(reward => reward.TacticSkillId == skill);
            foreach (var kind in new[] { ShanheNodeKind.Battle, ShanheNodeKind.EliteBattle, ShanheNodeKind.BossBattle })
            {
                var session = Create(kind, rewards, [reward.Id]);
                Enter(session);
                var active = kind == ShanheNodeKind.BossBattle;
                Need(session.PendingBattleSetup!.TacticSkillIds.SequenceEqual(active ? [skill] : Array.Empty<string>()) &&
                    session.Snapshot.TacticIds.SequenceEqual([reward.Id]) && session.Snapshot.ExtraSkillIds.Count == 0 &&
                    session.Snapshot.Bonuses == new ShanheTacticBonuses(),
                    "An actually equipped source stays owned in every encounter and projects its program only for the typed boss battle, independently of the node id.");
                AssertOpening(CreateGame(session.PendingBattleSetup!), active ? bonus : 0, active ? skill : null);
            }
        }
        var unowned = Create(ShanheNodeKind.BossBattle, rewards, []);
        Enter(unowned);
        Need(unowned.PendingBattleSetup!.TacticSkillIds.Count == 0,
            "A boss encounter cannot grant any unequipped source merely because it exists in the reward catalogue.");
        AssertOpening(CreateGame(unowned.PendingBattleSetup!), 0, null);
        RejectMixedBossOnlyRewards(rewards[0]);
    }

    public static void NativeSecondFormKeepsTheAlreadyDealtPlayerHand()
    {
        var rewards = SourceRewards();
        var reward = rewards.Single(reward => reward.TacticSkillId == StandardShanheBossOpeningTacticsPackage.WeiyuChoumouSkillId);
        var session = Create(ShanheNodeKind.BossBattle, rewards, [reward.Id], secondForm: true);
        Enter(session);
        var game = CreateGame(session.PendingBattleSetup!);
        AssertOpening(game, 2, reward.TacticSkillId);
        Accept(game, new StartGameCommand());
        Until(game, () => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var initial = game.CardMovements.Where(move => move.Reason == CardMoveReasons.InitialDeal).ToArray();
        var playerHand = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).Order().ToArray();
        Accept(game, new UseProgramSkillCommand(0, Driver, "stage", [], [2], game.Revision, game.PendingDecision!.PromptId));
        Until(game, () => game.Events.Any(item => item.Payload is ShanheBossStageStartedEvent));
        var snapshot = game.CreateSnapshot(0);
        Need(snapshot is { Winner: Winner.None, CurrentSeat: 2, Phase: TurnPhase.NotStarted } &&
            snapshot.Players[2] is { GeneralId: Second, Hp: 5, MaxHp: 6, IsAlive: true } &&
            snapshot.Players[0].Hand.Select(card => card.Id).Order().SequenceEqual(playerHand) &&
            game.CardMovements.Where(move => move.Reason == CardMoveReasons.InitialDeal && move.To == CardLocation.Hand(0))
                .SequenceEqual(initial.Where(move => move.To == CardLocation.Hand(0))) &&
            game.Events.Count(item => item.Payload is ShanheBossStageStartedEvent) == 1 &&
            !game.Events.Any(item => item.Payload is PlayerDiedEvent or WinnerDeterminedEvent),
            "Genuine lethal native damage switches the boss form in the same encounter while retaining the player's exact physical hand and never dealing the boss-only opening bonus again.");
        Need(game.CardMovements.Count(move => move.To == CardLocation.Hand(2)) ==
                initial.Count(move => move.To == CardLocation.Hand(2)) + 2,
            "Only the second form receives its configured two-card stage draw; the tactic grants no cards to either allied or enemy seats.");
    }

    private static ShanheRewardDefinition[] SourceRewards()
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        return Tiers.Select(tier => definition.Rewards.Single(reward => reward.TacticSkillId == tier.Skill)).ToArray();
    }

    private static ShanheCampaignDefinition Definition(ShanheNodeKind kind, IReadOnlyList<ShanheRewardDefinition> rewards, bool secondForm = false)
    {
        // Deliberately invert the name hint: only Kind determines whether opening cards qualify.
        var nodeId = kind == ShanheNodeKind.BossBattle ? "ordinary-guard" : "boss-palace";
        var encounter = new ShanheEncounterDefinition("guard", "原生首领开局",
            [new(First, Hp: secondForm ? 1 : null)], [new(Ally)])
        { BossTransition = secondForm ? new(0, Second, 6, 5, 2) : null };
        return new("fixture:boss-opening", "首领战初始实体牌", [Owner], [nodeId],
            [new(nodeId, "按类型限定", kind, [], [encounter])], rewards) { Schools = StandardShanheCampaignCatalog.CurrentSchools };
    }

    private static ShanheCampaignSession Create(ShanheNodeKind kind, IReadOnlyList<ShanheRewardDefinition> rewards,
        string[] held, bool secondForm = false)
    {
        var probed = false;
        var session = ShanheCampaignSession.Create(Definition(kind, rewards, secondForm),
            new(83, Owner) { EquippedTacticIds = held }, setup =>
            {
                if (!probed)
                {
                    Need(setup.TacticSkillIds.ToHashSet().SetEquals(Tiers.Select(tier => tier.Skill)
                            .Concat(StandardShanheCampaignCatalog.CurrentSchools.Select(ShanheSchoolEffects.SkillId).OfType<string>())),
                        "The registry-only probe still discovers every potential boss program even when its first node is a normal or elite battle and no source is owned.");
                    probed = true;
                }
                return BattleRegistry.Value;
            });
        Need(probed, "Campaign construction must validate all potential battle tactic programs.");
        return session;
    }

    private static void AssertOpening(GameEngine game, int bonus, string? activeSkill)
    {
        var snapshot = game.CreateSnapshot(0);
        var deals = game.CardMovements.Where(move => move.Reason == CardMoveReasons.InitialDeal).ToArray();
        Need(snapshot.Players.Select(player => player.HandCount).SequenceEqual([2 + bonus, 2, 2]) &&
            deals.Length == 6 + bonus && deals.Select(move => move.CardId).Distinct().Count() == deals.Length &&
            deals.All(move => move.CardId > 0 && move.From == CardLocation.DrawPile && move.To.Zone == CardZoneKind.Hand) &&
            snapshot.Players.All(player => player.Skills!.Count(skill => Tiers.Any(tier => tier.Skill == skill.ContentId)) ==
                (player.Seat == 0 && activeSkill is not null ? 1 : 0)) &&
            snapshot.Players[0].Skills!.Any(skill => skill.ContentId == activeSkill) == (activeSkill is not null) &&
            snapshot.DrawPileCount == 96 - deals.Length && !game.CardMovements.Any(move => move.Reason == CardMoveReasons.Draw),
            "The native opening query deals only the player's extra real random deck entities; the ally and enemy retain their ordinary hands and no later draw substitutes for initial dealing.");
    }

    private static void RejectMixedBossOnlyRewards(ShanheRewardDefinition source)
    {
        foreach (var invalid in new[]
        {
            source with { TacticSkillId = null }, source with { Gold = 1 }, source with { Bonuses = new(InitialHand: 1) },
            source with { StartingCards = [new(CardKind.Slash)] }, source with { SkillId = Driver },
            source with { CampaignEconomy = new(StartingGold: 1) }
        })
        {
            try { _ = ShanheCampaignContent.FreezeAndValidate(Definition(ShanheNodeKind.BossBattle, [invalid])); }
            catch (ArgumentException) { continue; }
            throw new InvalidOperationException("A boss-only reward must reject a missing tactic program or an immediate reward payload.");
        }
    }

    private static void Enter(ShanheCampaignSession session)
    {
        var result = session.Submit(new MoveShanheNodeCommand(session.Snapshot.AvailableNodes.Single().Id, session.Revision));
        Need(result.Accepted, result.Error ?? "Rejected boss-opening campaign command.");
    }

    private static GameEngine CreateGame(ShanheBattleSetup setup) => GameEngine.CreateStandard(new()
    {
        Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
        HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
        UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
    }, BattleRegistry.Value);

    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native boss-opening command."); }

    private static void Until(GameEngine game, Func<bool> condition)
    {
        for (var step = 0; step < 100; step++)
        {
            if (condition()) return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Missing bounded native boss-opening boundary.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boss-opening", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"stage","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner",
                    "optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "原生初始牌夹具", description = "实体发牌与首领阶段转换" }) }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "玩家", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Ally, "友军", "supporter", Quiet, "shu", 4));
            builder.AddGeneral(new(First, "第一阶段", "supporter", Quiet, "wei", 4));
            builder.AddGeneral(new(Second, "第二阶段", "supporter", Quiet, "wei", 6));
            builder.AddDeck(new(Deck, "随机原生实体牌", 2, 2, [])
            { PhysicalCards = Enumerable.Range(0, 96).Select(index =>
                new ContentDeckPhysicalCard("standard:peach", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "首领开局牌实战", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, First, Second], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
