using System.Text.Json.Nodes;
using CardGame.Core;

internal static partial class PhaseStartCardControlChecks
{
    private static void HuaManUsesFrozenDirectedHistory()
    {
        HuaManRejectsStaleActorQualification();
        var (g, registry) = StartDirected("souying-claim");
        SouyingPlay(g, CardKind.IronChain, [1]); SouyingReachPlay(g, forbidOffer: true);
        var card = SouyingPlay(g, CardKind.Duel, [1]);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "souying-before"));
        var original = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == card);
        var originalPoint = History(g).FindFinalizedUse(original.Id)!.HistoryPoint!;
        Need(new CardUseHistoryTestWorld(History(g), originalPoint, target: 1).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "The second real directed card freezes its own original qualification before the higher-priority observer runs.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Answer(g, c => c.Targets.SequenceEqual([1]));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "souying-after"));
        var nested = E<ActualCardUseTargetsFinalizedEvent>(g).Single(e => e.Action.ActionId > original.Action!.ActionId && e.Action.EffectiveKind == CardKind.Slash);
        Need(new CardUseHistoryTestWorld(History(g), History(g).FindFinalizedUse(nested.NativeUseId)!.HistoryPoint!, target: 1).Int(CardUseHistoryTestWorld.DirectedCount) == 3 &&
            new CardUseHistoryTestWorld(History(g), originalPoint, target: 1).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "A real nested third use cannot refresh the waiting original card's frozen second-use ordinal.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(g, p => Optional(p, "ol:souying")); Activate(g);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "huaman"));
        var paid = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.HuaMan is not null);
        Need(paid.HuaMan!.Souying is { NativeUseId: var id } && id == original.Id,
            "The actual Souying candidate retains its original second-use qualification after the nested child returns.");
        var cost = V(g, 0).Hand.Single(c => c.Kind == CardKind.DrawTwo).Id;
        g = Cold(g, registry); Answer(g, c => c.Cards.SequenceEqual([cost])); SouyingReachPlay(g, forbidOffer: true);
        var mutation = E<SouyingNativeMutationEvent>(g).Single();
        Need(!mutation.Cancelled && mutation.NativeUseId == original.Id && mutation.ClaimedCount == 1 &&
            g.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.huaman.discard") == 1 &&
            g.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.huaman.claim") == 1 &&
            V(g, 0).Hand.Any(c => c.Id == card) && E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == original.Id) == 1,
            "One actual discard claims the original processing entity once while its true native use still finishes after cold recovery.");
        _ = Cold(g, registry);
        HuaManCancelsTheReverseDirectedUse();
    }

    private static void HuaManRejectsStaleActorQualification()
    {
        var (g, registry) = StartDirected("souying-actor");
        SouyingPlay(g, CardKind.IronChain, [1]); SouyingReachPlay(g, forbidOffer: true);
        var card = SouyingPlay(g, CardKind.Duel, [1]);
        SouyingReach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"), forbidOffer: true);
        Answer(g, c => c.Targets.SequenceEqual([2]));
        SouyingReach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "souying-replaced"), forbidOffer: true);
        var current = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == card);
        var updated = E<ActualCardUseActionUpdatedEvent>(g).Last(e => e.NativeUseId == current.Id);
        var frozen = History(g).FindFinalizedUse(current.Id)!;
        Need(current.Action is { ActorSeat: 2, ProviderSeat: 0 } && current.TargetSeats.SequenceEqual([1]) &&
            updated.Action is { ActorSeat: 2, ProviderSeat: 0 } && updated.Action.ActionId == current.Action.ActionId &&
            frozen.Action is { ActorSeat: 0, ProviderSeat: 0 } && frozen.AcceptedActorSeat == 0 &&
            new CardUseHistoryTestWorld(History(g), frozen.HistoryPoint!, target: 1).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "A real actor rewrite publishes the new actor while keeping the original provider, accepted identity and first-finalized qualification.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        SouyingReachPlay(g, forbidOffer: true);
        Need(E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == current.Id) == 1 &&
            History(g).FindCompletedUse(current.Id)!.Action is { ActorSeat: 2, ProviderSeat: 0 } &&
            !E<SouyingNativeMutationEvent>(g).Any(),
            "The rewritten native card completes once without offering an old actor's unusable Souying qualification.");
        _ = Cold(g, registry);
    }

    private static void HuaManCancelsTheReverseDirectedUse()
    {
        var (g, registry) = StartDirected("souying-cancel");
        Use(g, "souying-incoming", [1]); SouyingReachPlay(g, forbidOffer: true);
        var hp = V(g, 0).Hp;
        Use(g, "souying-incoming", [1]); Reach(g, p => Optional(p, "ol:souying"));
        var original = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.SelectedActorDuelOrigin is not null);
        var originalPoint = History(g).FindFinalizedUse(original.Id)!.HistoryPoint!;
        Need(original.SourceSeat == 1 && original.TargetSeats.SequenceEqual([0]) &&
            new CardUseHistoryTestWorld(History(g), originalPoint, target: 0).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "The second actual foreign actor's Duel freezes the reverse direction in this same actual turn.");
        g = Cold(g, registry); Activate(g);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "huaman"));
        var cost = V(g, 0).Hand.Single(c => c.Kind == CardKind.DrawTwo).Id;
        Answer(g, c => c.Cards.SequenceEqual([cost])); SouyingReachPlay(g, forbidOffer: true);
        Need(E<SouyingNativeMutationEvent>(g).Single() is { Cancelled: true, ClaimedCount: 0 } mutation && mutation.NativeUseId == original.Id &&
            V(g, 0).Hp == hp && E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == original.Id) == 1 &&
            !E<DamageAppliedEvent>(g).Any(e => e.SourceCardUseResolutionId == original.Id) &&
            g.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.huaman.discard") == 1 &&
            History(g).FindCompletedUse(original.Id)!.Action.EffectiveDesignatedTargetSeats.Count == 0 &&
            new CardUseHistoryTestWorld(History(g), originalPoint, target: 0).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "A real target-side payment cancels the second incoming Duel, preserves the frozen ordinal and returns once without applying damage.");
        _ = Cold(g, registry);
    }

    private static int SouyingPlay(GameEngine g, CardKind kind, IReadOnlyList<int> targets)
    {
        var action = g.GetHumanLegalActions().First(a => a.ConversionSource is null && a.CardId is { } id &&
            V(g, 0).Hand.Any(c => c.Id == id && c.Kind == kind) && a.TargetSeats.SequenceEqual(targets));
        DirectedPlay(g, action); return action.CardId!.Value;
    }
    private static void SouyingReachPlay(GameEngine g, bool forbidOffer) => SouyingReach(g, p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, forbidOffer);
    private static void SouyingReach(GameEngine g, Func<PendingDecision, bool> done, bool forbidOffer) => Reach(g, p =>
    {
        Need(!forbidOffer || !Optional(p, "ol:souying"), "No additional Souying offer may be hidden by automatic prompt skipping.");
        return done(p);
    });
    private static void RegisterSouyingFixture(IContentRegistryBuilder b, List<string> skills)
    {
        var actual = CharacterFixtureContent.EmbeddedProgramBundle("ol-hua-man");
        const string id = "ol:souying";
        b.AddSkill(new(id, actual.Presentations[id].Name, actual.Presentations[id].Description)
            { Program = actual.Programs[id], ProgramPresentation = actual.Presentations[id] });
        skills.Add(id);
    }
    private static void ConfigureHuaManHistoryFixture(JsonNode rules, string scenario, List<string> skills)
    {
        if (scenario == "souying-claim")
        {
            skills.Add(Watch);
            rules["skills"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"fixture:phase-control-watch","revision":1,"triggers":[{"id":"souying-nested","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["duel"],"priority":100,"usageScope":"game","usageLimit":1,"optional":false,"effects":[
              {"op":"chooseOption","target":"owner","resultBind":"souying-before","options":[{"id":"continue"}]},
              {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"useVirtualSlash","target":"selectedTarget"},
              {"op":"chooseOption","target":"owner","resultBind":"souying-after","options":[{"id":"continue"}]}]}]}
            """));
        }
        if (scenario == "souying-actor")
        {
            skills.Add(Watch);
            rules["skills"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"fixture:phase-control-watch","revision":1,"triggers":[{"id":"souying-actor-rewrite","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["duel"],"priority":100,"usageScope":"game","usageLimit":1,"optional":false,"effects":[
              {"op":"selectTarget","target":"owner","targetKind":"otherLegalCurrentCardTarget"},
              {"op":"replaceCurrentCardUseActor","target":"selectedTarget"},
              {"op":"chooseOption","target":"owner","resultBind":"souying-replaced","options":[{"id":"continue"}]}]}]}
            """));
        }
        if (scenario == "souying-cancel")
            rules["skills"]![1]!["activations"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"souying-incoming","minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhase":2,"effects":[{"op":"useSelectedActorDuel","target":"owner"}]}
            """));
    }
}
