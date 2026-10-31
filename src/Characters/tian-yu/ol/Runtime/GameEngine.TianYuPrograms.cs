namespace CardGame.Core;

// 扫狄 evidence: one scalar record per added target. The owning use and action
// are already public (their acceptance is committed before the finalized-target
// window), so the record only names the seat this skill appended.
public sealed record ProgramSaodiTargetsAddedEvent(long FrameId, string SkillId, string BindingId,
    string SkillInstanceId, int OwnerSeat, long CardUseFrameId, long CardActionId, int AddedSeat) : IGameEvent;

// Shared directed distance reduction: the grant/revoke pair records one owner
// holding a distance reduction against one target. Any content skill can grant
// through the 追讨-shaped ops; EvaluateDistance applies active grants as additive
// outgoing-distance contributions, so the state is derived from committed
// history on cold recovery and no runtime map survives a checkpoint.
public sealed record ProgramDirectedDistanceGrantedEvent(long FrameId, string SkillId, string BindingId,
    string SkillInstanceId, int OwnerSeat, int TargetSeat, int Amount) : IGameEvent;

public sealed record ProgramDirectedDistanceRevokedEvent(long FrameId, string SkillId, string BindingId,
    string SkillInstanceId, int OwnerSeat, int TargetSeat) : IGameEvent;
