namespace CardGame.Core;

// 狷狭 evidence: one scalar record per virtually used trick, the committed debt
// that promises the retaliation, and the retaliation settlement that closes each
// launch. The outstanding debt is derived from committed events only, so cold
// recovery replays the accepted commands into the same state.
public sealed record ProgramJuanxiaTrickUsedEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int TargetSeat, string TrickKind, int CardId, bool Paid) : IGameEvent;
public sealed record ProgramJuanxiaDebtEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int TargetSeat, int TrickCount) : IGameEvent;
public sealed record ProgramJuanxiaRetaliationEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int DebtorSeat, long LaunchFrameId, int OfferedCount, int UsedCount) : IGameEvent;

// 狷狭 pending state rides the owning program frame, exactly like BijingPunish.
public enum JuanxiaStage { Choice, Card, Fire, Child, Retaliate, Slash }
public sealed record JuanxiaLaunchState(int TargetSeat, int UsedCount,
    IReadOnlyList<string> UsedKinds, JuanxiaStage Stage, string? PendingKind, int RevealedCardId = 0);
public sealed record JuanxiaRetaliationState(int DebtorSeat, int Remaining,
    IReadOnlyList<long> LaunchFrameIds, IReadOnlyList<int> LaunchCounts, JuanxiaStage Stage);

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public JuanxiaLaunchState? JuanxiaLaunch { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public JuanxiaRetaliationState? JuanxiaRetaliate { get; init; }
}
