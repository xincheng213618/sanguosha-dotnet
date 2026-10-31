#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CixiongDoubleSwordsHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private CixiongDoubleSwordsState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).CixiongDoubleSwords ?? throw new InvalidOperationException("The CixiongDoubleSwords continuation lost its owner state.");

    private void Update(Func<CixiongDoubleSwordsState, CixiongDoubleSwordsState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { CixiongDoubleSwords = update(state.CixiongDoubleSwords!) });
    internal CixiongDoubleSwordsHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, CardAttackHandle attack)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { CixiongDoubleSwords = new CixiongDoubleSwordsState { Active = false, } });
    }

    internal CixiongDoubleSwordsHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public CixiongDoubleSwordsStage Stage { get => State.Stage; set => Update(state => state with { Stage = value }); }
}
