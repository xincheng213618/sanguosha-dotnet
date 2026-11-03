using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum SkillProgramInvocationSource { InitialHandGain, PublicHandReveal, AfterTurnEnded, CardAction, Movement }
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
    bool RetainOwnerDeath = false, bool UsageReserved = false)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? CandidateIndex { get; init; }
}

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
    internal static bool IsMovementWindow(SkillProgramTriggerWindow window) => window is
        SkillProgramTriggerWindow.CardsMoved or SkillProgramTriggerWindow.CardsGained;

    internal static bool IsCardActionWindow(SkillProgramTriggerWindow window) => window is
        SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseBeforeTargetEffects or
        SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardResponseAccepted or
        SkillProgramTriggerWindow.CardResponseCompleted or SkillProgramTriggerWindow.CardSupplyCompleted or
        SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.SlashTargetRedirecting or
        SkillProgramTriggerWindow.SlashBeforeResponse or SkillProgramTriggerWindow.SlashFullyDodged;

    internal static ProgramInvocationAuthorization AcceptCardAction(ResolutionFrame? parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, long programFrameId, bool retainOwnerDeath)
    {
        if (parent is not ProgramCardTriggerWindowFrame card || !card.Activated || programFrameId <= 0 ||
            !MatchesCardAction(card, candidate, context))
            throw new InvalidOperationException("An accepted card-action invocation requires its original candidate and window.");
        if (card.Candidates[card.CandidateIndex].FrozenContext is null && context.CardUse is { } source &&
            (context.SourceSeat != card.Action.ActorSeat || source.ActorSeat != card.Action.ActorSeat ||
             source.EffectiveKind != card.Action.EffectiveKind))
            throw new InvalidOperationException("An accepted card-action invocation requires its original source context.");
        return new(card.Id, card.ParentFrameId, context.Window, candidate,
            ProgramFrameId: programFrameId, RetainOwnerDeath: retainOwnerDeath) { CandidateIndex = card.CandidateIndex };
    }

    private static bool MatchesCardAction(ProgramCardTriggerWindowFrame parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!IsCardActionWindow(context.Window) || parent.Id != context.ParentFrameId ||
            context.CardUse is not { } use || use.CardActionId != parent.Action.ActionId ||
            use.ParentCardUseFrameId != parent.ParentFrameId || candidate.OwnerSeat != context.OwnerSeat ||
            candidate.OccurrenceIndex != context.OccurrenceIndex || parent.CandidateIndex < 0 ||
            parent.CandidateIndex >= parent.Candidates.Count)
            return false;
        var current = parent.Candidates[parent.CandidateIndex];
        if (RuntimeSkills.RuntimeRules_Core_CardActions.ToSharedCandidate(current) != candidate ||
            context.TargetSeat != (current.OpponentSeat >= 0 ? (int?)current.OpponentSeat : null))
            return false;
        if (current.FrozenContext is not { } frozen)
            return RuntimeSkills.RuntimeRules_Core_CardActions.GetCardActionWindow(parent) == context.Window;
        // Native role/target updates may change the parent's current Action.
        // An accepted invocation retains its candidate's original source facts.
        return frozen.Window == context.Window && frozen.SourceSeat == context.SourceSeat &&
            frozen.TargetSeat == context.TargetSeat && frozen.CardUse is { } original &&
            original.CardActionId == use.CardActionId && original.ParentCardUseFrameId == use.ParentCardUseFrameId &&
            original.ActorSeat == use.ActorSeat && original.EffectiveKind == use.EffectiveKind;
    }

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

    internal static ProgramInvocationAuthorization AcceptMovement(ResolutionFrame? parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, long programFrameId,
        bool retainOwnerDeath)
    {
        if (parent is not CardsMovedTriggerWindowFrame movement || programFrameId <= 0 ||
            !MatchesMovement(movement, candidate, context))
            throw new InvalidOperationException("An accepted movement invocation requires its original candidate and window.");
        return new(movement.Id, movement.Batch.ParentFrameId ?? 0, context.Window, candidate,
            ProgramFrameId: programFrameId, RetainOwnerDeath: retainOwnerDeath)
            { CandidateIndex = movement.CandidateIndex };
    }

    private static bool MatchesMovement(CardsMovedTriggerWindowFrame movement,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        movement.Id == context.ParentFrameId && context.MovementBatch == movement.Batch &&
        IsMovementWindow(context.Window) && candidate.OwnerSeat == context.OwnerSeat &&
        context.OccurrenceIndex == candidate.OccurrenceIndex && movement.CandidateIndex >= 0 &&
        movement.CandidateIndex < movement.Candidates.Count &&
        movement.Candidates[movement.CandidateIndex] == candidate &&
        (movement.Contexts is not { } contexts || movement.CandidateIndex < contexts.Count &&
            contexts[movement.CandidateIndex] == context);

    internal static CardsMovedTriggerWindowFrame AttachAcceptedMovement(CardsMovedTriggerWindowFrame parent,
        ProgramInvocationAuthorization accepted) => parent with
    {
        InvocationAuthorizations = (parent.InvocationAuthorizations ?? []).Where(previous =>
            previous.ProgramFrameId == 0 || previous.CandidateIndex != accepted.CandidateIndex)
            .Append(accepted).ToArray()
    };

    internal static ProgramInvocationAuthorization? Find(ResolutionFrame? parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, long? programFrameId = null)
    {
        if (parent is ProgramCardTriggerWindowFrame card)
            return programFrameId is > 0 && card.Activated && MatchesCardAction(card, candidate, context)
                ? card.InvocationAuthorizations?.SingleOrDefault(authorization => authorization.ProgramFrameId == programFrameId &&
                    authorization.WindowId == card.Id && authorization.NativeFrameId == card.ParentFrameId &&
                    authorization.Window == context.Window && authorization.Candidate == candidate &&
                    authorization.CandidateIndex == card.CandidateIndex)
                : null;
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
            if (!MatchesMovement(movement, candidate, context)) return null;
            // Accepted rights exist only for the original child. Collection and
            // admission without a child ID can see only earlier issued rights.
            if (programFrameId is > 0 && movement.InvocationAuthorizations?.SingleOrDefault(a =>
                a.ProgramFrameId == programFrameId && a.CandidateIndex == movement.CandidateIndex &&
                a.WindowId == movement.Id && a.NativeFrameId == (movement.Batch.ParentFrameId ?? 0) &&
                a.Window == context.Window && a.Candidate == candidate) is { } accepted)
                return accepted;
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
        Find(parent, authorization.Candidate, context, frame.Id) is { } original &&
        original == (original.ProgramFrameId == 0 ? authorization with { ProgramFrameId = 0 } : authorization);
}
