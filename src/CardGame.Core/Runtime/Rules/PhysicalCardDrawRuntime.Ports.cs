namespace CardGame.Core;

internal interface IPhysicalCardDrawOperations : IProgramRuntimeOperations
{
    IReadOnlyList<int> DrawCards(CharacterState player, int count, bool log,
        CardMoveReason? reason = null, CardConversionSource? directProducer = null);
    Card? DrawOne(CharacterState player, CardMoveReason reason);
    Card? DrawOneToProcessing(CardMoveReason reason);
    bool EnsureDrawPile();
}

// The match's existing physical zone store owns this order mutation. Its
// caller supplies the original match random source, never a copied stream.
internal interface ICardDrawPileOrder : IProgramRuntimeOperations
{
    void ShuffleDrawPile(IMatchRandomSource random);
}

internal interface IPhysicalCardDrawPolicies : IProgramRuntimeOperations
{
    bool DrawFromBottom(CharacterState player);
    Card MinimumDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary);
    bool TryChooseDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, out Card? material);
    void ExcludeDeckEquipment(CardLocation location);
    bool AllowsDiscardRecycling { get; }
    void HandleEmptyDrawPile();
    void NotifyDeckRecycled();
}

internal interface IPhysicalCardDrawMovement : IProgramRuntimeOperations
{
    void Move(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    void MoveAll(CardLocation from, CardLocation to, CardMoveReason reason);
}

internal interface IPhysicalCardDrawLogs : IProgramRuntimeOperations
{
    void Add(string type, string message, int? actor = null);
}
