using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum SkillStateAction { Suppress, Lose, Transfer }
public enum SkillStateBoundaryEvent { TurnCleanup, TurnStarted, TurnEnded, RoundAdvanced, PhaseStarted, PhaseEnded, Death, DyingStarted, DyingEnded, ActualUse, ActualTurnReturned }
public enum SkillStateBoundaryTiming { Before, After }
public enum SkillStateSubject { Owner, Target, TurnOwner, Any }

public sealed record SkillStateBoundarySpec(SkillStateBoundaryEvent Event, SkillStateSubject? Participant,
    SkillStateBoundaryTiming Timing = SkillStateBoundaryTiming.After, bool SkipIssued = false,
    ActualDiscardRecoveryPhaseKind? Phase = null);

public sealed class SkillStateSpec
{
    public SkillStateSpec(SkillStateAction action, SkillStateMatch match,
        IReadOnlyList<SkillStateBoundarySpec>? until = null, ProgramExpression? @while = null, bool chooseOne = false)
    {
        if (action == SkillStateAction.Transfer && @while is not null)
            throw new ArgumentException("Conditional borrowing is not supported by skillState.", nameof(@while));
        Action = action; Match = match; Until = Array.AsReadOnly((until ?? []).ToArray());
        While = @while; ChooseOne = chooseOne;
    }
    public SkillStateAction Action { get; }
    public SkillStateMatch Match { get; }
    public IReadOnlyList<SkillStateBoundarySpec> Until { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramExpression? While { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ChooseOne { get; }

    internal bool SuppressesPrintedEquipmentEffects { get; private init; }

    internal static SkillStateSpec SuppressEquipment(IReadOnlyList<SkillStateBoundarySpec> until) =>
        new(SkillStateAction.Suppress, new(filter: new("not", ProgramExpressionType.Bool, [SkillStateFilters.Character])), until)
        {
            SuppressesPrintedEquipmentEffects = true
        };
}

public sealed record SkillStateBoundaryEnd(SkillStateBoundaryEvent Event, int? Seat,
    SkillStateBoundaryTiming Timing, long IssuedOccurrence, bool SkipIssued,
    ActualDiscardRecoveryPhaseKind? Phase = null);
public sealed record SkillStateBoundary(SkillStateBoundaryEvent Event, int? Seat,
    SkillStateBoundaryTiming Timing, long Occurrence, ActualDiscardRecoveryPhaseKind? Phase = null);

public sealed record SkillStateOrigin(long FrameId, int EffectIndex, CardUseEffectSource Source,
    long? CardUseFrameId = null, long? CardActionId = null)
{
    public int IssuedTurn { get; init; }
}

internal static class ProgramRuleLifetimeBoundaries
{
    internal static bool Matches(SkillStateBoundaryEnd end, SkillStateBoundary boundary) =>
        end.Event == boundary.Event &&
        (end.Seat == boundary.Seat || end.Seat is null && end.Event is SkillStateBoundaryEvent.TurnStarted or SkillStateBoundaryEvent.TurnEnded) &&
        end.Timing == boundary.Timing && end.Phase == boundary.Phase &&
        (end.Event == SkillStateBoundaryEvent.DyingEnded && !end.SkipIssued
            ? boundary.Occurrence == end.IssuedOccurrence
            : end.SkipIssued ? boundary.Occurrence > end.IssuedOccurrence : boundary.Occurrence >= end.IssuedOccurrence);
}

public sealed class SkillStateLifetime(SkillStateOrigin? origin, IReadOnlyList<SkillStateBoundaryEnd> until)
{
    public SkillStateOrigin? Origin { get; } = origin;
    public IReadOnlyList<SkillStateBoundaryEnd> Until { get; } = Array.AsReadOnly(until.ToArray());
}

internal interface IProgramRuleLifetimes : IProgramSelectionQuery
{
    SkillStateLifetime? CreateApplicationLifetime(int owner, int target,
        IReadOnlyList<SkillStateBoundarySpec> until, SkillStateConditionContext context);
}

internal interface IProgramRuleBoundaries : IProgramRuntimeOperations
{
    void ProcessSkillStateBoundary(SkillStateBoundary boundary);
}

public sealed record SkillStateConditionContext(int OwnerSeat, int TargetSeat, ActualDiscardRecoveryPhaseKey? IssuedPhase = null,
    int? IssuedTurnNumber = null, long? IssuedDyingFrameId = null, int? EventTargetSeat = null);
public sealed record SkillStateDeclarationSource(string SkillId, string GrantId, int DeclarationIndex,
    SkillStateDeclaration Declaration);
public sealed record SkillStateRestriction(string Id, string SourceId, SkillStateAction Action,
    SkillStateMatch Match, SkillStateConditionContext Context, SkillStateLifetime Lifetime,
    ProgramExpression? While = null, SkillStateDeclarationSource? DeclarationSource = null)
{
    internal bool SuppressesPrintedEquipmentEffects { get; init; }
}

public sealed partial record SkillGrant
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SkillStateLifetime? Lifetime { get; init; }
}

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SkillStateSpec? SkillState { get; internal init; }
}

public sealed record ProgramSkillStateOption(string SkillId, IReadOnlyList<string> GrantIds);
public sealed record ProgramSkillStateChoice(int Seat, IReadOnlyList<ProgramSkillStateOption> Options);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramSkillStateChoice? SkillStateChoice { get; init; }
}

public sealed record SkillStateChangedEvent(int Seat, int SourceSeat, string SkillId,
    SkillStateAction Action, string SourceId) : IGameEvent
{
    public SkillStateOrigin? Origin { get; init; }
    public int IssuedTurn { get; init; }
}
public sealed record SkillStateExpiredEvent(int Seat, string SourceId, SkillStateOrigin? Origin) : IGameEvent;

internal interface ISkillStateProgramHost
{
    SkillProgramStepOutcome ChangeSkillState(ProgramSkillFrame frame, SkillProgramEffect effect, int targetSeat);
}
