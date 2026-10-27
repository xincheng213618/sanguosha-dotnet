using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheFengyiEncounterChecks
{
    private const string Dong = "shanhe:s5-dongzhuo-fengyi", Jiuchi = "shanhe:s5-jiuchi-14210", Daoshu = "shanhe:s5-daoshu-952";
    private const string Roulin = "shanhe:s5-roulin-14211", Benghuai = "boundary:benghuai-current", Zaiqi = "shanhe:s5-zaiqi-887";
    private const string Lvbu = "shanhe:s5-lvbu-baji", Diao = "shanhe:s5-diao-chan-biyue", Encounter = "luoyang:fengyi-s5";

    public static void ActualGaichaoSingleEnemyRecipeKeepsSourceHealthDrawAndConditionalPackagesCold()
    {
        const string gaichao = StandardShanheS5GaichaoPackage.DongZhuoGaichaoId;
        const string encounterId = "luoyang:gaichao-s5";
        var aliasPackage = new StandardShanheS5GaichaoPackage().Manifest.Id;
        var sourcePackage = new StandardShanheS5FengyiPackage().Manifest.Id;
        foreach (var difficulty in new[] { 1, 2, 3 })
        {
            var definition = StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var actual = definition.Nodes.Single(node => node.Id == "luoyang").Encounters!.Single(item => item.Id == encounterId);
            Need(actual.Enemies is [{ GeneralId: gaichao, MaxHpBonus: 0, Hp: null, InitialHandBonus: 0,
                    DrawBonus: 0, SlashBonus: 0, StartingHand: null }] &&
                actual.Enemies.All(member => (member.ExtraSkillIds?.Count ?? 0) == 0 && (member.QualifiedPrintedLordSkillIds?.Count ?? 0) == 0) &&
                actual.Allies is [{ GeneralId: "classic:sun-jian" }],
                "The actual 2687 candidate is one raw source enemy at every local tier, with the explicitly local Sun Jian companion and no invented growth, fixed hand or extra Lord qualification.");
            var original = StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.LocalEditionId);
            Need(original.Nodes.SelectMany(node => node.Encounters ?? []).All(item => item.Id != encounterId &&
                    item.Enemies.All(member => member.GeneralId != gaichao)),
                "The 2024 edition never acquires the independent 2687 recipe through a shared map node.");
            if (difficulty != 1) continue;

            var fengyi = definition.Nodes.Single(node => node.Id == "luoyang").Encounters!.Single(item => item.Id == Encounter);
            var oldSetup = new ShanheBattleSetup(83, 1, "luoyang", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [],
                fengyi.Enemies, fengyi.Allies!, ShanheCampaignContent.GetContentHash(definition));
            var oldRegistry = StandardShanheCampaignCatalog.CreateBattleRegistry(oldSetup);
            var oldHash = oldRegistry.ContentHash;
            Need(!oldRegistry.Generals.ContainsKey(gaichao) && oldRegistry.Packages.All(package => package.Id != aliasPackage),
                "The existing three-enemy Fengyi recipe does not import the unrelated single-enemy alias package.");

            var shortDefinition = new ShanheCampaignDefinition("fixture-gaichao-source", "真实改朝罪魁入战", ["classic:guan-yu"], ["gaichao"],
                [new("gaichao", "真实单首领", ShanheNodeKind.BossBattle, [], Encounters: [actual])], []);
            var run = ShanheCampaignSession.Create(shortDefinition, new(83, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
            var moved = run.Submit(new MoveShanheNodeCommand("gaichao", run.Revision));
            Need(moved.Accepted, moved.Error ?? "The real Gaichao map transition was rejected.");
            var setup = run.PendingBattleSetup!;
            var encoded = ShanheCampaignJson.Serialize(run.CreateCheckpoint());
            var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(encoded), shortDefinition,
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            Need(run.Snapshot.Phase == ShanheCampaignPhase.Battle && setup.EnemyRoster is [{ GeneralId: gaichao }] &&
                setup.AllyRoster is [{ GeneralId: "classic:sun-jian" }] && setup.StartingCards.Count == 0 && setup.StartingEquipment.Count == 0 &&
                setup.EnemyRoster is IList<ShanheCombatantDefinition> { IsReadOnly: true } &&
                ShanheBattleSetup.ValueEquals(restored.PendingBattleSetup, setup) &&
                ShanheCampaignJson.Serialize(restored.CreateCheckpoint()) == encoded,
                "A real boss-node command and campaign cold replay preserve the exact single-enemy recipe instead of substituting the three-enemy form.");

            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            var mode = registry.Modes[ShanheBattleSetup.BattleModeId];
            var boss = registry.Generals[gaichao];
            var source = registry.Generals[Dong];
            Need(boss is { BaseHp: 15, InitialHp: 10, FactionId: "qun", Gender: GeneralGender.Male } &&
                boss.SkillIds.Order().SequenceEqual(new[] { Jiuchi, Roulin, Benghuai, Daoshu }.Order()) &&
                boss.SkillIds.Order().SequenceEqual(source.SkillIds.Order()) && registry.Skills.ContainsKey(Zaiqi) &&
                mode is { MinPlayers: 3, MaxPlayers: 3, ModeKind: ContentModeKind.Team, BossActivity: null, TeamActivity: null } &&
                mode.TeamCounts!.GetValueOrDefault(ShanheBattleSetup.PlayerTeamId) == 2 &&
                mode.TeamCounts!.GetValueOrDefault(ShanheBattleSetup.EnemyTeamId) == 1 &&
                registry.GetDeck(mode.DeckId!) is { InitialHandSize: 4, DrawPerTurn: 2 } &&
                registry.Packages.Count(package => package.Id == aliasPackage) == 1 &&
                registry.Packages.Count(package => package.Id == sourcePackage) == 1,
                "The independent 2687 alias has exactly the four verified source skills, raw 10/15 health and native H4/D2 quantities; its capability dependency is registered once.");
            var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            AssertGaichaoInitial(game, setup, gaichao); Start(game); AssertGaichaoInitial(game, setup, gaichao);
            Need(ShanheBattleSetup.ValueEquals(game.CreateCheckpoint().Options.ShanheBattle, setup) &&
                game.CreateCheckpoint().ContentHash == registry.ContentHash,
                "The real battle factory preserves the campaign-owned recipe and complete conditional content fingerprint.");
            Cold(game, registry);
            ReachGaichaoFirstDraw(game, gaichao);
            var draw = game.Events.Select(item => item.Payload).OfType<NativeDrawInvocationRecordedEvent>()
                .Single(fact => fact.OwnerSeat == 2 && fact.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Draw, ActorSeat: 2 });
            Need(draw is { Requested: 2, ActualCount: 2 } &&
                game.CardMovements.Count(move => move.Sequence > draw.SequenceBefore && move.Sequence <= draw.SequenceAfter &&
                    move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(2) && move.Reason == draw.Reason) == 2,
                "After real player and companion turns, the actual single boss pays one native two-card Draw with two real deck entities.");
            Cold(game, registry);

            var mixed = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { EnemyRoster = [new(gaichao), new(Dong)] });
            Need(mixed.Generals.ContainsKey(gaichao) && mixed.Generals.ContainsKey(Dong) &&
                mixed.Packages.Count(package => package.Id == aliasPackage) == 1 && mixed.Packages.Count(package => package.Id == sourcePackage) == 1 &&
                oldRegistry.ContentHash == oldHash && StandardShanheCampaignCatalog.CreateBattleRegistry(oldSetup).ContentHash == oldHash &&
                !oldRegistry.Generals.ContainsKey(gaichao),
                "A recipe containing both source identities imports their shared abilities once and cannot contaminate the old Fengyi registry cache.");
            var ordinary = definition.Nodes.SelectMany(node => node.Encounters ?? []).First();
            var ordinaryRegistry = StandardShanheCampaignCatalog.CreateBattleRegistry(oldSetup with
                { EnemyRoster = ordinary.Enemies, AllyRoster = ordinary.Allies ?? [] });
            Need(!ordinaryRegistry.Generals.ContainsKey(gaichao) && ordinaryRegistry.Packages.All(package => package.Id != aliasPackage),
                "An ordinary unearned recipe stays free of the new alias package after the new and mixed registries have both been used.");
        }
    }

    private static void AssertGaichaoInitial(GameEngine game, ShanheBattleSetup setup, string gaichao)
    {
        Need(game.PlayerCount == 3 && setup.AllyRoster.Count == 1 && setup.EnemyRoster.Count == 1,
            "The source enemy occupies native seat two in the explicitly local 2-versus-1 recipe.");
        var boss = game.CreateSnapshot(2).Players[2];
        Need(boss is { GeneralId: var id, Hp: 10, MaxHp: 15, Armor: 0, HandCount: 4, IsAlive: true } && id == gaichao &&
            boss.Equipment.Count == 0 && boss.Skills!.Select(skill => skill.ContentId).Order().SequenceEqual(new[] { Jiuchi, Roulin, Benghuai, Daoshu }.Order()) &&
            Enumerable.Range(0, 3).All(viewer => game.CreateSnapshot(viewer).Players[2] is
                { IsGeneralPublic: true, TeamId: ShanheBattleSetup.EnemyTeamId, Hp: 10, MaxHp: 15 }),
            "Construction and StartGame expose the exact living 10/15, zero-armor four-skill boss with four ordinary opening cards to every appropriate viewer.");
        Need(game.Events.All(item => item.Payload is not (DamageRequestedEvent or DamageAppliedEvent or AfterDamageEvent or
                ProgramSkillHpLostEvent or MaximumHpChangedEvent or PlayerDyingEvent)) &&
            game.CardMovements.Count(move => move.To == CardLocation.Hand(2) && move.Reason == CardMoveReasons.InitialDeal) == 4 &&
            game.CardMovements.Where(move => move.To == CardLocation.Hand(2) && move.Reason == CardMoveReasons.InitialDeal)
                .All(move => move.From == CardLocation.DrawPile),
            "The partial initial health is genuine state, and all four initial cards come from the native deck without generated fixed entities or a fake opening damage cost.");
    }

    private static void ReachGaichaoFirstDraw(GameEngine game, string gaichao)
    {
        for (var step = 0; step < 220; step++)
        {
            if (game.Events.Any(item => item.Payload is NativeDrawInvocationRecordedEvent
                { OwnerSeat: 2, Phase: { Kind: ActualDiscardRecoveryPhaseKind.Draw, ActorSeat: 2 } })) return;
            var prompt = game.CreateSnapshot(0).PendingDecision;
            GameCommand command = prompt switch
            {
                { Kind: DecisionKind.PlayCard } => new EndPlayPhaseCommand(0, game.Revision, prompt.PromptId),
                { Kind: DecisionKind.DiscardCards } => new DiscardCardsCommand(0,
                    prompt.ValidCardIds.Take(prompt.RequiredCardCount).ToArray(), prompt.PromptId, game.Revision),
                { Choices.Count: > 0 } => new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, game.Revision),
                null => new AdvanceOneStepCommand(game.Revision),
                _ => throw new InvalidOperationException("Unexpected human boundary before the Gaichao source's first actual Draw: " + JsonSerializer.Serialize(prompt))
            };
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "A native command before the real boss Draw was rejected.");
        }
        throw new InvalidOperationException("The actual source boss never reached its first native Draw: " + gaichao);
    }

    private static void Start(GameEngine game) { var result = game.Submit(new StartGameCommand()); Need(result.Accepted, result.Error?.Message ?? "Actual source encounter did not start."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, game.PlayerCount).Select(viewer => SnapshotJson.Serialize(game.CreateSnapshot(viewer))),
        Stack = JsonSerializer.Serialize(game.ResolutionStack), game.CardMovements, Events = game.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType())), Options = game.CreateCheckpoint().Options });
    private static void Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(State(game) == State(restored), "Cold native replay preserves every real seat, original partial starting HP, each viewer's cards and exact source-owned recipe without replaying a fake initialization cost.");
    }
}
