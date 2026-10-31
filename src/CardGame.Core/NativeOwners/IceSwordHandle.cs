#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class IceSwordHandle : global::CardGame.Core.GameEngine.ICardContinuationHandle
{
    private readonly global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private IceSwordState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).IceSword ?? throw new InvalidOperationException("The IceSword continuation lost its owner state.");

    private void Update(Func<IceSwordState, IceSwordState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { IceSword = update(state.IceSword!) });
    internal IceSwordHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int preventedDamageAmount)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { IceSword = new IceSwordState { Active = false, PreventedDamageAmount = preventedDamageAmount, DiscardedCardIds = [], } });
    }

    internal IceSwordHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public global::CardGame.Core.NativeOwners.CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public int PreventedDamageAmount { get => State.PreventedDamageAmount; }
    public bool Activated { get => State.Activated; set => Update(state => state with { Activated = value }); }
    public IReadOnlyList<int> DiscardedCardIds { get => State.DiscardedCardIds; }
    public NativeLogicalDiscardInvocationDraft? LogicalDiscardInvocation { get => State.LogicalDiscardInvocation; set => Update(state => state with { LogicalDiscardInvocation = value }); }

    public void AddDiscardedCard(int id) => Update(state => state with { DiscardedCardIds = Array.AsReadOnly(state.DiscardedCardIds.Append(id).ToArray()) });
}
