#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CixiongDoubleSwordsHandle : global::CardGame.Core.GameEngine.ICardContinuationHandle
{
    private readonly global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private CixiongDoubleSwordsState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).CixiongDoubleSwords ?? throw new InvalidOperationException("The CixiongDoubleSwords continuation lost its owner state.");

    private void Update(Func<CixiongDoubleSwordsState, CixiongDoubleSwordsState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { CixiongDoubleSwords = update(state.CixiongDoubleSwords!) });
    internal CixiongDoubleSwordsHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { CixiongDoubleSwords = new CixiongDoubleSwordsState { Active = false, } });
    }

    internal CixiongDoubleSwordsHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public global::CardGame.Core.NativeOwners.CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public CixiongDoubleSwordsStage Stage { get => State.Stage; set => Update(state => state with { Stage = value }); }
}
