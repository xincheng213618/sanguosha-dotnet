using System.Text.Json;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ShanheCampaignChecks
{
    public static void CurrentRankHealthStartsFromPrintedHpAndRetainsExplicitGrowthCold()
    {
        var roster = StandardShanheCampaignCatalog.PlayerRosterRegistry;
        Require(roster.Generals["classic:wei-yan"].BaseHp == 4 && roster.Generals["classic:qu-yi"].BaseHp == 4 &&
            roster.Generals["classic:zhuge-liang"].BaseHp == 3,
            "The production content gives Wei Yan and Qu Yi four printed HP and Zhuge Liang three.");
        foreach (var difficulty in new[] { 1, 2, 3 })
        foreach (var codex in new[] { false, true })
        foreach (var general in new[] { "classic:wei-yan", "classic:qu-yi" })
        {
            var definition = codex ? StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var options = new ShanheCampaignOptions(71, general)
            {
                PlayerRank = 5, UseCodexRewardPool = codex,
                EquippedTacticIds = [StandardShanheCampaignCatalog.CurrentDangtouRewardId]
            };
            var session = ShanheCampaignSession.Create(definition, options, StandardShanheCampaignCatalog.CreateBattleRegistry);
            Require(definition.StartingBonuses == new ShanheTacticBonuses() && session.Snapshot is { Hp: 6, MaxHp: 6, PlayerRank: 5 } &&
                session.Snapshot.Bonuses == ShanheGeneralRanks.GetBonuses(5) && session.Snapshot.TacticIds.SequenceEqual(options.EquippedTacticIds),
                "A fifth-rank four-HP general with only Dangtou uses only rank privileges in every current difficulty and catalogue path.");
            var checkpoint = ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint()));
            var restored = ShanheCampaignSession.Restore(checkpoint,
                StandardShanheCampaignCatalog.CreateDefinitionForCheckpoint(checkpoint, difficulty), StandardShanheCampaignCatalog.CreateBattleRegistry);
            Require(restored.Snapshot is { Hp: 6, MaxHp: 6 } && Snapshot(restored) == Snapshot(session),
                "Cold campaign replay does not reapply rank or insert old local preparation HP.");
            var firstNode = session.Snapshot.AvailableNodes[0];
            Accept(session, new MoveShanheNodeCommand(firstNode.Id, session.Revision));
            var setup = session.PendingBattleSetup!;
            Require(setup is { Hp: 6, MaxHpBonus: 2, PlayerRank: 5, InitialHandBonus: 2, DrawBonus: 1, SlashBonus: 1 },
                "The actual native recipe carries the rank's health, initial hand, normal draw and Slash bonuses exactly once.");
            var battle = StandardShanheCampaignCatalog.CreateBattle(setup);
            Require(battle.CreateSnapshot(0).Players[0] is { Hp: 6, MaxHp: 6, HandCount: 6 } &&
                RankNativeCounters(battle) == (3, 2, 6),
                "The real fifth-rank player starts with six HP, six physical cards, three normal draws, two Slashes and the ordinary six-card hand limit.");
            Accept(battle, new StartGameCommand());
            var coldBattle = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(battle.CreateCheckpoint())),
                StandardShanheCampaignCatalog.CreateBattleRegistry(setup));
            Require(coldBattle.CreateSnapshot(0).Players[0] is { Hp: 6, MaxHp: 6 } &&
                RankNativeCounters(coldBattle) == (3, 2, 6) &&
                SnapshotJson.Serialize(coldBattle.CreateSnapshot(0)) == SnapshotJson.Serialize(battle.CreateSnapshot(0)),
                "The real started battle retains the same six HP and full state after native checkpoint replay.");
        }

        var current = StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        foreach (var general in new[] { "classic:wei-yan", "classic:qu-yi", "classic:zhuge-liang" })
        foreach (var rank in new[] { 1, 2, 3, 4, 5 })
        {
            var session = ShanheCampaignSession.Create(current, new(71, general) { PlayerRank = rank }, StandardShanheCampaignCatalog.CreateBattleRegistry);
            var privilege = ShanheGeneralRanks.GetBonuses(rank);
            var expected = roster.Generals[general].BaseHp + privilege.MaxHp;
            Require(session.Snapshot.Hp == expected && session.Snapshot.MaxHp == expected,
                "Current health follows each general's own printed value and selected rank, rather than fixing every general at six.");
            Accept(session, new MoveShanheNodeCommand(session.Snapshot.AvailableNodes[0].Id, session.Revision));
            var battle = StandardShanheCampaignCatalog.CreateBattle(session.PendingBattleSetup!);
            Require(battle.CreateSnapshot(0).Players[0].HandCount == 4 + privilege.InitialHand &&
                RankNativeCounters(battle) == (2 + privilege.Draw, 1 + privilege.Slash, expected),
                "Every rank uses its printed opening-hand, normal-draw and Slash privileges while the ordinary hand limit follows actual HP.");
            var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(battle.CreateCheckpoint())),
                StandardShanheCampaignCatalog.CreateBattleRegistry(session.PendingBattleSetup!));
            Require(SnapshotJson.Serialize(cold.CreateSnapshot(0)) == SnapshotJson.Serialize(battle.CreateSnapshot(0)) &&
                RankNativeCounters(cold) == RankNativeCounters(battle), "Each rank's physical hand and native quota queries survive cold replay unchanged.");
        }
        var grown = ShanheCampaignSession.Create(current, new(71, "classic:wei-yan")
        { PlayerRank = 5, UseCodexRewardPool = true, EquippedTacticIds = ["tactic-build-2054", "tactic-build-2159",
            StandardShanheCampaignCatalog.CurrentMuniuLiumaRewardId, StandardShanheCampaignCatalog.CurrentLianjiRewardId] },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        Accept(grown, new MoveShanheNodeCommand(grown.Snapshot.AvailableNodes[0].Id, grown.Revision));
        var grownBattle = StandardShanheCampaignCatalog.CreateBattle(grown.PendingBattleSetup!);
        Require(grownBattle.CreateSnapshot(0).Players[0] is { Hp: 6, MaxHp: 6, HandCount: 5 },
            "Real logistics still subtracts one physical opening card from the six-card fifth-rank recipe.");
        Accept(grownBattle, new StartGameCommand());
        for (var step = 0; step < 80 && grownBattle.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
        {
            Require(grownBattle.PendingDecision?.PlayerSeat != 0, "The owned opening and passive tactics must reach the first real play prompt without extra player input.");
            Accept(grownBattle, new AdvanceOneStepCommand(grownBattle.Revision));
        }
        Require(grownBattle.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            grownBattle.CreateSnapshot(0).Players[0] is { Hp: 7, MaxHp: 7, HandCount: 11 } && RankNativeCounters(grownBattle) == (6, 3, 6),
            "Tipo still adds and recovers one HP; logistics and Muniu add three real normal draws, Lianji adds a Slash, and Muniu subtracts one from the actual HP-based hand limit.");
        var grownCold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(grownBattle.CreateCheckpoint())),
            StandardShanheCampaignCatalog.CreateBattleRegistry(grown.PendingBattleSetup!));
        Require(SnapshotJson.Serialize(grownCold.CreateSnapshot(0)) == SnapshotJson.Serialize(grownBattle.CreateSnapshot(0)) &&
            RankNativeCounters(grownCold) == (6, 3, 6),
            "An explicit paid health tactic retains its actual effect through native replay.");

        var oldPreparation = new ShanheTacticBonuses(MaxHp: 2, InitialHand: 2, Draw: 1, Slash: 1);
        var oldDefinition = current with { StartingBonuses = oldPreparation };
        var old = ShanheCampaignSession.Create(oldDefinition, new(71, "classic:wei-yan") { PlayerRank = 5, UseCodexRewardPool = true },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        Require(old.Snapshot is { Hp: 8, MaxHp: 8 }, "The old scenario recipe explains the original eight-HP result.");
        var oldJson = ShanheCampaignJson.Serialize(old.CreateCheckpoint());
        MustThrow(() => StandardShanheCampaignCatalog.CreateDefinitionForCheckpoint(ShanheCampaignJson.Deserialize(oldJson), 2));
        Require(ShanheCampaignJson.Serialize(old.CreateCheckpoint()) == oldJson,
            "The changed current recipe rejects a different old health fingerprint without mutating its source journal.");
        Require(StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.LocalEditionId).StartingBonuses == oldPreparation,
            "The separately named legacy local edition retains its entire original preparation recipe.");
    }

    private static (int Draw, int Slash, int HandLimit) RankNativeCounters(GameEngine battle)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var player = ((IReadOnlyList<CharacterState>)typeof(GameEngine).GetField("_players", flags)!.GetValue(battle)!)[0];
        var draw = (RuleQueryEvaluation)typeof(GameEngine).GetMethod("EvaluateDrawCount", flags)!.Invoke(battle, [player])!;
        Require(draw.Value is FiniteRuleQueryValue, "The rank fixture's native normal draw count is finite.");
        return (((FiniteRuleQueryValue)draw.Value).Value,
            (int)typeof(GameEngine).GetMethod("GetSlashUseLimit", flags)!.Invoke(battle, [player, null, null])!,
            (int)typeof(GameEngine).GetMethod("GetHandLimit", flags)!.Invoke(battle, [player])!);
    }

    public static void GeneralRankBonusesFreezeOnceAndLegacyRecipesStayUnchanged()
    {
        ShanheTacticBonuses[] privileges = [new(), new(InitialHand: 1), new(MaxHp: 1, InitialHand: 1, Slash: 1),
            new(MaxHp: 1, InitialHand: 2, Draw: 1, Slash: 1), new(MaxHp: 2, InitialHand: 2, Draw: 1, Slash: 1)];
        var definition = RankDefinition() with { StartingBonuses = new(1, 1, 1, 1) };
        foreach (var rank in new int?[] { null, 1, 2, 3, 4, 5 })
        {
            var privilege = privileges[(rank ?? 1) - 1];
            Require(ShanheGeneralRanks.GetBonuses(rank ?? 1) == privilege, "Rank privileges must match the verified tier table.");
            var session = ShanheCampaignSession.Create(definition, new(71, Player) { PlayerRank = rank }, Registry);
            var expected = new ShanheTacticBonuses(1 + privilege.MaxHp, 1 + privilege.InitialHand, 1 + privilege.Draw, 1 + privilege.Slash);
            Require(session.Snapshot.PlayerRank == (rank ?? 1) && !session.Snapshot.RebirthUsed &&
                session.Snapshot.Bonuses == expected && session.Snapshot.Hp == 4 + expected.MaxHp &&
                session.Snapshot.MaxHp == session.Snapshot.Hp && session.Snapshot.ExtraSkillIds.Count == 0 && session.Snapshot.ExtraSkillSlots == 0,
                "The selected rank adds to starting bonuses once, starts at its persistent maximum and spends no learned-skill slot.");
            var source = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            session = RestoreRankRun(session, definition);
            Require(session.Snapshot.Bonuses == expected && session.CreateCheckpoint().Options.PlayerRank == rank,
                "Cold reconstruction must retain the original nullable rank and cannot apply its growth again.");
            Accept(session, new MoveShanheNodeCommand("first", session.Revision));
            var setup = session.PendingBattleSetup!;
            Require(setup.PlayerRank == rank && setup.RebirthAvailable == (rank == 5) &&
                setup.ExtraSkillIds.Contains(ShanheGeneralRanks.RebirthSkillId) == (rank == 5) && session.Snapshot.ExtraSkillIds.Count == 0,
                "Only rank five adds the separate unspent rebirth to the actual battle recipe, keeping it outside replaceable skill slots.");
            var battle = CreateBattle(setup);
            var player = battle.CreateSnapshot(0).Players[0];
            Require(player.MaxHp == 4 + expected.MaxHp && player.Hp == player.MaxHp && player.HandCount == 4 + expected.InitialHand,
                "The native encounter receives the exact once-added rank health and opening-hand bonuses.");
            Require(RankNativeCounters(battle) == (2 + expected.Draw, 1 + expected.Slash, player.Hp),
                "Explicit authored starting draw and Slash bonuses remain additive with rank, and hand limit continues to follow actual HP.");
            Cold(session, definition);
            if (rank is null)
                Require(!source.Contains("\"PlayerRank\"", StringComparison.Ordinal) &&
                    !JsonSerializer.Serialize(setup).Contains("\"PlayerRank\"", StringComparison.Ordinal) &&
                    !JsonSerializer.Serialize(setup).Contains("\"RebirthAvailable\"", StringComparison.Ordinal),
                    "Unranked historical options and battle recipes omit the new fields instead of changing legacy checkpoint text.");
        }
        MustThrow(() => ShanheCampaignSession.Create(definition, new(71, Player) { PlayerRank = 0 }, Registry));
        MustThrow(() => ShanheCampaignSession.Create(definition, new(71, Player) { PlayerRank = 6 }, Registry));
    }

    public static void FifthRankRebirthUsageSurvivesTwoBattlesAndColdVictoryOrDefeat()
    {
        var definition = RankDefinition();
        foreach (var useFirstBattle in new[] { true, false })
        {
            var session = ShanheCampaignSession.Create(definition, new(71, Player) { PlayerRank = 5 }, Registry);
            Accept(session, new MoveShanheNodeCommand("first", session.Revision));
            var first = BeginRankBattle(session.PendingBattleSetup!);
            if (useFirstBattle) first = UseRankRebirth(first);
            FinishRankBattle(first, victory: true);
            Require(session.CompleteBattle(first, session.Revision).Accepted && session.Snapshot.Phase == ShanheCampaignPhase.Map &&
                session.Snapshot.RebirthUsed == useFirstBattle && session.Snapshot.ExtraSkillIds.Count == 0,
                "A replay-verified first victory records only an actually consumed rebirth, never an unused grant or learned-skill slot.");
            session = RestoreRankRun(session, definition);
            Accept(session, new MoveShanheNodeCommand("second", session.Revision));
            var next = session.PendingBattleSetup!;
            Require(next.RebirthAvailable == !useFirstBattle &&
                next.ExtraSkillIds.Contains(ShanheGeneralRanks.RebirthSkillId) == !useFirstBattle && session.Snapshot.RebirthUsed == useFirstBattle,
                "The next encounter carries an unused campaign rebirth, while a spent one is absent even after cold journal reconstruction.");
            session = RestoreRankRun(session, definition);
            var second = BeginRankBattle(session.PendingBattleSetup!);
            if (!useFirstBattle) second = UseRankRebirth(second);
            FinishRankBattle(second, victory: useFirstBattle);
            Require(session.CompleteBattle(second, session.Revision).Accepted && session.Snapshot.RebirthUsed &&
                session.Snapshot.Phase == (useFirstBattle ? ShanheCampaignPhase.Won : ShanheCampaignPhase.Lost) &&
                session.Snapshot.Bonuses == ShanheGeneralRanks.GetBonuses(5),
                "Actual rebirth consumption survives both final victory and a later genuine defeat, with no second application of rank growth.");
            RestoreRankRun(session, definition);
        }
    }

    private static ShanheCampaignDefinition RankDefinition() => new("fixture:rank-campaign", "升阶征程", [Player], ["first"],
        [new("first", "首战", ShanheNodeKind.Battle, ["second"], [new("guard", "守军", [new(Enemy)])]),
         new("second", "再战", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(Enemy)])])], [], ExtraSkillSlots: 0);

    private static ShanheCampaignSession RestoreRankRun(ShanheCampaignSession session, ShanheCampaignDefinition definition)
    {
        var checkpoint = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(checkpoint), definition, Registry);
        Require(ShanheCampaignJson.Serialize(restored.CreateCheckpoint()) == checkpoint && Snapshot(restored) == Snapshot(session),
            "The original rank options, battle proofs, current recipe and expedition rebirth consumption must all derive identically from cold command replay.");
        return restored;
    }

    private static GameEngine BeginRankBattle(ShanheBattleSetup setup)
    {
        var battle = CreateBattle(setup);
        Accept(battle, new StartGameCommand());
        ReachRankBoundary(battle, () => battle.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        return battle;
    }

    private static GameEngine UseRankRebirth(GameEngine battle)
    {
        var hand = battle.CreateSnapshot(0).Players[0].HandCount;
        Accept(battle, new UseProgramSkillCommand(0, Force, "surrender", [], [], battle.Revision, battle.PendingDecision!.PromptId));
        static bool Rebirth(PromptChoice choice) => choice.Parameters.GetValueOrDefault("response") == "program-trigger" &&
            choice.Parameters.GetValueOrDefault("skill-id") == ShanheGeneralRanks.RebirthSkillId;
        ReachRankBoundary(battle, () => battle.PendingDecision is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 } prompt && prompt.Choices.Any(Rebirth));
        var checkpoint = GameCheckpointJson.Serialize(battle.CreateCheckpoint());
        var before = SnapshotJson.Serialize(battle.CreateSnapshot(0));
        battle = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), Registry(battle.CreateCheckpoint().Options.ShanheBattle!));
        Require(SnapshotJson.Serialize(battle.CreateSnapshot(0)) == before && GameCheckpointJson.Serialize(battle.CreateCheckpoint()) == checkpoint,
            "A real self-dying rebirth offer survives cold battle replay before its once-only usage is consumed.");
        var decision = battle.PendingDecision!;
        Accept(battle, new AnswerPromptCommand(0, decision.PromptId, decision.Choices.Single(Rebirth).Id, battle.Revision));
        ReachRankBoundary(battle, () => battle.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Require(battle.CreateSnapshot(0).Players[0] is { Hp: 5 } player && player.HandCount == hand + 5 &&
            battle.Events.Select(e => e.Payload).OfType<SkillUsageConsumedEvent>().Count(e => e.SkillOwnerSeat == 0 &&
                e.SkillId == ShanheGeneralRanks.RebirthSkillId && e.Scope == SkillUsageScope.Game && e.Count == 1) == 1,
            "The real production rebirth produces one native game usage receipt and gains five cards while recovering to five HP.");
        return battle;
    }

    private static void FinishRankBattle(GameEngine battle, bool victory)
    {
        Accept(battle, new UseProgramSkillCommand(0, Force, victory ? "defeat" : "surrender", [], victory ? [1] : [],
            battle.Revision, battle.PendingDecision!.PromptId));
        ReachRankBoundary(battle, () => battle.State.Status == EngineStatus.Completed);
    }

    private static void ReachRankBoundary(GameEngine battle, Func<bool> arrived)
    {
        for (var step = 0; step < 60; step++)
        {
            if (arrived()) return;
            Require(battle.PendingDecision?.PlayerSeat != 0, "Unexpected player input before the bounded rank fixture reached its native boundary.");
            Accept(battle, new AdvanceOneStepCommand(battle.Revision));
        }
        throw new InvalidOperationException("The bounded rank fixture did not reach its expected native boundary.");
    }
}
