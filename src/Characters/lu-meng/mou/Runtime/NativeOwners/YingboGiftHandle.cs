#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YingboGiftHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, GroupCardHandleFactory createGroupCardHandle, long ownerId) : GameEngine.ICardContinuationHandle
{
    public long OwnerFrameId => ownerId;
    public long ResolutionId => ownerId;
    private YingboGiftState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(ownerId).YingboGift ?? throw new InvalidOperationException("Yingbo gift lost its owner state.");
    public int SourceSeat => State.SourceSeat;
    public Card Card => getCoreGameEngineCardAttackStateRuntime().ReadCardAppearance(State.Card);
    public CardKind CardKind => State.CardKind;
    public YingboGiftContinuation Continuation => State.Continuation;
    public CardAttackHandle? Attack => State.AttackOwnerId is { } id ? createCardAttackHandle(id) : null;
    public GroupCardHandle? Group => State.GroupOwnerId is { } id ? createGroupCardHandle(id) : null;
}
