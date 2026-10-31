#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YingboGiftHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::CardGame.Core.NativeOwners.GroupCardHandleFactory createGroupCardHandle, long ownerId) : global::CardGame.Core.GameEngine.ICardContinuationHandle
{
    public long OwnerFrameId => ownerId;
    public long ResolutionId => ownerId;
    private YingboGiftState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(ownerId).YingboGift ?? throw new InvalidOperationException("Yingbo gift lost its owner state.");
    public int SourceSeat => State.SourceSeat;
    public Card Card => getCoreGameEngineCardAttackStateRuntime().ReadCardAppearance(State.Card);
    public CardKind CardKind => State.CardKind;
    public YingboGiftContinuation Continuation => State.Continuation;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? Attack => State.AttackOwnerId is { } id ? createCardAttackHandle(id) : null;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? Group => State.GroupOwnerId is { } id ? createGroupCardHandle(id) : null;
}
