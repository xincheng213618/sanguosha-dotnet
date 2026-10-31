using System.Text.Json;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class SkillStateChecks
{
    private const string Driver = "fixture:skill-state-driver", Counter = "fixture:skill-state-counter";
    private const string Locked = "fixture:skill-state-locked", Owner = "fixture:skill-state-owner";
    private const string Mode = "identity:classic-shared-skill-state";

    public static void StateChangesPreserveOwnershipChoicesInstancesAndExpiryCold()
    {
        foreach (var (name, check) in new (string, Action)[]
        {
            ("suppression", SuppressionKeepsOwnershipAndExemptsLockedSkills),
            ("chosen return", ChosenLossReturnsToOrdinaryDrawCold),
            ("overlapping expiry", OverlappingExpiryPreservesTheExistingInstance),
            ("next owner turn expiry", AfterCurrentTurnWaitsForTheNextOwnerTurnEnd),
            ("current round expiry", CurrentRoundExpiresWithoutRoundEndingTracking),
            ("next turn start expiry", UntilTurnStartOfWaitsForTheSpecifiedOwnersNextTurn),
            ("existing transfer", TransferReusesAnAlreadyOwnedInstance),
            ("named future source", NamedSuppressionCoversAnIndependentlyAcquiredSource),
            ("filter resources", FiltersShareExpressionResourcesAndReadSkillFacts),
            ("printed source filter", PrintedFilterDoesNotLoseAnAcquiredSourceOfTheSameSkill),
            ("continuous HP condition", WhileTracksHpWithoutRemovingItsRestriction),
            ("legacy JSON rejection", LegacySkillStateFieldsAreRejected),
            ("native phase boundaries", PhaseBoundariesUseActualTokensAndSelectedTiming),
            ("boundary OR and atomic cleanup", BoundarySkipAndAlternativesClearBorrowedStateBeforeFacts),
            ("default cleanup timing", DefaultAfterCleanupExpiresOnTheNativeClock),
            ("nested boundary issuance", NestedFactIssuanceSkipsTheOriginalPhaseOccurrence),
            ("issued phase condition", IssuedPhaseConditionSurvivesInsertionAndRestoration),
            ("permanent death expiry", PermanentRestrictionExpiresOnItsTargetDeath),
            ("consumed Alcohol Slash", ConsumedAlcoholSlashQualifiesOnlyItsOriginalDamage)
        })
            try { check(); }
            catch (Exception error) { throw new InvalidOperationException($"Shared skill state {name}: {error.Message}", error); }
    }

    private static void SuppressionKeepsOwnershipAndExemptsLockedSkills()
    {
        var s = Scene.Start();
        var instance = s.CounterState(0).SkillInstanceId;
        Require(s.CanUse(0, Counter, "pulse") && s.CanUse(0, Locked, "pulse"), "Both original skills start effective.");
        s.Use(0, Driver, "suppress-self"); s.Play(0);
        Require(s.Owns(0, Counter) && s.Owns(0, Driver) && s.Owns(0, Locked) &&
            !s.CanUse(0, Counter, "pulse") && !s.CanUse(0, Driver, "choose") && s.CanUse(0, Locked, "pulse"),
            "Non-locked suppression retains owned printed skills while removing their activations; a locked skill remains effective.");
        Require(!s.GrantQualified(0, Counter) && s.GrantQualified(0, Locked),
            "Shared source-instance qualification follows the same restriction decision and locked exemption.");
        s.Cold();
        Require(s.Owns(0, Counter) && !s.CanUse(0, Counter, "pulse") && s.CanUse(0, Locked, "pulse"),
            "Cold replay retains the ownership/effectiveness distinction.");
        s.Play(1); s.Play(0);
        Require(s.CanUse(0, Counter, "pulse") && s.CanUse(0, Driver, "choose") && s.CounterState(0).SkillInstanceId == instance,
            "Actual turn-end expiry restores the existing skill instance and its activations.");
    }

    private static void ChosenLossReturnsToOrdinaryDrawCold()
    {
        var s = Scene.Start();
        var hand = s.View(0).HandCount;
        s.Use(0, Driver, "choose", 1);
        s.Reach(() => s.Pending is { } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skill-state"));
        var pending = s.Pending!;
        Require(pending.PlayerSeat == 0 && pending.Choices.Count == 1 &&
            pending.Choices[0].Parameters.GetValueOrDefault("skill") == Counter &&
            s.G.CreateSnapshot(0).Players[1].Skills!.Any(skill => skill.ContentId == Counter) &&
            s.G.CreateSnapshot(1).PendingDecision is null && s.View(0).HandCount == hand,
            "The owner privately chooses a publicly owned non-locked target skill before the following draw runs.");
        var frameId = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver).Id;
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("skill") == Counter); s.Play(0);
        Require(!s.Owns(1, Counter) && s.Owns(1, Locked) && s.View(0).HandCount == hand + 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == frameId && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) &&
                m.Reason.Value == $"skill-program.{Driver}.Draw") == 1,
            "The chosen loss completes on its original frame, then executes one ordinary native draw.");
        s.Cold(); s.Play(1);
        Require(s.Owns(1, Counter) && s.CanUse(1, Counter, "pulse"),
            "Temporary loss expires at the actual current turn end and restores the target's original skill.");
    }

    private static void OverlappingExpiryPreservesTheExistingInstance()
    {
        var s = Scene.Start(); s.Play(1);
        s.Use(1, Counter, "mark"); s.Play(1);
        var original = s.CounterState(1);
        Require(original.Value, "The target has a genuinely changed, persistent skill state before suppression.");
        s.Play(0);
        s.Use(0, Driver, "overlap-now", 1); s.Play(0);
        s.Use(0, Driver, "overlap-through-target", 1); s.Play(0);
        s.Play(1);
        Require(s.Owns(1, Counter) && !s.CanUse(1, Counter, "pulse") && s.CanUse(1, Locked, "pulse"),
            "Expiring one current-turn restriction must retain the independent restriction through the target's current turn.");
        s.Cold();
        Require(!s.CanUse(1, Counter, "pulse"), "Cold replay preserves the remaining overlapping restriction.");
        s.Play(2); s.Play(1);
        var restored = s.CounterState(1);
        Require(s.CanUse(1, Counter, "pulse") && restored.SkillInstanceId == original.SkillInstanceId && restored.Value &&
            !s.CanUse(1, Counter, "mark"),
            "The nearest matching target turn end restores the same changed instance without resetting its game quota.");
        s.Cold();
        Require(s.CounterState(1) == restored && s.CanUse(1, Counter, "pulse"), "Expired state and original instance cold-replay together.");
    }

    private static void AfterCurrentTurnWaitsForTheNextOwnerTurnEnd()
    {
        foreach (var before in new[] { false, true })
        {
            var s = Scene.Start();
            s.Use(0, Driver, before ? "through-next-owner-before-end" : "through-next-owner-end", 1); s.Play(0);
            var source = s.E<SkillStateChangedEvent>().Single(e => e.Seat == 1 && e.SkillId == Counter).SourceId;
            s.Play(1);
            Require(s.E<TurnEndedEvent>().Count(e => e.ActorSeat == 0) == 1 &&
                s.Owns(1, Counter) && !s.CanUse(1, Counter, "pulse") && s.CanUse(1, Locked, "pulse"),
                "AfterCurrentTurn keeps the non-locked restriction through the issuing owner's current turn end.");
            s.Cold();
            Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.StateLeases(1) == 1,
                "Cold replay retains the restriction that deliberately skipped the first matching end.");
            s.Play(0);
            Require(!s.Effective(1, Counter), "The target's intervening turn end cannot expire the owner's next-turn restriction.");
            s.Play(1);
            Require(s.E<TurnEndedEvent>().Count(e => e.ActorSeat == 0) == 2 && s.StateLeases(1) == 0 &&
                s.Owns(1, Counter) && s.CanUse(1, Counter, "pulse") && s.CanUse(1, Locked, "pulse"),
                "The owner's next actual turn end removes the restriction and restores the original target abilities.");
            var ended = s.G.Events.Last(e => e.Payload is TurnEndedEvent { ActorSeat: 0 }).Sequence;
            var expired = s.G.Events.Single(e => e.Payload is SkillStateExpiredEvent expiry && expiry.SourceId == source).Sequence;
            Require(before ? expired < ended : expired > ended,
                "Explicit beforeTurnEnded expires before the actual end fact; the default expires after that fact.");
            s.Cold();
            Require(s.G.Events.Last(e => e.Payload is TurnEndedEvent { ActorSeat: 0 }).Sequence == ended &&
                s.G.Events.Single(e => e.Payload is SkillStateExpiredEvent expiry && expiry.SourceId == source).Sequence == expired,
                "Cold replay preserves the selected lifecycle boundary and exact expiry/end publication order.");
        }
    }

    private static void CurrentRoundExpiresWithoutRoundEndingTracking()
    {
        var s = Scene.Start();
        Require(!s.G.RoundEndingEntries.Value.TracksRoundEnding, "The shared fixture has no optional round-ending capability.");
        var round = s.E<RoundStartedEvent>().Last().RoundNumber;
        s.Use(0, Driver, "through-current-round", 1); s.Play(0);
        var source = s.E<SkillStateChangedEvent>().Single(e => e.Seat == 1 && e.SkillId == Counter).SourceId;
        s.Play(1);
        Require(s.E<RoundStartedEvent>().Last().RoundNumber == round && s.Owns(1, Counter) && !s.CanUse(1, Counter, "pulse"),
            "A named current-round restriction preserves ownership and remains effective throughout the original round.");
        s.Cold();
        Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.StateLeases(1) == 1,
            "Cold replay retains a current-round restriction without enabling round-ending tracking.");
        s.Reach(() => s.E<RoundStartedEvent>().Last().RoundNumber > round);
        var started = s.G.Events.Last(e => e.Payload is RoundStartedEvent next && next.RoundNumber > round).Sequence;
        var expired = s.G.Events.Single(e => e.Payload is SkillStateExpiredEvent expiry && expiry.SourceId == source).Sequence;
        Require(s.E<RoundEndedEvent>().Length == 0 && expired < started && s.StateLeases(1) == 0 &&
            s.Owns(1, Counter) && s.Effective(1, Counter),
            "The native round clock expires the restriction before the next round starts, even without a RoundEnded fact.");
        s.Cold(); s.Play(0);
        Require(s.E<RoundEndedEvent>().Length == 0 && s.Effective(1, Counter) && s.StateLeases(1) == 0,
            "Cold replay reaches the owner's next play with the current-round restriction already removed.");
    }

    private static void UntilTurnStartOfWaitsForTheSpecifiedOwnersNextTurn()
    {
        var s = Scene.Start();
        var turn = s.G.State.TurnNumber;
        s.Use(0, Driver, "until-next-owner-start", 1); s.Play(0);
        var source = s.E<SkillStateChangedEvent>().Single(e => e.Seat == 1 && e.SkillId == Counter).SourceId;
        s.Play(1);
        Require(s.Owns(1, Counter) && !s.CanUse(1, Counter, "pulse") && s.CanUse(1, Locked, "pulse") &&
            !s.E<SkillStateExpiredEvent>().Any(e => e.SourceId == source),
            "The affected target's unrelated turn start leaves the owner's next-start restriction active.");
        s.Cold();
        Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.StateLeases(1) == 1,
            "Cold replay retains the non-locked restriction while waiting for its specified owner.");
        s.Reach(() => s.E<TurnStartedEvent>().Any(e => e.ActorSeat == 0 && e.TurnNumber > turn));
        var started = s.G.Events.Single(e => e.Payload is TurnStartedEvent next && next.ActorSeat == 0 && next.TurnNumber > turn).Sequence;
        var expired = s.G.Events.Single(e => e.Payload is SkillStateExpiredEvent expiry && expiry.SourceId == source).Sequence;
        Require(expired < started && s.StateLeases(1) == 0 && s.Owns(1, Counter) && s.Effective(1, Counter),
            "The restriction expires before the specified owner's strictly later TurnStarted fact.");
        s.Cold(); s.Play(1);
        Require(s.CanUse(1, Counter, "pulse") && s.E<SkillStateExpiredEvent>().Count(e => e.SourceId == source) == 1,
            "The target's next play restores the original ability after cold replay without a repeated expiry.");
    }

    private static void TransferReusesAnAlreadyOwnedInstance()
    {
        var s = Scene.Start();
        s.Use(0, Counter, "mark"); s.Play(0);
        s.Use(0, Counter, "pulse"); s.Play(0);
        var original = s.CounterState(0);
        var acquired = s.E<SkillsAcquiredEvent>().Count(e => e.PlayerSeat == 0 && e.SkillIds.Contains(Counter));
        s.Use(0, Driver, "transfer", 1); s.Play(0);
        Require(s.Owns(0, Counter) && s.CounterState(0) == original &&
            !s.CanUse(0, Counter, "mark") && !s.CanUse(0, Counter, "pulse") &&
            s.E<SkillsAcquiredEvent>().Count(e => e.PlayerSeat == 0 && e.SkillIds.Contains(Counter)) == acquired,
            "Transferring an already owned skill retains its original state and paid quota without announcing a second acquisition.");
        s.Cold(); s.Play(1); s.Play(0);
        Require(s.Owns(0, Counter) && s.CounterState(0) == original && !s.CanUse(0, Counter, "pulse"),
            "Expiring the transferred source keeps the recipient's permanent source, state and game quota.");
    }

    private static void NamedSuppressionCoversAnIndependentlyAcquiredSource()
    {
        var s = Scene.Start();
        s.Use(0, Driver, "lose-own-counter"); s.Play(0);
        Require(!s.Owns(0, Counter) && !s.Effective(0, Counter), "The original Counter source was genuinely lost first.");
        s.Use(0, Driver, "suppress-absent-and-grant"); s.Play(0);
        Require(s.Owns(0, Counter) && !s.Effective(0, Counter) && !s.CanUse(0, Counter, "pulse") &&
            s.E<SkillsAcquiredEvent>().Count(e => e.PlayerSeat == 0 && e.SkillIds.Contains(Counter)) == 1,
            "Named suppression issued while Counter is absent also covers its later independent acquisition source.");
        s.Cold();
        Require(s.Owns(0, Counter) && !s.Effective(0, Counter), "Cold replay preserves a named restriction over a later skill source.");
        s.Play(1); s.Play(0);
        Require(s.Owns(0, Counter) && s.Effective(0, Counter) && s.CanUse(0, Counter, "pulse") &&
            s.CounterGrants(0).Any(g => g.SourceId.StartsWith("acquired:", StringComparison.Ordinal) && g.IsEnabled) &&
            s.CounterGrants(0).Where(g => g.SourceId is CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource).All(g => !g.IsEnabled),
            "Expiry restores only the later acquired source; the permanently lost printed source stays disabled.");
    }

    private static void FiltersShareExpressionResourcesAndReadSkillFacts()
    {
        foreach (var fact in new[] { "skill.locked", "skill.limited", "skill.awakening", "skill.lord", "skill.printed", "skill.ordinal", "skill.character" })
            LoadFilter(JsonSerializer.Serialize(new object[] { "==", new object[] { "number", fact }, 0 }));
        LoadFilter("[\"==\",[\"number\",\"threshold\"],0]", captureThreshold: true);
        foreach (var filter in new[] { "[\"==\",[\"number\",\"missing-number\"],0]", "[\"==\",[\"boundCount\",\"missing-cards\"],0]" })
        {
            var rejected = false;
            try { LoadFilter(filter); }
            catch (InvalidOperationException) { rejected = true; }
            Require(rejected, "Skill filters must use the existing number/card resource validation for missing bindings.");
        }
        var s = Scene.Start();
        s.Use(0, Driver, "filter", 1); s.Play(0);
        Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.Owns(1, Locked) && s.Effective(1, Locked),
            "The expression reads the candidate's printed ordinal and locked tag, suppressing Counter while leaving Locked effective.");
        s.Cold();
        Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.Effective(1, Locked),
            "Skill-fact expression selection cold-replays without converting possession into effectiveness.");
    }

    private static void PrintedFilterDoesNotLoseAnAcquiredSourceOfTheSameSkill()
    {
        var s = Scene.Start();
        s.Use(0, Driver, "grant-own-counter"); s.Play(0);
        var printed = s.CounterGrants(0).Single(g => g.SourceId == CharacterState.PrimarySkillSource);
        var acquired = s.CounterGrants(0).Single(g => g.SourceId.StartsWith("acquired:", StringComparison.Ordinal));
        s.Use(0, Driver, "lose-printed-counter"); s.Play(0);
        Require(!s.OwnsInstance(0, Counter, printed.SkillInstanceId) && s.OwnsInstance(0, Counter, acquired.SkillInstanceId) &&
            s.Owns(0, Counter) && s.Effective(0, Counter) && s.CounterGrants(0).Count == 2,
            "A printed filter removes only the printed source's possession while its same-named acquisition stays effective.");
        s.Cold(); s.Play(1); s.Play(0);
        Require(s.OwnsInstance(0, Counter, printed.SkillInstanceId) && s.OwnsInstance(0, Counter, acquired.SkillInstanceId),
            "Expiry restores the exact printed instance without replacing or revoking the independent acquired source.");
    }

    private static void WhileTracksHpWithoutRemovingItsRestriction()
    {
        var s = Scene.Start();
        var grants = s.RealGrants(1).ToArray();
        s.Use(0, Driver, "while-wounded", 1); s.Play(0);
        Require(s.Owns(1, Counter) && s.Effective(1, Counter) && s.StateLeases(1) == 1 && s.RealGrants(1).SequenceEqual(grants),
            "A false continuous condition retains an inactive restriction separately from the original real grants.");
        s.Use(0, Driver, "injure-one", 1); s.Play(0);
        Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.StateLeases(1) == 1,
            "A genuine HP change immediately enables the same restriction without a new skill grant or re-issue.");
        s.Cold();
        Require(!s.Effective(1, Counter), "Cold replay preserves an active continuous HP condition.");
        s.Use(0, Driver, "heal-one", 1); s.Play(0);
        Require(s.Owns(1, Counter) && s.Effective(1, Counter) && s.StateLeases(1) == 1 &&
            s.RealGrants(1).SequenceEqual(grants) && s.E<SkillStateExpiredEvent>().Length == 0,
            "When the condition becomes false again the skill immediately recovers; its restriction is retained rather than expired.");
        s.Cold();
        Require(s.Effective(1, Counter) && s.StateLeases(1) == 1, "Inactive continuous state cold-replays without recreating a grant.");
    }

    private static void LegacySkillStateFieldsAreRejected()
    {
        foreach (var legacy in new[] { "\"scope\":\"all\"", "\"duration\":\"permanent\"", "\"filter\":true", "\"skillIds\":[\"fixture:skill-state-counter\"]", "\"characterOnly\":true", "\"printedOnly\":true",
            "\"excludeLocked\":true", "\"expireOnDeath\":true", "\"untilTurnEndOf\":{\"kind\":\"owner\"}",
            "\"untilTurnStartOf\":{\"kind\":\"owner\"}", "\"afterCurrentTurn\":true", "\"endTiming\":\"beforeTurnEnded\"" })
        {
            var field = legacy.Split(':')[0].Trim('"');
            var rejected = false;
            try { LoadFilter("true", legacyProperty: legacy); }
            catch (InvalidOperationException error) when (error.Message.Contains(field, StringComparison.Ordinal)) { rejected = true; }
            Require(rejected, "The loader must explicitly reject retired skill-state property " + field + ".");
        }
    }

    private static void LoadFilter(string filter, bool captureThreshold = false, string? legacyProperty = null)
    {
        var capture = captureThreshold ? "{\"op\":\"captureNumber\",\"target\":\"owner\",\"resultBind\":\"threshold\",\"valueExpr\":0}," : "";
        var legacy = legacyProperty is null ? "" : "," + legacyProperty;
        _ = SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,
              "activations":[{"id":"filter","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                "effects":[{{{capture}}}{"op":"skillState","target":"owner","action":"suppress","match":{"filter":{{{filter}}}},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}]{{{legacy}}}}]}]}]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "Filter", description = "Shared expression resources" } } }));
    }

    private static void PhaseBoundariesUseActualTokensAndSelectedTiming()
    {
        foreach (var atStart in new[] { true, false })
        {
            var s = Scene.Start();
            s.Use(0, Driver, atStart ? "until-target-play-start" : "until-target-play-end", 1); s.Play(0);
            var restriction = s.Restriction(1);
            var end = restriction.Lifetime.Until.Single();
            s.Reach(() => s.E<ActualPhaseEndedEvent>().Any(e => e.Phase.ActorSeat == 1 && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw));
            var drawEnded = s.G.Events.Single(e => e.Payload is ActualPhaseEndedEvent ended &&
                ended.Phase.ActorSeat == 1 && ended.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw).Sequence;
            var expirySoFar = s.G.Events.SingleOrDefault(e => e.Payload is SkillStateExpiredEvent expiry && expiry.SourceId == restriction.SourceId);
            // One native step can finish Draw and begin Play synchronously.
            Require(expirySoFar is null ? !s.Effective(1, Counter) && s.StateLeases(1) == 1 :
                atStart && expirySoFar.Sequence > drawEnded && s.StateLeases(1) == 0,
                "Other participants' Play boundaries and the target's earlier phases cannot end a target-Play restriction.");
            s.Cold(); s.Play(1);
            var phase = s.E<ActualDiscardRecoveryPhaseStartedEvent>().Single(e => e.Phase.ActorSeat == 1 && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play).Phase;
            Require(phase.Token > end.IssuedOccurrence && end.Seat == 1 && end.Phase == phase.Kind && s.Effective(1, Counter) == atStart,
                "The target's actual Play token admits a before-start expiry while an after-end restriction remains active.");
            if (!atStart) { s.Cold(); s.Play(2); }
            var boundary = s.G.Events.Single(e => atStart
                ? e.Payload is ActualDiscardRecoveryPhaseStartedEvent started && started.Phase == phase
                : e.Payload is ActualPhaseEndedEvent ended && ended.Phase == phase).Sequence;
            var expired = s.G.Events.Single(e => e.Payload is SkillStateExpiredEvent expiry && expiry.SourceId == restriction.SourceId).Sequence;
            Require((atStart ? expired < boundary : expired > boundary) && s.StateLeases(1) == 0 && s.Effective(1, Counter),
                "Phase expiry uses the same real phase key and the requested side of its Started/Ended fact.");
            s.Cold();
            Require(s.StateLeases(1) == 0 && s.Effective(1, Counter), "Cold replay preserves native phase expiry and restored availability.");
        }
    }

    private static void BoundarySkipAndAlternativesClearBorrowedStateBeforeFacts()
    {
        var s = Scene.Start();
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", flags)!.GetValue(s.G)!;
        var operations = DispatchProxy.Create<IProgramSkillChangeOperations, ExpiryOperationsProbe>();
        var probe = (ExpiryOperationsProbe)operations;
        probe.Actual = capabilities.Get<IProgramSkillChangeOperations>();
        var runtime = new ProgramSkillChangeRuntime(capabilities.Get<IProgramSkillChangeFacts>(), operations,
            capabilities.Get<ISkillGrantRevisionObservations>(), capabilities.Get<IMatchParticipantStateAccess>(),
            capabilities.Get<IProgramTurnQueries>(), capabilities.Get<IActualDiscardRecoveryPhaseQueries>());
        var token = s.E<ActualDiscardRecoveryPhaseStartedEvent>().Last().Phase.Token;
        var source = s.RealGrants(0).Single(grant => grant.SkillId == Driver);
        var permanent = s.CounterGrants(0).Single().GrantId;
        var spec = new SkillStateSpec(SkillStateAction.Transfer, new([Counter]),
            [new(SkillStateBoundaryEvent.PhaseStarted, SkillStateSubject.Target, SkillStateBoundaryTiming.Before, true, ActualDiscardRecoveryPhaseKind.Play),
             new(SkillStateBoundaryEvent.PhaseEnded, SkillStateSubject.Target, SkillStateBoundaryTiming.After, false, ActualDiscardRecoveryPhaseKind.Draw)]);
        foreach (var alternative in new[] { true, false })
        {
            var origin = new SkillStateOrigin(900100, alternative ? 0 : 1, new(Driver, "transfer", 0, source.SkillInstanceId));
            runtime.IssueSkillState(origin, 1, spec);
            var restriction = s.Restriction(1);
            var borrowed = s.CounterGrants(0).Single(grant => grant.Lifetime is not null).GrantId;
            var before = s.RealGrants(0).ToArray();
            var revisions = (s.SkillRevision(0), s.SkillRevision(1), probe.AdvancedFacts);
            var rejected = false;
            try { runtime.IssueSkillState(origin, 1, new(SkillStateAction.Transfer, new([Counter]),
                [new(SkillStateBoundaryEvent.TurnEnded, SkillStateSubject.Owner)])); }
            catch (InvalidOperationException) { rejected = true; }
            Require(rejected && ReferenceEquals(s.Restriction(1), restriction) && s.RealGrants(0).SequenceEqual(before) &&
                (s.SkillRevision(0), s.SkillRevision(1), probe.AdvancedFacts) == revisions,
                "Reissuing a transfer source cannot replace the donor lifetime while retaining a different borrowed-grant lifetime.");
            var observed = probe.ExpiredFacts;
            probe.BeforeExpiry = fact =>
            {
                if (fact.SourceId != restriction.SourceId) return;
                Require(s.StateLeases(1) == 0 && s.CounterGrants(0).All(grant => grant.GrantId != borrowed),
                    "Before the first expiry rule fact both the target restriction and the borrowed positive grant must already be gone.");
            };
            foreach (var boundary in new[]
            {
                new SkillStateBoundary(SkillStateBoundaryEvent.PhaseStarted, 2, SkillStateBoundaryTiming.Before, token + 1, ActualDiscardRecoveryPhaseKind.Play),
                new SkillStateBoundary(SkillStateBoundaryEvent.PhaseStarted, 1, SkillStateBoundaryTiming.Before, token + 1, ActualDiscardRecoveryPhaseKind.Draw),
                new SkillStateBoundary(SkillStateBoundaryEvent.PhaseStarted, 1, SkillStateBoundaryTiming.After, token + 1, ActualDiscardRecoveryPhaseKind.Play),
                new SkillStateBoundary(SkillStateBoundaryEvent.PhaseStarted, 1, SkillStateBoundaryTiming.Before, token, ActualDiscardRecoveryPhaseKind.Play)
            }) runtime.ProcessSkillStateBoundary(boundary);
            Require(s.StateLeases(1) == 1 && s.CounterGrants(0).Any(grant => grant.GrantId == borrowed) && probe.ExpiredFacts == observed,
                "Participant, phase, timing and strictly later occurrence all qualify a boundary; skipIssued must retain the same token.");
            runtime.ProcessSkillStateBoundary(alternative
                ? new(SkillStateBoundaryEvent.PhaseEnded, 1, SkillStateBoundaryTiming.After, token, ActualDiscardRecoveryPhaseKind.Draw)
                : new(SkillStateBoundaryEvent.PhaseStarted, 1, SkillStateBoundaryTiming.Before, token + 1, ActualDiscardRecoveryPhaseKind.Play));
            Require(probe.ExpiredFacts == observed + 2 && s.Effective(1, Counter) && s.CounterGrants(0).Single().GrantId == permanent,
                "Either endpoint independently expires the borrowed pair while retaining the recipient's permanent source.");
        }
    }

    public class ExpiryOperationsProbe : DispatchProxy
    {
        internal object Actual = null!;
        internal Action<SkillStateExpiredEvent> BeforeExpiry = _ => { };
        internal Func<IEnumerable<IGameEvent>>? ReadAdvancing;
        internal int ExpiredFacts;
        internal int AdvancedFacts;
        protected override object? Invoke(MethodInfo? method, object?[]? args)
        {
            if (method!.Name == "get_AdvancingFacts" && ReadAdvancing is not null) return ReadAdvancing();
            if (method.Name == nameof(IProgramSkillChangeOperations.AdvanceFact))
            { AdvancedFacts++; if (args![0] is SkillStateExpiredEvent fact) { BeforeExpiry(fact); ExpiredFacts++; } }
            return method.Invoke(Actual, args);
        }
    }

    private static void DefaultAfterCleanupExpiresOnTheNativeClock()
    {
        var s = Scene.Start();
        s.Use(0, Driver, "cleanup-default-after", 1); s.Play(0);
        Require(!s.Effective(1, Counter), "The default-after cleanup restriction is active in its issuing turn.");
        s.Cold(); s.Play(1);
        Require(s.StateLeases(1) == 0 && s.CanUse(1, Counter, "pulse") && s.E<TurnCardUseEffectsExpiredEvent>().Length == 0,
            "Native cleanup emits its default After boundary even when there are no expiring card-use grants.");
    }

    private static void NestedFactIssuanceSkipsTheOriginalPhaseOccurrence()
    {
        var s = Scene.Start();
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(s.G)!;
        var facts = DispatchProxy.Create<IProgramSkillChangeFacts, ExpiryOperationsProbe>();
        var probe = (ExpiryOperationsProbe)facts; probe.Actual = capabilities.Get<IProgramSkillChangeFacts>();
        var runtime = new ProgramSkillChangeRuntime(facts, capabilities.Get<IProgramSkillChangeOperations>(),
            capabilities.Get<ISkillGrantRevisionObservations>(), capabilities.Get<IMatchParticipantStateAccess>(),
            capabilities.Get<IProgramTurnQueries>(), capabilities.Get<IActualDiscardRecoveryPhaseQueries>());
        var source = s.RealGrants(0).Single(grant => grant.SkillId == Driver);
        var nested = new NestedPhaseIssuer { Runtime = runtime,
            Origin = new(900200, 0, new(Driver, "transfer", 0, source.SkillInstanceId)),
            Spec = new(SkillStateAction.Suppress, new([Counter]),
                [new(SkillStateBoundaryEvent.PhaseStarted, SkillStateSubject.Target, SkillStateBoundaryTiming.After, true, ActualDiscardRecoveryPhaseKind.Play)]) };
        var methods = typeof(NestedPhaseIssuer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(method => method.IsDefined(typeof(ProgramRuleObserverAttribute)))
            .Concat(typeof(ProgramSkillChangeRuntime).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(method => method.Name is "BeforeBoundaryEvent" or "AfterBoundaryEvent"));
        var observers = new ProgramRuleObservers(ProgramRuleObserverCatalog.Discover(methods),
            (method, type) => method.CreateDelegate(type, method.DeclaringType == typeof(NestedPhaseIssuer) ? nested : runtime));
        nested.Progression = new(observers, new FactSink()); probe.ReadAdvancing = () => nested.Progression.AdvancingFacts;
        var old = s.E<ActualDiscardRecoveryPhaseStartedEvent>().Last().Phase;
        var phase = old with { Token = old.Token + 100, ActorSeat = 1 };
        nested.Progression.AdvanceFact(new ActualDiscardRecoveryPhaseStartedEvent(phase));
        Require(s.Restriction(1).Lifetime.Until.Single().IssuedOccurrence == phase.Token && !s.Effective(1, Counter) &&
            !nested.Progression.AdvancingFacts.Any(),
            "A nested ordinary fact retains its enclosing phase token for skipIssued, and the synchronous fact stack unwinds.");
        runtime.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.PhaseStarted, 1, SkillStateBoundaryTiming.After, phase.Token + 1, phase.Kind));
        Require(s.StateLeases(1) == 0 && s.Effective(1, Counter), "Only a strictly later matching occurrence expires the nested issuance.");
    }

    private sealed class NestedPhaseIssuer
    {
        internal RuleProgressionRuntime Progression = null!;
        internal ProgramSkillChangeRuntime Runtime = null!;
        internal SkillStateOrigin Origin = null!;
        internal SkillStateSpec Spec = null!;
        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 40)]
        private void Nested(ActualDiscardRecoveryPhaseStartedEvent _) => Progression.AdvanceFact(new GameStartedEvent(4));
        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 10)]
        private void Issue(GameStartedEvent _) => Runtime.IssueSkillState(Origin, 1, Spec);
        [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 1)]
        private void State() { }
    }

    private sealed class FactSink : IRuleProgressionOperations
    {
        public void QueueFact(IGameEvent payload) { }
        public bool BeginCharacterStateWindow() => false;
        public bool BeginHpChangedWindow() => false;
        public bool BeginCardsMovedWindow() => false;
        public void RequestStatePublication() { }
    }

    private static void IssuedPhaseConditionSurvivesInsertionAndRestoration()
    {
        var s = Scene.Start();
        var issued = s.E<ActualDiscardRecoveryPhaseStartedEvent>().Last().Phase;
        var player = ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine.GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(s.G)!)[0];
        var phases = new MutablePhase { Value = issued };
        using var document = JsonDocument.Parse("[\"inIssuedPhase\",\"target\"]");
        var condition = ProgramExpressions.Parse(document.RootElement, "fixture");
        var world = new SkillStateConditionWorld(new(0, 0, issued), player, phases: phases);
        Require(ProgramExpressions.EvaluateBool(condition, world) == true, "The original actor is in its exact issued phase.");
        phases.Value = issued with { Token = issued.Token + 1, Kind = ActualDiscardRecoveryPhaseKind.Draw };
        Require(ProgramExpressions.EvaluateBool(condition, world) == false, "An inserted phase suspends the original phase condition.");
        phases.Value = issued;
        Require(ProgramExpressions.EvaluateBool(condition, world) == true, "Restoring the original key reactivates the same condition.");
        phases.Value = issued with { Token = issued.Token + 2 };
        Require(ProgramExpressions.EvaluateBool(condition, world) == false, "A later same-kind phase cannot impersonate the original phase token.");
    }

    private sealed class MutablePhase : IActualDiscardRecoveryPhaseQueries
    {
        internal ActualDiscardRecoveryPhaseKey? Value;
        public ActualDiscardRecoveryPhaseKey? Current() => Value;
        public bool IsRecorded(ActualDiscardRecoveryPhaseKey phase) => phase == Value;
    }

    private static void PermanentRestrictionExpiresOnItsTargetDeath()
    {
        var s = Scene.Start();
        s.Use(0, Driver, "permanent-until-death", 1); s.Play(0);
        var issued = s.E<SkillStateChangedEvent>().Single(e => e.Seat == 1 && e.SkillId == Counter && e.Action == SkillStateAction.Lose);
        Require(!s.Owns(1, Counter) && s.StateLeases(1) == 1 && s.Effective(0, Driver),
            "Permanent loss with death expiry retains a target-owned lease without losing the issuing skill.");
        s.Cold();
        s.Use(0, Driver, "kill-target", 1);
        s.Reach(() => s.E<PlayerDiedEvent>().Any(e => e.VictimSeat == 1));
        Require(!s.View(1).IsAlive && s.View(0).IsAlive && s.StateLeases(1) == 0 &&
            s.E<SkillStateExpiredEvent>().Count(e => e.Seat == 1 && e.SourceId == issued.SourceId) == 1,
            "The affected target's native death expires a permanent restriction independently of the living turn owner.");
        s.Cold();
        Require(s.StateLeases(1) == 0 && s.E<SkillStateExpiredEvent>().Count(e => e.SourceId == issued.SourceId) == 1,
            "Cold replay does not restore the expired lease or publish a second expiry.");
    }

    private static void ConsumedAlcoholSlashQualifiesOnlyItsOriginalDamage()
    {
        foreach (var (drank, chained) in new[] { (false, false), (true, false), (true, true) })
        {
            var s = Scene.Start();
            if (chained)
                foreach (var seat in new[] { 1, 2 }) { s.Use(0, Driver, "chain", seat); s.Play(0); }
            if (drank) { s.PlayAs("wine"); s.Play(0); s.Cold(); }
            var hp = s.View(1).Hp;
            s.PlayAs("fire-slash", 1); s.Play(0);
            var damage = s.E<DamageAppliedEvent>().Where(e => !e.SourceLess && e.SourceSeat == 0).ToArray();
            var states = s.E<SkillStateChangedEvent>().Where(e => e.Action == SkillStateAction.Suppress && e.SkillId == Counter).ToArray();
            Require(s.View(1).Hp == hp - (drank ? 2 : 1) && damage.Length == (chained ? 2 : 1) &&
                damage.All(e => e.Amount == (drank ? 2 : 1) && e.Nature == DamageNature.Fire) &&
                s.E<CardUseAlcoholConsumptionCapturedEvent>().Length == (drank ? 1 : 0),
                "Real converted Slash consumes Alcohol once; linked fire damage uses the original enhanced amount.");
            Require(states.Length == (drank ? 1 : 0) && states.All(e => e.Seat == 0 && e.Origin?.Source.SkillId == Driver) &&
                s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Driver && e.BindingId == "alcohol-state") == (drank ? 1 : 0) &&
                s.Owns(0, Counter) && s.Effective(0, Counter) == !drank,
                "Only the owner's original Alcohol-consuming Slash damage offers the binding and issues one named suppression; ordinary and chain-propagated damage do not.");
            s.Cold();
            Require(s.Effective(0, Counter) == !drank, "The shared Alcohol qualifier and resulting named state cold-replay together.");
        }
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? Pending => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot View(int seat) => G.CreateSnapshot(seat).Players[seat];
        private CharacterState Player(int seat) => ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine
            .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(G)!)[seat];
        public bool Owns(int seat, string skill) => G.NationalSkillsEntries.HasOwnedRuntimeSkill(Player(seat), skill);
        public bool Effective(int seat, string skill) => G.NationalSkillsEntries.HasRuntimeSkill(Player(seat), skill);
        public bool GrantQualified(int seat, string skill)
        {
            var player = Player(seat);
            var grant = player.SkillGrants.Grants.Single(g => g.SkillId == skill && g.IsEnabled);
            return G.NationalSkillsEntries.HasRuntimeSkillInstance(player, grant.SkillId, grant.SkillInstanceId);
        }
        public bool OwnsInstance(int seat, string skill, string instance) => G.NationalSkillsEntries.HasOwnedRuntimeSkillInstance(Player(seat), skill, instance);
        public int StateLeases(int seat) => Player(seat).SkillGrants.Restrictions.Count;
        public long SkillRevision(int seat) => Player(seat).SkillGrants.Revision;
        public SkillStateRestriction Restriction(int seat) => Player(seat).SkillGrants.Restrictions.Single();
        public IReadOnlyList<SkillGrant> RealGrants(int seat) => Player(seat).SkillGrants.Grants;
        public IReadOnlyList<SkillGrant> CounterGrants(int seat) => Player(seat).SkillGrants.Grants.Where(g => g.SkillId == Counter).ToArray();
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool CanUse(int seat, string skill, string activation) => G.GetLegalActions(seat).Any(a => a.ProgramSkillId == skill && a.ProgramActivationId == activation);
        public ProgramBooleanStateSnapshot CounterState(int seat) => View(seat).SkillRuntimeStates!.Single(s => s.SkillId == Counter).BooleanStates!.Single(s => s.StateId == "marked");
        public void Use(int seat, string skill, string activation, params int[] targets)
        {
            Require(Pending is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat && CanUse(seat, skill, activation),
                $"Missing genuine activation {seat}/{skill}/{activation}.");
            Submit(new UseProgramSkillCommand(seat, skill, activation, [], targets, G.Revision, Pending!.PromptId));
        }
        public void Answer(Func<PromptChoice, bool> match)
        { var p = Pending!; Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void PlayAs(string binding, params int[] targets)
        {
            var action = G.GetLegalActions(0).First(a => a.ConversionSource is { SkillId: Driver } source &&
                source.BindingId == binding && a.TargetSeats.SequenceEqual(targets));
            Submit(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, Pending!.PromptId, action.PlayedCardKind)
            { ConversionSource = action.ConversionSource });
        }
        public void Play(int seat) => Reach(() => Pending is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Reach(Func<bool> stop)
        {
            for (var step = 0; step < 500; step++)
            {
                if (stop()) return;
                if (Pending is not { } p) { Submit(new AdvanceOneStepCommand(G.Revision)); continue; }
                switch (p.Kind)
                {
                    case DecisionKind.SelectGeneral:
                        var general = p.PlayerSeat == 0 ? Owner : p.ValidContentIds[0];
                        Submit(new SelectGeneralCommand(p.PlayerSeat, general, G.Revision, p.PromptId)); break;
                    case DecisionKind.PlayCard:
                        Submit(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); break;
                    case DecisionKind.DiscardCards:
                        Submit(new DiscardCardsCommand(p.PlayerSeat, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision)); break;
                    case DecisionKind.RescueDying:
                        Answer(choice => choice.Cards.Count == 0); break;
                    case DecisionKind.RespondDodge:
                        Answer(choice => choice.Parameters.GetValueOrDefault("response") == "take-damage"); break;
                    default: throw new InvalidOperationException($"Unexpected shared skill-state prompt {p.Kind}, seat {p.PlayerSeat}.");
                }
            }
            throw new InvalidOperationException("Shared skill-state fixture missed its bounded native boundary.");
        }
        private void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Require(result.Accepted, result.Error?.Message ?? "A genuine skill-state command was rejected.");
        }
        public void Cold()
        {
            var views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))).ToArray();
            var commands = G.AcceptedCommands.Count;
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Require(G.AcceptedCommands.Count == commands && Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat)) == views[seat]),
                "Cold command replay preserves choices, every viewer's privacy, skill state and accepted command prefix.");
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4,
                HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry), registry);
            scene.Submit(new StartGameCommand()); scene.Play(0);
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shared-skill-state", "1.0.0", "Shared skill-state command checks");
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"viewAs":[
                    {"id":"wine","inputKinds":["dodge"],"inputSuits":[],"sourceZones":["hand"],"outputKind":"alcohol","forPlay":true,"forResponse":false,"useOnly":true},
                    {"id":"fire-slash","inputKinds":["dodge"],"inputSuits":[],"sourceZones":["hand"],"outputKind":"fireSlash","forPlay":true,"forResponse":false,"useOnly":true}
                  ],"activations":[
                    {"id":"suppress-self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"owner","action":"suppress","match":{"filter":["and",["==",["number","skill.character"],1],["==",["number","skill.locked"],0]]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}]}]},
                    {"id":"choose","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"lose","match":{"filter":["and",["==",["number","skill.character"],1],["==",["number","skill.locked"],0]]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}],"chooseOne":true},
                      {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true}]},
                    {"id":"overlap-now","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.character"],1]},"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"after"}]}]},
                    {"id":"overlap-through-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.character"],1]},"until":[{"event":"turnEnded","participant":{"kind":"target"},"timing":"after"}]}]},
                    {"id":"through-next-owner-end","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"filter":["and",["==",["number","skill.character"],1],["==",["number","skill.locked"],0]]},"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"after","skipIssued":true}]}]},
                    {"id":"through-next-owner-before-end","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"filter":["and",["==",["number","skill.character"],1],["==",["number","skill.locked"],0]]},"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"before","skipIssued":true}]}]},
                    {"id":"through-current-round","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.character"],1]},"until":[{"event":"roundAdvanced"}]}]},
                    {"id":"until-next-owner-start","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"filter":["and",["==",["number","skill.character"],1],["==",["number","skill.locked"],0]]},"until":[{"event":"turnStarted","participant":{"kind":"owner"},"timing":"before","skipIssued":true}]}]},
                    {"id":"transfer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"transfer","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.character"],1]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}]}]},
                    {"id":"lose-own-counter","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.character"],1]},"until":[]}]},
                    {"id":"suppress-absent-and-grant","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"owner","action":"suppress","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.character"],1]},"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"after"}]},
                      {"op":"grantSkills","target":"owner","skillIds":["{{{Counter}}}"]}]},
                    {"id":"grant-own-counter","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"grantSkills","target":"owner","skillIds":["{{{Counter}}}"]}]},
                    {"id":"lose-printed-counter","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.printed"],1]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}]}]},
                    {"id":"while-wounded","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Counter}}}"]},"until":[],"while":["<",["hp","target"],["maxHp","target"]]}]},
                    {"id":"injure-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"loseHp","target":"selectedTarget","amount":1}]},
                    {"id":"heal-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"recover","target":"selectedTarget","amount":1}]},
                    {"id":"until-target-play-start","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Counter}}}"]},"until":[{"event":"phaseStarted","participant":{"kind":"target"},"timing":"before","skipIssued":true,"phase":"play"}]}]},
                    {"id":"until-target-play-end","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Counter}}}"]},"until":[{"event":"phaseEnded","participant":{"kind":"target"},"timing":"after","phase":"play"}]}]},
                    {"id":"cleanup-default-after","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Counter}}}"]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"}}]}]},
                    {"id":"filter","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"filter":["and",["==",["number","skill.character"],1],["and",["==",["number","skill.locked"],0],["==",["number","skill.printed"],1],["==",["number","skill.ordinal"],1]]]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}]}]},
                    {"id":"permanent-until-death","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"lose","match":{"skillIds":["{{{Counter}}}"],"filter":["==",["number","skill.character"],1]},"until":[{"event":"death","participant":{"kind":"target"}}]}]},
                    {"id":"kill-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"loseHp","target":"selectedTarget","amount":20}]},
                    {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"setChainedState","target":"selectedTarget","chained":true}]}
                  ],"triggers":[{"id":"alcohol-state","window":"afterDamageApplied","subject":"damageSource","optional":false,
                    "damageOccurrence":"perDamage","requiresConsumedAlcoholSlash":true,"effects":[
                      {"op":"skillState","target":"owner","action":"suppress","match":{"skillIds":["{{{Counter}}}"]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}]}]}]},
                  {"id":"{{{Counter}}}","revision":1,"states":[{"id":"marked","initialValue":false,"visibility":"public","resetScope":"game","reacquirePolicy":"preserveUntilGameEnd"}],"activations":[
                    {"id":"mark","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"usesPerGame":1,"effects":[{"op":"setBooleanState","target":"owner","stateId":"marked","value":true}]},
                    {"id":"pulse","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"usesPerGame":1,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{{Locked}}}","revision":1,"activations":[{"id":"pulse","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"usesPerGame":1,"effects":[{"op":"draw","target":"owner","amount":1}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "State driver", description = "Shared state effects" },
                        [Counter] = new { name = "Counter", description = "Persistent state and quota" }, [Locked] = new { name = "Locked", description = "Locked exemption" } } }));
            foreach (var (id, program) in rules.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, Tags = id == Locked ? SkillTag.Locked : SkillTag.None });
            builder.AddGeneral(new(Owner, "State owner", "supporter", Driver, "wei", 8, [Counter, Locked]));
            for (var i = 1; i < 4; i++) builder.AddGeneral(new($"fixture:skill-state-other-{i}", "Public target", "supporter", Counter, "qun", 8, [Locked]));
            builder.AddDeck(new("fixture:skill-state-deck", "Quiet fixed deck", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 160).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, i % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "Shared skill state", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, "fixture:skill-state-deck",
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, "fixture:skill-state-other-1", "fixture:skill-state-other-2", "fixture:skill-state-other-3"]));
        }
    }
}
