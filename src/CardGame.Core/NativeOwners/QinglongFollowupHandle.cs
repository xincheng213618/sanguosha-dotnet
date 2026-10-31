#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QinglongFollowupHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, FangtianHalberdHandleFactory createFangtianHalberdHandle, long ownerId) : GameEngine.ICardContinuationHandle
{
    public long OwnerFrameId => ownerId;
    private QinglongFollowupState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(ownerId).QinglongFollowup ?? throw new InvalidOperationException("Qinglong lost its suspended continuation.");
    public long OuterResolutionId => ownerId;
    public CardAttackHandle? NextAttack => State.NextAttackOwnerId is { } id ? createCardAttackHandle(id) : null;
    public FangtianHalberdHandle? FangtianContinuation => State.FangtianOwnerId is { } id ? createFangtianHalberdHandle(id) : null;
    public PendingDecision? NextDecision => State.Decision is { } decision ? EngineOwners.CoreGameEngineCardAttackStateRuntime.RestoreSuspendedDecision(decision) : null;
    public EngineStatus Status => State.Status;
}
