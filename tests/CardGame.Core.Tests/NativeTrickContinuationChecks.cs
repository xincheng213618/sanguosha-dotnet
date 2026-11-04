using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeTrickContinuationChecks
{
    public static void OriginalUseIdentityAndIssuedStagesSurviveLiveRoleChanges()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var zones = (CardZoneStore)capabilities.Get<ICardZoneQueries>();
        var runtime = capabilities.Get<INativeTrickContinuations>();
        var priorTop = frames.Top;
        CheckStagesAndShapes(frames, zones, runtime);
        CheckOriginalUseAndLiveRoles(frames, runtime);
        Need(ReferenceEquals(priorTop, frames.Top), "Trick continuation checks restore the original owning stack.");
    }

    private static void CheckStagesAndShapes(IProgramRuntimeFrames frames, CardZoneStore zones,
        INativeTrickContinuations runtime)
    {
        var entities = zones.CardsAt(CardLocation.DrawPile).Take(2).ToArray();
        zones.Move(entities[0].Id, CardLocation.DrawPile, CardLocation.Processing);
        zones.Move(entities[1].Id, CardLocation.DrawPile, CardLocation.Judgment(1));
        foreach (var form in new[] { "zero", "physical", "carrier" })
        {
            var card = form == "zero" ? new Card(0, CardKind.Duel, Suit.None, 0) : entities[form == "physical" ? 0 : 1];
            var kind = form == "carrier" ? CardKind.Indulgence : CardKind.Duel;
            var costs = form == "physical" ? new[] { new CardActionCost(card.Id, card.Kind, CardLocation.Hand(0)) } : [];
            var id = frames.NextId();
            var action = Action(id * 100, kind, costs: costs);
            var use = new CardUseFrame(id, 0, card.Id, kind, [1], PhysicalCardIds: costs.Select(cost => cost.CardId).ToArray())
                { NativeRepresentation = card, NativeRepresentationIsCarrier = form == "carrier", Action = action,
                  ProgramUseCommitted = true };
            frames.Push(use);
            try
            {
                var descriptor = new ProgramTrickContinuation(card.Id,
                    form == "carrier" ? LegalActionKind.Indulgence : LegalActionKind.Duel);
                var committed = new ProgramCardTriggerWindowFrame(frames.NextId(), use.Id, action,
                    ProgramCardContinuation.CommittedTrick, [], TrickContinuation: descriptor);
                runtime.ValidateIssuance(use.Id, action, ProgramCardContinuation.CommittedTrick, descriptor);
                Need(runtime.Matches(committed) && runtime.Read(committed).Use.Id == use.Id &&
                        !use.ProgramUseAccepted && !use.FinalizedTrickProgramsStarted,
                    "Zero-entity, physical and carrier tricks share their actual committed-use boundary without requiring a later acceptance or finalization.");
                var before = committed with { Continuation = ProgramCardContinuation.BeforeTrickTargetEffects };
                frames.Replace(use.Id, use with { ProgramUseCommitted = false });
                runtime.ValidateIssuance(use.Id, action, ProgramCardContinuation.BeforeTrickTargetEffects, descriptor);
                Need(runtime.Matches(before), "A real before-effects continuation does not invent an unrelated acceptance prerequisite.");
                Need(!runtime.Matches(committed), "A committed continuation requires the actual committed stage.");
                var finalized = committed with { Continuation = ProgramCardContinuation.FinalizedTrick };
                Need(!runtime.Matches(finalized), "A finalized continuation cannot run before its actual finalization stage.");
                frames.Replace(use.Id, use with { FinalizedTrickProgramsStarted = true });
                runtime.ValidateIssuance(use.Id, action, ProgramCardContinuation.FinalizedTrick, descriptor);
                Need(runtime.Matches(finalized), "The original use's finalized stage authorizes its issued trick continuation.");
                var delayed = committed with { Continuation = ProgramCardContinuation.FinalizedDelayedPlacement };
                Need(!runtime.Matches(delayed), "Delayed-placement continuation requires its own accepted delayed-card boundary.");
                frames.Replace(use.Id, use with { FinalizedTrickProgramsStarted = true, ProgramUseAccepted = true });
                Need(runtime.Matches(delayed) == (form == "carrier"),
                    "Finalized delayed placement belongs to a delayed use, independently of whether its physical carrier paid a material.");
                if (form == "carrier")
                    runtime.ValidateIssuance(use.Id, action, ProgramCardContinuation.FinalizedDelayedPlacement, descriptor);

                foreach (var malformed in new[]
                {
                    committed with { ParentFrameId = long.MaxValue },
                    committed with { Action = Action(action.ActionId + 1, kind, costs: costs) },
                    committed with { Action = Action(action.ActionId, kind, type: CardActionType.Response, costs: costs) },
                    committed with { Continuation = ProgramCardContinuation.CompletedCard },
                    committed with { TrickContinuation = descriptor with { EffectCardId = card.Id + 100000 } },
                    committed with { TrickContinuation = null },
                    committed with { SimpleContinuation = new(card.Id, SimpleCardUseEffect.Alcohol) },
                    committed with { CompletedResponseReturn = new(ProgramCompletedResponseKind.Nullification) },
                    committed with { AttackOwnerFrameId = use.Id },
                    committed with { ResponseCompletion = new(action.ActionId, use.Id, null, ProgramCardContinuation.DuelSlash, 1, 0) },
                    committed with { CardSupplyCompletion = new() { ActionId = action.ActionId, ParentFrameId = use.Id } }
                })
                {
                    Need(!runtime.Matches(malformed), "An unrelated use, Action, stage, effect identity or competing return shape cannot authorize a trick continuation.");
                    Reject(() => runtime.Read(malformed));
                }
                var unchanged = frames.Find(use.Id);
                Reject(() => runtime.ValidateIssuance(use.Id, Action(action.ActionId + 1, kind),
                    ProgramCardContinuation.CommittedTrick, descriptor));
                Reject(() => runtime.ValidateIssuance(use.Id, action, ProgramCardContinuation.CompletedCard, descriptor));
                Need(ReferenceEquals(unchanged, frames.Find(use.Id)),
                    "Rejected trick continuation identities leave the original native use untouched.");
            }
            finally { frames.Pop(use.Id, use.Kind); }
        }
    }

    private static void CheckOriginalUseAndLiveRoles(IProgramRuntimeFrames frames, INativeTrickContinuations runtime)
    {
        var id = frames.NextId();
        var originalAction = Action(id * 100, CardKind.Duel);
        var use = new CardUseFrame(id, 0, 0, CardKind.Duel, [1], PhysicalCardIds: [])
            { Action = originalAction, NativeRepresentation = new(0, CardKind.Duel, Suit.None, 0),
              ProgramUseCommitted = true, FinalizedTrickProgramsStarted = true };
        frames.Push(use);
        try
        {
            var descriptor = new ProgramTrickContinuation(0, LegalActionKind.Duel);
            var windowId = frames.NextId();
            var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.CardUseTargetsFinalized, windowId,
                0, SourceSeat: 0, TargetSeat: 1);
            var candidate = new ProgramCardTriggerCandidate(0, 1, "fixture:trick-continuation", "observe", "hash",
                FrozenContext: context);
            var frozen = new ProgramCardTriggerWindowFrame(windowId, use.Id, originalAction,
                ProgramCardContinuation.FinalizedTrick, [candidate], TrickContinuation: descriptor);
            frozen = JsonSerializer.Deserialize<ProgramCardTriggerWindowFrame>(JsonSerializer.Serialize(frozen))!;
            var changed = use with { SourceSeat = 2, TargetSeats = [3, 1],
                Action = Action(originalAction.ActionId, CardKind.Duel, actor: 2, targets: [3, 1]) };
            frames.Replace(use.Id, changed);
            Need(runtime.Matches(frozen), "The same original Action remains identifiable after a legitimate actor and target update.");
            var read = runtime.Read(frozen);
            Need(read.Use.SourceSeat == 2 && read.Use.TargetSeats.SequenceEqual([3, 1]) &&
                    read.Use.Action!.ProviderSeat == originalAction.ProviderSeat && read.Continuation == descriptor &&
                    frozen.Action.ActorSeat == 0 && frozen.Action.TargetSeats.SequenceEqual([1]) &&
                    frozen.Candidates.Single().FrozenContext is { SourceSeat: 0, TargetSeat: 1 },
                "Return reads the original use's current actor and targets without refreshing already issued window qualification or changing the original material provider.");

            var nestedId = frames.NextId();
            var nested = new CardUseFrame(nestedId, 2, 0, CardKind.Duel, [3], PhysicalCardIds: [])
                { Action = Action(nestedId * 100, CardKind.Duel, actor: 2, targets: [3]),
                  NativeRepresentation = new(0, CardKind.Duel, Suit.None, 0), FinalizedTrickProgramsStarted = true };
            frames.Push(nested);
            try
            {
                Need(runtime.Matches(frozen), "Identity reads still find the original suspended use beneath a same-kind nested use.");
                Reject(() => runtime.Read(frozen));
            }
            finally { frames.Pop(nested.Id, nested.Kind); }
            Need(runtime.Read(frozen).Use.Id == use.Id && ReferenceEquals(frames.Find(use.Id), changed),
                "Draining the real child returns to the same owning use without replacing its Action or paying again.");

            var actualId = frames.NextId();
            var actualContext = new ProgramSkillWindowContext(SkillProgramTriggerWindow.OtherActualUseTargeted,
                actualId, 1, SourceSeat: 0, TargetSeat: 1)
                { ActualUseTarget = new(use.Id, originalAction.ActionId, 0, 0, CardKind.Duel, 1, 1, 0, null) };
            var actual = new ActualUseTargetWindowFrame(actualId, use.Id, ActualUseTargetReturnKind.OrdinaryTrick,
                [new(1, "fixture:trick-continuation", "observe", "instance", "hash", 0)], [actualContext],
                new(0, LegalActionKind.Duel, null, null));
            actual = JsonSerializer.Deserialize<ActualUseTargetWindowFrame>(JsonSerializer.Serialize(actual))!;
            Need(runtime.Read(actual).Use.SourceSeat == 2 && actual.Contexts.Single().ActualUseTarget!.ActorSeat == 0,
                "The actual-target return retains its original frozen context while resuming the same use's current actor.");
            Reject(() => runtime.Read(actual with { ParentFrameId = long.MaxValue }));
            Reject(() => runtime.Read(actual with { ReturnKind = ActualUseTargetReturnKind.Slash }));
            Reject(() => runtime.Read(actual with { TrickReturn = actual.TrickReturn! with { EffectCardId = 100000 } }));
            Reject(() => runtime.Read(actual with { Contexts = [actualContext with
                { ActualUseTarget = actualContext.ActualUseTarget! with { ActionId = originalAction.ActionId + 1 } }] }));
            Reject(() => runtime.Read(actual with { Contexts = [actualContext with
                { ActualUseTarget = actualContext.ActualUseTarget! with { CardUseFrameId = long.MaxValue } }] }));
        }
        finally { frames.Pop(use.Id, use.Kind); }
    }

    private static CardActionContext Action(long id, CardKind kind, int actor = 0, int[]? targets = null,
        CardActionType type = CardActionType.Use, IReadOnlyList<CardActionCost>? costs = null) =>
        new(id, null, type, actor, 0, null, null, null, kind, targets ?? [1], costs ?? [], [],
            effectiveSuit: Suit.None, effectiveRank: 0);
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A trick continuation escaped its original use, Action, stage or issued return shape.");
    }
}
