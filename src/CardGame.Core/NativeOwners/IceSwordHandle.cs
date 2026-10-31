#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class IceSwordHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private IceSwordState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).IceSword ?? throw new InvalidOperationException("The IceSword continuation lost its owner state.");

    private void Update(Func<IceSwordState, IceSwordState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { IceSword = update(state.IceSword!) });
    internal IceSwordHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, CardAttackHandle attack, int preventedDamageAmount)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { IceSword = new IceSwordState { Active = false, PreventedDamageAmount = preventedDamageAmount, DiscardedCardIds = [], } });
    }

    internal IceSwordHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public int PreventedDamageAmount { get => State.PreventedDamageAmount; }
    public bool Activated { get => State.Activated; set => Update(state => state with { Activated = value }); }
    public IReadOnlyList<int> DiscardedCardIds { get => State.DiscardedCardIds; }
    public NativeLogicalDiscardInvocationDraft? LogicalDiscardInvocation { get => State.LogicalDiscardInvocation; set => Update(state => state with { LogicalDiscardInvocation = value }); }

    public void AddDiscardedCard(int id) => Update(state => state with { DiscardedCardIds = Array.AsReadOnly(state.DiscardedCardIds.Append(id).ToArray()) });
}
