using System.Text.Json;
using CardGame.Core;
using static BoundaryWangJiScenario;

internal static class BoundaryWangJiMobileChecks
{
    public static void QizhiPaidChargeActualPrivateHandSequentialDiscardCold()
    {
        var scene = CreateScene(withWatch: true); var game = scene.Game; var enemy = EnemySeat(game);
        Need(Charge(game) == 1, "Printed 1/2 charge initializes once before the first turn.");
        var enemyBefore = game.CreateSnapshot(0).Players[enemy].HandCount;
        ActivateQizhi(game, enemy); Play(game);
        Need(Charge(game) == 0 && game.CreateSnapshot(0).Players[enemy].HandCount == enemyBefore + 2, "Qizhi spends one real charge to draw two native entities for its selected recipient.");
        UseCard(game, CardKind.DrawTwo); Reach(game, Inspecting);
        var first = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.BoundaryWangJi?.Stage == BoundaryWangJiStage.Inspect);
        var ids = first.BoundaryWangJi!.Viewed.ToArray();
        Need(ids.Length == enemyBefore + 2 && ids.ToHashSet().SetEquals(game.CreateSnapshot(0).PrivateRevealedCards!.Select(card => card.Id)) &&
            game.CreateSnapshot(1).PrivateRevealedCards is null && game.CreateSnapshot(1).Players[enemy].Hand.Count == (enemy == 1 ? enemyBefore + 2 : 0),
            "The owner sees the recipient's entire actual current hand; another viewer receives no inspection or hidden hand.");
        var paid = ids.First(); Answer(game, choice => choice.Cards.SequenceEqual([paid])); Play(game);
        Need(game.CardMovements.Count(move => move.CardId == paid && move.Reason.Value == "skill-program.boundary:qizhi-mobile.discard") == 1, "The inspected selected physical card is actually discarded once.");
        UseCard(game, CardKind.Alcohol); Reach(game, Inspecting); Answer(game, choice => choice.Cards.Count == 1);
        DrainUntil(game, () => Charging(Decision(game))); Answer(game, choice => choice.Targets.SequenceEqual([0])); Play(game);
        Need(Charge(game) == 1, "The second true basic/instant-trick use restores one selected charge.");
        ActivateQizhi(game, 0); Play(game);
        UseCard(game, CardKind.DrawTwo); Reach(game, Inspecting);
        var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.BoundaryWangJi?.Stage == BoundaryWangJiStage.Inspect);
        Need(original.BoundaryWangJi!.Targets.SequenceEqual(new[] { 0, enemy }), "Later uses inspect every actual phase recipient in action order, including self.");
        var selfPaid = Decision(game)!.Choices.First().Cards.Single(); Answer(game, choice => choice.Cards.SequenceEqual([selfPaid])); Reach(game, WatchPending);
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == original.Id);
        Need(parent.BoundaryWangJi is { Stage: BoundaryWangJiStage.DiscardChildren, Cursor: 0, Payment: { } receipt } && receipt.CardId == selfPaid,
            "The paid native logical discard remains on its original inspection cursor while the real movement child pauses.");
        Cold(ref game, scene.Registry); ContinueWatch(game); Reach(game, Inspecting);
        var resumed = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == original.Id);
        Need(resumed.BoundaryWangJi is { Cursor: 1 } && resumed.BoundaryWangJi.Targets[1] == enemy && game.CardMovements.Count(move => move.CardId == selfPaid && move.Reason.Value == "skill-program.boundary:qizhi-mobile.discard") == 1,
            "Cold recovery resumes the next recipient without paying the preceding discard twice.");
        Answer(game, choice => choice.Cards.Count == 1); Play(game);
        UseCard(game, CardKind.DrawTwo); Reach(game, Inspecting); Answer(game, choice => choice.Cards.Count == 1);
        DrainUntil(game, () => Inspecting(Decision(game))); Answer(game, choice => choice.Cards.Count == 1);
        DrainUntil(game, () => Charging(Decision(game))); Answer(game, choice => choice.Targets.SequenceEqual([0])); Play(game);
        var action = game.GetLegalActions().Single(action => action.SkillId == Qizhi);
        Need(Charge(game) == 1 && action.SelectableTargetSeats.Count == 2 && !action.SelectableTargetSeats.Contains(0) && !action.SelectableTargetSeats.Contains(enemy), "Each recipient stays unavailable for the whole actual Play phase even after gaining new charge.");
    }

    public static void JinquOnlyTrueBasicTrickPairsAndHistoricalPeakNativeDraw()
    {
        var scene = CreateScene(withWatch: true, foreignCharge: true); var game = scene.Game;
        UseCard(game, CardKind.Crossbow); Play(game);
        var chain = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.IronChain).Id;
        Accept(game, new RecastCardCommand(0, chain, game.Revision, Decision(game)!.PromptId)); Play(game);
        Need(game.Events.Select(envelope => envelope.Payload).OfType<BoundaryWangJiUseEvent>().Count() == 0 && Charge(game) == 1, "Equipment use and a native recast do not advance the basic/ordinary-trick pair counter.");
        UseCard(game, CardKind.DrawTwo); Play(game);
        Need(game.Events.Select(envelope => envelope.Payload).OfType<BoundaryWangJiUseEvent>().Count() == 1 && Charge(game) == 1, "One actual ordinary trick use alone does not recharge.");
        UseCard(game, CardKind.DrawTwo); DrainUntil(game, () => Charging(Decision(game)));
        Cold(ref game, scene.Registry); Answer(game, choice => choice.Targets.SequenceEqual([0])); Play(game);
        Need(Charge(game) == 2 && game.Events.Select(envelope => envelope.Payload).OfType<BoundaryWangJiUseEvent>().Count() == 2, "Two completed true ordinary-trick uses allow exactly one original-frame charge grant, capped at two.");
        var enemy = EnemySeat(game); ActivateQizhi(game, enemy); Play(game);
        Need(Charge(game) == 1, "Spending charge after its peak leaves one current point.");
        UseCard(game, CardKind.DrawTwo); Reach(game, Inspecting); Answer(game, choice => choice.Cards.Count == 1); Play(game);
        var empty = game.CreateSnapshot(0).Players.Single(player => player.GeneralId == ForeignEmpty).Seat;
        var full = game.CreateSnapshot(0).Players.Single(player => player.GeneralId == ForeignFull).Seat;
        UseCard(game, CardKind.DrawTwo); Reach(game, Inspecting); Answer(game, choice => choice.Cards.Count == 1);
        DrainUntil(game, () => Charging(Decision(game))); Answer(game, choice => choice.Targets.SequenceEqual([empty])); Play(game);
        Need((game.CreateSnapshot(0).Players[empty].Markers ?? []).Single(marker => marker.Kind == PlayerMarkerKind.MouMengHuoCharge).Count == 1 && Charge(game) == 1,
            "Jinqu can select another actual charge-skill holder and adds one point to that holder's native pool without charging Wang Ji.");
        UseCard(game, CardKind.Alcohol); Reach(game, Inspecting); Answer(game, choice => choice.Cards.Count == 1); Play(game);
        var slash = game.GetLegalActions().First(action => action.Kind == LegalActionKind.Slash);
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, Decision(game)!.PromptId, slash.PlayedCardKind));
        Reach(game, Inspecting); Answer(game, choice => choice.Cards.Count == 1);
        DrainUntil(game, () => Charging(Decision(game))); Answer(game, choice => choice.Targets.SequenceEqual([full])); Play(game);
        Need((game.CreateSnapshot(0).Players[full].Markers ?? []).Single(marker => marker.Kind == PlayerMarkerKind.MouJiangWeiCharge).Count == 4 && Charge(game) == 1,
            "An external Jinqu point respects the selected existing pool's full four-point capacity; it creates neither a new pool nor an overflow point.");
        var before = game.CreateSnapshot(0).Players[0].HandCount;
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, Decision(game)!.PromptId)); Reach(game, WatchPending);
        var ending = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Jinqu && frame.TriggerId == "ending-peak");
        Need(ending.BoundaryWangJi is { Stage: BoundaryWangJiStage.DrawChildren, Requested: 2, Draw.ActualDrawIds.Count: 2 } && game.CreateSnapshot(0).Players[0].HandCount == before + 2,
            "The locked ending draw is the historical two-point peak, rather than the current one-point charge or a fixed classic reward.");
        var endingId = ending.Id; DrainUntil(game, () => !game.ResolutionStack.Any(frame => frame.Id == endingId));
        Need(game.Events.Select(envelope => envelope.Payload).OfType<BoundaryWangJiDrawReturnedEvent>().Count(entry => entry.FrameId == endingId && entry.Actual == 2) == 1, "The original native highest-charge draw settles once through its gained-card children.");
    }
    private static int Charge(GameEngine game) => game.CreateSnapshot(0).Players[0].Markers?.SingleOrDefault(marker => marker.Kind == PlayerMarkerKind.BoundaryWangJiCharge)?.Count ?? 0;
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void Cold(ref GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(JsonSerializer.Serialize(game.CreateSnapshot(0)) == JsonSerializer.Serialize(restored.CreateSnapshot(0)), "The command prefix restores the same original paid instruction, private inspection and exact target cursor.");
        game = restored;
    }
}
