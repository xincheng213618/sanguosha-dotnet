#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FangtianHalberdHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly Func<RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private FangtianHalberdState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).FangtianHalberd ?? throw new InvalidOperationException("The FangtianHalberd continuation lost its owner state.");

    private void Update(Func<FangtianHalberdState, FangtianHalberdState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { FangtianHalberd = update(state.FangtianHalberd!) });
    internal FangtianHalberdHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime, CardAttackHandleFactory createCardAttackHandle, long resolutionId, int sourceSeat, Card card, CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardConversionSource? conversionSource = null)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreNativeNullificationFramesRuntime = getCoreNativeNullificationFramesRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = resolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { FangtianHalberd = new FangtianHalberdState { Active = false, SourceSeat = sourceSeat, Card = new(card.Id, card.Kind, card.Suit, card.Rank), EffectiveCardKind = effectiveCardKind, IgnoresArmor = ignoresArmor, DamageAmount = damageAmount, UsesFangtian = usesFangtian, CountedTowardSlashLimit = countedTowardSlashLimit, ConversionSource = conversionSource, TargetSeats = Array.AsReadOnly(targetSeats.ToArray()), } });
    }

    internal FangtianHalberdHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreNativeNullificationFramesRuntime = getCoreNativeNullificationFramesRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public long ResolutionId => OwnerFrameId;
    public int SourceSeat { get => State.SourceSeat; }
    public Card Card { get => getCoreGameEngineCardAttackStateRuntime().ReadCardAppearance(State.Card); }
    public CardKind EffectiveCardKind { get => State.EffectiveCardKind; }
    public bool IgnoresArmor { get => State.IgnoresArmor; }
    public int DamageAmount { get => State.DamageAmount; }
    public bool UsesFangtian { get => State.UsesFangtian; }
    public bool CountedTowardSlashLimit { get => State.CountedTowardSlashLimit; }
    public CardConversionSource? ConversionSource { get => State.ConversionSource; }
    public bool DamageWasApplied { get => getCoreGameEngineCardAttackStateRuntime().GetCardUseCausedDamage(OwnerFrameId); set => getCoreGameEngineCardAttackStateRuntime().SetCardUseCausedDamage(OwnerFrameId, value); }
    public IReadOnlyList<int> TargetSeats { get => State.TargetSeats; }
    public int TargetIndex { get => getCoreGameEngineCardAttackStateRuntime().GetCardUseTargetIndex(OwnerFrameId); set => getCoreNativeNullificationFramesRuntime().SetCardUseTargetIndex(OwnerFrameId, value); }
    public CardAttackHandle? CurrentAttack { get => State.CurrentAttackOwnerId is { } id ? createCardAttackHandle(id) : null; set => Update(state => state with { CurrentAttackOwnerId = value?.ResolutionId }); }
}
