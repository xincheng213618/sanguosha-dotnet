namespace CardGame.Core;

// Scheduler provenance is issued once on the child frame. It is reconstructed
// by command replay and never supplied by a command or a serialized snapshot.
internal sealed record RuntimeFrameOrigin(long? ParentId, ResolutionFrameKind? ParentKind,
    ProgramFrameOrigin? Program)
{
    internal static RuntimeFrameOrigin Capture(ResolutionFrame? parent, ResolutionFrame child) =>
        new(parent?.Id, parent?.Kind, child is ProgramSkillFrame program ? ProgramFrameOrigin.Capture(program) : null);

    internal bool Matches(ResolutionFrame frame) => frame is ProgramSkillFrame program
        ? Program is { } origin && origin.Matches(program) : Program is null;
}

internal sealed record ProgramFrameOrigin(int OwnerSeat, string SkillId, string BindingId,
    string GameplayHash, string SkillInstanceId, string? TriggerId, long? ParentFrameId)
{
    internal static ProgramFrameOrigin Capture(ProgramSkillFrame frame) => new(frame.OwnerSeat,
        frame.SkillId, frame.ActivationId, frame.GameplayHash, frame.SkillInstanceId, frame.TriggerId, frame.ParentFrameId);

    internal bool Matches(ProgramSkillFrame frame) => OwnerSeat == frame.OwnerSeat && SkillId == frame.SkillId &&
        BindingId == frame.ActivationId && GameplayHash == frame.GameplayHash && SkillInstanceId == frame.SkillInstanceId &&
        TriggerId == frame.TriggerId && ParentFrameId == frame.ParentFrameId;
}

public abstract partial record ResolutionFrame
{
    // Internal and write-once by FrameStore. Record copies retain this receipt.
    [System.Text.Json.Serialization.JsonIgnore]
    internal RuntimeFrameOrigin? RuntimeOrigin { get; set; }
}

public sealed record ProgramInsertionReturn(long ScopeRootFrameId, long? SuspendedDyingFrameId);

public sealed partial record ProgramAttackReturn
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public ProgramInsertionReturn? Insertion { get; init; }
}
