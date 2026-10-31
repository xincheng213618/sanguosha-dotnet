#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QilinBowHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private QilinBowState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).QilinBow ?? throw new InvalidOperationException("The QilinBow continuation lost its owner state.");

    private void Update(Func<QilinBowState, QilinBowState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { QilinBow = update(state.QilinBow!) });
    internal QilinBowHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, CardAttackHandle attack)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { QilinBow = new QilinBowState { Active = false, } });
    }

    internal QilinBowHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
}
