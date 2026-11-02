using System.Text.Json.Nodes;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class PhaseStartCardControlChecks
{
    private const string QietingObserver = "fixture:directed-qieting-observer";
    private static void QietingUsesLatestDeclaredNativeHistory()
    {
        QietingUsesLatestDeclaredNativeHistory(lateUse: false);
        QietingUsesLatestDeclaredNativeHistory(lateUse: true);
        QietingRejectsDeclaredLegacySlashAndRealDamage();
    }
    private static void QietingUsesLatestDeclaredNativeHistory(bool lateUse)
    {
        var (g, registry) = StartDirected(lateUse ? "qieting-latest" : "qieting-reward");
        var card = SouyingPlay(g, CardKind.IronChain, [1]); Play(g);
        var useId = E<CardUseDeclaredEvent>(g).Single(e => e.CardId == card).ResolutionId;
        var latest = History(g).FindNativeUse(useId)!; var turn = g.State.TurnNumber;
        var cancelledPoint = new CardUseHistoryPoint(turn, 0, History(g).CardUseHistoryCursor, useId);
        var confirmation = History(g).ReadCardUseFacts(cancelledPoint, CardUseHistoryView.Confirmations).Single(f => f.NativeUseId == useId);
        Need(E<NativeCardUseHistoryRecordedEvent>(g).Any(e => e.Snapshot.NativeUseId == useId && e.Snapshot.DesignatedTargets.SequenceEqual([1])) &&
            latest.DesignatedTargets.Count == 0 && confirmation.ConfirmedTargets!.SequenceEqual([1]) &&
            new CardUseHistoryTestWorld(History(g), cancelledPoint).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == false,
            "A real native card first targets another player, then cancels its targets; end qualification reads its latest version instead of an ever union.");
        g = Cold(g, registry);
        Need(History(g).ReadCardUseFacts(cancelledPoint, CardUseHistoryView.Confirmations).Single(f => f.NativeUseId == useId).ConfirmedTargets!.SequenceEqual([1]) &&
            confirmation.ConfirmedTargets!.SequenceEqual([1]), "Actual confirmation remains immutable after native cancellation and cold recovery.");
        End(g);
        if (lateUse)
        {
            Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
            Answer(g, c => c.Targets.SequenceEqual([1])); ReachQietingLive(g);
            var late = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == 0 && f.CardKind == CardKind.Slash);
            Need(E<CardUseDeclaredEvent>(g).Any(e => e.ResolutionId == late.Id) &&
                History(g).FindNativeUse(late.Id) is { ActorSeat: 0 } snapshot && snapshot.DesignatedTargets.SequenceEqual([1]) &&
                new CardUseHistoryTestWorld(History(g), new(turn, 0, History(g).CardUseHistoryCursor)).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true,
                "A real after-End higher-priority Slash enters history before either Qieting candidate creates its own cutoff.");
            g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        }
        var skills = new HashSet<string>(lateUse ? ["boundary:qieting-mobile"] : ["ol:qieting", "boundary:qieting-mobile"], StringComparer.Ordinal);
        var frames = new List<long>();
        while (skills.Count > 0)
        {
            Reach(g, p =>
            {
                Need(g.State.TurnNumber == turn, "Both original end candidates must be offered before the next actual turn.");
                Need(!lateUse || !Optional(p, "ol:qieting"), "OL Qieting cannot offer after a higher-priority native use has targeted another player before its candidate cutoff.");
                return Optional(p, "ol:qieting") || Optional(p, "boundary:qieting-mobile");
            });
            Need(P(g)!.PlayerSeat == 1 && skills.Remove(P(g)!.SkillPrompt!.SkillId), "The real observer receives each distinct original Qieting candidate once.");
            Activate(g); Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "ordinary-qieting"));
            var frame = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.OrdinaryQieting is not null);
            var q = frame.OrdinaryQieting!.Qualification; frames.Add(frame.Id);
            Need(q.ActualTurn == turn && q.TurnOwnerSeat == 0 &&
                new CardUseHistoryTestWorld(History(g), q).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == lateUse,
                "Each candidate freezes its actual entry cutoff; mobile Qieting separately ignores damage occurring after the real TurnEnded boundary.");
            g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option") == "draw");
        }
        DrainQietingEndWithoutOffers(g, turn);
        Need(frames.All(id => E<OrdinaryQietingDrawReturnedEvent>(g).Count(e => e.FrameId == id && e.ActualCount == 1) == 1 &&
            E<OrdinaryQietingCompletedEvent>(g).Count(e => e.FrameId == id && e.Paid) == 1),
            "Each real end reward draws and returns once after cold recovery without rebuilding its frozen qualification.");
        _ = Cold(g, registry);
    }

    private static void QietingRejectsDeclaredLegacySlashAndRealDamage()
    {
        var (g, registry) = StartDirected("qieting-legacy"); Answer(g, c => c.Targets.SequenceEqual([1])); ReachQietingLive(g);
        var legacy = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == 0 && f.CardKind == CardKind.Slash);
        var producer = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver && f.TriggerId == "legacy-declared-slash");
        var turn = g.State.TurnNumber;
        Need(legacy.Action is null && legacy.PhysicalCardIds is { Count: 0 } &&
            E<CardUseDeclaredEvent>(g).Any(e => e.ResolutionId == legacy.Id && e.SourceSeat == 0) &&
            History(g).FindNativeUse(legacy.Id) is { ActionId: null, ActorSeat: 0 } native && native.DesignatedTargets.SequenceEqual([1]) &&
            new CardUseHistoryTestWorld(History(g), new(turn, 0, History(g).CardUseHistoryCursor)).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true,
            "The original Action-null legacy Slash is observed only through its actual declared native ID and real targets.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue"); Play(g);
        Need(!g.ResolutionStack.Any(f => f.Id == legacy.Id || f.Id == producer.Id) &&
            E<ProgramBindingResolvedEvent>(g).Count(e => e.FrameId == producer.Id && e.Activated && e.Completed) == 1 &&
            E<CardUseDeclaredEvent>(g).Count(e => e.ResolutionId == legacy.Id) == 1 &&
            E<DamageAppliedEvent>(g).Count(e => e.SourceCardUseResolutionId == legacy.Id && e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount > 0) == 1,
            "The Action-null legacy attack leaves the stack, returns to its original binding once and applies other-player damage once before the end cutoff.");
        End(g); DrainQietingEndWithoutOffers(g, turn);
        _ = Cold(g, registry);
    }

    private static void ReachQietingLive(GameEngine g) => Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "qieting-live"));
    private static void DrainQietingEndWithoutOffers(GameEngine g, int turn) => Reach(g, p =>
    {
        if (g.State.TurnNumber > turn) return true;
        Need(!Optional(p, "ol:qieting") && !Optional(p, "boundary:qieting-mobile"), "No ineligible or repeated Qieting prompt may be hidden by automatic skipping.");
        return false;
    });
    private static ShanheStartingCard[] QietingStartingHand() =>
        [new(CardKind.IronChain, Suit.Club, 7), new(CardKind.Dodge, Suit.Heart, 8), new(CardKind.Peach, Suit.Spade, 9),
            new(CardKind.Alcohol, Suit.Diamond, 10), new(CardKind.IronChain, Suit.Spade, 11), new(CardKind.Peach, Suit.Diamond, 12), new(CardKind.Dodge, Suit.Club, 13)];
    private static void RegisterQietingFixture(IContentRegistryBuilder builder, string scenario)
    {
        var definitions = scenario == "qieting-confirmed-current"
            ? new[] { ("boundary-cai-fu-ren", "boundary:qieting-current") }
            : new[] { ("ol-cai-fu-ren", "ol:qieting"), ("boundary-cai-fu-ren-mobile", "boundary:qieting-mobile") };
        foreach (var (bundle, id) in definitions)
        {
            var actual = CharacterFixtureContent.EmbeddedProgramBundle(bundle);
            builder.AddSkill(new(id, actual.Presentations[id].Name, actual.Presentations[id].Description)
                { Program = actual.Programs[id], ProgramPresentation = actual.Presentations[id] });
        }
        builder.AddGeneral(new(QietingObserver, "真实窃听观察者", "", "fixture:directed-no-draw", "qun", 20,
            definitions.Select(item => item.Item2).ToArray()));
    }
    private static void ConfigureQietingHistoryFixture(JsonNode rules, string scenario, List<string> skills)
    {
        if (!scenario.StartsWith("qieting", StringComparison.Ordinal)) return;
        skills.Add(Watch);
        rules["skills"]![1]!["triggers"] = JsonNode.Parse(scenario == "qieting-legacy" ? """
        [{"id":"legacy-declared-slash","window":"playPhaseStarting","subject":"owner","optional":false,"effects":[
          {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":false}]}]
        """ : """
        [{"id":"late-end-slash","window":"afterTurnEnded","subject":"owner","priority":100,"optional":false,"usageScope":"game","usageLimit":1,"effects":[
          {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"useVirtualSlash","target":"selectedTarget"}]}]
        """);
        if (scenario is "qieting-reward" or "qieting-confirmed-current") rules["skills"]![1]!["triggers"] = new JsonArray();
        rules["skills"]!.AsArray().Add(JsonNode.Parse("""
        {"id":"fixture:phase-control-watch","revision":1,"triggers":[{"id":"qieting-native-live","window":"afterDamageApplied","subject":"any","damageOccurrence":"perDamage","condition":{"kind":"damageSourceIsOwner"},"optional":false,"usageScope":"game","usageLimit":1,
          "effects":[{"op":"chooseOption","target":"owner","resultBind":"qieting-live","options":[{"id":"continue"}]}]}]}
        """));
        if (scenario != "qieting-legacy")
        {
            skills.Add("fixture:qieting-overflow");
            rules["skills"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"fixture:qieting-overflow","revision":1,"triggers":[{"id":"cancel-other-targets","window":"cardUseTargetsFinalized","ownerRelation":"actor","singleActionInstance":true,"optional":false,
              "effects":[{"op":"cancelOtherCurrentUseTargetsIfOverHandLimit","target":"owner"}]}]}
            """));
        }
    }
}
