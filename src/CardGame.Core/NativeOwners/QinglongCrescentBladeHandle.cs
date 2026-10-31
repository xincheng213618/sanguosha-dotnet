#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QinglongCrescentBladeHandle : global::CardGame.Core.GameEngine.ICardContinuationHandle
{
    private readonly global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private QinglongCrescentBladeState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).QinglongCrescentBlade ?? throw new InvalidOperationException("The QinglongCrescentBlade continuation lost its owner state.");

    private void Update(Func<QinglongCrescentBladeState, QinglongCrescentBladeState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { QinglongCrescentBlade = update(state.QinglongCrescentBlade!) });
    internal QinglongCrescentBladeHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { QinglongCrescentBlade = new QinglongCrescentBladeState { Active = false, } });
    }

    internal QinglongCrescentBladeHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public global::CardGame.Core.NativeOwners.CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public bool FactionSlashAttempted { get => State.FactionSlashAttempted; set => Update(state => state with { FactionSlashAttempted = value }); }
}
