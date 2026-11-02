using System.Reflection;
using System.Text.Json.Nodes;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

internal static partial class PhaseStartCardControlChecks
{
    private static void ActualCardUseHistoryKeepsNativeOrderAndPrincipalTargets()
    {
        var (g, registry) = StartDirected("history");
        var first = HistoryUse(g, CardKind.IronChain, [1]);
        Need(History(g).FindFinalizedUse(first.Id) is { Finalized: true, Completed: false } live && live.PrimaryTargets!.SequenceEqual([1]) &&
            History(g).ReadCardUseFacts(History(g).FindFinalizedUse(first.Id)!.HistoryPoint!, CardUseHistoryView.Finalizations).Count == 1 &&
            E<ActualCardActionRecordedEvent>(g).Count(e => e.Action.ActionId == first.Action!.ActionId) == 1,
            "One actual accepted use is finalized before completion, without inventing a previous use.");
        g = Cold(g, registry); HistoryContinue(g);
        Need(History(g).FindFinalizedUse(first.Id) is { Completed: true }, "Only the actual native return marks the first use complete.");
        var second = HistoryUse(g, CardKind.IronChain, [1]);
        Need(History(g).ReadCardUseFacts(History(g).FindFinalizedUse(second.Id)!.HistoryPoint!, CardUseHistoryView.Finalizations)
                .TakeLast(2).Select(f => f.NativeUseId).SequenceEqual(new long?[] { first.Id, second.Id }) &&
            History(g).ReadCardUseFacts(new(g.State.TurnNumber, 0, History(g).CardUseHistoryCursor), CardUseHistoryView.Latest).Count(f => f.DeclarerSeat == 0 && f.ActionId <= second.Action!.ActionId) == 2 &&
            History(g).FindFinalizedUse(second.Id)!.PrimaryTargets!.SequenceEqual([1]),
            "Two real cards in the same actual Play share their true principal target and accepted order.");
        g = Cold(g, registry); HistoryContinue(g);
        Need(History(g).FindFinalizedUse(second.Id) is { Completed: true }, "The second native use completes once after cold restoration.");

        (g, registry) = StartDirected("history"); var unrelated = HistoryUse(g, CardKind.IronChain, [2]); HistoryContinue(g);
        var borrowed = HistoryUse(g, CardKind.BorrowedSword, [1, 2]);
        Need(borrowed.Action!.TargetSeats.SequenceEqual([1, 2]) && History(g).FindFinalizedUse(borrowed.Id)!.PrimaryTargets!.SequenceEqual([1]) &&
            !History(g).FindFinalizedUse(unrelated.Id)!.PrimaryTargets!.Intersect(History(g).FindFinalizedUse(borrowed.Id)!.PrimaryTargets!).Any(),
            "A genuine Borrowed Sword records its weapon holder, while its paired Slash victim cannot create a false common target.");
        var borrowedConfirmation = History(g).ReadCardUseFacts(new(g.State.TurnNumber, 0, History(g).CardUseHistoryCursor), CardUseHistoryView.Confirmations)
            .Single(f => f.NativeUseId == borrowed.Id);
        Need(borrowedConfirmation.ConfirmedTargets!.SequenceEqual([1, 2]) && borrowedConfirmation.PrimaryTargets!.SequenceEqual([1]),
            "The real Borrowed Sword confirmation exposes its raw holder/victim payload separately from the shared principal-target view.");
        g = Cold(g, registry); HistoryContinue(g);
        Need(E<BorrowedSwordResolvedEvent>(g).Single(e => e.ResolutionId == borrowed.Id) is { WeaponOwnerSeat: 1, SlashTargetSeat: 2 },
            "The real compound card keeps its original holder/victim native resolution.");
        var draw = HistoryUse(g, CardKind.DrawTwo, []);
        Need(History(g).FindFinalizedUse(draw.Id)!.PrimaryTargets!.SequenceEqual([0]) && draw.Action!.TargetSeats.SequenceEqual([0]),
            "The actual empty-target DrawTwo declaration normalizes to its implicit self principal target.");
        g = Cold(g, registry); HistoryContinue(g);
        SuFeiUsesSharedFinalizedHistory();
        GuoSiTruePairOffersAndCompletesNativeSnatch();
        foreach (var dodge in new[] { false, true }) GuoSiResponsesCutAcceptedPairs(dodge);
        ConfirmedNativeTargetsStayFrozenWhileLiveTargetsChange();
    }

