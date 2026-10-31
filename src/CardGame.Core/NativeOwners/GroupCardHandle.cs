#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GroupCardHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly Func<RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries> getCoreGameEngineChainedStateBasicReturnsRuntime;
    private readonly Func<RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime;
    private readonly Func<RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private GroupCardState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).GroupCard ?? throw new InvalidOperationException("The GroupCard continuation lost its owner state.");

    private void Update(Func<GroupCardState, GroupCardState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { GroupCard = update(state.GroupCard!) });
    internal GroupCardHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries> getCoreGameEngineChainedStateBasicReturnsRuntime, Func<RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime, Func<RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime, CardAttackHandleFactory createCardAttackHandle, long resolutionId, int sourceSeat, Card card, IReadOnlyList<int> targetSeats, GroupCardEffect effect, CardKind? requiredCardKind, IReadOnlyList<Card>? physicalCards = null)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineChainedStateBasicReturnsRuntime = getCoreGameEngineChainedStateBasicReturnsRuntime;
        this.getCoreGameEngineVirtualOrdinaryTrickUsesRuntime = getCoreGameEngineVirtualOrdinaryTrickUsesRuntime;
        this.getCoreNativeNullificationFramesRuntime = getCoreNativeNullificationFramesRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = resolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { GroupCard = new GroupCardState { Active = false, SourceSeat = sourceSeat, DamageSourceSeat = sourceSeat, Card = new(card.Id, card.Kind, card.Suit, card.Rank), TargetSeats = Array.AsReadOnly(targetSeats.ToArray()), Effect = effect, RequiredCardKind = requiredCardKind, PhysicalCardIds = Array.AsReadOnly((physicalCards ?? [card]).Select(card => card.Id).ToArray()), DamageClaimedPhysicalCardIds = [], RevealedCardIds = [], } });
    }

    internal GroupCardHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries> getCoreGameEngineChainedStateBasicReturnsRuntime, Func<RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime, Func<RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineChainedStateBasicReturnsRuntime = getCoreGameEngineChainedStateBasicReturnsRuntime;
        this.getCoreGameEngineVirtualOrdinaryTrickUsesRuntime = getCoreGameEngineVirtualOrdinaryTrickUsesRuntime;
        this.getCoreNativeNullificationFramesRuntime = getCoreNativeNullificationFramesRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public long ResolutionId => OwnerFrameId;
    public int SourceSeat { get => State.SourceSeat; }
    public int DamageSourceSeat { get => State.DamageSourceSeat; set => Update(state => state with { DamageSourceSeat = value }); }
    public Card Card { get => (getCoreGameEngineChainedStateBasicReturnsRuntime().IsChainedStateBasicUse(OwnerFrameId) ? getCoreGameEngineChainedStateBasicReturnsRuntime().ReadChainedStateBasicUseAppearance(OwnerFrameId, State.Card) : getCoreGameEngineVirtualOrdinaryTrickUsesRuntime().ReadVirtualOrdinaryTrickUseAppearance(OwnerFrameId, State.Card)); }
    public IReadOnlyList<int> TargetSeats { get => State.TargetSeats; }
    public GroupCardEffect Effect { get => State.Effect; }
    public CardKind? RequiredCardKind { get => State.RequiredCardKind; }
    public IReadOnlyList<Card> PhysicalCards { get => State.PhysicalCardIds.Select(getCoreGameEngineCardAttackStateRuntime().GetAttackCard).ToArray(); }
    public int TargetIndex { get => getCoreGameEngineCardAttackStateRuntime().GetCardUseTargetIndex(OwnerFrameId); set => getCoreNativeNullificationFramesRuntime().SetCardUseTargetIndex(OwnerFrameId, value); }
    public CardAttackHandle? CurrentAttack { get => State.CurrentAttackOwnerId is { } id ? createCardAttackHandle(id) : null; set => Update(state => state with { CurrentAttackOwnerId = value?.ResolutionId }); }
    public IReadOnlyList<int> DamageClaimedPhysicalCardIds { get => State.DamageClaimedPhysicalCardIds; }
    public IReadOnlyList<int> RevealedCardIds { get => State.RevealedCardIds; }
    public int? NullificationCompletedTargetIndex { get => State.NullificationCompletedTargetIndex; set => Update(state => state with { NullificationCompletedTargetIndex = value }); }

    public void AddRevealedCard(int id) => Update(state => state with { RevealedCardIds = Array.AsReadOnly(state.RevealedCardIds.Append(id).ToArray()) });
    public void RemoveRevealedCard(int id) => Update(state => state with { RevealedCardIds = Array.AsReadOnly(state.RevealedCardIds.Where(item => item != id).ToArray()) });
    public void ClearRevealedCards() => Update(state => state with { RevealedCardIds = [] });
    public void SetClaimedPhysicalCards(IReadOnlyList<int> ids) => Update(state => state with { DamageClaimedPhysicalCardIds = Array.AsReadOnly(ids.ToArray()) });
}
