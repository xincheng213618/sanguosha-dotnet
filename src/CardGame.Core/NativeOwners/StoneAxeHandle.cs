#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class StoneAxeHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private StoneAxeState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).StoneAxe ?? throw new InvalidOperationException("The StoneAxe continuation lost its owner state.");

    private void Update(Func<StoneAxeState, StoneAxeState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { StoneAxe = update(state.StoneAxe!) });
    internal StoneAxeHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, CardAttackHandle attack, IReadOnlyList<int> candidateCardIds)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { StoneAxe = new StoneAxeState { Active = false, CandidateCardIds = Array.AsReadOnly(candidateCardIds.ToArray()) } });
    }

    internal StoneAxeHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public IReadOnlyList<int> CandidateCardIds => State.CandidateCardIds;
}
