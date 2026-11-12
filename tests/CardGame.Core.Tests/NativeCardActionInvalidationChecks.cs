using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeCardActionInvalidationChecks
{
    public static void OriginalOwnersKeepUseResponseAndSimpleCancellationScopes()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var admission = new Admission();
        var runtime = new NativeCardActionInvalidationRuntime(frames, admission);
        var depth = frames.OfType<ResolutionFrame>().Count;
        var action = UseAction(101);
        var use = new CardUseFrame(frames.NextId(), 0, 0, CardKind.Slash, [1], PhysicalCardIds: [])
        { Action = action, CardAttack = new() { SourceSeat = 0, CardUserSeat = 0, TargetSeat = 1, EffectiveCardKind = CardKind.Slash } };
        var other = new CardUseFrame(frames.NextId(), 0, 0, CardKind.Slash, [1], PhysicalCardIds: [])
        { Action = UseAction(201), CardAttack = use.CardAttack };
        frames.Push(use); frames.Push(other);
        try
        {
            var unchanged = frames.Find(use.Id);
            Reject(() => runtime.IssueUse(long.MaxValue, action, new()));
            Reject(() => runtime.IssueUse(use.Id, null, new()));
            Reject(() => runtime.IssueUse(use.Id, UseAction(102), new()));
            Need(ReferenceEquals(unchanged, frames.Find(use.Id)), "Invalid native identities reject before changing the original use.");
            runtime.IssueUse(use.Id, action, new());
            var issued = frames.Find(use.Id);
            runtime.IssueUse(use.Id, action, new());
            Need(ReferenceEquals(issued, frames.Find(use.Id)) && runtime.IsUseIneffective(use.Id) &&
                !runtime.IsUseIneffective(other.Id) && !runtime.SuppressesRemainingCardDamage(use.Id),
                "Whole-use ineffectiveness is idempotent and does not grant another use or already-issued damage permission.");

            var effects = new List<SimpleCardUseEffect> { SimpleCardUseEffect.Alcohol, SimpleCardUseEffect.Recovery };
            var late = new NativeSimpleCardCancellationPolicy(NativeSimpleCardCancellationBoundary.BeforeTargetEffects,
                effects, new("fixture.late-cleanup"), Priority: 30);
            effects.Add(SimpleCardUseEffect.EquipmentPlacement);
            var equipment = new NativeSimpleCardCancellationPolicy(NativeSimpleCardCancellationBoundary.EarlySimple,
                [SimpleCardUseEffect.EquipmentPlacement], new("fixture.equipment-cleanup"), Priority: 10);
            var all = new NativeSimpleCardCancellationPolicy(NativeSimpleCardCancellationBoundary.EarlySimple,
                Enum.GetValues<SimpleCardUseEffect>(), new("fixture.early-cleanup"), Priority: 0);
            runtime.IssueUse(use.Id, action, new(SimpleCancellation: late));
            runtime.IssueUse(use.Id, action, new(SimpleCancellation: equipment));
            Need(runtime.TryReadSimpleCancellation(use.Id, SimpleCardUseEffect.Recovery,
                    NativeSimpleCardCancellationBoundary.EarlySimple) is null &&
                runtime.TryReadSimpleCancellation(use.Id, SimpleCardUseEffect.EquipmentPlacement,
                    NativeSimpleCardCancellationBoundary.BeforeTargetEffects) is null &&
                runtime.TryReadSimpleCancellation(use.Id, SimpleCardUseEffect.EquipmentPlacement,
                    NativeSimpleCardCancellationBoundary.EarlySimple)?.CleanupReason == equipment.CleanupReason,
                "Early equipment cleanup and late basic-card cleanup retain their actual native scheduling boundaries and frozen effect sets.");
            runtime.IssueUse(use.Id, action, new(SimpleCancellation: all));
            var equallyEarly = new NativeSimpleCardCancellationPolicy(NativeSimpleCardCancellationBoundary.EarlySimple,
                [SimpleCardUseEffect.EquipmentPlacement], new("fixture.later-equal-priority"));
            runtime.IssueUse(use.Id, action, new(SimpleCancellation: equallyEarly));
            runtime.IssueUse(use.Id, action, new(SuppressRemainingCardDamage: true));
            issued = frames.Find(use.Id);
            runtime.IssueUse(use.Id, action, new(SuppressRemainingCardDamage: true, SimpleCancellation: all));
            Need(ReferenceEquals(issued, frames.Find(use.Id)) && runtime.SuppressesRemainingCardDamage(use.Id) &&
                runtime.TryReadSimpleCancellation(use.Id, SimpleCardUseEffect.EquipmentPlacement,
                    NativeSimpleCardCancellationBoundary.EarlySimple)?.CleanupReason == all.CleanupReason &&
                ((CardUseFrame)issued!).NativeUseInvalidation!.SimpleCancellations.Count == 4,
                "Independent native policies select by explicit priority then issuance order; exact reentry does not duplicate a policy or withdraw damage suppression.");
            var changed = ((CardUseFrame)issued!) with { SourceSeat = 2, TargetSeats = [],
                Action = UseAction(action.ActionId, actor: 2, kind: CardKind.Duel, targets: []) };
            frames.Replace(use.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(changed))!);
            Need(runtime.IsUseIneffective(use.Id) && runtime.SuppressesRemainingCardDamage(use.Id) &&
                ((CardUseFrame)frames.Find(use.Id)!).NativeUseInvalidation!.SimpleCancellations is System.Collections.IList { IsReadOnly: true } &&
                runtime.TryReadSimpleCancellation(use.Id, SimpleCardUseEffect.Recovery,
                    NativeSimpleCardCancellationBoundary.BeforeTargetEffects)?.Effects is System.Collections.IList { IsReadOnly: true },
                "Original-use identity and frozen policies survive JSON recovery and real actor, kind and target changes without requalifying the issuing source.");

            var dodge = new CardActionContext(202, other.Action!.ActionId, CardActionType.Response,
                1, 2, 1, 2, 0, CardKind.Dodge, [], [], []);
            runtime.IssueDodge(other.Id, dodge);
            var dodged = frames.Find(other.Id);
            runtime.IssueDodge(other.Id, dodge);
            Need(ReferenceEquals(dodged, frames.Find(other.Id)) && runtime.IsDodgeIneffective(other.Id, dodge) &&
                !runtime.IsUseIneffective(other.Id) && !runtime.IsDodgeIneffective(use.Id, dodge) &&
                !runtime.IsDodgeIneffective(other.Id, Response(203, other.Action.ActionId, CardKind.Dodge, 1, 2)),
                "A genuine supplied Dodge keeps its logical responder and distinct provider, and invalidates only that response action.");
            Reject(() => runtime.IssueDodge(other.Id, Response(204, other.Action.ActionId, CardKind.Dodge, 2, 2)));
            frames.Replace(other.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize((CardUseFrame)frames.Find(other.Id)!))!);
            Need(runtime.IsDodgeIneffective(other.Id, dodge) &&
                ((CardUseFrame)frames.Find(other.Id)!).IneffectiveDodgeActionIds is System.Collections.IList { IsReadOnly: true },
                "Response action identities remain frozen on their actual incoming use through JSON recovery.");

            var counter = Response(301, other.Action.ActionId, CardKind.Nullification, 1, 2);
            var window = new NullificationWindowFrame(frames.NextId(), other.Id, 0, [1], 0,
                CardKind.Duel, LegalActionKind.Duel, null, null, [1, 2], ChainDepth: 1)
            { ResponseRuleNode = new(1, counter), NamedAmbushResponse = new(counter.ActionId, 1, 0, false, 0) };
            frames.Push(window);
            runtime.IssueCounterspell(window.Id, counter);
            var countered = frames.Find(window.Id);
            runtime.IssueCounterspell(window.Id, counter);
            Need(ReferenceEquals(countered, frames.Find(window.Id)) &&
                runtime.IsCounterspellIneffective((NullificationWindowFrame)countered!, counter) &&
                !runtime.IsUseIneffective(other.Id) && !runtime.IsDodgeIneffective(other.Id, counter),
                "A cancelled Nullification belongs to its exact live response node, with no cancellation of the root trick or its Dodge.");
            var next = Response(302, other.Action.ActionId, CardKind.Nullification, 2, 1);
            var planned = ((NullificationWindowFrame)countered!) with
            { ResponseRuleNode = new(2, next), NamedAmbushResponse = new(next.ActionId, 2, 0, false, 1) };
            frames.Replace(window.Id, planned);
            Reject(() => runtime.IssueCounterspell(window.Id, next));
            var advanced = planned with { ChainDepth = 2 };
            frames.Replace(window.Id, advanced);
            Need(!runtime.IsCounterspellIneffective(advanced, counter) && !runtime.IsCounterspellIneffective(advanced, next),
                "A planned or later counterspell depth cannot borrow the earlier action's cancelled state.");
            runtime.IssueCounterspell(window.Id, next);
            frames.Replace(window.Id, JsonSerializer.Deserialize<NullificationWindowFrame>(
                JsonSerializer.Serialize((NullificationWindowFrame)frames.Find(window.Id)!))!);
            Need(runtime.IsCounterspellIneffective((NullificationWindowFrame)frames.Find(window.Id)!, next),
                "The exact cancelled chain node survives JSON recovery without an Accepted or private proof history scan.");

            var protocol = capabilities.Get<INativeCounterspellResponseContinuations>();
            var cancelled = protocol.Complete(window.Id)!;
            Need(cancelled.Invalidated && cancelled.Action.ActionId == next.ActionId && !cancelled.Window.EffectNullified &&
                cancelled.Window.NamedAmbushResponse is null && protocol.Complete(window.Id) is null,
                "The real native completion consumes one cancelled node without toggling its previous result or completing it twice.");
            var prepared = protocol.Prepare(cancelled.Window, 1, 0);
            Reject(() => runtime.IssueCounterspell(window.Id, next));
            var actual = Response(303, other.Action.ActionId, CardKind.Nullification, 1, 2);
            var bound = protocol.Bind(window.Id, actual);
            Need(prepared.ChainDepth == 3 && !prepared.EffectNullified && bound.ResponseRuleNode?.Action.ActionId == actual.ActionId &&
                bound.NamedAmbushResponse?.ActionId == actual.ActionId && bound.ChainDepth == 3,
                "Prepare advances the actual depth without toggling; Bind establishes the complete native accepted node before any notification.");
            var completed = protocol.Complete(window.Id)!;
            Need(!completed.Invalidated && completed.Action.ActionId == actual.ActionId && completed.Window.EffectNullified &&
                completed.Window.NamedAmbushResponse is null && protocol.Complete(window.Id) is null,
                "An ordinary zero-material node settles through the same real native protocol and flips the result exactly once without an observer.");

            var legacy = new CardUseFrame(frames.NextId(), 0, 0, CardKind.Alcohol, [0], PhysicalCardIds: []);
            frames.Push(legacy);
            Reject(() => runtime.IssueUse(legacy.Id, null, new()));
            admission.LegacyUseId = legacy.Id;
            runtime.IssueUse(legacy.Id, null, new());
            Need(runtime.IsUseIneffective(legacy.Id) && ((CardUseFrame)frames.Find(legacy.Id)!).NativeUseInvalidation!.ActionId is null,
                "An admitted actionless native use keeps its actual null identity; merely omitting an action does not create that permission.");
        }
        finally
        {
            while (frames.OfType<ResolutionFrame>().Count > depth)
            {
                var top = frames.Top!;
                frames.Pop(top.Id, top.Kind);
            }
        }
    }

    private static CardActionContext UseAction(long id, int actor = 0, CardKind kind = CardKind.Slash,
        IReadOnlyList<int>? targets = null) => new(id, null, CardActionType.Use, actor, actor, null, null, null,
            kind, targets ?? [1], [], []);
    private static CardActionContext Response(long id, long? parent, CardKind kind, int actor, int provider) =>
        new(id, parent, CardActionType.Response, actor, provider, null, actor, 0, kind, [], [], []);
    private sealed class Admission : INativeCardUseAdmission
    {
        internal long? LegacyUseId { get; set; }
        public bool IsLegacyProducerUse(CardUseFrame use) => use.Id == LegacyUseId;
        public bool IsDeclaredUse(CardUseFrame use, CardUseDeclaredEvent declaration) => false;
    }
    private static void Reject(Action action)
    {
        try { action(); } catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("An invalid native invalidation was accepted.");
    }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
