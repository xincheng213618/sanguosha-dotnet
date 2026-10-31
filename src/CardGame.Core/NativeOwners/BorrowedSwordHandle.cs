#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BorrowedSwordHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly Func<RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries> getCoreGameEngineChainedStateBasicReturnsRuntime;
    private readonly Func<RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private BorrowedSwordState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).BorrowedSword ?? throw new InvalidOperationException("The BorrowedSword continuation lost its owner state.");

    private void Update(Func<BorrowedSwordState, BorrowedSwordState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { BorrowedSword = update(state.BorrowedSword!) });
    internal BorrowedSwordHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries> getCoreGameEngineChainedStateBasicReturnsRuntime, Func<RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime, CardAttackHandleFactory createCardAttackHandle, long resolutionId, int sourceSeat, int weaponOwnerSeat, int slashTargetSeat, Card card)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineChainedStateBasicReturnsRuntime = getCoreGameEngineChainedStateBasicReturnsRuntime;
        this.getCoreGameEngineVirtualOrdinaryTrickUsesRuntime = getCoreGameEngineVirtualOrdinaryTrickUsesRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = resolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { BorrowedSword = new BorrowedSwordState { Active = false, SourceSeat = sourceSeat, WeaponOwnerSeat = weaponOwnerSeat, SlashTargetSeat = slashTargetSeat, Card = new(card.Id, card.Kind, card.Suit, card.Rank), } });
    }

    internal BorrowedSwordHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries> getCoreGameEngineChainedStateBasicReturnsRuntime, Func<RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineChainedStateBasicReturnsRuntime = getCoreGameEngineChainedStateBasicReturnsRuntime;
        this.getCoreGameEngineVirtualOrdinaryTrickUsesRuntime = getCoreGameEngineVirtualOrdinaryTrickUsesRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public long ResolutionId => OwnerFrameId;
    public int SourceSeat { get => State.SourceSeat; }
    public int WeaponOwnerSeat { get => State.WeaponOwnerSeat; }
    public int SlashTargetSeat { get => State.SlashTargetSeat; }
    public Card Card { get => (getCoreGameEngineChainedStateBasicReturnsRuntime().IsChainedStateBasicUse(OwnerFrameId) ? getCoreGameEngineChainedStateBasicReturnsRuntime().ReadChainedStateBasicUseAppearance(OwnerFrameId, State.Card) : getCoreGameEngineVirtualOrdinaryTrickUsesRuntime().ReadVirtualOrdinaryTrickUseAppearance(OwnerFrameId, State.Card)); }
    public bool AwaitingSlashChoice { get => State.AwaitingSlashChoice; set => Update(state => state with { AwaitingSlashChoice = value }); }
    public bool FactionSlashAttempted { get => State.FactionSlashAttempted; set => Update(state => state with { FactionSlashAttempted = value }); }
    public CardAttackHandle? ActiveAttack { get => State.ActiveAttackOwnerId is { } id ? createCardAttackHandle(id) : null; set => Update(state => state with { ActiveAttackOwnerId = value?.ResolutionId }); }
    public int? SlashCardId { get => State.SlashCardId; set => Update(state => state with { SlashCardId = value }); }
    public CardKind? EffectiveSlashKind { get => State.EffectiveSlashKind; set => Update(state => state with { EffectiveSlashKind = value }); }
}
