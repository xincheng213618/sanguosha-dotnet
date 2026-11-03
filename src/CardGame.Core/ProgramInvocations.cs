using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum SkillProgramInvocationSource { InitialHandGain, PublicHandReveal, AfterTurnEnded }
public enum SkillProgramInvocationOwnerDeath { Stop, Retain }
public sealed record SkillProgramTriggerInvocation(SkillProgramInvocationSource Source,
    SkillProgramInvocationOwnerDeath OwnerDeath = SkillProgramInvocationOwnerDeath.Stop);

public sealed partial class SkillProgramTrigger
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SkillProgramTriggerInvocation? Invocation { get; internal init; }
}

/// <summary>Permission for one admitted binding in its original native window; never skill ownership.</summary>
public sealed record ProgramInvocationAuthorization(long WindowId, long NativeFrameId,
    SkillProgramTriggerWindow Window, ProgramTriggerCandidate Candidate, long ProgramFrameId = 0,
    bool RetainOwnerDeath = false, bool UsageReserved = false);

public abstract partial record ResolutionFrame
{
    private IReadOnlyList<ProgramInvocationAuthorization>? _invocationAuthorizations;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ProgramInvocationAuthorization>? InvocationAuthorizations
    {
        get => _invocationAuthorizations;
        init => _invocationAuthorizations = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramInvocationAuthorization? InvocationAuthorization { get; init; }
}

internal static class ProgramInvocations
{
    internal static ProgramInvocationAuthorization AcceptAfterTurnEnded(ResolutionFrame? parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, long programFrameId,
        bool retainOwnerDeath)
    {
        if (parent is not DeferredTurnEndFrame ending ||
            !MatchesAfterTurnEnded(ending, candidate, context) ||
            ending.AfterTurnEnded!.CurrentChild!.FrameId != programFrameId)
            throw new InvalidOperationException("An accepted ending invocation requires its original candidate and child.");
        return new(ending.Id, ending.Id, SkillProgramTriggerWindow.AfterTurnEnded, candidate,
            RetainOwnerDeath: retainOwnerDeath);
    }

    private static bool MatchesAfterTurnEnded(DeferredTurnEndFrame parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        parent.Id == context.ParentFrameId && context.Window == SkillProgramTriggerWindow.AfterTurnEnded &&
        parent.AfterTurnEnded is { CurrentChild: { } child } window && child.Candidate == candidate &&
        window.ItemIndex >= 0 && window.ItemIndex < window.Items.Count &&
        window.Items[window.ItemIndex] is { Kind: TurnEndingBoundaryItemKind.Program } item &&
        item.Candidate == candidate && item.Facts == context.Facts &&
        candidate.OwnerSeat == context.OwnerSeat && candidate.OccurrenceIndex == context.OccurrenceIndex &&
        context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat;

    internal static ProgramInvocationAuthorization? Find(ResolutionFrame? parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (parent is DeferredTurnEndFrame ending)
            return MatchesAfterTurnEnded(ending, candidate, context)
                ? ending.InvocationAuthorizations?.SingleOrDefault(authorization => authorization.ProgramFrameId == 0 &&
                    authorization.WindowId == ending.Id && authorization.NativeFrameId == ending.Id &&
                    authorization.Window == context.Window && authorization.Candidate == candidate)
                : null;
        if (parent is PublicHandRevealWindowFrame reveal)
        {
            if (reveal.Id != context.ParentFrameId || context.Window != SkillProgramTriggerWindow.PublicHandRevealed ||
                reveal.ClaimIndex < 0 || reveal.ClaimIndex >= reveal.Claims.Count ||
                context.PublicHandReveal != reveal.Claims[reveal.ClaimIndex] ||
                candidate.OwnerSeat != context.OwnerSeat || candidate.OccurrenceIndex != context.OccurrenceIndex ||
                candidate != PublicHandRevealWindowRuntime.Candidate(reveal.Claims[reveal.ClaimIndex], reveal.ClaimIndex))
                return null;
            return reveal.InvocationAuthorizations?.SingleOrDefault(a => a.ProgramFrameId == 0 &&
                a.WindowId == reveal.Id && a.NativeFrameId == reveal.ParentFrameId &&
                a.Window == context.Window && a.Candidate == candidate);
        }
        if (parent is CardsMovedTriggerWindowFrame movement)
        {
            if (movement.Id != context.ParentFrameId || context.MovementBatch != movement.Batch ||
                context.Window is not (SkillProgramTriggerWindow.CardsMoved or SkillProgramTriggerWindow.CardsGained) ||
                candidate.OwnerSeat != context.OwnerSeat || context.OccurrenceIndex != candidate.OccurrenceIndex ||
                movement.CandidateIndex < 0 || movement.CandidateIndex >= movement.Candidates.Count ||
                movement.Candidates[movement.CandidateIndex] != candidate ||
                movement.Contexts is { } contexts && (movement.CandidateIndex >= contexts.Count ||
                    contexts[movement.CandidateIndex] != context)) return null;
            return movement.InvocationAuthorizations?.SingleOrDefault(a => a.ProgramFrameId == 0 &&
                a.WindowId == movement.Id && a.NativeFrameId == (movement.Batch.ParentFrameId ?? 0) &&
                a.Window == context.Window && a.Candidate == candidate);
        }
        if (parent is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId ||
            window.ParentFrameId != context.DamageFrameId || window.TriggerWindow != context.Window ||
            candidate.OwnerSeat != context.OwnerSeat || context.OccurrenceIndex != candidate.OccurrenceIndex ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            window.Candidates[window.CandidateIndex].ToProgramCandidate() != candidate)
            return null;
        return window.InvocationAuthorizations?.SingleOrDefault(authorization => authorization.ProgramFrameId == 0 &&
            authorization.WindowId == window.Id && authorization.NativeFrameId == window.ParentFrameId &&
            authorization.Window == context.Window && authorization.Candidate == candidate);
    }

    internal static bool Allows(ProgramSkillFrame frame, ResolutionFrame? parent) =>
        frame.InvocationAuthorization is { } authorization && authorization.ProgramFrameId == frame.Id &&
        frame.OwnerSeat == authorization.Candidate.OwnerSeat && frame.SkillId == authorization.Candidate.SkillId &&
        frame.TriggerId == authorization.Candidate.BindingId && frame.ActivationId == authorization.Candidate.BindingId &&
        frame.SkillInstanceId == authorization.Candidate.SkillInstanceId &&
        frame.GameplayHash == authorization.Candidate.GameplayHash && frame.WindowContext is { } context &&
        (parent is not DeferredTurnEndFrame ending || ending.AfterTurnEnded?.CurrentChild?.FrameId == frame.Id) &&
        Find(parent, authorization.Candidate, context) == (authorization with { ProgramFrameId = 0 });
}
