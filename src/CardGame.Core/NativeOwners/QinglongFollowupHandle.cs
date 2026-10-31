#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QinglongFollowupHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::CardGame.Core.NativeOwners.FangtianHalberdHandleFactory createFangtianHalberdHandle, long ownerId) : global::CardGame.Core.GameEngine.ICardContinuationHandle
{
    public long OwnerFrameId => ownerId;
    private QinglongFollowupState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(ownerId).QinglongFollowup ?? throw new InvalidOperationException("Qinglong lost its suspended continuation.");
    public long OuterResolutionId => ownerId;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? NextAttack => State.NextAttackOwnerId is { } id ? createCardAttackHandle(id) : null;
    public global::CardGame.Core.NativeOwners.FangtianHalberdHandle? FangtianContinuation => State.FangtianOwnerId is { } id ? createFangtianHalberdHandle(id) : null;
    public PendingDecision? NextDecision => State.Decision is { } decision ? global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime.RestoreSuspendedDecision(decision) : null;
    public EngineStatus Status => State.Status;
}
