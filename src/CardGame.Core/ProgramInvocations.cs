using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>Permission for one admitted binding in its original native window; never skill ownership.</summary>
public sealed record ProgramInvocationAuthorization(long WindowId, long NativeFrameId,
    SkillProgramTriggerWindow Window, ProgramTriggerCandidate Candidate, long ProgramFrameId = 0);

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
    internal static ProgramInvocationAuthorization? Find(ResolutionFrame? parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (parent is CardsMovedTriggerWindowFrame movement)
        {
            if (movement.Id != context.ParentFrameId || context.MovementBatch?.Id != movement.Batch.Id ||
                context.Window != SkillProgramTriggerWindow.CardsMoved || candidate.OwnerSeat != context.OwnerSeat ||
                movement.CandidateIndex < 0 || movement.CandidateIndex >= movement.Candidates.Count ||
                movement.Candidates[movement.CandidateIndex] != candidate) return null;
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
        frame.TriggerId == authorization.Candidate.BindingId && frame.SkillInstanceId == authorization.Candidate.SkillInstanceId &&
        frame.GameplayHash == authorization.Candidate.GameplayHash && frame.WindowContext is { } context &&
        Find(parent, authorization.Candidate, context) == (authorization with { ProgramFrameId = 0 });
}