    private static void SuFeiUsesSharedFinalizedHistory()
    {
        var (g, registry) = StartDirected("history-sufei"); HistoryUse(g, CardKind.IronChain, [1]); HistoryContinue(g);
        var second = HistoryUse(g, CardKind.IronChain, [1]);
        Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(g, p => Optional(p, "ol:lianpian")); Activate(g);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "lianpian"));
        Need(E<LianpianDrawPaidEvent>(g).Single() is { ActualCount: 1 } paid && paid.NativeUseId == second.Id &&
            P(g)!.Choices.Where(c => c.Parameters.GetValueOrDefault("branch") == "give").SelectMany(c => c.Targets).SequenceEqual([1]),
            "The real Lianpian consumer pays one draw and offers only the actual shared principal target.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "keep"); Play(g);
        Need(E<LianpianDrawPaidEvent>(g).Count() == 1 && History(g).FindFinalizedUse(second.Id) is { Completed: true },
            "The actual Lianpian choice cold-restores and returns to the original native use once.");
    }

    private static void GuoSiResponsesCutAcceptedPairs(bool dodge)
    {
        var (g, registry) = StartDirected(dodge ? "history-dodge" : "history-nullification");
        var first = HistoryUse(g, CardKind.IronChain, [1], forbidSidao: true); HistoryContinue(g, forbidSidao: true);
        CardUseFrame interrupted;
        if (dodge)
        {
            Use(g, "history-poke", [1]); HistoryReach(g, p => p is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge }, forbidSidao: true);
            interrupted = g.ResolutionStack.OfType<CardUseFrame>().Last();
            Need(interrupted.SourceSeat == 1 && interrupted.CardKind == CardKind.Slash, "The own-turn Dodge answers another actor's true retaliatory Slash.");
        }
        else
        {
            interrupted = HistoryUse(g, CardKind.Duel, [1], forbidSidao: true);
            Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            HistoryReach(g, p => p is { PlayerSeat: 0, Kind: DecisionKind.Nullification }, forbidSidao: true);
        }
        var kind = dodge ? CardKind.Dodge : CardKind.Nullification;
        var cost = V(g, 0).Hand.Single(c => c.Kind == kind).Id;
        g = Cold(g, registry); Answer(g, c => c.Cards.Contains(cost)); HistoryReach(g, p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, forbidSidao: true);
        var response = E<CardActionAcceptedEvent>(g).Select(e => e.Action).Single(a => a.Type == CardActionType.Response && a.PhysicalCards.Any(c => c.CardId == cost));
        Need(response.ParentActionId == interrupted.Action!.ActionId && History(g).FindAction(response.ActionId) is
            { NativeUseId: null, PrimaryTargets.Count: 0, Completed: true, IsSelfNativeResponseUse: true },
            "The original own native response returns once and cuts accepted order with an empty target set and no synthetic Use frame.");
        if (dodge)
        {
            var next = HistoryUse(g, CardKind.IronChain, [1], forbidSidao: true);
            Need(History(g).ReadCardUseFacts(new(g.State.TurnNumber, 0, History(g).CardUseHistoryCursor), CardUseHistoryView.Latest)
                .Where(f => f.DeclarerSeat == 0 && (f.Type == CardActionType.Use || f.IsSelfNativeResponseUse) && f.ActionId <= next.Action!.ActionId)
                .TakeLast(2).Select(f => f.ActionId).SequenceEqual(new long?[] { response.ActionId, next.Action!.ActionId }) &&
                History(g).ReadCardUseFacts(History(g).FindFinalizedUse(next.Id)!.HistoryPoint!, CardUseHistoryView.Finalizations)
                    .Where(f => f.ActorSeat == 0).TakeLast(2).Select(f => f.NativeUseId).SequenceEqual(new long?[] { first.Id, next.Id }),
                "Dodge interrupts the accepted pair while finalized-only history retains the previous real card use.");
            HistoryContinue(g, forbidSidao: true);
        }
        else
        {
            var point = History(g).FindCompletedUse(interrupted.Id)!.HistoryPoint!;
            var accepted = History(g).ReadCardUseFacts(point, CardUseHistoryView.Accepted);
            Need(accepted.Where(f => f.DeclarerSeat == 0 && (f.Type == CardActionType.Use || f.IsSelfNativeResponseUse))
                .TakeLast(2).Select(f => f.ActionId!.Value).SequenceEqual([interrupted.Action!.ActionId, response.ActionId]),
                "The actual Duel completion freezes its accepted tail after the nested counterspell, rather than its old acceptance-time prefix.");
        }
        Need(E<CardResponseCompletedEvent>(g).Count(e => e.ActionId == response.ActionId) == 1,
            "The checked path contains one actual response completion and never publishes a Sidao offer across it.");
        _ = Cold(g, registry);
    }

    private static void GuoSiTruePairOffersAndCompletesNativeSnatch()
    {
        var (g, registry) = StartDirected("history-sidao");
        var weapon = V(g, 1).Equipment.Single().Id;
        HistoryUse(g, CardKind.IronChain, [1]); HistoryContinue(g, forbidSidao: true);
        var second = HistoryUse(g, CardKind.IronChain, [1]);
        Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(g, p => Optional(p, "ol:sidao"));
        var completionPoint = History(g).FindCompletedUse(second.Id)!.HistoryPoint!;
        var pair = History(g).ReadCardUseFacts(completionPoint, CardUseHistoryView.Accepted)
            .Where(f => f.DeclarerSeat == 0 && (f.Type == CardActionType.Use || f.IsSelfNativeResponseUse)).TakeLast(2).ToArray();
        Need(History(g).FindCompletedUse(second.Id) is { Completed: true } &&
            pair.Length == 2 && pair.All(e => e.Finalized && e.Completed && e.PrimaryTargets!.SequenceEqual([1])),
            "Two genuinely completed same-target Uses offer the real Sidao skill.");
        Activate(g); Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "guo-si"));
        g = Cold(g, registry);
        var option = g.GetRuntimeModule<CharacterGuoSiOlRuntime>().SidaoActions(0, [1]).Select((action, index) => (action, index))
            .First(item => item.action.TargetCardId == weapon).index;
        Answer(g, c => c.Parameters.GetValueOrDefault("option") == option.ToString(System.Globalization.CultureInfo.InvariantCulture));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "history-finalized"));
        var native = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.GuoSiNative is not null);
        Need(native.CardKind == CardKind.Snatch && native.Action!.ParentActionId == second.Action!.ActionId &&
            native.GuoSiNative!.TargetCardId == weapon && E<GuoSiNativeIssuedEvent>(g).Single().Receipt.NativeUseId == native.Id,
            "The accepted choice pays for an actual native Snatch tied to the second original action.");
        g = Cold(g, registry); HistoryContinue(g);
        var returned = E<GuoSiNativeReturnedEvent>(g).Count(e => e.Receipt.NativeUseId == native.Id);
        var finished = E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == native.Id);
        Need(returned == 1, $"The original Snatch must return once: use={native.Id}, returned={returned}, finished={finished}.");
        Need(finished == 1, $"The original Snatch must finish once: use={native.Id}, finished={finished}.");
        var moves = g.CardMovements.Where(m => m.CardId == weapon).ToArray();
        var claims = moves.Where(m => m.From == CardLocation.Equipment(1) && m.To == CardLocation.Processing).ToArray();
        var gains = moves.Where(m => m.From == CardLocation.Processing && m.To == CardLocation.Hand(0)).ToArray();
        Need(claims.Length == 1 && gains.Length == 1 && claims[0].Sequence < gains[0].Sequence && V(g, 0).Hand.Any(c => c.Id == weapon),
            $"The explicitly chosen weapon must transfer once after cold: use={native.Id}, chosen={native.GuoSiNative!.TargetCardId}, entity={weapon}, moves={string.Join(';', moves.Select(m => $"{m.Sequence}:{m.From}->{m.To}:{m.Reason.Value}"))}.");
    }

    private static CardUseFrame HistoryUse(GameEngine g, CardKind kind, IReadOnlyList<int> targets, bool forbidSidao = false)
    {
        var action = g.GetHumanLegalActions().First(a => a.CardId is not null && a.ConversionSource is null &&
            V(g, 0).Hand.Any(c => c.Id == a.CardId && c.Kind == kind) && a.TargetSeats.SequenceEqual(targets));
        DirectedPlay(g, action);
        HistoryReach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "history-finalized"), forbidSidao);
        return g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == action.CardId);
    }
    private static void HistoryContinue(GameEngine g, bool forbidSidao = false)
    { Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue"); HistoryReach(g, p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, forbidSidao); }
    private static void HistoryReach(GameEngine g, Func<PendingDecision, bool> done, bool forbidSidao) => Reach(g, p =>
    {
        Need(!forbidSidao || !Optional(p, "ol:sidao"), "No Sidao prompt may be published across the actual intervening native response.");
        return done(p);
    });
    private static IActualCardUseHistoryQueries History(GameEngine g) =>
        ((ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(g)!).Get<IActualCardUseHistoryQueries>();
    private static ShanheStartingCard[] HistoryStartingHand() =>
        [new(CardKind.IronChain, Suit.Club, 7), new(CardKind.IronChain, Suit.Heart, 8), new(CardKind.BorrowedSword, Suit.Spade, 9),
            new(CardKind.DrawTwo, Suit.Diamond, 10), new(CardKind.Duel, Suit.Heart, 11), new(CardKind.Nullification, Suit.Club, 12), new(CardKind.Dodge, Suit.Spade, 2)];

    private static void ConfigureHistoryFixture(JsonNode rules, string scenario, List<string> skills)
    {
        if (!scenario.StartsWith("history", StringComparison.Ordinal)) return;
        skills.Add(Watch);
        if (scenario == "history-sufei") skills.Add("ol:lianpian");
        if (scenario is "history-dodge" or "history-nullification" or "history-sidao") skills.Add("ol:sidao");
        rules["skills"]!.AsArray().Add(JsonNode.Parse("""
        {"id":"fixture:phase-control-watch","revision":1,"triggers":[{"id":"history-finalized","window":"cardUseTargetsFinalized","ownerRelation":"actor","priority":100,"optional":false,
          "effects":[{"op":"chooseOption","target":"owner","resultBind":"history-finalized","options":[{"id":"continue"}]}]}]}
        """));
        if (scenario is "history-dodge" or "history-meihun")
        {
            rules["skills"]![1]!["activations"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"history-poke","minTargets":1,"maxTargets":1,"usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]}
            """));
            rules["skills"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"fixture:history-retaliate","revision":1,"triggers":[{"id":"real-retaliatory-slash","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","usageScope":"game","usageLimit":1,"optional":false,
              "condition":{"kind":"not","children":[{"kind":"directCardUseDamage"}]},"effects":[{"op":"selectTarget","target":"owner","targetKind":"eventSource"},{"op":"useVirtualSlash","target":"selectedTarget"}]}]}
            """));
        }
        ConfigureConfirmedHistoryFixture(rules, scenario);
    }
}
