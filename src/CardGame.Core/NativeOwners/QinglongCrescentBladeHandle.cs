#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QinglongCrescentBladeHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private QinglongCrescentBladeState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).QinglongCrescentBlade ?? throw new InvalidOperationException("The QinglongCrescentBlade continuation lost its owner state.");

    private void Update(Func<QinglongCrescentBladeState, QinglongCrescentBladeState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { QinglongCrescentBlade = update(state.QinglongCrescentBlade!) });
    internal QinglongCrescentBladeHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, CardAttackHandle attack)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { QinglongCrescentBlade = new QinglongCrescentBladeState { Active = false, } });
    }

    internal QinglongCrescentBladeHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public bool FactionSlashAttempted { get => State.FactionSlashAttempted; set => Update(state => state with { FactionSlashAttempted = value }); }
}
